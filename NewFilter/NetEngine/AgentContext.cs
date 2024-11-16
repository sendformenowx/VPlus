using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CORE_NETWORKING;
using Framework;
using NewFilter.AgentUtils;
using NewFilter.CORE;
using NewFilter.ItemInfo;
using Ranks;
using ReverseLoc;
using SQL;

namespace NewFilter.NetEngine;

public class AgentContext : BaseSecurityModule
{
	public AsyncServer.MODULE_TYPE MODULE_TYPE = AsyncServer.MODULE_TYPE.AgentServer;

	public DateTime LAST_STALL_TIME = new DateTime(2020, 12, 31);

	public DateTime LAST_GLOBAL_TIME = new DateTime(2020, 12, 31);

	public DateTime LAST_EXREQ_TIME = new DateTime(2020, 12, 31);

	public DateTime LAST_GUILDREQ_TIME = new DateTime(2020, 12, 31);

	public DateTime LAST_UNIREQ_TIME = new DateTime(2020, 12, 31);

	public DateTime LAST_ZERK_TIME = new DateTime(2020, 12, 31);

	public DateTime LAST_REVERSE_TIME = new DateTime(2020, 12, 31);

	public DateTime LAST_SPAWN_TIME = new DateTime(2020, 12, 31);

	public DateTime LAST_REST_TIME = new DateTime(2020, 12, 31);

	public DateTime LAST_EXIT_TIME = new DateTime(2020, 12, 31);

	public DateTime LAST_TITLE_TIME = new DateTime(2020, 12, 31);

	public bool inreturn = false;

	public bool innpc = false;

	public bool INSIDE_FTW = false;

	public bool INSIDE_BA = false;

	public bool INSIDE_FGW = false;

	public bool INSIDE_JUPITER = false;

	public bool INSIDE_TEMPLE = false;

	public bool INSIDE_HWT = false;

	public bool INSIDE_CTF = false;

	public bool INSIDE_PT = false;

	public bool INSIDE_SURVIVAL = false;

	public bool INSIDE_LMS = false;

	public bool INSIDE_PVPR = false;

	public bool INSIDE_KILLGUI = false;

	public static List<short> FORTRESS_REGIONS = new List<short>();

	public static List<short> BA_REGIONS = new List<short>();

	public static List<short> JUPITER_REGIONS = new List<short>();

	public static List<short> FGW_REGIONS = new List<short>();

	public static List<short> HWT_REGIONS = new List<short>();

	public static List<string> TEMPLE_REGIONS = new List<string>();

	public static List<uint> FW_TELEPORT_ID = new List<uint>();

	public static List<uint> TEMPLE_TELEPORT_ID = new List<uint>();

	public static List<uint> JUB_TELEPORT_ID = new List<uint>();

	public static List<uint> HWT_TELEPORT_ID = new List<uint>();

	public static List<string> RETYPE_NAMES = new List<string>();

	public static int CharIcon = 0;

	private string TitleHEXColor;

	private string NameHEXColor;

	public string SOCKET_IP = string.Empty;

	public string SOCKET_IP2 = string.Empty;

	public string HWID = string.Empty;

	public DateTime START_TIME = DateTime.Now;

	public int PACKET_COUNT = 0;

	public Timer PacketTimer = null;

	public uint UNIQUE_ID = 0u;

	public string StrUserID = string.Empty;

	public string Charname = string.Empty;

	public string CHAR_PW = string.Empty;

	public uint TOKEN_ID = 0u;

	public short CurRegion;

	public int WorldID;

	public byte CurLevel;

	public int CharID;

	public byte JobType;

	public bool FirstSpawn = false;

	public int CharToken = 0;

	public int CharSilk = 0;

	public bool EUChar = false;

	public bool CHChar = false;

	public byte HwanLevel = 0;

	private bool is_locked = true;

	private int lock_fail_attempt = 0;

	private int has_code = 0;

	private byte GroupSpawnType = 0;

	private ushort GroupSpawnAmount = 0;

	public Dictionary<byte, string> CharTitles;

	public Dictionary<byte, string> CharTitlesNew;

	public List<_CharChest> CharChest;

	private DateTime afk_timer = DateTime.Now;

	private bool isAfk = false;

	private bool isAfkSymbol = true;

	private bool isOnline = false;

	public bool STALLING = false;

	public bool PARTYSTATUS = false;

	public bool DEAD_STATUS = false;

	public bool ZERKING = false;

	public bool PVP_FLAG = false;

	public bool JOB_FLAG = false;

	public bool CTF_REG = false;

	public bool INVISIBLE_STATUS = false;

	public bool IsCharScreen = false;

	public bool UserLogged = false;

	public bool JOB_YELLOW_LINE = false;

	private string Jobname = string.Empty;

	public string NewCustomTitle = string.Empty;

	public GatewayContext CORRESPONDING_GW_SESSION = null;

	public Dictionary<byte, SavedLocation> SavedLocations;

	public bool isStartNotice = false;

	private bool global = false;

	public int TOT_BYTES_CNT { get; set; } = 0;


	public double BPS { get; set; } = 0.0;


	public uint FELLOW_PET { get; set; } = 0u;


	public uint ATTACK_PET { get; set; } = 0u;


	public uint GRAB_PET { get; set; } = 0u;


	public bool RIDING_PET { get; set; } = false;


	public AgentContext(Socket _CLIENT_SOCKET)
	{
		try
		{
			this.SOCKET_IP2 = ((IPEndPoint)_CLIENT_SOCKET.RemoteEndPoint).Address.ToString();
			base.CLIENT_SOCKET = _CLIENT_SOCKET;
			this.SOCKET_IP = base.CLIENT_SOCKET.RemoteEndPoint.ToString();
			base.PROXY_SOCKET = new CustomSocket();
			Utils.FLOOD_LIST_AGENT.Add(this.SOCKET_IP.Split(':')[0]);
			base.LOCAL_BUFFER = new TransferBuffer(8192, 0, 0);
			base.REMOTE_BUFFER = new TransferBuffer(8192, 0, 0);
			base.LOCAL_SECURITY = new Security();
			base.LOCAL_SECURITY.GenerateSecurity(blowfish: true, security_bytes: true, handshake: true);
			base.REMOTE_SECURITY = new Security();
			this.CharTitles = new Dictionary<byte, string>();
			this.CharTitlesNew = new Dictionary<byte, string>();
			this.CharChest = new List<_CharChest>();
			this.SavedLocations = new Dictionary<byte, SavedLocation>();
			if (MainMenu.FLOOD_COUNT > 0 && this.SOCKET_IP.Split(':')[0] != "192.168.8.101")
			{
				Task.Factory.StartNew(delegate
				{
					if (Utils.FLOOD_COUNT_AGENT(this.SOCKET_IP.Split(':')[0]) > MainMenu.FLOOD_COUNT)
					{
						MainMenu.WriteLine(1, $"Client({this.SOCKET_IP}) disconnected for {this.MODULE_TYPE} flooding.");
						Utils.BLOCK_IP(this.SOCKET_IP.Split(':')[0], $"{this.MODULE_TYPE} DoS");
						AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
					}
				});
			}
			if (this.PacketTimer == null)
			{
				this.PacketTimer = new Timer(PacketTimerEvent, null, 0, MainMenu.AGENT_PACKET_RESET);
			}
			this.ConnectToServerModules();
		}
		catch (Exception ex)
		{
			AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			MainMenu.WriteLine(1, "AgentContext" + ex?.ToString() + base.PROXY_SOCKET);
		}
	}

