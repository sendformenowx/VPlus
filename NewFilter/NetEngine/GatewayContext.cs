using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CORE_NETWORKING;
using Framework;
using NewFilter.CORE;

namespace NewFilter.NetEngine;

public class GatewayContext : BaseSecurityModule
{
	public AsyncServer.MODULE_TYPE MODULE_TYPE = AsyncServer.MODULE_TYPE.GatewayServer;

	public DateTime START_TIME = DateTime.Now;

	public ConcurrentQueue<Packet> GW_TRAFFIC = new ConcurrentQueue<Packet>();

	public string SOCKET_IP = string.Empty;

	public string REDIR_IP = string.Empty;

	public uint TOKEN_ID = 0u;

	public bool GATEWAY_PRE_CONNECTION = false;

	public double PPS = 0.0;

	public double BPS = 0.0;

	public ushort REDIR_PORT = 0;

	public int TOT_PACKET_CNT = 0;

	public int TOT_BYTES_CNT = 0;

	public GatewayContext CORRESPONDING_GW_SESSION = null;

	public bool PatchSent = false;

	public bool ListSent = false;

	public string user_id;

	public int PACKET_COUNT = 0;

	public Timer PacketTimer = null;

	public int TotalPlayersOnline { get; set; } = 0;


	public GatewayContext(Socket _CLIENT_SOCKET)
	{
		try
		{
			base.CLIENT_SOCKET = _CLIENT_SOCKET;
			this.SOCKET_IP = ((IPEndPoint)_CLIENT_SOCKET.RemoteEndPoint).Address.ToString();
			base.PROXY_SOCKET = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Utils.FLOOD_LIST_GATEWAY.Add(this.SOCKET_IP.Split(':')[0]);
			base.LOCAL_BUFFER = new TransferBuffer(8192, 0, 0);
			base.REMOTE_BUFFER = new TransferBuffer(8192, 0, 0);
			base.LOCAL_SECURITY = new Security();
			base.LOCAL_SECURITY.GenerateSecurity(blowfish: true, security_bytes: true, handshake: true);
			base.REMOTE_SECURITY = new Security();
			if (MainMenu.FLOOD_COUNT > 0)
			{
				Task.Factory.StartNew(delegate
				{
					if (Utils.FLOOD_COUNT_GATEWAY(this.SOCKET_IP.Split(':')[0]) > MainMenu.FLOOD_COUNT)
					{
						MainMenu.WriteLine(3, $"Client({this.SOCKET_IP}) disconnected for {this.MODULE_TYPE} flooding.");
						Utils.BLOCK_IP(this.SOCKET_IP.Split(':')[0], $"{this.MODULE_TYPE} DoS");
						AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
					}
				});
			}
			if (this.PacketTimer == null)
			{
				this.PacketTimer = new Timer(PacketTimerEvent, null, 0, MainMenu.GATEWAY_PACKET_RESET);
			}
			this.ConnectToServerModules();
		}
		catch (Exception ex)
		{
			AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			MainMenu.WriteLine(1, "GatewayContext" + ex?.ToString() + base.PROXY_SOCKET);
		}
	}

	private void PacketTimerEvent(object e)
	{
		try
		{
			if (this.PACKET_COUNT > MainMenu.GW_PPS_VALUE)
			{
				MainMenu.WriteLine(2, "Client(" + this.SOCKET_IP + ") was kicked for exceeding GatewayServer packet limit/second");
				AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
				Utils.BLOCK_IP(this.SOCKET_IP.Split(':')[0], $"{this.MODULE_TYPE} packet limit/second");
				return;
			}
		}
		catch
		{
		}
		this.PACKET_COUNT = 0;
	}

	public override async void ConnectToServerModules()
	{
		try
		{
			bool flag;
			flag = base.PROXY_SOCKET != null;
			if (flag)
			{
				flag = (await base.PROXY_SOCKET.ConnectToSocket(MainMenu.Server_IP, MainMenu.Gateway_Server_port)).Connected;
			}
			if (flag)
			{
				this.ReceiveFromClient();
				this.SendToClient();
			}
			else
			{
				AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			}
		}
		catch (Exception EX)
		{
			AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			MainMenu.WriteLine(2, "GW_ASYNC_CONNECT_TO_MODULE" + EX?.ToString() + base.PROXY_SOCKET);
			MainMenu.WriteLine(2, "Gateway Server açık değil yada bağlantı kurulamıyor.Gateway Server bağlantı ip lerini veya port ayarlarını konrol edin...");
		}
	}

