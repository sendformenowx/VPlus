using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CORE_NETWORKING;
using Framework;
using NewFilter.CORE;

namespace NewFilter.NetEngine;

public class DownloadComtext : BaseSecurityModule
{
	public AsyncServer.MODULE_TYPE MODULE_TYPE = AsyncServer.MODULE_TYPE.DownloadServer;

	public DateTime START_TIME = DateTime.Now;

	public double PPS = 0.0;

	public double BPS = 0.0;

	public int TOT_PACKET_CNT = 0;

	public int TOT_BYTES_CNT = 0;

	public string SOCKET_IP = string.Empty;

	public int PACKET_COUNT = 0;

	public Timer PacketTimer = null;

	public DownloadComtext(Socket _CLIENT_SOCKET)
	{
		try
		{
			base.CLIENT_SOCKET = _CLIENT_SOCKET;
			this.SOCKET_IP = base.CLIENT_SOCKET.RemoteEndPoint.ToString();
			base.PROXY_SOCKET = new CustomSocket();
			Utils.FLOOD_LIST_DOWNLOAD.Add(this.SOCKET_IP.Split(':')[0]);
			base.LOCAL_BUFFER = new TransferBuffer(8192, 0, 0);
			base.REMOTE_BUFFER = new TransferBuffer(8192, 0, 0);
			base.LOCAL_SECURITY = new Security();
			base.LOCAL_SECURITY.GenerateSecurity(blowfish: true, security_bytes: true, handshake: true);
			base.REMOTE_SECURITY = new Security();
			if (MainMenu.FLOOD_COUNT > 0)
			{
				Task.Factory.StartNew(delegate
				{
					if (Utils.FLOOD_COUNT_DOWNLOAD(this.SOCKET_IP.Split(':')[0]) > MainMenu.FLOOD_COUNT)
					{
						MainMenu.WriteLine(1, $"Client({this.SOCKET_IP}) disconnected for {this.MODULE_TYPE} flooding.");
						Utils.BLOCK_IP(this.SOCKET_IP.Split(':')[0], $"{this.MODULE_TYPE} DoS");
						AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
					}
				});
			}
			if (this.PacketTimer == null)
			{
				this.PacketTimer = new Timer(PacketTimerEvent, null, 0, MainMenu.DOWNLOAD_PACKET_RESET);
			}
			this.ConnectToServerModules();
		}
		catch (Exception ex)
		{
			AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			MainMenu.WriteLine(1, "DownloadComtext" + ex?.ToString() + base.PROXY_SOCKET);
		}
	}

	private void PacketTimerEvent(object e)
	{
		try
		{
			if (this.PACKET_COUNT > 1000)
			{
				MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ") was kicked for exceeding DownloadServer packet limit/second");
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
				flag = (await base.PROXY_SOCKET.ConnectToSocket(MainMenu.Server_IP, MainMenu.Download_Server_port)).Connected;
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
			MainMenu.WriteLine(1, "DW_ASYNC_CONNECT_TO_MODULE" + EX?.ToString() + base.PROXY_SOCKET);
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
				if (RemotePackets != null)
				{
					foreach (Packet _pck in RemotePackets)
					{
						if (_pck.Opcode == 8193)
						{
							this.ReceiveFromModule();
							continue;
						}
						if (_pck.Opcode == 20480 || _pck.Opcode == 36864)
						{
							this.SendToClient();
							continue;
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
			MainMenu.WriteLine(1, "DW_ASYNC_RECV_FROM_CLIENT" + EX);
		}
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
				foreach (Packet _pck in base.REMOTE_SECURITY.TransferIncoming())
				{
					if (_pck.Opcode == 20480 || _pck.Opcode == 36864)
					{
						this.SendToModule();
						continue;
					}
					base.LOCAL_SECURITY.Send(_pck);
					this.SendToClient();
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
			MainMenu.WriteLine(1, "DW_ASYNC_RECV_FROM_MODULE" + EX);
		}
	}

	public override void SendToClient()
	{
		try
		{
			List<KeyValuePair<TransferBuffer, Packet>> list;
			list = base.LOCAL_SECURITY.TransferOutgoing();
			for (int i = 0; i < list.Count; i++)
			{
				base.CLIENT_SOCKET.SendToSocket(list[i].Key.Buffer, list[i].Key.Size);
			}
		}
		catch (Exception ex)
		{
			AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			MainMenu.WriteLine(1, "DW_SendToClient" + ex);
		}
	}

	public override void SendToModule()
	{
		try
		{
			List<KeyValuePair<TransferBuffer, Packet>> list;
			list = base.REMOTE_SECURITY.TransferOutgoing();
			for (int i = 0; i < list.Count; i++)
			{
				base.PROXY_SOCKET.SendToSocket(list[i].Key.Buffer, list[i].Key.Size);
			}
		}
		catch (Exception ex)
		{
			AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			MainMenu.WriteLine(1, "DW_ASYNC_SEND_TO_MODULE" + ex);
		}
	}
}