	private void PacketTimerEvent(object e)
	{
		try
		{
			if (this.IsCharScreen && this.PACKET_COUNT > 7)
			{
				MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") was kicked for sending to many packets in char screen!");
				AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
				return;
			}
			if (this.PACKET_COUNT > MainMenu.AG_PPS_VALUE)
			{
				MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ") was kicked for exceeding AgentServer packet limit/second");
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
			TokenProvider token;
			token = TokenProvider.GetToken(this.SOCKET_IP2);
			if (token != null)
			{
				bool flag;
				flag = base.PROXY_SOCKET != null;
				if (flag)
				{
					flag = (await base.PROXY_SOCKET.ConnectToSocket(token.AgentIP, token.AgentPort)).Connected;
				}
				if (flag)
				{
					this.ReceiveFromClient();
					this.SendToClient();
				}
			}
			else
			{
				AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
			}
		}
		catch (Exception)
		{
			AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
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
				if (DateTime.Now.Subtract(this.START_TIME).TotalSeconds > 5.0 && MainMenu.AG_BPS_VALUE > 0)
				{
					this.BPS = Utils.GET_PER_SEC_RATE((ulong)(this.TOT_BYTES_CNT += RECEIVED_DATA), this.START_TIME);
					if (this.BPS > (double)MainMenu.AG_BPS_VALUE)
					{
						if (MainMenu.FIREWALLBANCHECK)
						{
							Utils.BLOCK_IP(this.SOCKET_IP.Split(':')[0], "AgentServer Exceeded AgentServer Bytes");
							AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
							MainMenu.WriteLine(1, $"[{this.SOCKET_IP}] exceeded AgentServer BytesP/S:[{this.BPS}/{MainMenu.AG_BPS_VALUE}] and has been banned via firewall.");
						}
						else
						{
							MainMenu.WriteLine(1, $"[{this.SOCKET_IP}] exceeded AgentServer BytesP/S:[{this.BPS}/{MainMenu.AG_BPS_VALUE}]");
							AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
						}
						return;
					}
				}
				if (RemotePackets != null)
				{
					int slot2;
					foreach (Packet _pck in RemotePackets)
					{
						if (_pck.Opcode != 28906)
						{
							this.PACKET_COUNT++;
						}
						if (this.IsCharScreen)
						{
							if (_pck.Opcode == 29016)
							{
								try
								{
									switch (_pck.ReadUInt8())
									{
									case 0:
										if (_pck.GetBytes().Length != 2)
										{
											MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #7");
											break;
										}
										if (_pck.ReadUInt8() != 7)
										{
											MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #13");
											break;
										}
										goto IL_06e8;
									case 1:
										if (_pck.GetBytes().Length != 7)
										{
											MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #8");
											break;
										}
										_pck.ReadUInt8();
										_pck.ReadUInt8();
										_pck.ReadUInt32();
										goto IL_06e8;
									default:
										MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #9");
										break;
									}
								}
								catch
								{
									MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #10");
								}
								continue;
							}
							goto IL_06e8;
						}
						goto IL_07ef;
						IL_82f1:
						if (MainMenu.OpCodeWhiteList && !MainMenu.WhiteList.Contains($"{_pck.Opcode:X}"))
						{
							MainMenu.WriteLine(1, $"Client({this.SOCKET_IP}:{this.StrUserID}) was kicked for sending to packet : {_pck.Opcode:X}");
							AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
							return;
						}
						base.REMOTE_SECURITY.Send(_pck);
						this.SendToModule();
						continue;
						IL_0e72:
						int Glabalstatus;
						Glabalstatus = _pck.ReadInt8();
						int globalslot;
						globalslot = _pck.ReadInt8();
						int globaltype;
						globaltype = _pck.ReadInt16();
						string GetRefItemID;
						GetRefItemID = Convert.ToString(Task.Run(async () => await sqlCon.GetRefItemID(this.Charname, globalslot.ToString())).Result);
						int argbInputColor;
						argbInputColor = int.Parse(Convert.ToString(Task.Run(async () => await sqlCon.GlobalColor(GetRefItemID)).Result).Replace("#", ""), NumberStyles.HexNumber);
						string messenge;
						messenge = _pck.ReadUnicode();
						switch (Glabalstatus)
						{
						case 1:
						{
							uint m_LinkedGlobalSlot;
							m_LinkedGlobalSlot = _pck.ReadUInt8();
							Packet itemlink3;
							itemlink3 = new Packet(4921);
							itemlink3.WriteInt8(1);
							itemlink3.WriteUnicode(this.Charname + ":" + messenge);
							itemlink3.WriteUInt32(Convert.ToInt32(argbInputColor));
							new _ItemInfo();
							_ItemInfo Item;
							Item = await sqlCon.Get_BindingInfo_by_Slot(await sqlCon.Get_ItemInfo_by_Slot(Convert.ToInt32(m_LinkedGlobalSlot), this.CharID));
							Utils.ItemLinkInfonum++;
							if (!Utils.ItemLinkInfo.ContainsKey(Utils.ItemLinkInfonum))
							{
								Utils.ItemLinkInfo.TryAdd(Utils.ItemLinkInfonum, Item);
							}
							if (Utils.ItemLinkInfo.ContainsKey(Utils.ItemLinkInfonum))
							{
								itemlink3 = this.SendB034(itemlink3, Utils.ItemLinkInfo[Utils.ItemLinkInfonum]);
							}
							AgentContext.SendPackagetoAll(itemlink3);
							break;
						}
						case 2:
						{
							Packet itemlink2;
							itemlink2 = new Packet(4921);
							itemlink2.WriteInt8(2);
							itemlink2.WriteUnicode(this.Charname + ":" + messenge);
							itemlink2.WriteUInt32(Convert.ToInt32(argbInputColor));
							AgentContext.SendPackagetoAll(itemlink2);
							break;
						}
						}
						this.GlobalSend(messenge, globalslot, globaltype);
						continue;
						IL_25c5:
						if (this.CurLevel < MainMenu.EXCHANGE_LEVEL)
						{
							this.SendMessage(3, MainMenu.EXCHANGE_LEVEL_NOTICE.Replace("%level%", MainMenu.EXCHANGE_LEVEL.ToString()));
							continue;
						}
						if (MainMenu.ITEM_LOCK && this.has_code > 0)
						{
							this.SendMessage(3, MainMenu.ITEM_LOCK_EXCHANGE_NOTICE);
							continue;
						}
						goto IL_82f1;
						IL_07ef:
						if (_pck.Opcode == 6025 && MainMenu.PC_LIMIT > 0)
						{
							switch (_pck.ReadUInt8())
							{
							case 1:
							{
								for (int ix2 = 0; ix2 < 29; ix2++)
								{
									if (_pck.ReadUInt8() != 237)
									{
										AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
										return;
									}
								}
								break;
							}
							case 2:
							{
								for (int ix = 0; ix < 29; ix++)
								{
									if (_pck.ReadUInt8() != 220)
									{
										AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
										return;
									}
								}
								break;
							}
							default:
								AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
								return;
							}
							this.HWID = Utils.DECRYPT_HWID(_pck);
							continue;
						}
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
						if (_pck.Opcode == 28679)
						{
							if (!this.IsCharScreen)
							{
								MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #0");
								continue;
							}
							switch (_pck.ReadUInt8())
							{
							case 1:
								try
								{
									_pck.ReadAscii();
									_pck.ReadUInt32();
									_pck.ReadUInt8();
									_pck.ReadUInt32();
									_pck.ReadUInt32();
									_pck.ReadUInt32();
									_pck.ReadUInt32();
								}
								catch
								{
									MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #1");
									continue;
								}
								break;
							case 2:
								if (_pck.GetBytes().Length > 1)
								{
									MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #2");
									continue;
								}
								break;
							case 3:
							{
								int name_length3;
								name_length3 = _pck.ReadAscii().Length;
								if (_pck.GetBytes().Length - name_length3 != 3)
								{
									MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #3");
									continue;
								}
								break;
							}
							case 4:
							{
								int name_length2;
								name_length2 = _pck.ReadAscii().Length;
								if (_pck.GetBytes().Length - name_length2 != 3)
								{
									MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #4");
									continue;
								}
								break;
							}
							case 5:
							{
								int name_length;
								name_length = _pck.ReadAscii().Length;
								if (_pck.GetBytes().Length - name_length != 3)
								{
									MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #5");
									continue;
								}
								break;
							}
							default:
								MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #6");
								continue;
							}
						}
						else
						{
							if (_pck.Opcode == 28764)
							{
								if (this.CurLevel < MainMenu.GLOBAL_LEVEL)
								{
									this.SendMessage(3, MainMenu.GLOBAL_LEVEL_NOTICE.Replace("%level%", MainMenu.GLOBAL_LEVEL.ToString()));
									continue;
								}
								try
								{
									if (this.Dellay(this.LAST_GLOBAL_TIME, MainMenu.GLOBAL_DELAY, MainMenu.GLOBAL_DELAY_NOTICE))
									{
										continue;
									}
									this.LAST_GLOBAL_TIME = DateTime.Now;
									goto IL_0e72;
								}
								catch
								{
									goto IL_0e72;
								}
							}
							if (_pck.Opcode == 29776)
							{
								if (!this.IsCharScreen)
								{
									MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use a Shard exploit #12");
									continue;
								}
							}
							else if (_pck.Opcode == 29966)
							{
								if (_pck.GetBytes().Length != 0)
								{
									MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to modify 0x750E bytes.");
									if (MainMenu.FIREWALLBANCHECK)
									{
										Utils.BLOCK_IP(this.SOCKET_IP.Split(':')[0], "attempted to modify 0x750E bytes.");
										AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
									}
									else
									{
										AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
									}
									return;
								}
							}
							else if (_pck.Opcode == 28853)
							{
								if (_pck.GetBytes().Length != 0)
								{
									MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to modify 0x70b5 bytes.");
									if (MainMenu.FIREWALLBANCHECK)
									{
										Utils.BLOCK_IP(this.SOCKET_IP.Split(':')[0], "attempted to modify 0x70b5 bytes.");
										AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
									}
									else
									{
										AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
									}
									return;
								}
								if (this.STALLING)
								{
									MainMenu.WriteLine(1, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to use stall exploit!");
									AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
									return;
								}
							}
							else
							{
								if (_pck.Opcode == 8194)
								{
									if (_pck.GetBytes().Length != 0)
									{
										MainMenu.WriteLine(2, "0x2002 size problem, contact coder");
										continue;
									}
									if (this.isOnline)
									{
										try
										{
											if (Convert.ToInt32(DateTime.Now.Subtract(this.afk_timer).TotalSeconds) > MainMenu.AFKMS)
											{
												this.isAfk = true;
											}
											else
											{
												this.isAfk = false;
											}
										}
										catch
										{
										}
										if (this.isAfk)
										{
											Packet afk3;
											afk3 = new Packet(29698);
											afk3.WriteUInt8(2);
											base.REMOTE_SECURITY.Send(afk3);
											this.SendToModule();
											this.isAfkSymbol = true;
										}
									}
									base.REMOTE_SECURITY.Send(_pck);
									this.SendToModule();
									continue;
								}
								if (_pck.Opcode == 24835)
								{
									this.TOKEN_ID = _pck.ReadUInt32();
									this.StrUserID = _pck.ReadAscii();
									this.CHAR_PW = _pck.ReadAscii();
									_pck.ReadUInt8();
									string AG_MODULE_CONNECTED;
									AG_MODULE_CONNECTED = base.PROXY_SOCKET.RemoteEndPoint.ToString();
									this.CORRESPONDING_GW_SESSION = AsyncServer.DISPOSED_GW_SESSIONS.LastOrDefault((KeyValuePair<Socket, GatewayContext> s) => s.Value.TOKEN_ID == this.TOKEN_ID && s.Value.REDIR_IP == AG_MODULE_CONNECTED.Split(':')[0] && s.Value.REDIR_PORT == Convert.ToInt32(AG_MODULE_CONNECTED.Split(':')[1])).Value;
								}
								else
								{
									if (_pck.Opcode == 28839)
									{
										if (_pck.ReadUInt8() != 1)
										{
											MainMenu.WriteLine(1, "[" + this.SOCKET_IP.Split(':')[0] + "] detected zerk state exploit.");
											AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
											return;
										}
										if (Utils.BlockedZerkForRegion.Contains(this.CurRegion.ToString()))
										{
											this.SendMessage(3, "Cannot use zerk inside this region.");
											continue;
										}
										if (this.CurLevel < MainMenu.ZERK_LEVEL)
										{
											this.SendMessage(3, MainMenu.ZERK_LEVEL_NOTICE.Replace("%level%", MainMenu.ZERK_LEVEL.ToString()));
											continue;
										}
										try
										{
											if (this.Dellay(this.LAST_ZERK_TIME, MainMenu.ZERK_DELAY, MainMenu.ZERK_DELAY_NOTICE))
											{
												continue;
											}
											this.LAST_ZERK_TIME = DateTime.Now;
											goto IL_82f1;
										}
										catch
										{
											goto IL_82f1;
										}
									}
									if (_pck.Opcode == 13584)
									{
										if (MainMenu.FIREWALLBANCHECK)
										{
											Utils.BLOCK_IP(this.SOCKET_IP.Split(':')[0], "AgentServer Detected SR_GameServer crash exploit");
											AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
											MainMenu.WriteLine(1, "[" + this.SOCKET_IP + "] detected SR_GameServer crash exploit and has been banned via firewall.");
										}
										else
										{
											MainMenu.WriteLine(1, "[" + this.SOCKET_IP + "] disconnected detected SR_GameServer crash exploit.");
											AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
										}
										return;
									}
									if (_pck.Opcode == 13481)
									{
										if (!_pck.ReadAscii().ToLower().Contains("avatar"))
										{
											MainMenu.WriteLine(1, "[" + this.SOCKET_IP.Split(':')[0] + "] detected avatar magic option exploit");
											AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
											return;
										}
										if (MainMenu.ITEM_LOCK && this.has_code > 0)
										{
											this.SendMessage(3, MainMenu.ITEM_LOCK_DRESS_BLUE_NOTICE);
											continue;
										}
									}
									else
									{
										if (_pck.Opcode == 29698)
										{
											continue;
										}
										if (_pck.Opcode == 28673)
										{
											if (!this.IsCharScreen)
											{
												MainMenu.WriteLine(2, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to send 0x7001 outside char screen!");
												continue;
											}
											try
											{
												this.Charname = _pck.ReadAscii();
												this.CharID = Convert.ToInt32(Task.Run(async () => await sqlCon.Get_CharID_by_CharName16(this.Charname)).Result);
												this.Jobname = Convert.ToString(Task.Run(async () => await sqlCon.CharJobName(this.Charname)).Result);
												if (_pck.GetBytes().Length - this.Charname.Length != 2)
												{
													MainMenu.WriteLine(2, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to modify 0x7001!");
													continue;
												}
											}
											catch
											{
												MainMenu.WriteLine(2, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to modify 0x7001!");
												continue;
											}
											if (MainMenu.BanHWID.Contains(this.HWID))
											{
												AgentContext.SendLoginErrorMsg("Sorry but you have HWID ban.", 267649099u, base.CLIENT_SOCKET);
												AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
												continue;
											}
											if (MainMenu.BanStrUserID.Contains(this.StrUserID))
											{
												AgentContext.SendLoginErrorMsg("Sorry but you have UserID ban.", 267649099u, base.CLIENT_SOCKET);
												AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
												continue;
											}
											if (MainMenu.BanIPList.Contains(this.SOCKET_IP))
											{
												AgentContext.SendLoginErrorMsg("Sorry but you have IP ban.", 267649099u, base.CLIENT_SOCKET);
												AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
												continue;
											}
											if (MainMenu.PC_LIMIT > 0)
											{
												if (this.HWID == string.Empty)
												{
													string OldHwid;
													OldHwid = await sqlCon.ReturnHWID(this.CharID);
													if (OldHwid == string.Empty)
													{
														AgentContext.SendLoginErrorMsg("Our hwid dll is not found or corrupted.", 267649099u, base.CLIENT_SOCKET);
														AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
														continue;
													}
													this.HWID = OldHwid;
													if (AsyncServer.GetHWIDCount(this.HWID) > MainMenu.PC_LIMIT)
													{
														AgentContext.SendLoginErrorMsg("Sorry but you have reached PC limit.", 267649099u, base.CLIENT_SOCKET);
														AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
														continue;
													}
												}
												else if (AsyncServer.GetHWIDCount(this.HWID) > MainMenu.PC_LIMIT)
												{
													AgentContext.SendLoginErrorMsg("Sorry but you have reached PC limit.", 267649099u, base.CLIENT_SOCKET);
													AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
													return;
												}
											}
											base.REMOTE_SECURITY.Send(_pck);
											this.SendToModule();
											continue;
										}
										if (_pck.Opcode == 28677)
										{
											if (_pck.GetBytes().Length != 1)
											{
												MainMenu.WriteLine(2, "Client(" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to modify 0x7005");
												AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
												return;
											}
											switch (_pck.ReadUInt8())
											{
											case 2:
												if (this.Dellay(this.LAST_REST_TIME, MainMenu.RESTART_DELAY, MainMenu.RESTART_DELAY_NOTICE))
												{
													continue;
												}
												this.LAST_REST_TIME = DateTime.Now;
												break;
											case 1:
												if (this.Dellay(this.LAST_EXIT_TIME, MainMenu.EXIT_DELAY, MainMenu.EXIT_DELAY_NOTICE))
												{
													continue;
												}
												this.LAST_EXIT_TIME = DateTime.Now;
												break;
											default:
												MainMenu.WriteLine(2, "[agent ] IP:{" + this.SOCKET_IP + "} User:{" + this.StrUserID + "} Opcode:{Iwa0x" + _pck.Opcode.ToString("X") + "} Bytes:{" + _pck.GetBytes().Length + "} Packet_c{" + this.PACKET_COUNT + "}");
												AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
												return;
											}
											base.REMOTE_SECURITY.Send(_pck);
											this.SendToModule();
											continue;
										}
										if (_pck.Opcode == 28678)
										{
											if (_pck.GetBytes().Length != 0)
											{
												MainMenu.WriteLine(1, "[" + this.SOCKET_IP.Split(':')[0] + "] attempted to modify 0x7006 bytes.");
												AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
												return;
											}
										}
										else if (_pck.Opcode == 28870)
										{
											MainMenu.WriteLine(1, "aaa");
										}
										else if (_pck.Opcode == 28688)
										{
											_pck.ReadUInt16();
										}
										else
										{
											if (_pck.Opcode == 28849)
											{
												_pck.ReadAscii();
												try
												{
													if (this.Dellay(this.LAST_STALL_TIME, MainMenu.STALL_DELAY, MainMenu.ZERK_DELAY_NOTICE))
													{
														continue;
													}
													this.LAST_STALL_TIME = DateTime.Now;
													goto IL_22de;
												}
												catch
												{
													goto IL_22de;
												}
											}
											if (_pck.Opcode == 28858)
											{
												this.STALLING = true;
											}
											else if (_pck.Opcode == 28850)
											{
												this.STALLING = false;
											}
											else
											{
												if (_pck.Opcode == 28915)
												{
													try
													{
														if (this.Dellay(this.LAST_GUILDREQ_TIME, MainMenu.GUILD_DELAY, MainMenu.GUILD_DELAY_NOTICE))
														{
															continue;
														}
														this.LAST_GUILDREQ_TIME = DateTime.Now;
														goto IL_82f1;
													}
													catch
													{
														goto IL_82f1;
													}
												}
												if (_pck.Opcode == 28923)
												{
													try
													{
														if (this.Dellay(this.LAST_UNIREQ_TIME, MainMenu.UNION_DELAY, MainMenu.UNION_DELAY_NOTICE))
														{
															continue;
														}
														this.LAST_UNIREQ_TIME = DateTime.Now;
														goto IL_82f1;
													}
													catch
													{
														goto IL_82f1;
													}
												}
												if (_pck.Opcode == 28834)
												{
													_pck.ReadUInt32();
													if (_pck.ReadUInt8() != 1)
													{
														MainMenu.WriteLine(1, "[" + this.SOCKET_IP + "] disconnected detected mastery exploit.");
														AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
														return;
													}
												}
												else
												{
													if (_pck.Opcode == 28801)
													{
														try
														{
															if (this.Dellay(this.LAST_EXREQ_TIME, MainMenu.EXCHANGE_DELAY, MainMenu.EXCHANGE_DELAY_NOTICE))
															{
																continue;
															}
															this.LAST_EXREQ_TIME = DateTime.Now;
															goto IL_25c5;
														}
														catch
														{
															goto IL_25c5;
														}
													}
													if (_pck.Opcode != 29808 && _pck.Opcode != 29810 && _pck.Opcode != 29822)
													{
														if (_pck.Opcode == 29907)
														{
															switch (_pck.ReadUInt8())
															{
															}
															if (this.CurLevel < MainMenu.BA_REQ_LEVEL)
															{
																this.SendMessage(3, MainMenu.BA_REQ_LEVEL_NOTICE.Replace("%level%", MainMenu.BA_REQ_LEVEL.ToString()));
																continue;
															}
														}
														else if (_pck.Opcode == 29874)
														{
															if (this.CurLevel < MainMenu.CTF_REQ_LEVEL)
															{
																this.SendMessage(3, MainMenu.CTF_REQ_LEVEL_NOTICE.Replace("%level%", MainMenu.CTF_REQ_LEVEL.ToString()));
																continue;
															}
														}
														else if (_pck.Opcode == 28788)
														{
															if (_pck.ReadUInt8() == 1)
															{
																byte num2;
																num2 = _pck.ReadUInt8();
																if (this.INSIDE_LMS && MainMenu.LMS_ATTACK_ON_OFF)
																{
																	this.SendMessage(3, "Usage of this skill not allowed this arena now.");
																	continue;
																}
																if (this.INSIDE_SURVIVAL && MainMenu.SURV_ATTACK_ON_OFF)
																{
																	this.SendMessage(3, "Usage of this skill not allowed this arena now.");
																	continue;
																}
																switch (num2)
																{
																case 4:
																{
																	Thread.Sleep(100);
																	ushort SkillID;
																	SkillID = _pck.ReadUInt16();
																	_pck.ReadUInt16();
																	if (Utils.RegionSkillBlock.ContainsKey(SkillID.ToString()) && Utils.RegionSkillBlock[SkillID.ToString()].ToString() == this.CurRegion.ToString())
																	{
																		this.SendMessage(3, "You cant use this Skill inside this region");
																		continue;
																	}
																	if (this.INSIDE_LMS && MainMenu.LMS_ATTACK_ON_OFF)
																	{
																		this.SendMessage(3, "Usage of this skill not allowed this arena now.");
																		continue;
																	}
																	break;
																}
																}
															}
														}
														else
														{
															if (_pck.Opcode == 4889)
															{
																int UniqueID2;
																UniqueID2 = _pck.ReadInt32();
																byte TelType2;
																TelType2 = _pck.ReadUInt8();
																int targetTeleport3;
																targetTeleport3 = _pck.ReadInt32();
																Packet teleport2;
																teleport2 = new Packet(28741, encrypted: true);
																teleport2.WriteInt32(UniqueID2);
																base.REMOTE_SECURITY.Send(teleport2);
																this.SendToModule();
																Packet teleport;
																teleport = new Packet(28762, encrypted: true);
																teleport.WriteInt32(UniqueID2);
																teleport.WriteInt8(TelType2);
																teleport.WriteInt32(targetTeleport3);
																base.REMOTE_SECURITY.Send(teleport);
																this.SendToModule();
																continue;
															}
															if (_pck.Opcode == 28762)
															{
																int UniqueID;
																UniqueID = _pck.ReadInt32();
																byte TelType;
																TelType = _pck.ReadUInt8();
																if (this.JobType != 4 && TelType == 2)
																{
																	int targetTeleport2;
																	targetTeleport2 = _pck.ReadInt32();
																	Packet afk4;
																	afk4 = new Packet(28747);
																	afk4.WriteUInt8(1);
																	base.REMOTE_SECURITY.Send(afk4);
																	this.SendToModule();
																	Packet afk2;
																	afk2 = new Packet(20801);
																	afk2.WriteInt32(UniqueID);
																	afk2.WriteUInt8(TelType);
																	afk2.WriteInt32(targetTeleport2);
																	base.LOCAL_SECURITY.Send(afk2);
																	this.SendToModule();
																	continue;
																}
																if (TelType == 2)
																{
																	int targetTeleport;
																	targetTeleport = _pck.ReadInt32();
																	if (targetTeleport == MainMenu.LMS_GATEID)
																	{
																		if (!MainMenu.LMS_GATE_OPENCLOSE)
																		{
																			this.SendMessage(3, "Gate will be open in the event time.");
																			continue;
																		}
																		if (!MainMenu.LMS_PLAYERSLIST.Contains(this.Charname))
																		{
																			this.SendMessage(3, "You are not registered LMS");
																			continue;
																		}
																		if (this.INSIDE_PT)
																		{
																			this.SendMessage(3, "Leave party for join event!");
																			continue;
																		}
																		if (this.JobType != 4)
																		{
																			this.SendMessage(3, "Out your job suit.");
																			continue;
																		}
																		if (this.CurLevel < Convert.ToInt32(MainMenu.LMS_REQUIRELEVEL))
																		{
																			this.SendMessage(3, MainMenu.LMS_REQUIRELEVEL_NOTICE);
																			continue;
																		}
																		if (MainMenu.LMS_PCLIMIT > 0 && AsyncServer.GetHwidCountLMS(this.HWID) + 1 > MainMenu.LMS_PCLIMIT && !this.INSIDE_LMS)
																		{
																			this.SendMessage(3, "You have reached the maxmimum allowed PC(s) to enter the Last Man Standing Arena.");
																			continue;
																		}
																	}
																	if (targetTeleport == MainMenu.SURV_GATEID)
																	{
																		if (!MainMenu.SURV_GATE_OPENCLOSE)
																		{
																			this.SendMessage(3, "Gate will be open in the event time.");
																			continue;
																		}
																		if (!MainMenu.SURV_PLAYERSLIST.Contains(this.Charname))
																		{
																			this.SendMessage(3, "You are not registered LMS");
																			continue;
																		}
																		if (this.INSIDE_PT)
																		{
																			this.SendMessage(3, "Leave party for join event!");
																			continue;
																		}
																		if (this.JobType != 4)
																		{
																			this.SendMessage(3, "Out your job suit.");
																			continue;
																		}
																		if (this.CurLevel < Convert.ToInt32(MainMenu.SURV_REQUIRELEVEL))
																		{
																			this.SendMessage(3, MainMenu.SURV_REQUIRELEVEL_NOTICE);
																			continue;
																		}
																		if (MainMenu.SURVIVAL_PC_LIMIT > 0 && AsyncServer.GetHwidCountLMS(this.HWID) + 1 > MainMenu.SURVIVAL_PC_LIMIT && !this.INSIDE_SURVIVAL)
																		{
																			this.SendMessage(2, "You have reached the maxmimum allowed PC(s) to enter the Last Man Standing Arena.");
																			continue;
																		}
																	}
																}
															}
															else if (_pck.Opcode != 12416)
															{
																if (_pck.Opcode == 28763)
																{
																	this.inreturn = false;
																}
																else if (_pck.Opcode == 28742)
																{
																	this.innpc = true;
																}
																else if (_pck.Opcode == 28747)
																{
																	this.innpc = false;
																}
																else
																{
																	if (_pck.Opcode == 28748)
																	{
																		uint num;
																		num = _pck.ReadUInt8();
																		uint value;
																		value = _pck.ReadUInt16();
																		switch (value)
																		{
																		case 6636u:
																		case 6637u:
																			if (this.INSIDE_SURVIVAL)
																			{
																				this.SendMessage(3, "Blocked action in this region.");
																				continue;
																			}
																			if (this.INSIDE_LMS)
																			{
																				this.SendMessage(3, "Blocked action in this region.");
																				continue;
																			}
																			switch (_pck.ReadUInt8())
																			{
																			case 2:
																			{
																				int tellregion;
																				tellregion = await sqlCon.Get_TelRegionID_by_CharIDD(this.CharID);
																				if (tellregion == MainMenu.LMS_REGIONID || this.INSIDE_LMS)
																				{
																					this.SendMessage(3, "You cannot teleport this region.");
																					continue;
																				}
																				if (tellregion == MainMenu.SURV_REGIONID || this.INSIDE_SURVIVAL)
																				{
																					this.SendMessage(3, "You cannot teleport this region.");
																					continue;
																				}
																				if (Utils.BlockedReverseRegion.Contains(tellregion.ToString()))
																				{
																					this.SendMessage(3, "You are not allowed to use reverse scroll to previous death point inside this region");
																					continue;
																				}
																				break;
																			}
																			case 3:
																			{
																				int deaddregion;
																				deaddregion = await sqlCon.Get_DeadRegionID_by_CharIDD(this.CharID);
																				if (deaddregion == MainMenu.LMS_REGIONID || this.INSIDE_LMS)
																				{
																					this.SendMessage(3, "You cannot teleport this region.");
																					continue;
																				}
																				if (Utils.BlockedReverseRegion.Contains(deaddregion.ToString()))
																				{
																					this.SendMessage(3, "You are not allowed to use reverse scroll to previous death point inside this region");
																					continue;
																				}
																				if (deaddregion == MainMenu.SURV_REGIONID || this.INSIDE_SURVIVAL)
																				{
																					this.SendMessage(3, "You cannot teleport this region.");
																					continue;
																				}
																				break;
																			}
																			}
																			if (MainMenu.JobBlockReversed)
																			{
																				try
																				{
																					if (this.JobType != 4)
																					{
																						this.SendMessage(3, "You cant use reverse while job mode.");
																						continue;
																					}
																				}
																				catch
																				{
																					continue;
																				}
																			}
																			try
																			{
																				if (this.Dellay(this.LAST_REVERSE_TIME, MainMenu.REVERSE_DELAY, MainMenu.REVERSE_DELAY_NOTICE))
																				{
																					continue;
																				}
																				this.LAST_REVERSE_TIME = DateTime.Now;
																				break;
																			}
																			catch
																			{
																				break;
																			}
																		case 2540u:
																		case 2541u:
																			this.inreturn = true;
																			if (MainMenu.SURV_WAR_START && this.INSIDE_SURVIVAL)
																			{
																				this.SendMessage(1, "Action Blocked! You can't use return scroll while Event running!");
																				continue;
																			}
																			if (MainMenu.LMS_WAR_START && this.INSIDE_LMS)
																			{
																				this.SendMessage(1, "Action Blocked! You can't use return scroll while Event running!");
																				continue;
																			}
																			break;
																		case 10732u:
																		case 10733u:
																			_pck.ReadAscii();
																			if (this.CurLevel < MainMenu.GLOBAL_LEVEL)
																			{
																				this.SendMessage(3, MainMenu.GLOBAL_LEVEL_NOTICE.Replace("%level%", MainMenu.GLOBAL_LEVEL.ToString()));
																				this.global = false;
																				continue;
																			}
																			try
																			{
																				if (this.Dellay(this.LAST_GLOBAL_TIME, MainMenu.GLOBAL_DELAY, MainMenu.GLOBAL_DELAY_NOTICE))
																				{
																					this.global = false;
																					continue;
																				}
																				this.LAST_GLOBAL_TIME = DateTime.Now;
																				goto IL_34cc;
																			}
																			catch
																			{
																				goto IL_34cc;
																			}
																		case 4588u:
																		case 4589u:
																			try
																			{
																				if (this.Dellay(this.LAST_SPAWN_TIME, 5, "You must wait {Time}s to continue"))
																				{
																					continue;
																				}
																				this.LAST_SPAWN_TIME = DateTime.Now;
																				break;
																			}
																			catch
																			{
																				break;
																			}
																		case 24301u:
																			{
																				slot2 = _pck.ReadUInt8();
																				string Codename;
																				Codename = Task.Run(async () => await sqlCon.prod_string($"EXEC {MainMenu.FILTER_DB}.._UseModelandGlowItems '{this.Charname}','{num}','{slot2}'")).Result;
																				if (Codename != "false")
																				{
																					AgentContext.UpgradeItem(this.Charname, slot2, Codename, num, value);
																				}
																				else
																				{
																					this.SendMessage(3, "This item is not suitable");
																				}
																				continue;
																			}
																			IL_34cc:
																			this.global = true;
																			break;
																		}
																		base.REMOTE_SECURITY.Send(_pck);
																		this.SendToModule();
																		continue;
																	}
																	if (_pck.Opcode == 28875)
																	{
																		_pck.ReadUInt8();
																		_pck.ReadUInt32();
																	}
																	else if (_pck.Opcode == 28709)
																	{
																		byte Chat_type;
																		Chat_type = _pck.ReadUInt8();
																		_pck.ReadUInt8();
																		_pck.ReadAscii();
																		switch (Chat_type)
																		{
																		}
																	}
																	else if (_pck.Opcode == 29449)
																	{
																		_pck.ReadAscii();
																		_pck.ReadAscii().ToLower();
																	}
																	else if (_pck.Opcode == 28777)
																	{
																		if (_pck.GetBytes().Length >= 13)
																		{
																			_pck.ReadUInt32();
																			_pck.ReadUInt32();
																			_pck.ReadUInt8();
																			_pck.ReadUInt8();
																			_pck.ReadUInt8();
																			_pck.ReadUInt8();
																			_pck.ReadAscii().ToLower();
																			if (MainMenu.DisablePartyRegion.Contains(this.CurRegion.ToString()))
																			{
																				this.SendMessage(3, "Cant create party in this region.");
																				continue;
																			}
																			if (this.INSIDE_LMS)
																			{
																				this.SendMessage(3, "Cant create party in this region.");
																				continue;
																			}
																			if (this.INSIDE_SURVIVAL)
																			{
																				this.SendMessage(3, "Cant create party in this region.");
																				continue;
																			}
																		}
																	}
																	else if (_pck.Opcode == 28768)
																	{
																		if (MainMenu.DisablePartyRegion.Contains(this.CurRegion.ToString()))
																		{
																			this.SendMessage(3, "You can't party in this area.");
																			continue;
																		}
																		if (this.INSIDE_LMS)
																		{
																			this.SendMessage(3, "Cant create party in this region.");
																			continue;
																		}
																		if (this.INSIDE_SURVIVAL)
																		{
																			this.SendMessage(3, "Cant create party in this region.");
																			continue;
																		}
																	}
																	else if (_pck.Opcode == 28781)
																	{
																		if (MainMenu.DisablePartyRegion.Contains(this.CurRegion.ToString()))
																		{
																			this.SendMessage(3, "You can't party in this area.");
																			continue;
																		}
																		if (this.INSIDE_LMS)
																		{
																			this.SendMessage(3, "Cant create party in this region.");
																			continue;
																		}
																		if (this.INSIDE_SURVIVAL)
																		{
																			this.SendMessage(3, "Cant create party in this region.");
																			continue;
																		}
																	}
																	else if (_pck.Opcode == 28724)
																	{
																		if (_pck.GetBytes().Length >= 1)
																		{
																			int type;
																			type = _pck.ReadUInt8();
																			if (type == 10 && this.CurLevel <= MainMenu.DROP_GOLD_LEVEL)
																			{
																				this.SendMessage(3, "Insufficient level. You need to be level 110 to do this.");
																				continue;
																			}
																			if (_pck.GetBytes().Length == 5)
																			{
																				_pck.ReadUInt8();
																				if (_pck.ReadUInt8() == 8 && MainMenu.JOB_PC_LIMIT > 0)
																				{
																					this.JOB_YELLOW_LINE = true;
																					if (AsyncServer.GetHWIDCountJob(this.HWID) > MainMenu.JOB_PC_LIMIT)
																					{
																						this.JOB_YELLOW_LINE = false;
																						this.SendMessage(3, MainMenu.JOB_PC_LIMIT_NOTICE);
																						continue;
																					}
																				}
																			}
																			if (MainMenu.ITEM_LOCK && this.has_code > 0 && (type == 9 || type == 7 || type == 10))
																			{
																				this.SendMessage(1, MainMenu.ITEM_LOCK_DROP_SELL_GOLD);
																				continue;
																			}
																		}
																	}
																	else if (_pck.Opcode != 28901)
																	{
																		if (_pck.Opcode == 28766)
																		{
																			if (this.INSIDE_LMS && MainMenu.LMS_WAR_START)
																			{
																				Packet leave_party2;
																				leave_party2 = new Packet(28769);
																				base.REMOTE_SECURITY.Send(leave_party2);
																				this.SendToModule();
																			}
																			if (this.INSIDE_SURVIVAL && MainMenu.SURV_WAR_START)
																			{
																				Packet leave_party;
																				leave_party = new Packet(28769);
																				base.REMOTE_SECURITY.Send(leave_party);
																				this.SendToModule();
																			}
																			int pl;
																			pl = _pck.GetBytes().Length;
																			if (pl == 10)
																			{
																				_pck.ReadUInt8();
																				_pck.ReadUInt8();
																				_pck.ReadUInt8();
																				_pck.ReadUInt8();
																				byte reg_or_cancel_flag;
																				reg_or_cancel_flag = _pck.ReadUInt8();
																				_pck.ReadUInt8();
																				if (reg_or_cancel_flag != 7)
																				{
																				}
																			}
																			else if (pl >= 11)
																			{
																				_pck.ReadUInt8();
																				_pck.ReadUInt8();
																				_pck.ReadUInt8();
																				_pck.ReadUInt8();
																				_pck.ReadUInt8();
																				_pck.ReadUInt8();
																				_pck.ReadUInt8();
																				_pck.ReadUInt8();
																				_pck.ReadUInt8();
																				_pck.ReadAscii();
																			}
																		}
																		else if (_pck.Opcode != 28705)
																		{
																			if (_pck.Opcode == 29008)
																			{
																				if (MainMenu.ITEM_LOCK && this.has_code > 0)
																				{
																					this.SendMessage(3, MainMenu.ITEM_LOCK_ALCHEMY_NOTICE);
																					continue;
																				}
																				if (_pck.ReadUInt8() == 2 && _pck.ReadUInt8() == 3)
																				{
																					_pck.ReadUInt8();
																					int slot;
																					slot = _pck.ReadUInt8();
																					try
																					{
																						int itemid;
																						itemid = Convert.ToInt32(Task.Run(async () => await sqlCon.GetRefItemID(this.Charname, slot.ToString())).Result);
																						int plus;
																						plus = Convert.ToInt32(Task.Run(async () => await sqlCon.GetOptLevel(this.Charname, slot.ToString())).Result);
																						byte TypeID1;
																						TypeID1 = Utils.RefObjCommon[Convert.ToUInt32(itemid)].TypeID1;
																						byte TypeID2;
																						TypeID2 = Utils.RefObjCommon[Convert.ToUInt32(itemid)].TypeID2;
																						byte TypeID3;
																						TypeID3 = Utils.RefObjCommon[Convert.ToUInt32(itemid)].TypeID3;
																						if (TypeID1 == 3 && TypeID2 == 1 && TypeID3 == 14)
																						{
																							if (plus >= MainMenu.DEVIL_PLUS_LIMIT)
																							{
																								this.SendMessage(3, MainMenu.DEVIL_PLUS_LIMIT_NOTICE);
																								continue;
																							}
																						}
																						else if (plus >= MainMenu.PLUS_LIMIT)
																						{
																							this.SendMessage(3, MainMenu.PLUS_LIMIT_NOTICE);
																							continue;
																						}
																					}
																					catch
																					{
																						continue;
																					}
																				}
																			}
																			else if (_pck.Opcode == 29009)
																			{
																				if (MainMenu.ITEM_LOCK && this.has_code > 0)
																				{
																					this.SendMessage(3, MainMenu.ITEM_LOCK_ALCHEMY_NOTICE);
																					continue;
																				}
																			}
																			else if (_pck.Opcode == 29015)
																			{
																				if (MainMenu.ITEM_LOCK && this.has_code > 0)
																				{
																					this.SendMessage(3, MainMenu.ITEM_LOCK_ALCHEMY_NOTICE);
																					continue;
																				}
																			}
																			else
																			{
																				if (_pck.Opcode == 28869)
																				{
																					this.afk_timer = DateTime.Now;
																					this.isAfk = false;
																					if (this.isAfkSymbol)
																					{
																						Packet afk;
																						afk = new Packet(29698);
																						afk.WriteUInt8(0);
																						base.REMOTE_SECURITY.Send(afk);
																						this.SendToModule();
																						this.isAfkSymbol = false;
																					}
																					base.REMOTE_SECURITY.Send(_pck);
																					this.SendToModule();
																					continue;
																				}
																				if (_pck.Opcode == 29974)
																				{
																					_pck.ReadUInt8();
																				}
																				else if (_pck.Opcode == 28897)
																				{
																					if (this.CurLevel < MainMenu.JOB_PC_LIMIT)
																					{
																						this.SendMessage(3, MainMenu.JOB_PC_LIMIT_NOTICE.Replace("%level%", MainMenu.JOB_PC_LIMIT.ToString()));
																						continue;
																					}
																				}
																				else if (_pck.Opcode == 29270)
																				{
																					_pck.ReadUInt32();
																					string grant_name;
																					grant_name = _pck.ReadAscii().ToString().ToLower();
																					if (grant_name.Contains("'") || grant_name.Contains("\""))
																					{
																						continue;
																					}
																				}
																				else if (_pck.Opcode == 28802 || _pck.Opcode == 28803)
																				{
																					if (MainMenu.ITEM_LOCK && this.has_code > 0)
																					{
																						this.SendMessage(3, MainMenu.ITEM_LOCK_EXCHANGE_NOTICE);
																						continue;
																					}
																				}
																				else if (_pck.Opcode == 29272)
																				{
																					if (MainMenu.ITEM_LOCK && this.has_code > 0)
																					{
																						this.SendMessage(3, MainMenu.ITEM_LOCK_IS_GUILD_SP);
																						continue;
																					}
																				}
																				else if (_pck.Opcode == 29266)
																				{
																					if (MainMenu.ITEM_LOCK && this.has_code > 0)
																					{
																						this.SendMessage(3, MainMenu.ITEM_LOCK_IS_GUILD_NOTICE);
																						continue;
																					}
																				}
																				else
																				{
																					if (_pck.Opcode == 13664)
																					{
																						this.JobType = _pck.ReadUInt8();
																						this.CurRegion = _pck.ReadInt16();
																						this.WorldID = _pck.ReadInt16();
																						if (this.CurRegion == -32759)
																						{
																							if (this.WorldID == 29)
																							{
																								this.INSIDE_CTF = true;
																							}
																							else
																							{
																								this.INSIDE_CTF = false;
																							}
																						}
																						if (this.JobType == 4)
																						{
																							this.JOB_FLAG = false;
																						}
																						else
																						{
																							this.JOB_FLAG = true;
																						}
																						continue;
																					}
																					if (_pck.Opcode != 28833)
																					{
																						if (_pck.Opcode == 13568)
																						{
																							MainMenu.WriteLine(1, "Someone trying to send filter msg which is supposed to be sent by GS addon. Remote address [" + this.SOCKET_IP + "].");
																							AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, AsyncServer.MODULE_TYPE.AgentServer);
																						}
																						else
																						{
																							if (_pck.Opcode == 13573)
																							{
																								await sqlCon.UpdateCharChest(this);
																								if (_pck.ReadUInt32() != this.UNIQUE_ID)
																								{
																									continue;
																								}
																								string CodeName;
																								CodeName = _pck.ReadAscii();
																								_CharChest _CharChest;
																								_CharChest = null;
																								foreach (_CharChest chest2 in this.CharChest)
																								{
																									if (chest2.ChestID.ToString() == CodeName)
																									{
																										_CharChest = chest2;
																										break;
																									}
																								}
																								if (_CharChest == null)
																								{
																									continue;
																								}
																								await sqlCon.EXEC_QUERY($"DELETE FROM {MainMenu.FILTER_DB}.dbo._CharChest where ID='{_CharChest.ChestID}'");
																								this.LiveItem(_CharChest.CodeName, _CharChest.Count, _CharChest.OptLevel, _CharChest.RandomizedStats, _CharChest.TypeStr);
																								foreach (_CharChest chest in this.CharChest)
																								{
																									if (chest.ChestID.ToString() == CodeName)
																									{
																										this.CharChest.Remove(chest);
																										break;
																									}
																								}
																								await Task.Delay(300);
																								await sqlCon.UpdateCharChest(this);
																								this.LoadChest();
																								continue;
																							}
																							if (_pck.Opcode == 6154)
																							{
																								switch (_pck.ReadUInt8())
																								{
																								case 1:
																								{
																									if (this.CharTitles.Count > 0)
																									{
																										Packet Title2;
																										Title2 = new Packet(20737);
																										Title2.WriteUInt8(this.CharTitles.Count);
																										foreach (KeyValuePair<byte, string> line2 in this.CharTitles.OrderBy((KeyValuePair<byte, string> x) => x.Key))
																										{
																											Title2.WriteUInt8(line2.Key);
																											Title2.WriteAscii(line2.Value);
																										}
																										base.LOCAL_SECURITY.Send(Title2);
																										this.SendToClient();
																									}
																									if (this.CharTitlesNew.Count <= 0)
																									{
																										break;
																									}
																									Packet Infonew;
																									Infonew = new Packet(20738);
																									Infonew.WriteUInt8(this.CharTitlesNew.Count);
																									foreach (KeyValuePair<byte, string> line in this.CharTitlesNew.OrderBy((KeyValuePair<byte, string> x) => x.Key))
																									{
																										Infonew.WriteUInt8(line.Key);
																										Infonew.WriteAscii(line.Value);
																									}
																									base.LOCAL_SECURITY.Send(Infonew);
																									this.SendToClient();
																									break;
																								}
																								case 2:
																									this.AnalyzerPacket();
																									break;
																								case 3:
																									this.LoadEventSchedule();
																									break;
																								case 5:
																									this.AttendanceEvent();
																									break;
																								case 6:
																									this.LoadDynamicRanking(1);
																									break;
																								case 7:
																									this.LoadDynamicRanking(2);
																									break;
																								case 8:
																									this.LoadDynamicRanking(3);
																									break;
																								case 9:
																									this.LoadDynamicRanking(4);
																									break;
																								case 10:
																									this.LoadDynamicRanking(5);
																									break;
																								case 11:
																									if (!MainMenu.LMS_REGISTERSTATUS)
																									{
																										this.RegisterStatus(0);
																									}
																									else
																									{
																										this.RegisterStatus(1);
																									}
																									if (!MainMenu.SURV_REGISTERSTATUS)
																									{
																										this.RegisterStatus(2);
																									}
																									else
																									{
																										this.RegisterStatus(3);
																									}
																									if (MainMenu.LG_REGISTER_STATUS)
																									{
																										this.RegisterStatus(4);
																									}
																									else
																									{
																										this.RegisterStatus(4);
																									}
																									break;
																								case 12:
																									this.LoadCustomNpc();
																									this.TokenPacket();
																									break;
																								case 13:
																									this.LoadCustomNpcTitleColors();
																									break;
																								case 14:
																									this.LoadCustomNpcNameColors();
																									break;
																								}
																								continue;
																							}
																							if (_pck.Opcode == 8250)
																							{
																								byte Type2;
																								Type2 = _pck.ReadUInt8();
																								switch (Type2)
																								{
																								case 1:
																								{
																									string IconIDs;
																									IconIDs = _pck.ReadUnicode();
																									int.TryParse(_pck.ReadUnicode().ToString(), out var GuiFiyat);
																									if (MainMenu.EnableMarketToken)
																									{
																										if (this.CharToken >= GuiFiyat)
																										{
																											int.TryParse(IconIDs, out var IconID3);
																											if (!Utils.CharIconLeft.ContainsKey(this.Charname))
																											{
																												Utils.CharIconLeft.TryAdd(this.Charname, IconID3);
																											}
																											else
																											{
																												Utils.CharIconLeft[this.Charname] = IconID3;
																											}
																											await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._UpdateIcons '{this.Charname}', '{IconID3}'");
																											AgentContext.SendPackagetoAll(AgentContext.UpdateLeftIcons(this.Charname, IconID3));
																											this.CharToken -= GuiFiyat;
																											await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._UpdateTokens '{this.Charname}', {this.CharToken}");
																											this.TokenPacket();
																										}
																										else
																										{
																											this.SendMessage(3, "Check to payment informations.");
																										}
																									}
																									if (MainMenu.EnableMarketSilk)
																									{
																										if (this.CharSilk >= GuiFiyat)
																										{
																											int.TryParse(IconIDs, out var IconID2);
																											if (!Utils.CharIconLeft.ContainsKey(this.Charname))
																											{
																												Utils.CharIconLeft.TryAdd(this.Charname, IconID2);
																											}
																											else
																											{
																												Utils.CharIconLeft[this.Charname] = IconID2;
																											}
																											await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._UpdateIcons '{this.Charname}', '{IconID2}'");
																											AgentContext.SendPackagetoAll(AgentContext.UpdateLeftIcons(this.Charname, IconID2));
																											await sqlCon.GIVE_SILK(this.StrUserID, -1 * GuiFiyat);
																										}
																										else
																										{
																											this.SendMessage(3, "Check to payment informations.");
																										}
																									}
																									if (MainMenu.EnableMarketGold)
																									{
																										int.TryParse(IconIDs, out var IconID);
																										if (!Utils.CharIconLeft.ContainsKey(this.Charname))
																										{
																											Utils.CharIconLeft.TryAdd(this.Charname, IconID);
																										}
																										else
																										{
																											Utils.CharIconLeft[this.Charname] = IconID;
																										}
																										AgentContext.ReduceGold(this.Charname, 10, GuiFiyat);
																										AgentContext.CharIcon = IconID;
																									}
																									continue;
																								}
																								case 2:
																								{
																									uint TitleColor;
																									TitleColor = _pck.ReadUInt32();
																									int.TryParse(_pck.ReadUnicode().ToString(), out var GuiFiyat2);
																									if (MainMenu.EnableMarketToken)
																									{
																										if (this.CharToken >= GuiFiyat2)
																										{
																											if (!Utils.TitleColorList.ContainsKey(this.Charname))
																											{
																												Utils.TitleColorList.TryAdd(this.Charname, TitleColor);
																											}
																											else
																											{
																												Utils.TitleColorList[this.Charname] = TitleColor;
																											}
																											string HEXColor3;
																											HEXColor3 = "#" + TitleColor.ToString("X");
																											await sqlCon.EXEC_QUERY("EXEC " + MainMenu.FILTER_DB + ".._UpdateTitleColorNpc '" + this.Charname + "', '" + HEXColor3 + "'");
																											AgentContext.SendPackagetoAll(AgentContext.UpdateTitleColor(this.Charname, TitleColor));
																											this.CharToken -= GuiFiyat2;
																											await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._UpdateTokens '{this.Charname}', {this.CharToken}");
																											this.TokenPacket();
																										}
																										else
																										{
																											this.SendMessage(3, "Check to payment informations.");
																										}
																									}
																									if (MainMenu.EnableMarketSilk)
																									{
																										if (this.CharSilk >= GuiFiyat2)
																										{
																											if (!Utils.TitleColorList.ContainsKey(this.Charname))
																											{
																												Utils.TitleColorList.TryAdd(this.Charname, TitleColor);
																											}
																											else
																											{
																												Utils.TitleColorList[this.Charname] = TitleColor;
																											}
																											string HEXColor2;
																											HEXColor2 = "#" + TitleColor.ToString("X");
																											await sqlCon.EXEC_QUERY("EXEC " + MainMenu.FILTER_DB + ".._UpdateTitleColorNpc '" + this.Charname + "', '" + HEXColor2 + "'");
																											AgentContext.SendPackagetoAll(AgentContext.UpdateTitleColor(this.Charname, TitleColor));
																											this.CharSilk -= GuiFiyat2;
																											this.CharSilk -= GuiFiyat2;
																											await sqlCon.GIVE_SILK(this.StrUserID, -1 * GuiFiyat2);
																										}
																										else
																										{
																											this.SendMessage(3, "Check to payment informations.");
																										}
																									}
																									if (MainMenu.EnableMarketGold)
																									{
																										if (!Utils.TitleColorList.ContainsKey(this.Charname))
																										{
																											Utils.TitleColorList.TryAdd(this.Charname, TitleColor);
																										}
																										else
																										{
																											Utils.TitleColorList[this.Charname] = TitleColor;
																										}
																										string HEXColor;
																										HEXColor = "#" + TitleColor.ToString("X");
																										AgentContext.ReduceGold(this.Charname, 11, GuiFiyat2);
																										this.TitleHEXColor = HEXColor;
																									}
																									continue;
																								}
																								case 3:
																								{
																									uint NameColor;
																									NameColor = _pck.ReadUInt32();
																									int.TryParse(_pck.ReadUnicode().ToString(), out var GuiFiyat3);
																									if (MainMenu.EnableMarketToken)
																									{
																										if (this.CharToken >= GuiFiyat3)
																										{
																											if (!Utils.CharNameColorList.ContainsKey(this.Charname))
																											{
																												Utils.CharNameColorList.TryAdd(this.Charname, NameColor);
																											}
																											else
																											{
																												Utils.CharNameColorList[this.Charname] = NameColor;
																											}
																											string HEXColor6;
																											HEXColor6 = "#" + NameColor.ToString("X");
																											await sqlCon.EXEC_QUERY("EXEC " + MainMenu.FILTER_DB + ".._UpdateCharacterNameColorNpc '" + this.Charname + "', '" + HEXColor6 + "'");
																											AgentContext.SendPackagetoAll(AgentContext.UpdateCharNameColor(this.Charname, NameColor));
																											this.CharToken -= GuiFiyat3;
																											await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._UpdateTokens '{this.Charname}', {this.CharToken}");
																											this.TokenPacket();
																										}
																										else
																										{
																											this.SendMessage(3, "Check to payment informations.");
																										}
																									}
																									if (MainMenu.EnableMarketSilk)
																									{
																										if (this.CharSilk >= GuiFiyat3)
																										{
																											if (!Utils.CharNameColorList.ContainsKey(this.Charname))
																											{
																												Utils.CharNameColorList.TryAdd(this.Charname, NameColor);
																											}
																											else
																											{
																												Utils.CharNameColorList[this.Charname] = NameColor;
																											}
																											string HEXColor5;
																											HEXColor5 = "#" + NameColor.ToString("X");
																											await sqlCon.EXEC_QUERY("EXEC " + MainMenu.FILTER_DB + ".._UpdateCharacterNameColorNpc '" + this.Charname + "', '" + HEXColor5 + "'");
																											AgentContext.SendPackagetoAll(AgentContext.UpdateCharNameColor(this.Charname, NameColor));
																											this.CharSilk -= GuiFiyat3;
																											await sqlCon.GIVE_SILK(this.StrUserID, -1 * GuiFiyat3);
																										}
																										else
																										{
																											this.SendMessage(3, "Check to payment informations.");
																										}
																									}
																									if (MainMenu.EnableMarketGold)
																									{
																										if (!Utils.CharNameColorList.ContainsKey(this.Charname))
																										{
																											Utils.CharNameColorList.TryAdd(this.Charname, NameColor);
																										}
																										else
																										{
																											Utils.CharNameColorList[this.Charname] = NameColor;
																										}
																										string HEXColor4;
																										HEXColor4 = "#" + NameColor.ToString("X");
																										AgentContext.ReduceGold(this.Charname, 12, GuiFiyat3);
																										this.NameHEXColor = HEXColor4;
																									}
																									continue;
																								}
																								case 4:
																									if (Utils.CharIconLeft.ContainsKey(this.Charname))
																									{
																										Utils.CharIconLeft.TryRemove(this.Charname, out var _);
																										await sqlCon.EXEC_QUERY("DELETE FROM " + MainMenu.FILTER_DB + ".._CharIcons where CharName = '" + this.Charname + "'");
																										AgentContext.SendPackagetoAll(AgentContext.RemoveLeftIcons(this.Charname));
																									}
																									continue;
																								case 5:
																									if (Utils.TitleColorList.ContainsKey(this.Charname))
																									{
																										Utils.TitleColorList.TryRemove(this.Charname, out var _);
																										await sqlCon.EXEC_QUERY("DELETE FROM " + MainMenu.FILTER_DB + ".._CharTitleColors where CharName = '" + this.Charname + "'");
																										AgentContext.SendPackagetoAll(AgentContext.RemoveTitleColor(this.Charname));
																									}
																									continue;
																								case 6:
																									if (Utils.CharNameColorList.ContainsKey(this.Charname))
																									{
																										Utils.CharNameColorList.TryRemove(this.Charname, out var _);
																										await sqlCon.EXEC_QUERY("DELETE FROM " + MainMenu.FILTER_DB + ".._CharNameColors where CharName = '" + this.Charname + "'");
																										AgentContext.SendPackagetoAll(AgentContext.RemoveCharNameColor(this.Charname));
																									}
																									continue;
																								case 7:
																								{
																									string Title;
																									Title = _pck.ReadUnicode();
																									if (MainMenu.EnableMarketToken)
																									{
																										if (this.CharToken >= MainMenu.CustomTitlePrice)
																										{
																											if (!Utils.CustomTitle.ContainsKey(this.Charname))
																											{
																												Utils.CustomTitle.TryAdd(this.Charname, Title);
																											}
																											else
																											{
																												Utils.CustomTitle[this.Charname] = Title;
																											}
																											await sqlCon.EXEC_QUERY("EXEC " + MainMenu.FILTER_DB + ".._UpdateCustomTitle '" + this.Charname + "', '" + Title + "'");
																											AgentContext.SendPackagetoAll(AgentContext.UpdateCustomTitle(this.Charname, Title));
																											this.CharToken -= MainMenu.CustomTitlePrice;
																											await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._UpdateTokens '{this.Charname}', {this.CharToken}");
																											this.TokenPacket();
																										}
																										else
																										{
																											this.SendMessage(3, "Check to payment informations.");
																										}
																									}
																									if (!MainMenu.EnableMarketSilk)
																									{
																										break;
																									}
																									if (this.CharSilk >= MainMenu.CustomTitlePrice)
																									{
																										if (!Utils.CustomTitle.ContainsKey(this.Charname))
																										{
																											Utils.CustomTitle.TryAdd(this.Charname, Title);
																										}
																										else
																										{
																											Utils.CustomTitle[this.Charname] = Title;
																										}
																										await sqlCon.EXEC_QUERY("EXEC " + MainMenu.FILTER_DB + ".._UpdateCustomTitle '" + this.Charname + "', '" + Title + "'");
																										AgentContext.SendPackagetoAll(AgentContext.UpdateCustomTitle(this.Charname, Title));
																										this.CharSilk -= MainMenu.CustomTitlePrice;
																										await sqlCon.GIVE_SILK(this.StrUserID, -1 * MainMenu.CustomTitlePrice);
																									}
																									else
																									{
																										this.SendMessage(3, "Check to payment informations.");
																									}
																									continue;
																								}
																								}
																								if (Type2 == 8 && Utils.CustomTitle.ContainsKey(this.Charname))
																								{
																									Utils.CustomTitle.TryRemove(this.Charname, out var d4);
																									await sqlCon.EXEC_QUERY("DELETE FROM " + MainMenu.FILTER_DB + ".._CustomTitles where CharName16 = '" + this.Charname + "'");
																									if (this.EUChar)
																									{
																										AgentContext.SendPackagetoAll(AgentContext.RemoveCustomTitle(this.Charname, Utils.RefHwan[this.HwanLevel].Title_EU70));
																									}
																									else if (this.CHChar)
																									{
																										AgentContext.SendPackagetoAll(AgentContext.RemoveCustomTitle(this.Charname, Utils.RefHwan[this.HwanLevel].Title_CH70));
																									}
																									d4 = null;
																								}
																								continue;
																							}
																							if (_pck.Opcode == 4898)
																							{
																								string MenuID2;
																								MenuID2 = _pck.ReadAscii().ToString();
																								if (MenuID2.Contains("!registerlg") && MainMenu.LG_ENABLE)
																								{
																									if (MainMenu.LG_REGISTER_STATUS)
																									{
																										if (sqlCon.prod_int("SELECT RegStatus FROM " + MainMenu.FILTER_DB + ".dbo._Event_LG with(nolock) WHERE CharName = '" + this.Charname + "'").Result == 1)
																										{
																											this.SendMessage(1, MainMenu.LG_REGISTED_NOTICE);
																										}
																										else
																										{
																											AgentContext.ReduceGold(this.Charname, 1, MainMenu.LG_TICKETPRICE);
																										}
																									}
																									else
																									{
																										this.SendMessage(1, "Lottery Gold registrations are currently closed.");
																									}
																									continue;
																								}
																								if (MenuID2.Contains("!registerlms") && MainMenu.LMS_ENABLE)
																								{
																									if (MainMenu.LMS_REGISTERSTATUS)
																									{
																										if (this.CurLevel < Convert.ToInt32(MainMenu.LMS_REQUIRELEVEL))
																										{
																											this.SendMessage(1, MainMenu.LMS_REQUIRELEVEL_NOTICE);
																											continue;
																										}
																										if (MainMenu.LMS_PLAYERSLIST.IndexOf(this.Charname) == -1)
																										{
																											MainMenu.LMS_PLAYERSLIST.Add(this.Charname);
																											this.SendMessage(1, MainMenu.LMS_REGISTERSUCCESS_NOTICE);
																										}
																										else
																										{
																											this.SendMessage(1, MainMenu.LMS_REGISTED_NOTICE);
																										}
																									}
																									else
																									{
																										this.SendMessage(1, "Last Man Standing registrations are currently closed.");
																									}
																								}
																								if (!MenuID2.Contains("!registersurv") || !MainMenu.SURV_ENABLE)
																								{
																									continue;
																								}
																								if (MainMenu.SURV_REGISTERSTATUS)
																								{
																									if (this.CurLevel < Convert.ToInt32(MainMenu.SURV_REQUIRELEVEL))
																									{
																										this.SendMessage(1, MainMenu.SURV_REQUIRELEVEL_NOTICE);
																									}
																									else if (MainMenu.SURV_PLAYERSLIST.IndexOf(this.Charname) == -1)
																									{
																										MainMenu.SURV_PLAYERSLIST.Add(this.Charname);
																										this.SendMessage(1, MainMenu.SURV_REGISTERSUCCESS_NOTICE);
																									}
																									else
																									{
																										this.SendMessage(1, MainMenu.SURV_REGISTED_NOTICE);
																									}
																								}
																								else
																								{
																									this.SendMessage(1, "Survival Arena registrations are currently closed.");
																								}
																								continue;
																							}
																							if (_pck.Opcode == 6156)
																							{
																								string ItemName;
																								ItemName = _pck.ReadUnicode();
																								byte[] ItemInfo;
																								ItemInfo = _pck.ReadUInt8Array(464);
																								Packet itemlink;
																								itemlink = new Packet(6156);
																								itemlink.WriteAscii_Chinese(this.Charname + "<" + ItemName + ">");
																								itemlink.WriteUInt8Array(ItemInfo);
																								AgentContext.SendPackagetoAll(itemlink);
																								continue;
																							}
																							if (_pck.Opcode == 4353)
																							{
																								DateTime moment;
																								moment = DateTime.Now;
																								moment.Day.ToString();
																								if (moment.Day < 28 && moment.Day > 0)
																								{
																									_ = Task.Run(async () => await sqlCon.prod_string("EXEC " + MainMenu.FILTER_DB + ".._DailyRew '" + this.Charname + "'")).Result;
																								}
																								this.AttendanceEvent();
																								continue;
																							}
																							if (_pck.Opcode == 4609)
																							{
																								byte Type;
																								Type = _pck.ReadUInt8();
																								if (Type == 1 && this.CharTitles.Count > 0)
																								{
																									byte TitleID;
																									TitleID = _pck.ReadUInt8();
																									if (TitleID == 0 || (this.CharTitles.ContainsKey(TitleID) && this.HwanLevel != TitleID))
																									{
																										this.HwanLevel = TitleID;
																										this.LiveTitleUpdate(this.HwanLevel);
																									}
																									continue;
																								}
																								if (Type == 2)
																								{
																									this.HwanLevel = _pck.ReadUInt8();
																									this.LiveTitleUpdate(this.HwanLevel);
																									continue;
																								}
																							}
																							else
																							{
																								if (_pck.Opcode == 4610)
																								{
																									switch (_pck.ReadUInt8())
																									{
																									case 1:
																										if (this.CharTitlesNew.Count > 0)
																										{
																											byte NewTitleID;
																											NewTitleID = _pck.ReadUInt8();
																											AgentContext.SendPackagetoAll(AgentContext.LiveUpdateNewTitles(this.Charname, NewTitleID));
																											await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.dbo._UpdateNewTitle '{this.Charname}','{NewTitleID}'");
																										}
																										break;
																									case 2:
																										AgentContext.SendPackagetoAll(AgentContext.RemoveNewTitles(this.Charname));
																										await sqlCon.EXEC_QUERY("DELETE FROM " + MainMenu.FILTER_DB + ".._iSroTitles where CharName = '" + this.Charname + "'");
																										break;
																									}
																									continue;
																								}
																								if (_pck.Opcode == 13569)
																								{
																									switch (_pck.ReadInt8())
																									{
																									case 1:
																										_pck.ReadUInt32();
																										this.LiveGrantUpdate(_pck.ReadAscii());
																										break;
																									}
																									continue;
																								}
																								if (_pck.Opcode == 13664)
																								{
																									this.JobType = _pck.ReadUInt8();
																									this.CurRegion = _pck.ReadInt16();
																									continue;
																								}
																								if (_pck.Opcode == 4888)
																								{
																									if (_pck.GetBytes().Length < 0)
																									{
																										continue;
																									}
																									string PINCode;
																									PINCode = _pck.ReadAscii().ToString();
																									string MenuID;
																									MenuID = _pck.ReadAscii().ToString();
																									if (MenuID.Contains("!lock") && MainMenu.ITEM_LOCK)
																									{
																										if (int.TryParse(PINCode.ToString(), out var code))
																										{
																											if (PINCode.ToString().Length == 4)
																											{
																												if (this.has_code == 0)
																												{
																													await Task.Run(async delegate
																													{
																														await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._CreateLock '{sqlCon.clean(this.StrUserID)}', {code}");
																													});
																													this.has_code = code;
																													this.is_locked = true;
																													this.CharLockStatus(2);
																													this.SendMessage(3, MainMenu.ITEM_LOCK_FIRST_TIME_NOTICE.Replace("{code}", code.ToString()));
																												}
																												else if (this.has_code == code)
																												{
																													if (this.is_locked)
																													{
																														this.CharLockStatus(2);
																														this.SendMessage(1, MainMenu.ITEM_LOCK_IS_UNLOCKED_NOTICE);
																														this.is_locked = true;
																													}
																													else
																													{
																														this.CharLockStatus(2);
																														this.SendMessage(1, MainMenu.ITEM_LOCK_IS_UNLOCKED_NOTICE);
																														this.is_locked = true;
																													}
																												}
																												else
																												{
																													if (this.lock_fail_attempt >= MainMenu.ITEM_LOCK_MAX_FAIL)
																													{
																														this.SendMessage(2, MainMenu.ITEM_LOCK_DISCONNECT_NOTICE);
																														await Task.Delay(1);
																														AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
																														return;
																													}
																													this.SendMessage(1, MainMenu.ITEM_LOCK_WRONG_CODE_NOTICE.Replace("{count}", this.lock_fail_attempt.ToString()).Replace("{max}", MainMenu.ITEM_LOCK_MAX_FAIL.ToString()));
																												}
																											}
																											else
																											{
																												this.SendMessage(1, MainMenu.ITEM_LOCK_PASSWORD_LENGTH_NOTICE);
																											}
																										}
																										else
																										{
																											this.SendMessage(1, MainMenu.ITEM_LOCK_INTEGER_NOTICE);
																										}
																									}
																									else
																									{
																										if (!MenuID.Contains("!unlock") || !MainMenu.ITEM_LOCK)
																										{
																											continue;
																										}
																										if (int.TryParse(PINCode.ToString(), out var code2))
																										{
																											if (PINCode.ToString().Length == 4)
																											{
																												if (this.has_code == 0)
																												{
																													this.SendMessage(1, MainMenu.ITEM_LOCK_NOT_EXIST_NOTICE);
																												}
																												else if (this.has_code == code2)
																												{
																													await Task.Run(async delegate
																													{
																														await sqlCon.EXEC_QUERY("EXEC [" + MainMenu.FILTER_DB + "].[dbo].[_RemoveLock] '" + this.StrUserID + "'");
																													});
																													this.CharLockStatus(1);
																													this.SendMessage(1, MainMenu.ITEM_UNLOCK_NOTICE);
																													this.has_code = 0;
																													this.is_locked = false;
																												}
																												else
																												{
																													this.lock_fail_attempt++;
																													if (this.lock_fail_attempt >= MainMenu.ITEM_LOCK_MAX_FAIL)
																													{
																														this.SendMessage(1, MainMenu.ITEM_LOCK_DISCONNECT_NOTICE);
																														await Task.Delay(1);
																														AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
																														return;
																													}
																													this.SendMessage(1, MainMenu.ITEM_LOCK_WRONG_CODE_NOTICE.Replace("{count}", this.lock_fail_attempt.ToString()).Replace("{max}", MainMenu.ITEM_LOCK_MAX_FAIL.ToString()));
																												}
																											}
																											else
																											{
																												this.SendMessage(1, MainMenu.ITEM_LOCK_PASSWORD_LENGTH_NOTICE);
																											}
																										}
																										else
																										{
																											this.SendMessage(1, MainMenu.ITEM_LOCK_INTEGER_NOTICE);
																										}
																									}
																									continue;
																								}
																								if (_pck.Opcode == 8203)
																								{
																									if (this.inreturn || this.STALLING || this.innpc || this.JobType != 4)
																									{
																										this.SendMessage(3, "Cannot choose another teleport method because you are now being teleported.");
																										continue;
																									}
																									byte reverse_Slot;
																									reverse_Slot = _pck.ReadUInt8();
																									ushort ItemTypes;
																									ItemTypes = _pck.ReadUInt16();
																									int posx;
																									posx = _pck.ReadInt32();
																									int posy;
																									posy = _pck.ReadInt32();
																									int posz;
																									posz = _pck.ReadInt32();
																									short region;
																									region = _pck.ReadInt16();
																									string charname;
																									charname = _pck.ReadUnicode();
																									int test;
																									test = await sqlCon.prod_int("Select WorldID from " + MainMenu.SHA_DB + ".._Char with (nolock) where CharName16 = '" + charname + "'");
																									this.Update_Item_Count(reverse_Slot, ItemTypes);
																									await Task.Delay(500);
																									this.LiveTeleport(test, test, (ushort)region, posx, posy, posz);
																									continue;
																								}
																								if (_pck.Opcode == 8202)
																								{
																									byte locationID;
																									locationID = _pck.ReadUInt8();
																									short Region;
																									Region = _pck.ReadInt16();
																									int Current_x;
																									Current_x = _pck.ReadInt32();
																									int Current_y;
																									Current_y = _pck.ReadInt32();
																									int Current_z;
																									Current_z = _pck.ReadInt32();
																									int WorldID;
																									WorldID = _pck.ReadInt32();
																									string LocationName;
																									LocationName = _pck.ReadAscii();
																									SavedLocation NewLoc;
																									NewLoc = new SavedLocation
																									{
																										PosX = Current_x,
																										PosY = Current_y,
																										PosZ = Current_z,
																										RegionID = Region,
																										LocationName = LocationName,
																										WorldID = WorldID
																									};
																									if (!this.SavedLocations.ContainsKey(locationID))
																									{
																										this.SavedLocations.Add(locationID, NewLoc);
																										await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._SavedLocationsProcedure '{this.CharID}', '{locationID}', '{Region}', '{Current_x}', '{Current_y}', '{Current_z}', '{WorldID}', '{LocationName}'");
																									}
																									continue;
																								}
																								if (_pck.Opcode == 6172)
																								{
																									if (this.inreturn || this.STALLING || this.innpc || this.JobType != 4)
																									{
																										this.SendMessage(3, "Cannot choose another teleport method because you are now being teleported.");
																										continue;
																									}
																									byte telslot;
																									telslot = _pck.ReadUInt8();
																									this.Update_Item_Count(_pck.ReadUInt8(), _pck.ReadUInt16());
																									new SavedLocation();
																									SavedLocation Loc;
																									Loc = this.SavedLocations[telslot];
																									await Task.Delay(500);
																									this.LiveTeleport(Loc.WorldID, Loc.WorldID, (ushort)Loc.RegionID, Loc.PosX, Loc.PosY, Loc.PosZ);
																									continue;
																								}
																								if (_pck.Opcode == 8207)
																								{
																									byte revid;
																									revid = _pck.ReadUInt8();
																									if (this.SavedLocations.ContainsKey(revid))
																									{
																										this.SavedLocations.Remove(revid);
																										await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._RemoveSavedLocation '{this.CharID}', '{revid}'");
																									}
																									continue;
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
						goto IL_82f1;
						IL_22de:
						if (this.INSIDE_SURVIVAL)
						{
							this.SendMessage(3, "Cant Create Stall in this region.");
						}
						if (this.CurLevel < MainMenu.STALL_LEVEL)
						{
							this.SendMessage(3, MainMenu.STALL_LEVEL_NOTICE.Replace("%level%", MainMenu.STALL_LEVEL.ToString()));
							continue;
						}
						if (MainMenu.ITEM_LOCK && this.has_code > 0)
						{
							this.SendMessage(3, MainMenu.ITEM_LOCK_STALL_NOTICE);
							continue;
						}
						if (this.RIDING_PET)
						{
							this.SendMessage(3, "You cannot open a stall while having pets summoned.");
							continue;
						}
						goto IL_82f1;
						IL_06e8:
						if (_pck.Opcode != 8194 && _pck.Opcode != 28673 && _pck.Opcode != 28679 && _pck.Opcode != 29776 && _pck.Opcode != 8467 && _pck.Opcode != 12306 && _pck.Opcode != 29966 && _pck.Opcode != 4608 && _pck.Opcode != 13664 && _pck.Opcode != 6025)
						{
							MainMenu.WriteLine(1, $"Client({this.SOCKET_IP}:{this.StrUserID}) attempted to use a Shard exploit #11 and used packet 0x{_pck.Opcode:X}");
							continue;
						}
						goto IL_07ef;
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
			MainMenu.WriteLine(1, "AG_ASYNC_RECV_FROM_CLIENT" + EX?.ToString() + base.CLIENT_SOCKET?.ToString() + "CHARNAME16_HOLDER");
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
						if (_pck.Opcode == 12320)
						{
							this.UNIQUE_ID = _pck.ReadUInt32();
						}
						else if (_pck.Opcode == 41219)
						{
							if (_pck.ReadUInt8() == 1)
							{
								this.UserLogged = true;
							}
							if (this.UserLogged)
							{
								this.IsCharScreen = true;
							}
						}
						else if (_pck.Opcode == 46350)
						{
							this.IsCharScreen = false;
						}
						else if (_pck.Opcode == 45057)
						{
							if (_pck.ReadUInt8() == 1)
							{
								this.IsCharScreen = false;
							}
						}
						else if (_pck.Opcode == 45108)
						{
							_pck.ReadUInt8();
							byte status;
							status = _pck.ReadUInt8();
							if (MainMenu.SOXDROPOTICE_NOTICE_CHECK)
							{
								switch (status)
								{
								case 6:
								{
									if (_pck.ReadUInt8() == 254)
									{
										break;
									}
									_pck.ReadInt32();
									uint GetRefItemID2;
									GetRefItemID2 = _pck.ReadUInt32();
									if (Utils.SoxDrop.ContainsKey(this.Charname))
									{
										bool test2;
										test2 = false;
										foreach (KeyValuePair<string, int> item in Utils.SoxDrop)
										{
											if (item.Value == GetRefItemID2)
											{
												test2 = true;
											}
										}
										if (test2)
										{
											base.LOCAL_SECURITY.Send(_pck);
											this.SendToClient();
											continue;
										}
									}
									Utils.SoxDrop.TryAdd(this.Charname, Convert.ToInt32(GetRefItemID2));
									if (!Utils.RefObjCommon.ContainsKey(GetRefItemID2))
									{
										break;
									}
									string codename2;
									codename2 = Utils.RefObjCommon[GetRefItemID2].CodeName128;
									if (codename2.Contains(MainMenu.SOX_DROP1) || codename2.Contains(MainMenu.SOX_DROP2))
									{
										string Msg2;
										Msg2 = MainMenu.SOXDROPNOTICE_NOTICE.Replace("{Charname}", this.Charname);
										Packet itemlink2;
										itemlink2 = new Packet(4921);
										itemlink2.WriteInt8(3);
										itemlink2.WriteUnicode(Msg2);
										itemlink2.WriteUInt32(Convert.ToInt32(16737095));
										itemlink2.WriteInt32(GetRefItemID2);
										itemlink2.WriteInt32(0);
										itemlink2.WriteInt32(GetRefItemID2);
										for (int l = 11; l < _pck.GetBytes().Length; l++)
										{
											itemlink2.WriteUInt8(_pck.ReadUInt8());
										}
										AgentContext.SendPackagetoAll(itemlink2);
									}
									break;
								}
								case 17:
								{
									_pck.ReadUInt32();
									_pck.ReadUInt8();
									_pck.ReadUInt32();
									uint GetRefItemID;
									GetRefItemID = _pck.ReadUInt32();
									if (Utils.SoxDrop.ContainsKey(this.Charname))
									{
										bool test;
										test = false;
										foreach (KeyValuePair<string, int> item2 in Utils.SoxDrop)
										{
											if (item2.Value == GetRefItemID)
											{
												test = true;
											}
										}
										if (test)
										{
											base.LOCAL_SECURITY.Send(_pck);
											this.SendToClient();
											continue;
										}
									}
									Utils.SoxDrop.TryAdd(this.Charname, Convert.ToInt32(GetRefItemID));
									if (!Utils.RefObjCommon.ContainsKey(GetRefItemID))
									{
										break;
									}
									string codename;
									codename = Utils.RefObjCommon[GetRefItemID].CodeName128;
									if (codename.Contains(MainMenu.SOX_DROP1) || codename.Contains(MainMenu.SOX_DROP2))
									{
										string Msg;
										Msg = MainMenu.SOXDROPNOTICE_NOTICE.Replace("{Charname}", this.Charname);
										Packet itemlink;
										itemlink = new Packet(4921);
										itemlink.WriteInt8(3);
										itemlink.WriteUnicode(Msg);
										itemlink.WriteUInt32(Convert.ToInt32(16737095));
										itemlink.WriteInt32(GetRefItemID);
										itemlink.WriteInt32(0);
										itemlink.WriteInt32(GetRefItemID);
										for (int k = 15; k < _pck.GetBytes().Length; k++)
										{
											itemlink.WriteUInt8(_pck.ReadUInt8());
										}
										AgentContext.SendPackagetoAll(itemlink);
									}
									break;
								}
								}
							}
						}
						else if (_pck.Opcode == 13493)
						{
							_pck.ReadInt16();
							_ = this.CurRegion;
						}
						else if (_pck.Opcode == 12328)
						{
							this.CurRegion = _pck.ReadInt16();
						}
						else if (_pck.Opcode == 12326)
						{
							if (_pck.ReadUInt8() != 6)
							{
							}
						}
						else if (_pck.Opcode == 45392)
						{
							_pck.Lock();
							try
							{
								_pck.ReadUInt16();
								_pck.ReadUInt8();
								int slot;
								slot = _pck.ReadUInt8();
								_pck.ReadUInt32();
								uint itemid;
								itemid = _pck.ReadUInt32();
								int plus;
								plus = _pck.ReadUInt8();
								if (plus > MainMenu.PULSNOTICE_NOTICE_Start && MainMenu.PULSNOTICE_NOTICE_CHECK)
								{
									_pck.ReadUInt64Array(2);
									if (_pck.ReadUInt8() == 0)
									{
										string plus_notice;
										plus_notice = MainMenu.PULSNOTICE_NOTICE;
										await Task.Delay(100);
										this.ItemPulsNotice(itemid, slot, plus_notice.Replace("{plus}", plus.ToString()));
									}
									else
									{
										_pck.ReadUInt8();
										_pck.ReadUInt32();
										uint opt;
										opt = _pck.ReadUInt32();
										string plus_notice2;
										plus_notice2 = MainMenu.PULSNOTICE_NOTICE.Replace("{plus}", (plus + opt).ToString());
										await Task.Delay(100);
										this.ItemPulsNotice(itemid, slot, plus_notice2.Replace("{plus}", opt.ToString()));
									}
								}
								if (plus >= 6 && Utils.Available_Avatar.Contains(Convert.ToInt32(itemid)))
								{
									_pck.ReadUInt64Array(2);
									if (_pck.ReadUInt8() == 0)
									{
										string plus_notice3;
										plus_notice3 = MainMenu.PULSNOTICE_NOTICE;
										await Task.Delay(100);
										this.ItemPulsNotice(itemid, slot, plus_notice3.Replace("{plus}", plus.ToString()));
									}
								}
							}
							catch
							{
							}
						}
						else if (_pck.Opcode == 46358)
						{
							if (_pck.ReadUInt8() == 1)
							{
								uint Current_Unique_ID;
								Current_Unique_ID = _pck.ReadUInt32();
								byte flagtype;
								flagtype = _pck.ReadUInt8();
								if (Current_Unique_ID == this.UNIQUE_ID)
								{
									if (flagtype == 0)
									{
										this.PVP_FLAG = false;
									}
									else if (flagtype == 1 || flagtype == 2 || flagtype == 3 || flagtype == 4 || flagtype == 5)
									{
										this.PVP_FLAG = true;
									}
								}
							}
						}
						else if (_pck.Opcode == 12307)
						{
							_pck.ReadUInt32();
							uint refObjId;
							refObjId = _pck.ReadUInt32();
							_pck.ReadUInt8();
							byte curLevel;
							curLevel = _pck.ReadUInt8();
							_pck.ReadUInt8();
							_pck.ReadUInt64();
							_pck.ReadUInt32();
							_pck.ReadUInt64();
							_pck.ReadUInt32();
							_pck.ReadUInt16();
							_pck.ReadUInt8();
							_pck.ReadUInt32();
							_pck.ReadUInt32();
							_pck.ReadUInt32();
							_pck.ReadUInt8();
							_pck.ReadUInt8();
							_pck.ReadUInt16();
							_pck.ReadUInt32();
							byte HwanLv;
							HwanLv = _pck.ReadUInt8();
							_pck.ReadUInt8();
							_pck.ReadUInt8();
							this.HwanLevel = HwanLv;
							this.CurLevel = curLevel;
							this.isOnline = true;
							if (refObjId < 14875)
							{
								this.EUChar = false;
								this.CHChar = true;
							}
							else
							{
								this.CHChar = false;
								this.EUChar = true;
							}
							if (!this.isStartNotice)
							{
								if (MainMenu.WELCOME_MSG_CHECK)
								{
									string welcomeText;
									welcomeText = MainMenu.WELCOME_TEXT_NOTICE;
									if (welcomeText.Contains("{playername}"))
									{
										welcomeText = welcomeText.Replace("{playername}", this.Charname);
									}
									if (welcomeText.Contains("{servername}"))
									{
										welcomeText = welcomeText.Replace("{servername}", MainMenu.SERVER_NAME);
									}
									if (welcomeText.Contains("{username}"))
									{
										welcomeText = welcomeText.Replace("{username}", this.StrUserID);
									}
									if (welcomeText.Length > 3 && welcomeText != string.Empty)
									{
										this.SendMessage(1, welcomeText);
									}
								}
								this.isStartNotice = true;
							}
							if (!this.FirstSpawn)
							{
								this.FirstSpawn = true;
								this.CheckLockStatus(this.StrUserID);
								this.LoadLeftIconList();
								this.LoadLeftCharIcons();
								this.LoadRightIconList();
								this.LoadRightCharIcons();
								this.LoadUpIconList();
								this.LoadUpCharIcons();
								this.LoadCharNameColors();
								this.LoadTitleColors();
								this.LoadNewTitles();
								this.LoadUniqueLog();
								this.LoadCustomTitles();
								sqlCon.LoadCharAnalyzer(this.Charname);
								this.CharToken = sqlCon.prod_int("SELECT Token FROM " + MainMenu.FILTER_DB + ".dbo._NpcCharTokens with(nolock) WHERE CharName16 = '" + this.Charname + "'").Result;
								this.CharSilk = sqlCon.prod_int("Select silk_own from SRO_VT_ACCOUNT.dbo.SK_Silk where JID = (Select UserJID from SRO_VT_SHARD.dbo._User where CharID = (Select CharID from SRO_VT_SHARD.dbo._Char where CharName16 = N'" + this.Charname + "'))").Result;
								await sqlCon.UpdateHwanList(this);
								await sqlCon.UpdateNewTitles(this);
								this.LoadEventSuitRegions();
								if (MainMenu.ChestWnd)
								{
									await sqlCon.UpdateCharChest(this);
									this.LoadChest();
								}
								if (MainMenu.NewReverse)
								{
									await sqlCon.LoadSavedLocations(this);
									this.LoadSavedLocations();
								}
								this.LoadAttendance();
								if (await sqlCon.CharIDHaveInUserInfo(this.CharID))
								{
									await sqlCon.UpdatePlayerInfo(this.StrUserID, status: true, this.SOCKET_IP.Split(':')[0], this.HWID ?? string.Empty);
								}
								else
								{
									await sqlCon.InsertPlayerInfo(this.CharID, this.Charname, this.StrUserID, this.SOCKET_IP.Split(':')[0], this.HWID ?? string.Empty);
								}
							}
						}
						else if (_pck.Opcode == 14431)
						{
							_pck.ReadUInt8();
						}
						else if (_pck.Opcode == 13522)
						{
							try
							{
								_pck.Lock();
								if (_pck.ReadUInt8() == 9)
								{
									switch (_pck.ReadUInt8())
									{
									case 0:
									{
										int c;
										c = _pck.ReadUInt8();
										if (c == 1)
										{
											_pck.ReadUInt8();
										}
										if (c == 2)
										{
											_pck.ReadUInt8();
										}
										if (c == 3)
										{
											_pck.ReadUInt8();
										}
										break;
									}
									case 1:
									{
										int c2;
										c2 = _pck.ReadUInt8();
										if (c2 == 1)
										{
											_pck.ReadUInt8();
										}
										if (c2 == 2)
										{
											_pck.ReadUInt8();
										}
										if (c2 == 3)
										{
											_pck.ReadUInt8();
										}
										break;
									}
									case 2:
									{
										int c3;
										c3 = _pck.ReadUInt8();
										if (c3 == 1)
										{
											_pck.ReadUInt8();
										}
										if (c3 == 2)
										{
											_pck.ReadUInt8();
										}
										if (c3 == 3)
										{
											_pck.ReadUInt8();
										}
										break;
									}
									case 3:
									{
										int c4;
										c4 = _pck.ReadUInt8();
										if (c4 == 1)
										{
											_pck.ReadUInt8();
										}
										if (c4 == 2)
										{
											_pck.ReadUInt8();
										}
										if (c4 == 3)
										{
											_pck.ReadUInt8();
										}
										break;
									}
									}
								}
							}
							catch
							{
								MainMenu.WriteLine(2, " b = hata _GiveHonorpoint");
							}
						}
						else if (_pck.Opcode == 12372)
						{
							if (_pck.ReadUInt32() == this.UNIQUE_ID)
							{
								this.CurLevel++;
							}
						}
						else if (_pck.Opcode == 45063)
						{
							if (_pck.ReadUInt8() == 2 && _pck.ReadUInt8() == 1)
							{
								byte char_count;
								char_count = _pck.ReadUInt8();
								this.MenuSettings();
								for (int cc = 0; cc < char_count; cc++)
								{
									_pck.ReadUInt32();
									_pck.ReadAscii();
									_pck.ReadUInt8();
									_pck.ReadUInt8();
									_pck.ReadUInt64();
									_pck.ReadUInt16();
									_pck.ReadUInt16();
									_pck.ReadUInt16();
									_pck.ReadUInt32();
									_pck.ReadUInt32();
									if (_pck.ReadUInt8() == 1)
									{
										_pck.ReadUInt32();
									}
									_pck.ReadUInt8();
									_pck.ReadUInt8();
									_pck.ReadUInt8();
									int ls_itemscount;
									ls_itemscount = _pck.ReadUInt8();
									for (int ic = 0; ic < ls_itemscount; ic++)
									{
										_pck.ReadUInt32();
										_pck.ReadUInt8();
									}
									int avatarcount;
									avatarcount = _pck.ReadUInt8();
									for (int ac = 0; ac < avatarcount; ac++)
									{
										_pck.ReadUInt32();
										_pck.ReadUInt8();
									}
								}
							}
						}
						else if (_pck.Opcode != 12545)
						{
							if (_pck.Opcode == 45259)
							{
								if (_pck.ReadUInt8() == 1)
								{
									uint playeruniqueid;
									playeruniqueid = _pck.ReadUInt32();
									byte status2;
									status2 = _pck.ReadUInt8();
									uint petuniqueid;
									petuniqueid = _pck.ReadUInt32();
									if (this.UNIQUE_ID == playeruniqueid && this.FELLOW_PET == petuniqueid)
									{
										switch (status2)
										{
										case 1:
											this.RIDING_PET = true;
											break;
										case 0:
											this.RIDING_PET = false;
											break;
										}
									}
								}
							}
							else if (_pck.Opcode == 12375)
							{
								if (_pck.ReadUInt32() == this.UNIQUE_ID)
								{
									_pck.ReadUInt8();
									_pck.ReadUInt8();
									switch (_pck.ReadUInt8())
									{
									case 1:
									{
										uint CURRENT_HP_1;
										CURRENT_HP_1 = _pck.ReadUInt32();
										if (CURRENT_HP_1 != 0)
										{
											this.DEAD_STATUS = false;
										}
										else if (CURRENT_HP_1 == 0)
										{
											this.DEAD_STATUS = true;
										}
										break;
									}
									case 2:
										_pck.ReadUInt32();
										break;
									case 3:
									{
										uint CURRENT_HP_2;
										CURRENT_HP_2 = _pck.ReadUInt32();
										if (CURRENT_HP_2 != 0)
										{
											this.DEAD_STATUS = false;
										}
										else if (CURRENT_HP_2 == 0)
										{
											this.DEAD_STATUS = true;
										}
										_pck.ReadUInt32();
										break;
									}
									case 4:
										if (_pck.ReadUInt32() != 0)
										{
										}
										break;
									}
								}
							}
							else if (_pck.Opcode == 14436)
							{
								_pck.Lock();
								switch (_pck.ReadUInt8())
								{
								case 1:
									this.PARTYSTATUS = false;
									break;
								case 2:
									this.PARTYSTATUS = true;
									break;
								}
								if (MainMenu.DisablePartyRegion.Contains(this.CurRegion.ToString()))
								{
									continue;
								}
							}
							else if (_pck.Opcode == 45159)
							{
								_pck.Lock();
								if (_pck.ReadUInt8() == 1)
								{
									this.PARTYSTATUS = true;
								}
								if (MainMenu.DisablePartyRegion.Contains(this.CurRegion.ToString()))
								{
									continue;
								}
							}
							else if (_pck.Opcode == 45152)
							{
								_pck.Lock();
								if (_pck.ReadUInt8() == 1)
								{
									this.PARTYSTATUS = true;
								}
								if (MainMenu.DisablePartyRegion.Contains(this.CurRegion.ToString()))
								{
									continue;
								}
							}
							else if (_pck.Opcode == 45161)
							{
								if (MainMenu.LPN_STATUS && _pck.GetBytes().Length >= 5)
								{
									_pck.ReadInt8();
									if (_pck.ReadInt32() == MainMenu.LPN_TARGETPARTYY)
									{
										this.SendMessage(1, MainMenu.LPN_WIN_NOTICE.Replace("%winner%", this.Charname).Replace("%reward%", MainMenu.LPN_ITEMNAME));
										await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._QuestionReward 0, '{this.Charname}', {MainMenu.LPN_ITEMREWARD}, {MainMenu.LPN_ITEMCOUNT}, 0, 0, 'Lucky Party Number'");
										MainMenu.LPN_STATUS = false;
										continue;
									}
								}
							}
							else if (_pck.Opcode == 12300)
							{
								switch (_pck.ReadUInt8())
								{
								case 5:
									_pck.ReadUInt8();
									UniqueLog.UniqueLogspawn(_pck.ReadUInt16());
									break;
								case 6:
								{
									_pck.ReadUInt8();
									uint MobID2;
									MobID2 = _pck.ReadUInt16();
									_pck.ReadAscii();
									string Name2;
									Name2 = _pck.ReadAscii();
									if (this.Charname == Name2 || this.Jobname == Name2)
									{
										UniqueLog.UniqueLogkill(Convert.ToInt32(MobID2), Name2);
										await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._UniqueKilled '{MobID2}', '{Name2}'");
									}
									break;
								}
								}
							}
							else if (_pck.Opcode == 12488)
							{
								_pck.Lock();
								if (_pck.GetBytes().Length >= 8)
								{
									_pck.ReadUInt32();
									_pck.ReadUInt32();
								}
							}
							else if (_pck.Opcode == 12489)
							{
								_pck.Lock();
								if (_pck.GetBytes().Length >= 4)
								{
									uint Dismounted_Pet_UniqueID;
									Dismounted_Pet_UniqueID = _pck.ReadUInt32();
									if (this.FELLOW_PET == Dismounted_Pet_UniqueID)
									{
										this.FELLOW_PET = 0u;
									}
									else if (this.ATTACK_PET == Dismounted_Pet_UniqueID)
									{
										this.ATTACK_PET = 0u;
									}
									else if (this.GRAB_PET == Dismounted_Pet_UniqueID)
									{
										this.GRAB_PET = 0u;
									}
								}
							}
							else if (_pck.Opcode == 12311)
							{
								this.GroupSpawnType = _pck.ReadUInt8();
								this.GroupSpawnAmount = _pck.ReadUInt16();
							}
							else if (_pck.Opcode == 12313)
							{
								try
								{
									for (int ib = 0; ib < this.GroupSpawnAmount; ib++)
									{
										if (this.GroupSpawnType == 1)
										{
											uint ID;
											ID = _pck.ReadUInt32();
											_RefObjCommon OBj;
											OBj = null;
											if (Utils.RefObjCommon.ContainsKey(ID))
											{
												OBj = Utils.RefObjCommon[ID];
											}
											if (OBj == null || OBj.TypeID1 != 1)
											{
												continue;
											}
											if (OBj.TypeID2 == 1)
											{
												_pck.ReadUInt8();
												_pck.ReadUInt8();
												_pck.ReadUInt8();
												_pck.ReadUInt8();
												_pck.ReadUInt8();
												byte Inventory_ItemCount;
												Inventory_ItemCount = _pck.ReadUInt8();
												for (int iic = 0; iic < Inventory_ItemCount; iic++)
												{
													uint Item_RefObjID;
													Item_RefObjID = _pck.ReadUInt32();
													_RefObjCommon Item;
													Item = null;
													if (Utils.RefObjCommon.ContainsKey(Item_RefObjID))
													{
														Item = Utils.RefObjCommon[Item_RefObjID];
													}
													if (Item != null && Item.TypeID1 == 3 && Item.TypeID2 == 1)
													{
														_pck.ReadUInt8();
													}
												}
												_pck.ReadUInt8();
												byte AvaInv_ItemCount;
												AvaInv_ItemCount = _pck.ReadUInt8();
												for (int aii = 0; aii < AvaInv_ItemCount; aii++)
												{
													uint AvaInvItem_RefObjID;
													AvaInvItem_RefObjID = _pck.ReadUInt32();
													_RefObjCommon Item2;
													Item2 = null;
													if (Utils.RefObjCommon.ContainsKey(AvaInvItem_RefObjID))
													{
														Item2 = Utils.RefObjCommon[AvaInvItem_RefObjID];
													}
													if (Item2 != null && Item2.TypeID1 == 3 && Item2.TypeID2 == 1)
													{
														_pck.ReadUInt8();
													}
												}
												if (_pck.ReadUInt8() == 1)
												{
													uint Mask_RefObjID;
													Mask_RefObjID = _pck.ReadUInt32();
													_RefObjCommon Item3;
													Item3 = null;
													if (Utils.RefObjCommon.ContainsKey(Mask_RefObjID))
													{
														Item3 = Utils.RefObjCommon[Mask_RefObjID];
													}
													if (Item3 != null && Item3.CodeName128.StartsWith("CHAR"))
													{
														_pck.ReadUInt8();
														byte MaskCount;
														MaskCount = _pck.ReadUInt8();
														for (int mc = 0; mc < MaskCount; mc++)
														{
															_pck.ReadUInt32();
														}
													}
												}
											}
											else if (OBj.TypeID2 == 2 && OBj.TypeID3 == 5)
											{
												_pck.ReadUInt32();
												_pck.ReadUInt32();
												_pck.ReadUInt16();
											}
											_pck.ReadUInt32();
											ushort positionRegionId;
											positionRegionId = _pck.ReadUInt16();
											_pck.ReadFloat();
											_pck.ReadFloat();
											_pck.ReadFloat();
											_pck.ReadUInt16();
											byte HasDestination;
											HasDestination = _pck.ReadUInt8();
											_pck.ReadUInt8();
											if (HasDestination == 1)
											{
												_pck.ReadUInt16();
												if (positionRegionId < 32767)
												{
													_pck.ReadUInt16();
													_pck.ReadUInt16();
													_pck.ReadUInt16();
												}
												else
												{
													_pck.ReadUInt32();
													_pck.ReadUInt32();
													_pck.ReadUInt32();
												}
											}
											else
											{
												_pck.ReadUInt8();
												_pck.ReadUInt16();
											}
										}
										else
										{
											_pck.ReadUInt32();
										}
									}
								}
								catch
								{
								}
							}
							else if (_pck.Opcode != 12309)
							{
								if (_pck.Opcode == 45089)
								{
									try
									{
										_pck.Lock();
										if (_pck.ReadUInt32() == this.UNIQUE_ID)
										{
											this.afk_timer = DateTime.Now;
											this.isAfk = false;
											if (this.isAfkSymbol)
											{
												Packet afk;
												afk = new Packet(29698);
												afk.WriteUInt8(0);
												base.REMOTE_SECURITY.Send(afk);
												this.SendToModule();
												this.isAfkSymbol = false;
											}
											if (_pck.ReadUInt8() == 1)
											{
												short Region;
												Region = _pck.ReadInt16();
												if (Region.ToString().Length == 5 && _pck.GetBytes().Length == 24)
												{
													this.CurRegion = Region;
												}
											}
										}
									}
									catch
									{
									}
								}
								else if (_pck.Opcode == 12511)
								{
									try
									{
										uint uniqueid;
										uniqueid = _pck.ReadUInt32();
										byte NewHwan;
										NewHwan = _pck.ReadUInt8();
										if (this.UNIQUE_ID == uniqueid)
										{
											this.HwanLevel = NewHwan;
										}
									}
									catch
									{
									}
								}
								else if (_pck.Opcode == 12306)
								{
									if (_pck.GetBytes().Length != 0)
									{
										if (MainMenu.FIREWALLBANCHECK)
										{
											Utils.BLOCK_IP(this.SOCKET_IP.Split(':')[0], "AgentServer modify 0x3012 bytes.");
											AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
											MainMenu.WriteLine(1, "Client (" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to modify 0x3012 bytes.");
										}
										else
										{
											AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
											MainMenu.WriteLine(1, "Client (" + this.SOCKET_IP + ":" + this.StrUserID + ") attempted to modify 0x3012 bytes.");
										}
										return;
									}
								}
								else
								{
									if (_pck.Opcode == 20496)
									{
										int MobID;
										MobID = _pck.ReadInt32();
										int cont;
										cont = _pck.ReadInt32();
										DPSWindow.addcolomn();
										DPSWindow.UniqueAtacker.Rows.Clear();
										for (int j = 1; j <= cont; j++)
										{
											uint PuniqueID;
											PuniqueID = _pck.ReadUInt32();
											int Atackvalue;
											Atackvalue = _pck.ReadInt32();
											string playername;
											playername = AgentContext.GetCharname(PuniqueID);
											if (playername != "")
											{
												DPSWindow.addrows(playername, Atackvalue);
											}
										}
										if (DPSWindow.UniqueAtacker.Rows.Count == 0)
										{
											continue;
										}
										Packet packet;
										packet = new Packet(20775);
										packet.WriteInt32(MobID);
										packet.WriteInt8(DPSWindow.UniqueAtacker.Rows.Count);
										int i;
										i = 0;
										foreach (DataRow dts in DPSWindow.UniqueAtacker.Rows)
										{
											string Name;
											Name = dts["charname"].ToString();
											int point;
											point = Convert.ToInt32(dts["Atackvalue"]);
											packet.WriteAscii(Name);
											packet.WriteAscii(DPSWindow.FormatNumber(point));
											i++;
										}
										AgentContext.SendPackagetoAll(packet);
										continue;
									}
									if (_pck.Opcode == 20497)
									{
										switch (_pck.ReadInt8())
										{
										case 1:
											switch (_pck.ReadInt8())
											{
											case 1:
											{
												int gold;
												gold = _pck.ReadInt32();
												this.SendMessage(11, "[" + DPSWindow.FormatNumber(gold) + "]gold removed.");
												this.SendMessage(3, MainMenu.LG_REGISTERSUCCESS_NOTICE);
												await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._LG_Events '{this.Charname}', '{gold}', '1'");
												break;
											}
											case 2:
											{
												int gold2;
												gold2 = _pck.ReadInt32();
												this.SendMessage(3, MainMenu.LG_GOLDREQUIRE_NOTICE.Replace("%gold%", DPSWindow.FormatNumber(gold2)));
												break;
											}
											}
											break;
										case 2:
											switch (_pck.ReadInt8())
											{
											case 1:
												this.SendMessage(11, "[" + DPSWindow.FormatNumber(_pck.ReadInt32()) + "]gold removed.");
												break;
											case 2:
											{
												int gold3;
												gold3 = _pck.ReadInt32();
												this.NewCustomTitle = "";
												this.SendMessage(3, MainMenu.LG_GOLDREQUIRE_NOTICE.Replace("%gold%", DPSWindow.FormatNumber(gold3)));
												break;
											}
											}
											break;
										case 10:
											switch (_pck.ReadInt8())
											{
											case 1:
												this.SendMessage(11, "[" + DPSWindow.FormatNumber(_pck.ReadInt32()) + "]gold removed.");
												await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._UpdateLeftIcons '{this.Charname}', '{AgentContext.CharIcon}'");
												break;
											case 2:
												_pck.ReadInt32();
												this.SendMessage(3, "Check to payment informations.");
												break;
											}
											break;
										case 11:
											switch (_pck.ReadInt8())
											{
											case 1:
												this.SendMessage(11, "[" + DPSWindow.FormatNumber(_pck.ReadInt32()) + "]gold removed.");
												await sqlCon.EXEC_QUERY("EXEC " + MainMenu.FILTER_DB + ".._UpdateTitleColor '" + this.Charname + "', '" + this.TitleHEXColor + "'");
												break;
											case 2:
												_pck.ReadInt32();
												this.SendMessage(3, "Check to payment informations.");
												break;
											}
											break;
										case 12:
											switch (_pck.ReadInt8())
											{
											case 1:
												this.SendMessage(11, "[" + DPSWindow.FormatNumber(_pck.ReadInt32()) + "]gold removed.");
												await sqlCon.EXEC_QUERY("EXEC " + MainMenu.FILTER_DB + ".._UpdateCharacterNameColor '" + this.Charname + "', '" + this.NameHEXColor + "'");
												break;
											case 2:
												_pck.ReadInt32();
												this.SendMessage(3, "Check to payment informations.");
												break;
											}
											break;
										}
										continue;
									}
									if (_pck.Opcode == 13061)
									{
										this.DEAD_STATUS = false;
										this.ZERKING = false;
										this.JOB_YELLOW_LINE = false;
										this.RIDING_PET = false;
										this.PVP_FLAG = false;
										this.inreturn = false;
										this.afk_timer = DateTime.Now;
										if (MainMenu.AutoCape.Contains(this.CurRegion.ToString()))
										{
											Packet LiveTitle2;
											LiveTitle2 = new Packet(13568);
											LiveTitle2.WriteUInt32(this.UNIQUE_ID);
											LiveTitle2.WriteUInt8(3);
											LiveTitle2.WriteUInt8(5);
											base.REMOTE_SECURITY.Send(LiveTitle2);
											this.SendToModule();
										}
										if (MainMenu.DisablePartyRegion.Contains(this.CurRegion.ToString()))
										{
											Packet leave_party;
											leave_party = new Packet(28769, encrypted: true);
											base.REMOTE_SECURITY.Send(leave_party);
											this.SendToModule();
											Packet LiveTitle;
											LiveTitle = new Packet(14436);
											LiveTitle.WriteUInt8(1);
											LiveTitle.WriteUInt8(11);
											LiveTitle.WriteUInt8(0);
											base.LOCAL_SECURITY.Send(LiveTitle);
											this.SendToClient();
										}
										short CurrentRegionID;
										CurrentRegionID = this.CurRegion;
										if (AgentContext.BA_REGIONS.Contains(CurrentRegionID))
										{
											this.INSIDE_BA = true;
										}
										else
										{
											this.INSIDE_BA = false;
										}
										if (CurrentRegionID == -32759)
										{
											if (await sqlCon.GetWorldID(this.CharID) == 29)
											{
												this.INSIDE_CTF = true;
											}
											else
											{
												this.INSIDE_CTF = false;
											}
										}
										if (AgentContext.FORTRESS_REGIONS.Contains(CurrentRegionID))
										{
											this.INSIDE_FTW = true;
										}
										else
										{
											this.INSIDE_FTW = false;
										}
										if (AgentContext.HWT_REGIONS.Contains(CurrentRegionID))
										{
											this.INSIDE_HWT = true;
										}
										else
										{
											this.INSIDE_HWT = false;
										}
										if (AgentContext.TEMPLE_REGIONS.Contains(CurrentRegionID.ToString()))
										{
											this.INSIDE_TEMPLE = true;
										}
										else
										{
											this.INSIDE_TEMPLE = false;
										}
										if (AgentContext.FGW_REGIONS.Contains(CurrentRegionID))
										{
											this.INSIDE_FGW = true;
										}
										else
										{
											this.INSIDE_FGW = false;
										}
										if (AgentContext.JUPITER_REGIONS.Contains(CurrentRegionID))
										{
											this.INSIDE_JUPITER = true;
										}
										else
										{
											this.INSIDE_JUPITER = false;
										}
										if (CurrentRegionID == MainMenu.SURV_REGIONID || CurrentRegionID == MainMenu.SURV_REGIONID2)
										{
											this.INSIDE_SURVIVAL = true;
										}
										else
										{
											this.INSIDE_SURVIVAL = false;
										}
										if (CurrentRegionID == MainMenu.LMS_REGIONID || CurrentRegionID == MainMenu.LMS_REGIONID2)
										{
											this.INSIDE_LMS = true;
										}
										else
										{
											this.INSIDE_LMS = false;
										}
										try
										{
											if (MainMenu.JOB_PC_LIMIT > 0 && this.JOB_FLAG && AsyncServer.GetHWIDCountJob(this.HWID) > MainMenu.JOB_PC_LIMIT)
											{
												this.SendMessage(3, MainMenu.JOB_PC_LIMIT_NOTICE);
												await Task.Delay(100);
												AsyncServer.DisconnectFromModule(base.CLIENT_SOCKET, this.MODULE_TYPE);
												return;
											}
										}
										catch
										{
										}
										try
										{
											if (MainMenu.BA_PC_LIMIT > 0 && AsyncServer.GetHwidCountBattleArena(this.HWID) > MainMenu.BA_PC_LIMIT && this.INSIDE_BA)
											{
												this.SendMessage(3, MainMenu.BA_PC_LIMIT_NOTICE);
												await Task.Delay(100);
												this.LiveTeleport(1, 1, 25000, 982f, 140f, 140f);
											}
										}
										catch
										{
										}
										try
										{
											if (MainMenu.CTF_PC_LIMIT > 0 && AsyncServer.GetHwidCountCTF(this.HWID) > MainMenu.CTF_PC_LIMIT && this.INSIDE_CTF)
											{
												this.SendMessage(3, MainMenu.CTF_PC_LIMIT_NOTICE);
												await Task.Delay(100);
												this.LiveTeleport(1, 1, 25000, 982f, 140f, 140f);
											}
										}
										catch
										{
										}
										try
										{
											if (MainMenu.FTW_PC_LIMIT > 0 && AsyncServer.GetHwidCountFortress(this.HWID) > MainMenu.FTW_PC_LIMIT && this.INSIDE_FTW)
											{
												this.SendMessage(3, MainMenu.FTW_PC_LIMIT_NOTICE);
												await Task.Delay(100);
												this.LiveTeleport(1, 1, 25000, 982f, 140f, 140f);
											}
										}
										catch
										{
										}
										try
										{
											if (MainMenu.HT_PC_LIMIT > 0 && AsyncServer.GetHwidCountJobTemple(this.HWID) > MainMenu.HT_PC_LIMIT && this.INSIDE_HWT)
											{
												this.SendMessage(3, MainMenu.HT_PC_LIMIT_NOTICE);
												await Task.Delay(100);
												this.LiveTeleport(1, 1, 25000, 982f, 140f, 140f);
											}
										}
										catch
										{
										}
										try
										{
											if (MainMenu.JOBT_PC_LIMIT > 0 && AsyncServer.GetHwidCountJobTemple(this.HWID) > MainMenu.JOBT_PC_LIMIT && this.INSIDE_TEMPLE)
											{
												this.SendMessage(3, "You have reached the maxmimum allowed PC(s) to enter JOB TEMPLE world.");
												await Task.Delay(100);
												this.LiveTeleport(1, 1, 25000, 982f, 140f, 140f);
											}
										}
										catch
										{
										}
										try
										{
											if (MainMenu.FGW_PC_LIMIT > 0 && AsyncServer.GetHwidCountFGW(this.HWID) > MainMenu.FGW_PC_LIMIT && this.INSIDE_FGW)
											{
												this.SendMessage(3, MainMenu.FGW_PC_LIMIT_NOTICE);
												await Task.Delay(100);
												this.LiveTeleport(1, 1, 25000, 982f, 140f, 140f);
											}
										}
										catch
										{
										}
										try
										{
											if (MainMenu.JUPITER_PC_LIMIT > 0 && AsyncServer.GetHwidCountJupiter(this.HWID) > MainMenu.JUPITER_PC_LIMIT && this.INSIDE_JUPITER)
											{
												this.SendMessage(3, MainMenu.JUPITER_PC_LIMIT_NOTICE);
												await Task.Delay(100);
												this.LiveTeleport(1, 1, 25000, 982f, 140f, 140f);
											}
										}
										catch
										{
										}
										try
										{
											if (MainMenu.SURVIVAL_PC_LIMIT > 0 && AsyncServer.GetHwidCountSurvival(this.HWID) > MainMenu.SURVIVAL_PC_LIMIT && this.INSIDE_SURVIVAL)
											{
												this.SendMessage(3, MainMenu.SURVIVAL_PC_LIMIT_NOTICE);
												await Task.Delay(100);
												this.LiveTeleport(1, 1, 25000, 982f, 140f, 140f);
											}
										}
										catch
										{
										}
										continue;
									}
								}
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
			MainMenu.WriteLine(1, "AG_ASYNC_RECV_FROM_MODULE" + EX?.ToString() + base.CLIENT_SOCKET?.ToString() + "CHARNAME16_HOLDER");
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
			MainMenu.WriteLine(1, "AG_SendToClient" + ex?.ToString() + base.CLIENT_SOCKET?.ToString() + "CHARNAME16_HOLDER");
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
			MainMenu.WriteLine(1, "AG_ASYNC_SEND_TO_MODULE" + ex?.ToString() + base.PROXY_SOCKET?.ToString() + "CHARNAME16_HOLDER");
		}
	}

	public static void SendPackagetoAll(Packet pck)
	{
		Task.Factory.StartNew(delegate
		{
			try
			{
				if (AsyncServer.AgentConnections.Keys != null && AsyncServer.AgentConnections.Keys.Count > 0)
				{
					foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> x) => x.Value.Charname.Length > 0))
					{
						item.Value.LOCAL_SECURITY.Send(pck);
						item.Value.SendToClient();
					}
					return;
				}
			}
			catch (Exception arg)
			{
				MainMenu.WriteLine(1, $"BroadCastToClients : excption {arg}");
			}
		});
	}

	public static void StartTime_PLAYERSLIST(int minites)
	{
		try
		{
			if (AsyncServer.AgentConnections.Keys == null || AsyncServer.AgentConnections.Keys.Count <= 0)
			{
				return;
			}
			foreach (KeyValuePair<Socket, AgentContext> agentConnection in AsyncServer.AgentConnections)
			{
				foreach (string item in MainMenu.SURV_PLAYERSLIST.ToList())
				{
					_ = item;
					Packet packet;
					packet = new Packet(13522);
					packet.WriteUInt8(8);
					packet.WriteUInt32(minites * 1000);
					agentConnection.Value.LOCAL_SECURITY.Send(packet);
					agentConnection.Value.SendToClient();
				}
			}
		}
		catch
		{
		}
	}

	public static void StartTime_LMS_PLAYERSLIST(int minites)
	{
		try
		{
			if (AsyncServer.AgentConnections.Keys == null || AsyncServer.AgentConnections.Keys.Count <= 0)
			{
				return;
			}
			foreach (KeyValuePair<Socket, AgentContext> agentConnection in AsyncServer.AgentConnections)
			{
				foreach (string item in MainMenu.LMS_PLAYERSLIST.ToList())
				{
					_ = item;
					Packet packet;
					packet = new Packet(13522);
					packet.WriteUInt8(8);
					packet.WriteUInt32(minites * 1000);
					agentConnection.Value.LOCAL_SECURITY.Send(packet);
					agentConnection.Value.SendToClient();
				}
			}
		}
		catch
		{
		}
	}

	public static void SendTimer(int minites)
	{
		try
		{
			if (AsyncServer.AgentConnections.Keys == null || AsyncServer.AgentConnections.Keys.Count <= 0)
			{
				return;
			}
			foreach (KeyValuePair<Socket, AgentContext> agentConnection in AsyncServer.AgentConnections)
			{
				Packet packet;
				packet = new Packet(13522);
				packet.WriteUInt8(8);
				packet.WriteUInt32(minites * 60000);
				agentConnection.Value.LOCAL_SECURITY.Send(packet);
				agentConnection.Value.SendToClient();
			}
		}
		catch
		{
		}
	}

	public static void SendPackagetoCharname_Client(Packet pck, string ThisCharname)
	{
		Task.Factory.StartNew(delegate
		{
			try
			{
				if (AsyncServer.AgentConnections.Keys != null && AsyncServer.AgentConnections.Keys.Count > 0)
				{
					foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> x) => x.Value.Charname == ThisCharname))
					{
						item.Value.LOCAL_SECURITY.Send(pck);
						item.Value.SendToClient();
					}
					return;
				}
			}
			catch (Exception arg)
			{
				MainMenu.WriteLine(1, $"BroadCastToClients : excption {arg}");
			}
		});
	}

	public static void SendPackagetoCharname_Module(Packet pck, string ThisCharname)
	{
		Task.Factory.StartNew(delegate
		{
			try
			{
				if (AsyncServer.AgentConnections.Keys != null && AsyncServer.AgentConnections.Keys.Count > 0)
				{
					foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> x) => x.Value.Charname == ThisCharname))
					{
						item.Value.REMOTE_SECURITY.Send(pck);
						item.Value.SendToModule();
					}
					return;
				}
			}
			catch (Exception arg)
			{
				MainMenu.WriteLine(1, $"BroadCastToClients : excption {arg}");
			}
		});
	}

	public static string GetCharname(uint UNIQUE_ID)
	{
		string result;
		result = "";
		try
		{
			if (AsyncServer.AgentConnections.Keys != null && AsyncServer.AgentConnections.Keys.Count > 0)
			{
				using IEnumerator<KeyValuePair<Socket, AgentContext>> enumerator = AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> x) => x.Value.UNIQUE_ID == UNIQUE_ID).GetEnumerator();
				if (enumerator.MoveNext())
				{
					result = enumerator.Current.Value.Charname;
					return result;
				}
			}
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(1, $"BroadCastToClients : excption {arg}");
		}
		return result;
	}

	public bool Dellay(DateTime Thisdelay, int MainDelay, string messange)
	{
		bool result;
		result = false;
		int num;
		num = Convert.ToInt32(DateTime.Now.Subtract(Thisdelay).TotalSeconds);
		if (num < MainDelay)
		{
			this.SendMessage(3, messange.Replace("{time}", (MainDelay - num).ToString()));
			result = true;
		}
		return result;
	}

	public static void UpgradeItem(int ID, string Codename, string charname)
	{
		Task.Factory.StartNew(delegate
		{
			try
			{
				if (AsyncServer.AgentConnections.Keys != null && AsyncServer.AgentConnections.Keys.Count > 0)
				{
					foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> x) => x.Value.Charname == charname))
					{
						Packet packet;
						packet = new Packet(13568);
						packet.WriteUInt32(item.Value.UNIQUE_ID);
						packet.WriteUInt8(16);
						packet.WriteInt32(ID);
						packet.WriteAscii(Codename);
						item.Value.REMOTE_SECURITY.Send(packet);
						item.Value.SendToModule();
					}
					return;
				}
			}
			catch (Exception arg)
			{
				MainMenu.WriteLine(1, $"BroadCastToClients : excption {arg}");
			}
		});
	}

	public static void ReduceGold(string charname, int status, long Removegold)
	{
		foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> key) => !string.IsNullOrEmpty(key.Value.Charname) && key.Value.Charname.Equals(charname)))
		{
			AgentContext value;
			value = item.Value;
			Packet packet;
			packet = new Packet(13568);
			packet.WriteUInt32(value.UNIQUE_ID);
			packet.WriteUInt8(15);
			packet.WriteUInt8(status);
			packet.WriteInt64(Removegold);
			packet.WriteValue<bool>(true);
			value.REMOTE_SECURITY.Send(packet);
			value.SendToModule();
		}
	}

	public static void SendCape(string charname, int pvpcape)
	{
		foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> key) => !string.IsNullOrEmpty(key.Value.Charname) && key.Value.Charname.Equals(charname)))
		{
			AgentContext value;
			value = item.Value;
			Packet packet;
			packet = new Packet(13568);
			packet.WriteUInt32(value.UNIQUE_ID);
			packet.WriteUInt8(3);
			packet.WriteUInt8(pvpcape);
			value.REMOTE_SECURITY.Send(packet);
			value.SendToModule();
		}
	}

	public static void AddGold(string charname, int gold)
	{
		bool flag;
		flag = false;
		foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> key) => !string.IsNullOrEmpty(key.Value.Charname) && key.Value.Charname.Equals(charname)))
		{
			AgentContext value;
			value = item.Value;
			Packet packet;
			packet = new Packet(13568);
			packet.WriteUInt32(value.UNIQUE_ID);
			packet.WriteUInt8(6);
			packet.WriteInt32(gold);
			packet.WriteValue<bool>(true);
			value.REMOTE_SECURITY.Send(packet);
			value.SendToModule();
			flag = true;
		}
		if (!flag)
		{
			_ = Task.Run(async () => await sqlCon.prod_int($"update  {MainMenu.SHA_DB}.._Char set RemainGold=RemainGold+{gold}  where CharName16='{charname}'")).Result;
			MainMenu.WriteLine(2, "çhar oyunda yok " + gold + "teslim edildi");
		}
	}

	public static void LiveTeleportWithCharName(string charname, int WorldLayerID, int WorldID, ushort RegionID, float x, float y, float z)
	{
		foreach (KeyValuePair<Socket, AgentContext> agentConnection in AsyncServer.AgentConnections)
		{
			if (agentConnection.Value.Charname == charname)
			{
				agentConnection.Value.LiveTeleport(WorldLayerID, WorldID, RegionID, x, y, z);
			}
		}
	}

	public void LiveTeleport(int WorldLayerID, int WorldID, ushort RegionID, float x, float y, float z)
	{
		Packet packet;
		packet = new Packet(13568);
		packet.WriteUInt32(this.UNIQUE_ID);
		packet.WriteUInt8(8);
		packet.WriteInt32(WorldLayerID);
		packet.WriteInt32(WorldID);
		packet.WriteUInt16(RegionID);
		packet.WriteValue<float>(x);
		packet.WriteValue<float>(y);
		packet.WriteValue<float>(z);
		base.REMOTE_SECURITY.Send(packet);
		this.SendToModule();
	}

	public void LiveGrantUpdate(string Grant)
	{
		Packet packet;
		packet = new Packet(13568);
		packet.WriteUInt32(this.UNIQUE_ID);
		packet.WriteUInt8(1);
		packet.WriteAscii(Grant);
		base.REMOTE_SECURITY.Send(packet);
		this.SendToModule();
	}

	public void LiveTitleUpdate(byte ID)
	{
		Packet packet;
		packet = new Packet(13568);
		packet.WriteUInt32(this.UNIQUE_ID);
		packet.WriteUInt8(2);
		packet.WriteUInt8(ID);
		base.REMOTE_SECURITY.Send(packet);
		this.SendToModule();
	}

	public void LiveItem(string Code, int Count, byte Plus, bool randomize_variance, int Storgetype = 0)
	{
		Packet packet;
		packet = new Packet(13568);
		packet.WriteUInt32(this.UNIQUE_ID);
		packet.WriteUInt8(9);
		packet.WriteInt32(Storgetype);
		packet.WriteAscii(Code);
		packet.WriteInt32(Count);
		packet.WriteUInt8(Plus);
		packet.WriteValue<bool>(randomize_variance);
		base.REMOTE_SECURITY.Send(packet);
		this.SendToModule();
	}

	public void Update_Item_Count(byte reverse_slot, ushort reverse_type)
	{
		Packet packet;
		packet = new Packet(28748, encrypted: true);
		packet.WriteUInt8(reverse_slot);
		packet.WriteUInt16(reverse_type);
		packet.WriteUInt8(7);
		packet.WriteUInt32(28u);
		base.REMOTE_SECURITY.Send(packet);
		this.SendToModule();
	}

	public void LoadCustomNpc()
	{
		try
		{
			if (sqlCon.IconNpc == null || sqlCon.IconNpc.Rows.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(8234);
			packet.WriteInt32(sqlCon.IconNpc.Rows.Count);
			foreach (DataRow row in sqlCon.IconNpc.Rows)
			{
				string value;
				value = row["ID"].ToString();
				string value2;
				value2 = row["Charicon"].ToString();
				string value3;
				value3 = row["Price"].ToString();
				string value4;
				value4 = row["Payment"].ToString();
				packet.WriteAscii(value);
				packet.WriteAscii(value2);
				packet.WriteAscii(value3);
				packet.WriteAscii(value4);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Icon left List Packet");
		}
	}

	public void LoadCustomNpcTitleColors()
	{
		try
		{
			if (sqlCon.TitleColorNpc == null || sqlCon.TitleColorNpc.Rows.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(8235);
			packet.WriteInt32(sqlCon.TitleColorNpc.Rows.Count);
			foreach (DataRow row in sqlCon.TitleColorNpc.Rows)
			{
				string value;
				value = row["ID"].ToString();
				string value2;
				value2 = row["ColorName"].ToString();
				int value3;
				value3 = int.Parse(Convert.ToString(row["ColorHexCode"]).Replace("#", ""), NumberStyles.HexNumber);
				string value4;
				value4 = row["Price"].ToString();
				string value5;
				value5 = row["Payment"].ToString();
				packet.WriteAscii(value);
				packet.WriteAscii(value2);
				packet.WriteInt32(value3);
				packet.WriteAscii(value4);
				packet.WriteAscii(value5);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Icon left List Packet");
		}
	}

	public void LoadCustomNpcNameColors()
	{
		try
		{
			if (sqlCon.NameColorNpc == null || sqlCon.NameColorNpc.Rows.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(8236);
			packet.WriteInt32(sqlCon.NameColorNpc.Rows.Count);
			foreach (DataRow row in sqlCon.NameColorNpc.Rows)
			{
				string value;
				value = row["ID"].ToString();
				string value2;
				value2 = row["ColorName"].ToString();
				int value3;
				value3 = int.Parse(Convert.ToString(row["ColorHexCode"]).Replace("#", ""), NumberStyles.HexNumber);
				string value4;
				value4 = row["Price"].ToString();
				string value5;
				value5 = row["Payment"].ToString();
				packet.WriteAscii(value);
				packet.WriteAscii(value2);
				packet.WriteInt32(value3);
				packet.WriteAscii(value4);
				packet.WriteAscii(value5);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Icon left List Packet");
		}
	}

	public void LoadDynamicRanking(int type)
	{
		try
		{
			if (type == 1 && sqlCon.UniqueRank != null && sqlCon.UniqueRank.Rows.Count > 0)
			{
				Packet packet;
				packet = new Packet(6171);
				packet.WriteUInt8(type);
				packet.WriteUInt8(sqlCon.UniqueRank.Rows.Count);
				foreach (DataRow row in sqlCon.UniqueRank.Rows)
				{
					int value;
					value = Convert.ToInt32(row["Rank"]);
					string value2;
					value2 = row["CharName16"].ToString();
					string value3;
					value3 = row["Guild"].ToString();
					string value4;
					value4 = row["Points"].ToString();
					packet.WriteInt32(value);
					packet.WriteUnicode(value2);
					packet.WriteUnicode(value3);
					packet.WriteUnicode(value4);
				}
				base.LOCAL_SECURITY.Send(packet);
				this.SendToClient();
			}
			if (type == 2 && sqlCon.HonorRank != null && sqlCon.HonorRank.Rows.Count > 0)
			{
				Packet packet2;
				packet2 = new Packet(6171);
				packet2.WriteUInt8(type);
				packet2.WriteUInt8(sqlCon.HonorRank.Rows.Count);
				foreach (DataRow row2 in sqlCon.HonorRank.Rows)
				{
					int value5;
					value5 = Convert.ToInt32(row2["Rank"]);
					string value6;
					value6 = row2["CharName"].ToString();
					string value7;
					value7 = row2["GuildName"].ToString();
					string value8;
					value8 = row2["GraduateCount"].ToString();
					packet2.WriteInt32(value5);
					packet2.WriteUnicode(value6);
					packet2.WriteUnicode(value7);
					packet2.WriteUnicode(value8);
				}
				base.LOCAL_SECURITY.Send(packet2);
				this.SendToClient();
			}
			if (type == 3 && sqlCon.TraderRank != null && sqlCon.TraderRank.Rows.Count > 0)
			{
				Packet packet3;
				packet3 = new Packet(6171);
				packet3.WriteUInt8(type);
				packet3.WriteUInt8(sqlCon.TraderRank.Rows.Count);
				foreach (DataRow row3 in sqlCon.TraderRank.Rows)
				{
					int value9;
					value9 = Convert.ToInt32(row3["Rank"]);
					string value10;
					value10 = row3["NickName16"].ToString();
					string value11;
					value11 = row3["GuildName"].ToString();
					string value12;
					value12 = row3["Exp"].ToString();
					packet3.WriteInt32(value9);
					packet3.WriteUnicode(value10);
					packet3.WriteUnicode(value11);
					packet3.WriteUnicode(value12);
				}
				base.LOCAL_SECURITY.Send(packet3);
				this.SendToClient();
			}
			if (type == 4 && sqlCon.HunterRank != null && sqlCon.HunterRank.Rows.Count > 0)
			{
				Packet packet4;
				packet4 = new Packet(6171);
				packet4.WriteUInt8(type);
				packet4.WriteUInt8(sqlCon.HunterRank.Rows.Count);
				foreach (DataRow row4 in sqlCon.HunterRank.Rows)
				{
					int value13;
					value13 = Convert.ToInt32(row4["Rank"]);
					string value14;
					value14 = row4["NickName16"].ToString();
					string value15;
					value15 = row4["GuildName"].ToString();
					string value16;
					value16 = row4["Exp"].ToString();
					packet4.WriteInt32(value13);
					packet4.WriteUnicode(value14);
					packet4.WriteUnicode(value15);
					packet4.WriteUnicode(value16);
				}
				base.LOCAL_SECURITY.Send(packet4);
				this.SendToClient();
			}
			if (type != 5 || sqlCon.ThiefRank == null || sqlCon.ThiefRank.Rows.Count <= 0)
			{
				return;
			}
			Packet packet5;
			packet5 = new Packet(6171);
			packet5.WriteUInt8(type);
			packet5.WriteUInt8(sqlCon.ThiefRank.Rows.Count);
			foreach (DataRow row5 in sqlCon.ThiefRank.Rows)
			{
				int value17;
				value17 = Convert.ToInt32(row5["Rank"]);
				string value18;
				value18 = row5["NickName16"].ToString();
				string value19;
				value19 = row5["GuildName"].ToString();
				string value20;
				value20 = row5["Exp"].ToString();
				packet5.WriteInt32(value17);
				packet5.WriteUnicode(value18);
				packet5.WriteUnicode(value19);
				packet5.WriteUnicode(value20);
			}
			base.LOCAL_SECURITY.Send(packet5);
			this.SendToClient();
		}
		catch
		{
			Console.WriteLine("Rank Paketinde hata.");
		}
	}

	public void AttendanceEvent()
	{
		DataTable result;
		result = Task.Run(async () => await sqlCon.GetList("SELECT * FROM " + MainMenu.FILTER_DB + ".._Attendance WHERE CharName = '" + this.Charname + "'")).Result;
		if (result.Rows.Count <= 0)
		{
			return;
		}
		Packet packet;
		packet = new Packet(8219);
		foreach (DataRow row in result.Rows)
		{
			for (int i = 1; i <= 28; i++)
			{
				packet.WriteInt8(Convert.ToInt32(Convert.ToString(row["Day" + i])));
			}
		}
		base.LOCAL_SECURITY.Send(packet);
		this.SendToClient();
	}

	public static Packet RegStatus(byte Switch)
	{
		Packet packet;
		packet = new Packet(4901);
		packet.WriteUInt8(Switch);
		return packet;
	}

	public void RegisterStatus(byte Switch)
	{
		Packet packet;
		packet = new Packet(4901);
		packet.WriteUInt8(Switch);
		base.LOCAL_SECURITY.Send(packet);
		this.SendToClient();
	}

	public void LoadSavedLocations()
	{
		try
		{
			if (this.SavedLocations.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(6175);
			packet.WriteUInt32(this.SavedLocations.Count);
			foreach (KeyValuePair<byte, SavedLocation> savedLocation in this.SavedLocations)
			{
				packet.WriteUInt8(savedLocation.Key);
				packet.WriteUnicode(savedLocation.Value.LocationName);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading new rew locs");
		}
	}

	public void TokenPacket()
	{
		try
		{
			Packet packet;
			packet = new Packet(4913);
			packet.WriteUInt32(this.CharToken);
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Token Packet");
		}
	}

	public void MenuSettings()
	{
		Packet packet;
		packet = new Packet(4617);
		packet.WriteInt8(MainMenu.AttendanceWnd);
		packet.WriteInt8(MainMenu.EventRegWnd);
		packet.WriteInt8(MainMenu.FacebookWnd);
		packet.WriteInt8(MainMenu.DiscordWnd);
		packet.WriteInt8(MainMenu.ChestWnd);
		packet.WriteAscii(MainMenu.DiscordURL);
		packet.WriteAscii(MainMenu.FacebookURL);
		packet.WriteInt8(MainMenu.EnableMarket);
		base.LOCAL_SECURITY.Send(packet);
		this.SendToClient();
	}

	public void LoadEventSuitRegions()
	{
		try
		{
			if (Utils.EventSuitRegionIds.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(20789);
			packet.WriteUInt32(Utils.EventSuitRegionIds.Count);
			foreach (ushort eventSuitRegionId in Utils.EventSuitRegionIds)
			{
				packet.WriteUInt16(eventSuitRegionId);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from event suits Packet");
		}
	}

	public void LoadChest()
	{
		try
		{
			Packet packet;
			packet = new Packet(20631);
			packet.WriteUInt32(this.CharChest.Count);
			foreach (_CharChest item in this.CharChest)
			{
				packet.WriteAscii(item.ChestID.ToString());
				packet.WriteInt32(item.LineNum);
				packet.WriteAscii(item.NameStrID128);
				packet.WriteAscii(item.CodeName);
				packet.WriteAscii(item.Count.ToString());
				packet.WriteInt8(item.RandomizedStats);
				packet.WriteAscii(item.OptLevel.ToString());
				packet.WriteAscii(item.From);
				packet.WriteAscii(item.RegisterTime.ToShortDateString());
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(1, "Error from Loading Chest Packet");
		}
	}

	public void LoadAttendance()
	{
		try
		{
			if (sqlCon.AttendanceRewards == null || sqlCon.AttendanceRewards.Rows.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(8218);
			packet.WriteInt32(sqlCon.AttendanceRewards.Rows.Count);
			foreach (DataRow row in sqlCon.AttendanceRewards.Rows)
			{
				int num;
				num = Convert.ToInt32(row["ID"]);
				int value;
				value = Convert.ToInt32(row["Days"]);
				int value2;
				value2 = Convert.ToInt32(row["ItemID"]);
				int value3;
				value3 = Convert.ToInt32(row["Unit"]);
				packet.WriteUInt8(num);
				packet.WriteInt32(value);
				packet.WriteInt32(value2);
				packet.WriteInt32(value3);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Attendance Rewards");
		}
	}

	public void SendMessage(int operetor, string messenge)
	{
		base.LOCAL_SECURITY.Send(AgentContext.MessagePacket(operetor, messenge));
		this.SendToClient();
	}

	public static void SendLoginErrorMsg(string Message, uint color, Socket CLIENT_SOCKET)
	{
		try
		{
			if (AsyncServer.AgentConnections.ContainsKey(CLIENT_SOCKET) && !string.IsNullOrEmpty(AsyncServer.AgentConnections[CLIENT_SOCKET].Charname))
			{
				Packet packet;
				packet = new Packet(45059);
				packet.WriteValue<string>(Message);
				packet.WriteValue<uint>(color);
				AsyncServer.AgentConnections[CLIENT_SOCKET].LOCAL_SECURITY.Send(packet);
				AsyncServer.AgentConnections[CLIENT_SOCKET].SendToClient();
			}
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(1, "SendErrorMsg operation has failed. " + ex.Message);
		}
	}

	public static Packet MessagePacket(int operetor, string messenge)
	{
		Packet packet;
		packet = new Packet(6159);
		packet.WriteInt8(operetor);
		packet.WriteUnicode(messenge);
		return packet;
	}

	public void CharLockStatus(byte Switch)
	{
		Packet packet;
		packet = new Packet(20736);
		packet.WriteUInt8(Switch);
		base.LOCAL_SECURITY.Send(packet);
		this.SendToClient();
	}

	public void CheckLockStatus(string strUserID)
	{
		if (MainMenu.ITEM_LOCK)
		{
			try
			{
				this.has_code = Task.Run(async () => await sqlCon.prod_int("EXEC [" + MainMenu.FILTER_DB + "].[dbo].[_GetLock] '" + strUserID + "'")).Result;
				if (this.has_code == 0)
				{
					this.is_locked = false;
					this.CharLockStatus(1);
				}
				else
				{
					this.is_locked = true;
					this.CharLockStatus(2);
				}
				return;
			}
			catch
			{
				return;
			}
		}
		this.is_locked = false;
		this.CharLockStatus(1);
	}

	public void AnalyzerPacket()
	{
		try
		{
			if (sqlCon.Analyzer == null || sqlCon.Analyzer.Rows.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(20739);
			foreach (DataRow row in sqlCon.Analyzer.Rows)
			{
				row["CharName16"].ToString();
				string text;
				text = row["SurvivalKill"].ToString();
				string text2;
				text2 = row["FtwKill"].ToString();
				string text3;
				text3 = row["BattleArenaKills"].ToString();
				string text4;
				text4 = row["JobKills"].ToString();
				string text5;
				text5 = row["GlobalPoints"].ToString();
				string text6;
				text6 = row["HonorPoints"].ToString();
				int.TryParse(text.ToString(), out var result);
				int.TryParse(text2.ToString(), out var result2);
				int.TryParse(text3.ToString(), out var result3);
				int.TryParse(text4.ToString(), out var result4);
				int.TryParse(text5.ToString(), out var result5);
				int.TryParse(text6.ToString(), out var result6);
				packet.WriteUInt32(5010u);
				packet.WriteUInt32(result);
				packet.WriteUInt32(result2);
				packet.WriteUInt32(result3);
				packet.WriteUInt32(result4);
				packet.WriteUInt32(result5);
				packet.WriteUInt32(result6);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Icon List Right Packet");
		}
	}

	public void LoadEventSchedule()
	{
		try
		{
			if (sqlCon.EventSchedule == null || sqlCon.EventSchedule.Rows.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(20740);
			packet.WriteInt32(sqlCon.EventSchedule.Rows.Count);
			foreach (DataRow row in sqlCon.EventSchedule.Rows)
			{
				int num;
				num = Convert.ToInt32(row["ID"]);
				string value;
				value = row["Name"].ToString();
				string value2;
				value2 = row["Day"].ToString();
				string value3;
				value3 = row["Datetime"].ToString();
				packet.WriteAscii(num);
				packet.WriteAscii(value);
				packet.WriteAscii(value2);
				packet.WriteAscii(value3);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Event Schedule");
		}
	}

	public static Packet RemoveCustomTitle(string Charname, string Title)
	{
		Packet packet;
		packet = new Packet(8267);
		packet.WriteUnicode(Charname);
		packet.WriteAscii(Title);
		return packet;
	}

	public static Packet AddToken(int CharToken)
	{
		Packet packet;
		packet = new Packet(4913);
		packet.WriteUInt32(CharToken);
		return packet;
	}

	public static Packet UpdateCustomTitle(string Charname, string title)
	{
		Packet packet;
		packet = new Packet(8266);
		packet.WriteUnicode(Charname);
		packet.WriteUnicode(title);
		return packet;
	}

	public void LoadCustomTitles()
	{
		try
		{
			if (Utils.CustomTitle == null || Utils.CustomTitle.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(8239);
			packet.WriteInt32(Utils.CustomTitle.Count);
			foreach (KeyValuePair<string, string> item in Utils.CustomTitle)
			{
				packet.WriteUnicode(item.Key);
				packet.WriteUnicode(item.Value);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from custom titles");
		}
	}

	public void LoadLeftIconList()
	{
		try
		{
			if (Utils.CharIconListMediaLeft == null || Utils.CharIconListMediaLeft.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(20742);
			packet.WriteInt32(Utils.CharIconListMediaLeft.Count);
			foreach (KeyValuePair<int, string> item in Utils.CharIconListMediaLeft)
			{
				packet.WriteInt32(item.Key);
				packet.WriteAscii(item.Value);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Icon left List Packet");
		}
	}

	public void LoadLeftCharIcons()
	{
		try
		{
			if (Utils.CharIconLeft == null || Utils.CharIconLeft.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(20743);
			packet.WriteInt32(Utils.CharIconLeft.Count);
			foreach (KeyValuePair<string, int> item in Utils.CharIconLeft)
			{
				packet.WriteUnicode(item.Key);
				packet.WriteInt32(item.Value);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Char Icons left Packet");
		}
	}

	public static Packet UpdateLeftIcons(string Charname, int IconID)
	{
		Packet packet;
		packet = new Packet(20744);
		packet.WriteUnicode(Charname);
		packet.WriteInt32(IconID);
		return packet;
	}

	public static Packet RemoveLeftIcons(string Charname)
	{
		Packet packet;
		packet = new Packet(20745);
		packet.WriteUnicode(Charname);
		return packet;
	}

	public void LoadRightIconList()
	{
		try
		{
			if (Utils.CharIconListMediaRight == null || Utils.CharIconListMediaRight.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(20752);
			packet.WriteInt32(Utils.CharIconListMediaRight.Count);
			foreach (KeyValuePair<int, string> item in Utils.CharIconListMediaRight)
			{
				packet.WriteInt32(item.Key);
				packet.WriteAscii(item.Value);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Icon List Right Packet");
		}
	}

	public void LoadRightCharIcons()
	{
		try
		{
			if (Utils.CharIconRight == null || Utils.CharIconRight.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(20753);
			packet.WriteInt32(Utils.CharIconRight.Count);
			foreach (KeyValuePair<string, int> item in Utils.CharIconRight)
			{
				packet.WriteUnicode(item.Key);
				packet.WriteInt32(item.Value);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Char Icons Right Packet");
		}
	}

	public static Packet UpdateRightIcons(string Charname, int IconID)
	{
		Packet packet;
		packet = new Packet(20754);
		packet.WriteUnicode(Charname);
		packet.WriteInt32(IconID);
		return packet;
	}

	public static Packet RemoveRightIcons(string Charname)
	{
		Packet packet;
		packet = new Packet(20755);
		packet.WriteUnicode(Charname);
		return packet;
	}

	public void LoadUpIconList()
	{
		try
		{
			if (Utils.CharIconListMediaUp == null || Utils.CharIconListMediaUp.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(20756);
			packet.WriteInt32(Utils.CharIconListMediaUp.Count);
			foreach (KeyValuePair<int, string> item in Utils.CharIconListMediaUp)
			{
				packet.WriteInt32(item.Key);
				packet.WriteAscii(item.Value);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Icon List Up Packet");
		}
	}

	public void LoadUpCharIcons()
	{
		try
		{
			if (Utils.CharIconUp == null || Utils.CharIconUp.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(20757);
			packet.WriteInt32(Utils.CharIconUp.Count);
			foreach (KeyValuePair<string, int> item in Utils.CharIconUp)
			{
				packet.WriteUnicode(item.Key);
				packet.WriteInt32(item.Value);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Char Icons Up Packet");
		}
	}

	public static Packet UpdateUpIcons(string Charname, int IconID)
	{
		Packet packet;
		packet = new Packet(20758);
		packet.WriteUnicode(Charname);
		packet.WriteInt32(IconID);
		return packet;
	}

	public static Packet RemoveUpIcons(string Charname)
	{
		Packet packet;
		packet = new Packet(20759);
		packet.WriteUnicode(Charname);
		return packet;
	}

	public void LoadCharNameColors()
	{
		try
		{
			if (Utils.CharNameColorList == null || Utils.CharNameColorList.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(20760);
			packet.WriteInt32(Utils.CharNameColorList.Count);
			foreach (KeyValuePair<string, uint> charNameColor in Utils.CharNameColorList)
			{
				packet.WriteUnicode(charNameColor.Key);
				packet.WriteInt32(charNameColor.Value);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Char Name Color Packet");
		}
	}

	public static Packet UpdateCharNameColor(string Charname, uint color)
	{
		Packet packet;
		packet = new Packet(20761);
		packet.WriteUnicode(Charname);
		packet.WriteUInt32(color);
		return packet;
	}

	public static Packet RemoveCharNameColor(string Charname)
	{
		Packet packet;
		packet = new Packet(20768);
		packet.WriteUnicode(Charname);
		return packet;
	}

	public void LoadTitleColors()
	{
		try
		{
			if (Utils.TitleColorList == null || Utils.TitleColorList.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(20769);
			packet.WriteInt32(Utils.TitleColorList.Count);
			foreach (KeyValuePair<string, uint> titleColor in Utils.TitleColorList)
			{
				packet.WriteUnicode(titleColor.Key);
				packet.WriteInt32(titleColor.Value);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Char Name Color Packet");
		}
	}

	public static Packet UpdateTitleColor(string Charname, uint color)
	{
		Packet packet;
		packet = new Packet(20770);
		packet.WriteUnicode(Charname);
		packet.WriteUInt32(color);
		return packet;
	}

	public static Packet RemoveTitleColor(string Charname)
	{
		Packet packet;
		packet = new Packet(20771);
		packet.WriteUnicode(Charname);
		return packet;
	}

	public void LoadNewTitles()
	{
		try
		{
			if (Utils.NewTitles == null || Utils.NewTitles.Count <= 0)
			{
				return;
			}
			Packet packet;
			packet = new Packet(20772);
			packet.WriteInt32(Utils.NewTitles.Count);
			foreach (KeyValuePair<string, string> newTitle in Utils.NewTitles)
			{
				packet.WriteUnicode(newTitle.Key);
				packet.WriteUnicode(newTitle.Value);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "Error from Loading Char Name Color Packet");
		}
	}

	public static Packet LiveUpdateNewTitles(string Charname, int TestID)
	{
		if (!Utils.NewTitles.ContainsKey(Charname))
		{
			Utils.NewTitles.TryAdd(Charname, Utils.RefNewHwan[TestID].TitleName);
		}
		else
		{
			Utils.NewTitles[Charname] = Utils.RefNewHwan[TestID].TitleName;
		}
		Packet packet;
		packet = new Packet(20773);
		packet.WriteUnicode(Charname);
		packet.WriteUnicode(Utils.RefNewHwan[TestID].TitleName);
		return packet;
	}

	public static Packet RemoveNewTitles(string Charname)
	{
		if (Utils.NewTitles.ContainsKey(Charname))
		{
			Utils.NewTitles.TryRemove(Charname, out var _);
		}
		Packet packet;
		packet = new Packet(20774);
		packet.WriteUnicode(Charname);
		return packet;
	}

	public static void AddKill(string Data1)
	{
		if (!Utils.Kills.ContainsKey(Data1))
		{
			Utils.Kills.TryAdd(Data1, 1);
		}
		else
		{
			Utils.Kills[Data1]++;
		}
		AgentContext.KillCounter();
	}

	public static void KillCounter()
	{
		Packet logpacket;
		Task.Factory.StartNew(delegate
		{
			try
			{
				if (Utils.Kills.Count > 0)
				{
					List<KeyValuePair<string, int>> list;
					list = Utils.Kills.OrderByDescending((KeyValuePair<string, int> o) => o.Value).Take(5).ToList();
					logpacket = new Packet(20776);
					logpacket.WriteValue<byte>(list.Count);
					foreach (KeyValuePair<string, int> item in list)
					{
						logpacket.WriteValue<string>(item.Key);
						logpacket.WriteValue<string>(DPSWindow.FormatNumber(item.Value));
					}
					Parallel.ForEach(AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_LMS || x.INSIDE_SURVIVAL), delegate(AgentContext client)
					{
						client.LOCAL_SECURITY.Send(logpacket);
						client.SendToClient();
					});
				}
			}
			catch
			{
			}
		});
	}

	public static void AddFtwPlayerKill(string Data1)
	{
		if (!Utils.FtwPlayerKills.ContainsKey(Data1))
		{
			Utils.FtwPlayerKills.TryAdd(Data1, 1);
		}
		else
		{
			Utils.FtwPlayerKills[Data1]++;
		}
		AgentContext.FtwPlayerKillCounter();
	}

	public static void FtwPlayerKillCounter()
	{
		Packet logpacket;
		Task.Factory.StartNew(delegate
		{
			try
			{
				if (Utils.FtwPlayerKills.Count > 0)
				{
					List<KeyValuePair<string, int>> list;
					list = Utils.FtwPlayerKills.OrderByDescending((KeyValuePair<string, int> o) => o.Value).Take(5).ToList();
					logpacket = new Packet(20777);
					logpacket.WriteValue<byte>(list.Count);
					foreach (KeyValuePair<string, int> item in list)
					{
						logpacket.WriteValue<string>(item.Key);
						logpacket.WriteValue<string>(DPSWindow.FormatNumber(item.Value));
					}
					Parallel.ForEach(AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_FTW), delegate(AgentContext client)
					{
						client.LOCAL_SECURITY.Send(logpacket);
						client.SendToClient();
					});
				}
			}
			catch
			{
			}
		});
	}

	public static void AddFtwGuildKill(string Data1)
	{
		if (!Utils.FtwGuildKills.ContainsKey(Data1))
		{
			Utils.FtwGuildKills.TryAdd(Data1, 1);
		}
		else
		{
			Utils.FtwGuildKills[Data1]++;
		}
		AgentContext.FtwGuildKillCounter();
	}

	public static void FtwGuildKillCounter()
	{
		Packet logpacket;
		Task.Factory.StartNew(delegate
		{
			try
			{
				if (Utils.FtwGuildKills.Count > 0)
				{
					List<KeyValuePair<string, int>> list;
					list = Utils.FtwGuildKills.OrderByDescending((KeyValuePair<string, int> o) => o.Value).Take(5).ToList();
					logpacket = new Packet(20784);
					logpacket.WriteValue<byte>(list.Count);
					foreach (KeyValuePair<string, int> item in list)
					{
						logpacket.WriteValue<string>(item.Key);
						logpacket.WriteValue<string>(DPSWindow.FormatNumber(item.Value));
					}
					Parallel.ForEach(AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_FTW), delegate(AgentContext client)
					{
						client.LOCAL_SECURITY.Send(logpacket);
						client.SendToClient();
					});
				}
			}
			catch
			{
			}
		});
	}

	public static void AddFtwUnionKill(string Data1)
	{
		if (!Utils.FtwUnionKills.ContainsKey(Data1))
		{
			Utils.FtwUnionKills.TryAdd(Data1, 1);
		}
		else
		{
			Utils.FtwUnionKills[Data1]++;
		}
		AgentContext.FtwUnionKillCounter();
	}

	public static void FtwUnionKillCounter()
	{
		Packet logpacket;
		Task.Factory.StartNew(delegate
		{
			try
			{
				if (Utils.FtwUnionKills.Count > 0)
				{
					List<KeyValuePair<string, int>> list;
					list = Utils.FtwUnionKills.OrderByDescending((KeyValuePair<string, int> o) => o.Value).Take(5).ToList();
					logpacket = new Packet(20785);
					logpacket.WriteValue<byte>(list.Count);
					foreach (KeyValuePair<string, int> item in list)
					{
						logpacket.WriteValue<string>(item.Key);
						logpacket.WriteValue<string>(DPSWindow.FormatNumber(item.Value));
					}
					Parallel.ForEach(AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_FTW), delegate(AgentContext client)
					{
						client.LOCAL_SECURITY.Send(logpacket);
						client.SendToClient();
					});
				}
			}
			catch
			{
			}
		});
	}

	public static Packet Getitemlinkinfo(Packet itemlink, uint m_LinkedGlobalSlot, int CharID)
	{
		foreach (DataRow row in Task.Run(async () => await sqlCon.GetList($"select* from {MainMenu.SHA_DB}.._Items join {MainMenu.SHA_DB}.._RefObjCommon on  _Items.GetRefItemID=_RefObjCommon.ID  where ID64=(select ItemID from {MainMenu.SHA_DB}.._Inventory where Slot ={m_LinkedGlobalSlot} and CharID={CharID})")).Result.Rows)
		{
			int ID64;
			ID64 = Convert.ToInt32(row["ID64"]);
			int value;
			value = Convert.ToInt32(row["GetRefItemID"]);
			int num;
			num = Convert.ToInt32(row["TypeID1"]);
			int num2;
			num2 = Convert.ToInt32(row["TypeID2"]);
			int num3;
			num3 = Convert.ToInt32(row["Data"]);
			if (num == 3 && num2 == 1)
			{
				byte value2;
				value2 = Convert.ToByte(row["OptLevel"]);
				long value3;
				value3 = Convert.ToInt64(row["Variance"]);
				byte b;
				b = Convert.ToByte(row["MagParamNum"]);
				itemlink.WriteInt32(0);
				itemlink.WriteInt32(value);
				itemlink.WriteUInt8(value2);
				itemlink.WriteInt64(value3);
				itemlink.WriteInt32(num3);
				itemlink.WriteUInt8(b);
				if (b != 0)
				{
					for (int i = 1; i <= b; i++)
					{
						itemlink.WriteInt64(Convert.ToInt64(row["MagParam" + i]));
					}
				}
				itemlink.WriteUInt8(1);
				DataTable result;
				result = Task.Run(async () => await sqlCon.GetList($"select *from {MainMenu.SHA_DB}.._BindingOptionWithItem where nItemDBID={ID64} and bOptType=1")).Result;
				itemlink.WriteUInt8(result.Rows.Count);
				if (result.Rows.Count != 0)
				{
					int num4;
					num4 = 0;
					foreach (DataRow row2 in result.Rows)
					{
						itemlink.WriteUInt8(num4);
						itemlink.WriteInt32(Convert.ToInt32(row2["nOptID"]));
						itemlink.WriteInt32(Convert.ToInt32(row2["nParam1"]));
						num4++;
					}
				}
				itemlink.WriteUInt8(2);
				DataTable result2;
				result2 = Task.Run(async () => await sqlCon.GetList($"select *from {MainMenu.SHA_DB}.._BindingOptionWithItem where nItemDBID={ID64} and bOptType=2")).Result;
				if (result2.Rows.Count != 0)
				{
					foreach (DataRow row3 in result2.Rows)
					{
						itemlink.WriteUInt8(1);
						itemlink.WriteUInt8(0);
						itemlink.WriteInt32(Convert.ToInt32(row3["nOptID"]));
						itemlink.WriteInt32(Convert.ToInt32(row3["nOptValue"]));
					}
				}
				else
				{
					itemlink.WriteUInt8(0);
				}
			}
			else if (num == 3 && num2 == 3)
			{
				itemlink.WriteInt32(0);
				itemlink.WriteInt32(value);
				itemlink.WriteInt16(num3);
			}
			else if (num == 3 && num2 == 2)
			{
				itemlink.WriteInt32(0);
				itemlink.WriteInt32(value);
				itemlink.WriteInt8(1);
			}
		}
		return itemlink;
	}

	public Packet SendB034(Packet response, _ItemInfo Item)
	{
		response.WriteUInt32(0u);
		response.WriteUInt32(Item.ItemID);
		switch (Item.TypeID2)
		{
		case 1:
		{
			response.WriteUInt8(Item.Plus);
			response.WriteUInt64(Item.Variance);
			response.WriteUInt32(Item.Durability);
			response.WriteUInt8(Item.MagParamNum);
			if (Item.MagParamNum > 0)
			{
				for (int i = 0; i < Item.MagParamNum; i++)
				{
					string text;
					text = Item.MagicOptions[i].ToString("X");
					uint value;
					value = uint.Parse(text.Substring(0, text.Length - 8), NumberStyles.HexNumber);
					response.WriteUInt32(uint.Parse(text.Substring(text.Length - 4), NumberStyles.HexNumber));
					response.WriteUInt32(value);
				}
			}
			response.WriteUInt8(1);
			response.WriteUInt8(Item.SocketOptions.Count);
			for (int j = 0; j < Item.SocketOptions.Count; j++)
			{
				response.WriteUInt8(Item.SocketOptions[j].Slot);
				response.WriteUInt16(Item.SocketOptions[j].ID);
				response.WriteUInt16(Item.SocketOptions[j].Value);
				response.WriteUInt32(Item.SocketOptions[j].nParam);
			}
			response.WriteUInt8(2);
			response.WriteUInt8(Item.AdvanceOptions.Count);
			for (int k = 0; k < Item.AdvanceOptions.Count; k++)
			{
				response.WriteUInt8(Item.AdvanceOptions[k].Slot);
				response.WriteUInt32(Item.AdvanceOptions[k].ID);
				response.WriteUInt32(Item.AdvanceOptions[k].Value);
			}
			break;
		}
		case 2:
			switch (Item.TypeID3)
			{
			case 1:
				response.WriteUInt8(1);
				break;
			case 2:
				response.WriteUInt32(0u);
				break;
			default:
				if (Item.TypeID4 == 3)
				{
					response.WriteInt32(Item.Durability);
				}
				break;
			}
			break;
		case 3:
			response.WriteUInt16(Item.Durability);
			if (Item.TypeID3 == 11)
			{
				if (Item.TypeID4 == 1 || Item.TypeID4 == 2)
				{
					response.WriteUInt8(0);
				}
				else if (Item.TypeID3 == 14 && Item.TypeID4 == 2)
				{
					response.WriteUInt8(0);
				}
			}
			break;
		}
		return response;
	}

	public async void ItemPulsNotice(uint GetRefItemID, int slot, string msg)
	{
		if (!Utils.RefObjCommon.ContainsKey(GetRefItemID))
		{
			return;
		}
		string codename;
		codename = Utils.RefObjCommon[GetRefItemID].CodeName128;
		if (codename.Contains(MainMenu.SOX_PLUS1) || codename.Contains(MainMenu.SOX_PLUS2))
		{
			msg = msg.Replace("{Charname}", this.Charname);
			Packet itemlink;
			itemlink = new Packet(4921);
			itemlink.WriteInt8(3);
			itemlink.WriteUnicode(msg);
			itemlink.WriteUInt32(Convert.ToInt32(65535));
			itemlink.WriteInt32(GetRefItemID);
			new _ItemInfo();
			_ItemInfo Item;
			Item = await sqlCon.Get_BindingInfo_by_Slot(await sqlCon.Get_ItemInfo_by_Slot(slot, this.CharID));
			Utils.ItemLinkInfonum++;
			if (!Utils.ItemLinkInfo.ContainsKey(Utils.ItemLinkInfonum))
			{
				Utils.ItemLinkInfo.TryAdd(Utils.ItemLinkInfonum, Item);
			}
			if (Utils.ItemLinkInfo.ContainsKey(Utils.ItemLinkInfonum))
			{
				itemlink = this.SendB034(itemlink, Utils.ItemLinkInfo[Utils.ItemLinkInfonum]);
			}
			AgentContext.SendPackagetoAll(itemlink);
		}
	}

	public static void UpgradeItem(string charname, int ID, string Codename, uint slot, uint type)
	{
		Task.Factory.StartNew(delegate
		{
			try
			{
				if (AsyncServer.AgentConnections.Keys != null && AsyncServer.AgentConnections.Keys.Count > 0)
				{
					foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> x) => x.Value.Charname == charname))
					{
						Packet packet;
						packet = new Packet(13568);
						packet.WriteUInt32(item.Value.UNIQUE_ID);
						packet.WriteUInt8(16);
						packet.WriteInt32(ID);
						packet.WriteAscii(Codename);
						item.Value.REMOTE_SECURITY.Send(packet);
						item.Value.SendToModule();
						item.Value.UseItem(slot, type);
					}
					return;
				}
			}
			catch (Exception arg)
			{
				MainMenu.WriteLine(1, $"BroadCastToClients : excption {arg}");
			}
		});
	}

	private void UseItem(uint slot1, uint reverse_type)
	{
		Packet packet;
		packet = new Packet(28748, encrypted: true);
		packet.WriteUInt8(slot1);
		packet.WriteUInt16(reverse_type);
		base.REMOTE_SECURITY.Send(packet);
		this.SendToModule();
	}

	public void SendRank(byte type, ConcurrentDictionary<string, DynamicRank> List)
	{
		if (List.Count <= 0)
		{
			return;
		}
		IEnumerable<KeyValuePair<string, DynamicRank>> enumerable;
		enumerable = List.OrderBy((KeyValuePair<string, DynamicRank> Ogrenci) => ((KeyValuePair<string, DynamicRank>)Ogrenci).Value.LineNum).ToList().Take(50);
		Packet packet;
		packet = new Packet(6171);
		packet.WriteUInt8(type);
		if (type == 3 || type == 4 || type == 5)
		{
			if (List.ContainsKey(this.Jobname))
			{
				packet.WriteUInt8(enumerable.ToList().Count + 1);
				packet.WriteInt32(List[this.Jobname].LineNum);
				packet.WriteUnicode(this.Charname);
				packet.WriteUnicode(List[this.Jobname].Guild);
				packet.WriteUnicode(List[this.Jobname].Points);
			}
			else
			{
				packet.WriteUInt8(enumerable.ToList().Count);
			}
		}
		else if (List.ContainsKey(this.Charname))
		{
			packet.WriteUInt8(enumerable.ToList().Count + 1);
			packet.WriteInt32(List[this.Charname].LineNum);
			packet.WriteUnicode(this.Charname);
			packet.WriteUnicode(List[this.Charname].Guild);
			packet.WriteUnicode(List[this.Charname].Points);
		}
		else
		{
			packet.WriteUInt8(enumerable.ToList().Count);
		}
		foreach (KeyValuePair<string, DynamicRank> item in enumerable)
		{
			packet.WriteInt32(item.Value.LineNum);
			packet.WriteUnicode(item.Key);
			packet.WriteUnicode(item.Value.Guild);
			packet.WriteUnicode(item.Value.Points.ToString());
		}
		base.LOCAL_SECURITY.Send(packet);
		this.SendToClient();
	}

	private void GlobalSend(string messange, int slot, int Type)
	{
		Packet packet;
		packet = new Packet(28748, encrypted: true);
		packet.WriteUInt8(slot);
		packet.WriteUInt16(Type);
		packet.WriteAscii(messange);
		base.REMOTE_SECURITY.Send(packet);
		this.SendToModule();
	}

	public void LoadUniqueLog()
	{
		try
		{
			foreach (KeyValuePair<int, UniqueMob> item in Utils.UniqueLog)
			{
				if (item.Value.Status == "")
				{
					Utils.UniqueLog.TryRemove(item.Key, out var _);
				}
			}
			Packet packet;
			packet = new Packet(6170);
			packet.WriteInt8(Utils.UniqueLog.Count);
			foreach (KeyValuePair<int, UniqueMob> item2 in Utils.UniqueLog)
			{
				packet.WriteInt32(item2.Key);
				packet.WriteUnicode(item2.Value.Status);
				packet.WriteUnicode(item2.Value.Killer);
			}
			base.LOCAL_SECURITY.Send(packet);
			this.SendToClient();
		}
		catch
		{
			MainMenu.WriteLine(2, "unuquekilllist");
		}
	}
}