	public override async void ReceiveFromClient()
	{
		try
		{
			int RECEIVED_DATA;
			RECEIVED_DATA = await base.CLIENT_SOCKET.RecvFromSocket(base.LOCAL_BUFFER.Buffer, base.LOCAL_BUFFER.Buffer.Length);
			if (RECEIVED_DATA > 0)
			{
				base.LOCAL_SECURITY.Recv(base.LOCAL_BUFFER.Buffer, 0, RECEIVED_DATA);
				List<Packet> RemotePackets;
				RemotePackets = base.LOCAL_SECURITY.TransferIncoming();
				if (DateTime.Now.Subtract(this.START_TIME).TotalSeconds > 5.0 && MainMenu.GW_BPS_VALUE > 0)
				{
					this.BPS = Utils.GET_PER_SEC_RATE((ulong)(this.TOT_BYTES_CNT += RECEIVED_DATA), this.START_TIME);
					if (this.BPS > (double)MainMenu.GW_BPS_VALUE)
					{
						if (MainMenu.FIREWALLBANCHECK)
						{
							Utils.BLOCK_IP(this.SOCKET_IP.Split(':')[0], "GatewayServer Exceeded GatewayServer Bytes");
							AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
							MainMenu.WriteLine(3, $"[{this.SOCKET_IP}] exceeded GatewayServer BytesP/S:[{this.BPS}/{MainMenu.GW_BPS_VALUE}] and has been banned via firewall.");
						}
						else
						{
							MainMenu.WriteLine(3, $"[{this.SOCKET_IP}] exceeded GatewayServer BytesP/S:[{this.BPS}/{MainMenu.GW_BPS_VALUE}]");
							AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
						}
						return;
					}
				}
				if (RemotePackets != null)
				{
					foreach (Packet _pck in RemotePackets)
					{
						if (_pck.Opcode == 8193)
						{
							this.ReceiveFromModule();
							if (_pck.GetBytes().Length != 12)
							{
								MainMenu.WriteLine(3, "Client(" + this.SOCKET_IP + ") tried to exploit 0x2001");
								AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
								return;
							}
							continue;
						}
						if (_pck.Opcode == 20480 || _pck.Opcode == 36864)
						{
							this.SendToClient();
							continue;
						}
						if (_pck.Opcode == 24833)
						{
							if (_pck.GetBytes().Length != 0)
							{
								MainMenu.WriteLine(3, "Opcode 0x6101 IP Address " + this.SOCKET_IP + " disconnected try to exploit bytes length");
								AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
								return;
							}
							this.ListSent = true;
						}
						else if (_pck.Opcode == 8194)
						{
							if (_pck.GetBytes().Length != 0)
							{
								MainMenu.WriteLine(3, "[" + this.SOCKET_IP + "] detected GatewayServer invalid ping.");
								Utils.BLOCK_IP(this.SOCKET_IP.Split(':')[0], "GatewayServer invalid ping.");
								AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
								return;
							}
						}
						else if (_pck.Opcode == 24832)
						{
							if (this.PatchSent)
							{
								MainMenu.WriteLine(3, "GatewayServer detected " + this.SOCKET_IP + " try to exploit opcode 0x6100");
								Utils.BLOCK_IP(this.SOCKET_IP.Split(':')[0], "GatewayServer Attempted to use a Clientless exploit Packet 0x6100");
								AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
								return;
							}
							this.PatchSent = true;
						}
						else if (_pck.Opcode == 24834)
						{
							_pck.ReadUInt8();
							string user_id;
							user_id = _pck.ReadAscii().ToLower();
							_pck.ReadAscii();
							_pck.ReadUInt16();
							if (MainMenu.GM_ACCOUNT_List.Contains(user_id) && !MainMenu.GM_IP_List.Contains(this.SOCKET_IP))
							{
								Packet new_packet2;
								new_packet2 = new Packet(41218, encrypted: false);
								new_packet2.WriteUInt8(2);
								new_packet2.WriteUInt8(7);
								base.LOCAL_SECURITY.Send(new_packet2);
								this.SendToClient();
								AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
								return;
							}
							if (MainMenu.MAINTENANCE && !MainMenu.GM_IP_List.Contains(this.SOCKET_IP))
							{
								Packet new_packet;
								new_packet = new Packet(41218, encrypted: false);
								new_packet.WriteUInt8(2);
								new_packet.WriteUInt8(7);
								base.LOCAL_SECURITY.Send(new_packet);
								this.SendToClient();
								AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
								return;
							}
						}
						base.REMOTE_SECURITY.Send(_pck);
						this.SendToModule();
					}
				}
				this.ReceiveFromClient();
			}
			else
			{
				AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			}
		}
		catch (Exception EX)
		{
			AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			MainMenu.WriteLine(1, "GW_ASYNC_RECV_FROM_CLIENT" + EX?.ToString() + base.CLIENT_SOCKET);
		}
	}

	public void LoadCustomUniqueNotifys()
	{
		try
		{
			if (sqlCon.CustomUniqueNotices == null || sqlCon.CustomUniqueNotices.Rows.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(4625);
			packet.WriteUInt8(sqlCon.CustomUniqueNotices.Rows.Count);
			foreach (DataRow row in sqlCon.CustomUniqueNotices.Rows)
			{
				string value;
				value = row["UniqueSTRID"].ToString();
				string value2;
				value2 = row["UniqueSpawnNotice"].ToString();
				string value3;
				value3 = row["UniqueDespawnNotice"].ToString();
				string value4;
				value4 = row["UniqueKillNotice"].ToString();
				packet.WriteUnicode(value);
				packet.WriteUnicode(value2);
				packet.WriteUnicode(value3);
				packet.WriteUnicode(value4);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading uniqnotifys");
		}
	}

	public void senddcid()
	{
		Packet packet;
		packet = new Packet(4624);
		packet.WriteInt64(MainMenu.DiscordInstanceID);
		packet.WriteUInt8(MainMenu.NewReverse);
		packet.WriteUInt8(MainMenu.OldMainPopup);
		packet.WriteUInt8(MainMenu.ItemComparison);
		packet.WriteInt32(MainMenu.MasteryLimit);
		packet.WriteInt32(MainMenu.MaxPartyLevelLimit);
		packet.WriteUInt8(MainMenu.PermanenyAlchemy);
		packet.WriteUInt8(MainMenu.GuildJobMode);
		packet.WriteInt32(MainMenu.CustomTitlePrice);
		packet.WriteUnicode(MainMenu.CustomTitleBirim);
		this.LoadCustomUniqueNotifys();
		base.LOCAL_SECURITY.Send(packet);
		this.SendToClient();
	}

	public override async void ReceiveFromModule()
	{
		try
		{
			int RECEIVED_DATA;
			RECEIVED_DATA = await base.PROXY_SOCKET.RecvFromSocket(base.REMOTE_BUFFER.Buffer, base.REMOTE_BUFFER.Buffer.Length);
			if (RECEIVED_DATA > 0)
			{
				base.REMOTE_SECURITY.Recv(base.REMOTE_BUFFER.Buffer, 0, RECEIVED_DATA);
				List<Packet> RemotePackets;
				RemotePackets = base.REMOTE_SECURITY.TransferIncoming();
				if (RemotePackets != null)
				{
					foreach (Packet _pck in RemotePackets)
					{
						if (_pck.Opcode == 20480 || _pck.Opcode == 36864)
						{
							this.SendToModule();
							continue;
						}
						if (_pck.Opcode == 41216)
						{
							if (_pck.ReadUInt8() == 2 && _pck.ReadUInt8() == 2)
							{
								_pck.ReadAscii();
								_pck.ReadUInt16();
								uint serverVersion;
								serverVersion = _pck.ReadUInt32();
								byte hasEntries;
								hasEntries = _pck.ReadUInt8();
								Packet SERVER_GATEWAY_PATCH_RESPONSE;
								SERVER_GATEWAY_PATCH_RESPONSE = new Packet(41216, encrypted: false, massive: true);
								SERVER_GATEWAY_PATCH_RESPONSE.WriteUInt8(2);
								SERVER_GATEWAY_PATCH_RESPONSE.WriteUInt8(2);
								SERVER_GATEWAY_PATCH_RESPONSE.WriteAscii(MainMenu.Proxy_IP);
								SERVER_GATEWAY_PATCH_RESPONSE.WriteUInt16(MainMenu.Download_Public_port);
								SERVER_GATEWAY_PATCH_RESPONSE.WriteUInt32(serverVersion);
								SERVER_GATEWAY_PATCH_RESPONSE.WriteUInt8(hasEntries);
								while (hasEntries == 1)
								{
									uint fileIdentity;
									fileIdentity = _pck.ReadUInt32();
									string fileName;
									fileName = _pck.ReadAscii();
									string filePath;
									filePath = _pck.ReadAscii();
									uint fileSize;
									fileSize = _pck.ReadUInt32();
									byte isPacked;
									isPacked = _pck.ReadUInt8();
									hasEntries = _pck.ReadUInt8();
									SERVER_GATEWAY_PATCH_RESPONSE.WriteUInt32(fileIdentity);
									SERVER_GATEWAY_PATCH_RESPONSE.WriteAscii(fileName);
									SERVER_GATEWAY_PATCH_RESPONSE.WriteAscii(filePath);
									SERVER_GATEWAY_PATCH_RESPONSE.WriteUInt32(fileSize);
									SERVER_GATEWAY_PATCH_RESPONSE.WriteUInt8(isPacked);
									SERVER_GATEWAY_PATCH_RESPONSE.WriteUInt8(hasEntries);
								}
								base.LOCAL_SECURITY.Send(SERVER_GATEWAY_PATCH_RESPONSE);
								this.SendToClient();
								continue;
							}
						}
						else
						{
							if (_pck.Opcode == 41217)
							{
								Packet SERVER_GATEWAY_SHARD_LIST_RESPONSE;
								SERVER_GATEWAY_SHARD_LIST_RESPONSE = new Packet(41217);
								byte GlobalOperationFlag2;
								GlobalOperationFlag2 = _pck.ReadUInt8();
								SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteUInt8(GlobalOperationFlag2);
								if (GlobalOperationFlag2 == 1)
								{
									byte GlobalOperationType;
									GlobalOperationType = _pck.ReadUInt8();
									string GlobalOperationName;
									GlobalOperationName = _pck.ReadAscii();
									GlobalOperationFlag2 = _pck.ReadUInt8();
									SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteUInt8(GlobalOperationType);
									SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteAscii(GlobalOperationName);
									SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteUInt8(GlobalOperationFlag2);
								}
								byte ShardFlag2;
								ShardFlag2 = _pck.ReadUInt8();
								SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteUInt8(ShardFlag2);
								if (ShardFlag2 == 1)
								{
									uint ShardID;
									ShardID = _pck.ReadUInt16();
									_pck.ReadAscii();
									uint ShardCurrent;
									ShardCurrent = _pck.ReadUInt16();
									uint ShardCapacity;
									ShardCapacity = _pck.ReadUInt16();
									_pck.ReadUInt8();
									byte GlobalOperationID;
									GlobalOperationID = _pck.ReadUInt8();
									ShardFlag2 = _pck.ReadUInt8();
									SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteUInt16(ShardID);
									SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteAscii(MainMenu.SERVER_NAME);
									SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteUInt16(ShardCurrent);
									SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteUInt16(ShardCapacity);
									if (MainMenu.MAINTENANCE && !MainMenu.GM_IP_List.Contains(this.SOCKET_IP))
									{
										SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteUInt8(0);
									}
									else
									{
										SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteUInt8(1);
									}
									SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteUInt8(GlobalOperationID);
									SERVER_GATEWAY_SHARD_LIST_RESPONSE.WriteUInt8(ShardFlag2);
								}
								base.LOCAL_SECURITY.Send(SERVER_GATEWAY_SHARD_LIST_RESPONSE);
								this.SendToClient();
								this.senddcid();
								continue;
							}
							if (_pck.Opcode == 41218)
							{
								byte ServerState;
								ServerState = _pck.ReadUInt8();
								if (ServerState == 1)
								{
									this.TOKEN_ID = _pck.ReadUInt32();
									this.REDIR_IP = _pck.ReadAscii();
									this.REDIR_PORT = _pck.ReadUInt16();
									new TokenProvider(this.SOCKET_IP, this.REDIR_IP, this.REDIR_PORT);
									MainMenu.WriteLine(3, "SOCKET_IP: " + this.SOCKET_IP + "   REDIR_IP: " + this.REDIR_IP + "   REDIR_PORT: " + this.REDIR_PORT);
									Packet CLIENT_GATEWAY_LOGIN_REQUEST_RESPONSE;
									CLIENT_GATEWAY_LOGIN_REQUEST_RESPONSE = new Packet(41218, encrypted: true);
									CLIENT_GATEWAY_LOGIN_REQUEST_RESPONSE.WriteUInt8(ServerState);
									CLIENT_GATEWAY_LOGIN_REQUEST_RESPONSE.WriteUInt32(this.TOKEN_ID);
									if (MainMenu.Server_IP == this.SOCKET_IP)
									{
										CLIENT_GATEWAY_LOGIN_REQUEST_RESPONSE.WriteAscii(MainMenu.Server_IP);
									}
									else
									{
										CLIENT_GATEWAY_LOGIN_REQUEST_RESPONSE.WriteAscii(MainMenu.Proxy_IP);
									}
									CLIENT_GATEWAY_LOGIN_REQUEST_RESPONSE.WriteUInt16(MainMenu.Agent_Public_port);
									base.LOCAL_SECURITY.Send(CLIENT_GATEWAY_LOGIN_REQUEST_RESPONSE);
									this.SendToClient();
									this.GATEWAY_PRE_CONNECTION = true;
									AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
									continue;
								}
							}
							else if (_pck.Opcode == 8994)
							{
								Packet CLIENT_GATEWAY_LOGIN_IBUV_ANSWER;
								CLIENT_GATEWAY_LOGIN_IBUV_ANSWER = new Packet(25379, encrypted: false);
								CLIENT_GATEWAY_LOGIN_IBUV_ANSWER.WriteAscii(MainMenu.CAPCHA);
								base.REMOTE_SECURITY.Send(CLIENT_GATEWAY_LOGIN_IBUV_ANSWER);
								this.SendToModule();
								continue;
							}
						}
						base.LOCAL_SECURITY.Send(_pck);
						this.SendToClient();
					}
				}
				this.ReceiveFromModule();
			}
			else
			{
				AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			}
		}
		catch (Exception EX)
		{
			AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			MainMenu.WriteLine(1, "GW_ASYNC_RECV_FROM_MODULE" + EX);
		}
	}

	public override void SendToClient()
	{
		try
		{
			List<KeyValuePair<TransferBuffer, Packet>> list;
			list = base.LOCAL_SECURITY.TransferOutgoing();
			if (list == null)
			{
				return;
			}
			foreach (KeyValuePair<TransferBuffer, Packet> item in list)
			{
				base.CLIENT_SOCKET.SendToSocket(item.Key.Buffer, item.Key.Size);
			}
		}
		catch (Exception ex)
		{
			AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			MainMenu.WriteLine(1, "GW_SendToClient" + ex);
		}
	}

	public override void SendToModule()
	{
		try
		{
			List<KeyValuePair<TransferBuffer, Packet>> list;
			list = base.REMOTE_SECURITY.TransferOutgoing();
			if (list == null)
			{
				return;
			}
			foreach (KeyValuePair<TransferBuffer, Packet> item in list)
			{
				base.PROXY_SOCKET.SendToSocket(item.Key.Buffer, item.Key.Size);
			}
		}
		catch (Exception ex)
		{
			AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			MainMenu.WriteLine(1, "GW_ASYNC_SEND_TO_MODULE" + ex);
		}
	}
}
