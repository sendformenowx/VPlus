using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Framework;
using NewFilter;
using NewFilter.CORE;
using NewFilter.NetEngine;

namespace Filter;

public class Agent
{
	public byte[] Buffer;

	public Socket Socket;

	public Security Security;

	public Clientlesss Clientless;

	public static Clientlesss Clientless2;

	public bool isExit;

	public uint loginID;

	public string agentIP;

	public ushort agentPort;

	public bool isCreatingCharacter;

	public static bool isHaveEvent = false;

	public bool teleporting = false;

	public bool isVisible = false;

	public static Hashtable spawned_world_ids = new Hashtable();

	public static List<string> QACList = new List<string>();

	public static List<string> MDMac_List = new List<string>();

	public static List<string> ROSMac_List = new List<string>();

	public static List<string> JOBMac_List = new List<string>();

	public static bool EVENT_STATUS = false;

	public static bool TRIVIA_EVENT = false;

	public static bool HNS_EVENT = false;

	public static string TargetCHARNAME16 = string.Empty;

	public void ExitAutoEvent()
	{
		this.isExit = true;
		this.Disconnect();
	}

	public Agent(Clientlesss _clientless, uint _id, string _ip, ushort _port)
	{
		MainMenu.Global.AgentGlobal = this;
		this.Socket = null;
		this.Security = new Security();
		this.Buffer = new byte[4096];
		this.Clientless = _clientless;
		Agent.Clientless2 = _clientless;
		this.isExit = false;
		this.loginID = _id;
		this.agentIP = _ip;
		this.agentPort = _port;
		this.isCreatingCharacter = false;
		try
		{
			this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.Socket.BeginConnect(this.agentIP, this.agentPort, ConnectCallback, null);
		}
		catch (Exception ex)
		{
			if (!this.isExit)
			{
				this.isExit = true;
				MainMenu.WriteLine(3, "[AutoEvent]: Agent connecting error " + ex.ToString());
				this.Disconnect();
			}
		}
	}

	private void ConnectCallback(IAsyncResult ar)
	{
		try
		{
			this.Socket.EndConnect(ar);
		}
		catch (Exception ex)
		{
			if (!this.isExit)
			{
				this.isExit = true;
				MainMenu.WriteLine(3, "[AutoEvent]: Agent connecting error " + ex.ToString());
				this.Disconnect();
			}
		}
		this.BeginReceive();
	}

	private void BeginReceive()
	{
		if (this.isExit)
		{
			return;
		}
		try
		{
			this.Socket.BeginReceive(this.Buffer, 0, 4096, SocketFlags.None, RecieveCallback, null);
		}
		catch (Exception ex)
		{
			if (!this.isExit)
			{
				this.isExit = true;
				MainMenu.WriteLine(3, "[AutoEvent]: Agent connecting error " + ex.ToString());
				this.Disconnect();
			}
		}
	}

	private async void RecieveCallback(IAsyncResult ar)
	{
		int Recieved;
		Recieved = 0;
		try
		{
			if (this.Socket != null)
			{
				Recieved = this.Socket.EndReceive(ar);
			}
		}
		catch (Exception Ex2)
		{
			if (!this.isExit)
			{
				this.isExit = true;
				MainMenu.WriteLine(3, "[AutoEvent]: Agent connecting error " + Ex2.ToString());
				this.Disconnect();
			}
		}
		if (this.isExit)
		{
			return;
		}
		if (Recieved > 0)
		{
			try
			{
				this.Security.Recv(this.Buffer, 0, Recieved);
			}
			catch (Exception Ex3)
			{
				if (!this.isExit)
				{
					this.isExit = true;
					MainMenu.WriteLine(3, "[AutoEvent]: Agent connecting error " + Ex3.ToString());
					this.Disconnect();
				}
			}
			if (this.isExit)
			{
				return;
			}
			try
			{
				List<Packet> packets;
				packets = this.Security.TransferIncoming();
				if (packets != null)
				{
					foreach (Packet _pck in packets)
					{
						if (_pck.Opcode == 13493)
						{
							this.Send(new Packet(13494));
						}
						else if (_pck.Opcode == 12627)
						{
							this.Send(new Packet(29966));
						}
						else if (_pck.Opcode == 46350)
						{
							this.teleporting = false;
						}
						else if (_pck.Opcode == 12307 && !MainMenu.AUTOEVENT_ALREADY)
						{
							MainMenu.AUTOEVENT_ALREADY = true;
							new Thread(EventHandler).Start();
							MainMenu.WriteLine(1, "[AutoEvent]: Spawned In Game Successfully [ " + MainMenu.CLIENT_CharName + " ] ");
						}
						if (_pck.Opcode == 12416)
						{
							new Thread((ThreadStart)delegate
							{
								this.EXCH_parse(_pck);
							}).Start();
						}
						else if (_pck.Opcode == 12300)
						{
							new Thread((ThreadStart)delegate
							{
								this.SpawnUnique_Parse(_pck);
							}).Start();
						}
						else if (_pck.Opcode == 12305)
						{
							if (MainMenu.GMK_STATUS)
							{
								await Task.Delay(1000);
								string KillerName;
								KillerName = Task.Run(async () => await sqlCon.prod_string("Select KillerName from " + MainMenu.FILTER_DB + ".._GMKiller where KilledName like '" + MainMenu.CLIENT_CharName + "'")).Result;
								if (KillerName == "NULL")
								{
									AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, "Somethings is wrongs. Sorry for this round"));
									MainMenu.WriteLine(3, "[AutoEvent]: Error Loading Gm Killer Name ");
								}
								else
								{
									AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.GMK_WIN_NOTICE.Replace("%player%", KillerName).Replace("%reward%", MainMenu.GMK_ITEMNAME.ToString())));
									await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._QuestionReward 0, '{KillerName}', {MainMenu.GMK_ITEMID}, {MainMenu.GMK_ITEMCOUNT}, 0, 0, 'GM Killer Event'");
									await sqlCon.EXEC_QUERY("TRUNCATE TABLE " + MainMenu.FILTER_DB + ".._GMKiller");
									MainMenu.WriteLine(1, "[AutoEvent]: Gm Killer Event has finished! ");
								}
								this.GM_Killer_End();
							}
						}
						else if (_pck.Opcode == 45161)
						{
							if (_pck.GetBytes().Length >= 5)
							{
								_pck.ReadInt8();
								int ptnumber;
								ptnumber = (MainMenu.PT_NR_RECORD = _pck.ReadInt32());
								if (MainMenu.LPN_ENABLE && MainMenu.LPN_TARGETPARTYY == ptnumber)
								{
									foreach (KeyValuePair<Socket, AgentContext> Char in AsyncServer.AgentConnections)
									{
										MainMenu.LPN_ENABLE = false;
										AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LPN_WIN_NOTICE.Replace("%winner%", Char.Value.Charname).Replace("%reward%", MainMenu.LPN_ITEMNAME)));
										this.CloseParty(Convert.ToUInt32(MainMenu.PT_NR_RECORD));
									}
								}
							}
						}
						else if (_pck.Opcode == 12326)
						{
							if (_pck.ReadUInt8() != 2)
							{
								continue;
							}
							string Sender;
							Sender = _pck.ReadAscii();
							string Message;
							Message = _pck.ReadAscii();
							if (!Agent.TRIVIA_EVENT || sqlCon.querycheck(Message))
							{
								continue;
							}
							Message.ToLower();
							string a;
							a = sqlCon.getSingleArray("exec " + MainMenu.FILTER_DB + ".._CheckMyAnswer '" + Sender + "', '" + Message + "'")[0];
							if (a == "1")
							{
								Agent.TRIVIA_EVENT = false;
								if (MainMenu.QNA_ITEMCOUNT > 0)
								{
									AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.QNA_WIN.Replace("%winner%", Sender).Replace("%reward%", MainMenu.QNA_ITEMNAME.ToString()).Replace("%answer%", Message)));
									await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._QuestionReward 0, '{Sender}', {MainMenu.QNA_ITEMREWARD}, {MainMenu.QNA_ITEMCOUNT}, 0, 0, 'Question And Answer Event'");
								}
							}
							else if (a == "0")
							{
								this.SendPM(Sender, "Sorry this is wrong answer");
							}
							continue;
						}
						if (Handler.Agent(this.Clientless, _pck) == Handler.ReturnType.Break)
						{
							break;
						}
					}
				}
			}
			catch (Exception Ex)
			{
				if (!this.isExit)
				{
					this.isExit = true;
					MainMenu.WriteLine(3, "[AutoEvent]: Agent connecting error " + Ex.ToString());
					this.Disconnect();
				}
			}
			if (!this.isExit)
			{
				this.SendToServer();
			}
		}
		else if (!this.isExit)
		{
			this.isExit = true;
			this.Disconnect();
		}
		this.BeginReceive();
	}

	public void SendToServer()
	{
		try
		{
			List<KeyValuePair<TransferBuffer, Packet>> list;
			list = this.Security.TransferOutgoing();
			if (list == null)
			{
				return;
			}
			foreach (KeyValuePair<TransferBuffer, Packet> item in list)
			{
				this.Socket.BeginSend(item.Key.Buffer, item.Key.Offset, item.Key.Size, SocketFlags.None, SendCallback, null);
			}
		}
		catch (Exception ex)
		{
			if (!this.isExit)
			{
				this.isExit = true;
				MainMenu.WriteLine(3, "[AutoEvent]: Agent connecting error " + ex.ToString());
				this.Disconnect();
			}
		}
	}

	private void SendCallback(IAsyncResult ar)
	{
		try
		{
			this.Socket.EndSend(ar);
		}
		catch (Exception ex)
		{
			if (!this.isExit)
			{
				this.isExit = true;
				MainMenu.WriteLine(3, "[AutoEvent]: Agent connecting error " + ex.ToString());
				this.Disconnect();
			}
		}
	}

	public void Disconnect()
	{
		this.Clientless.DC = true;
		MainMenu.WriteLine(1, "[AutoEvent]: Disconnected from Server");
		if (this.Clientless.Mode == ClientlessMode.Agent)
		{
			this.Clientless.Mode = ClientlessMode.None;
		}
		try
		{
			this.Socket.Shutdown(SocketShutdown.Both);
			this.Socket.Close();
		}
		catch
		{
		}
		try
		{
			if (this.Socket != null)
			{
				this.Socket = null;
			}
			if (this.Security != null)
			{
				this.Security = null;
			}
			if (this.Buffer != null)
			{
				this.Buffer = null;
			}
		}
		catch
		{
		}
	}

	private void Send(Packet packet)
	{
		this.Clientless.AG.Security.Send(packet);
	}

	public void EventHandler()
	{
		try
		{
			while (MainMenu.AUTOEVENT_ALREADY)
			{
				string text;
				text = DateTime.Now.ToString("HH:mm");
				string text2;
				text2 = DateTime.Now.DayOfWeek.ToString();
				DataTable result;
				result = Task.Run(async () => await sqlCon.GetList("SELECT * FROM " + MainMenu.FILTER_DB + ".._EventTime WHERE [Service] = 1")).Result;
				if (result == null || result.Rows.Count <= 0)
				{
					continue;
				}
				foreach (DataRow row in result.Rows)
				{
					string text3;
					text3 = row["EventName"].ToString();
					string text4;
					text4 = row["Day"].ToString();
					string text5;
					text5 = row["Hour"].ToString();
					if (Agent.EVENT_STATUS || !(text == text5) || (!(text2 == text4) && !(text4 == "Allday")))
					{
						continue;
					}
					switch (text3)
					{
					case "Question And Answer":
						if (MainMenu.QNA_ENABLE)
						{
							new Thread(Question_And_Answrer_Event).Start();
						}
						else
						{
							MainMenu.WriteLine(3, "[AutoEvent]: Question And Answer Event can't start while other event is running!");
						}
						break;
					case "Hide And Seek":
						if (MainMenu.HNS_ENABLE)
						{
							new Thread(HideAndSeek).Start();
						}
						else
						{
							MainMenu.WriteLine(3, "[AutoEvent]: hide and seek Event can't start while other event is running!");
						}
						break;
					case "GM Killer":
						if (MainMenu.GMK_ENABLE)
						{
							new Thread(GM_Killer_Event).Start();
						}
						else
						{
							MainMenu.WriteLine(3, "[AutoEvent]: gm killer Event can't start while other event is running!");
						}
						break;
					case "Search And Destroy":
						if (MainMenu.SND_ENABLE)
						{
							new Thread(Search_And_Destroy_Event).Start();
						}
						else
						{
							MainMenu.WriteLine(3, "[AutoEvent]: search and destroy Event can't start while other event is running!");
						}
						break;
					case "Lottery Gold":
						if (MainMenu.LG_ENABLE)
						{
							new Thread(Lottery_Gold_Event).Start();
						}
						else
						{
							MainMenu.WriteLine(3, "[AutoEvent]: LG Event can't start while other event is running!");
						}
						break;
					case "Lucky Party Number":
						if (MainMenu.LPN_ENABLE)
						{
							new Thread(Lucky_Party_Number).Start();
						}
						else
						{
							MainMenu.WriteLine(3, "[AutoEvent]: LPN Event can't start while other event is running!");
						}
						break;
					case "Last Man Standing":
						if (MainMenu.LMS_ENABLE)
						{
							new Thread(LMSEventHandler).Start();
						}
						else
						{
							MainMenu.WriteLine(3, "[AutoEvent]: LMS can't start while other event is running!");
						}
						break;
					case "Survival Event":
						if (MainMenu.SURV_ENABLE)
						{
							new Thread(SurvivalEvent).Start();
						}
						else
						{
							MainMenu.WriteLine(3, "[AutoEvent]: Survival Event can't start while other event is running!");
						}
						break;
					}
				}
				Thread.Sleep(59000);
			}
		}
		catch
		{
			MainMenu.WriteLine(1, "error auto event");
		}
	}

	private async void Question_And_Answrer_Event()
	{
		try
		{
			MainMenu.WriteLine(1, MainMenu.QNA_STARTNOTICE);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.QNA_STARTNOTICE));
			await Task.Delay(30000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.QNA_INFONOTICE.Replace("%roundcount%", MainMenu.QNA_ROUNDS.ToString()).Replace("%time%", MainMenu.QNA_TIMETOANSWER.ToString())));
			await Task.Delay(15000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.QNA_INFOCHAR.Replace("%charname%", MainMenu.CLIENT_CharName.ToString())));
			await Task.Delay(15000);
			int Event_Counter;
			Event_Counter = 0;
			while (true)
			{
				Event_Counter++;
				string Question;
				Question = sqlCon.getSingleArray("EXEC " + MainMenu.FILTER_DB + ".._SelectQuest 1")[0];
				await Task.Delay(5000);
				AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.QNA_ROUNDINFO.Replace("%round%", Event_Counter.ToString()).Replace("%question%", Question)));
				Agent.TRIVIA_EVENT = true;
				await Task.Delay(MainMenu.QNA_TIMETOANSWER * 1000);
				Agent.TRIVIA_EVENT = false;
				if (Event_Counter >= MainMenu.QNA_ROUNDS)
				{
					break;
				}
				await Task.Delay(5000);
			}
			await Task.Delay(5000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.QNA_END));
			MainMenu.WriteLine(2, MainMenu.QNA_END);
		}
		catch
		{
			MainMenu.WriteLine(3, "Soru cevap eventi hatalı.");
		}
	}

	private async void HideAndSeek()
	{
		try
		{
			MainMenu.WriteLine(1, MainMenu.HNS_STARTNOTICE);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.HNS_STARTNOTICE));
			await Task.Delay(30000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.HNS_INFONOTICE.Replace("%roundcount%", MainMenu.HNS_ROUNDS.ToString()).Replace("%time%", MainMenu.HNS_TIMETOSEARCH.ToString())));
			int Event_Counter;
			Event_Counter = 0;
			while (true)
			{
				IL_00ff:
				Event_Counter++;
				DataTable read;
				read = Task.Run(async () => await sqlCon.GetList("SELECT top 1* FROM " + MainMenu.FILTER_DB + ".._HideAndSeek_Points ORDER BY NEWID ()")).Result;
				if (read == null || read.Rows.Count <= 0)
				{
					break;
				}
				foreach (DataRow r in read.Rows)
				{
					string RegionID;
					RegionID = Convert.ToString(r["RegionID"]);
					string X;
					X = Convert.ToString(r["X"]);
					string Y;
					Y = Convert.ToString(r["Y"]);
					string Z;
					Z = Convert.ToString(r["Z"]);
					string PlaceName;
					PlaceName = Convert.ToString(r["Place_Name"]);
					await Task.Delay(5000);
					this.Warp(RegionID, X, Y, Z);
					await Task.Delay(20000);
					AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.HNS_PLACEINFO.Replace("%placename%", PlaceName).Replace("%round%", Event_Counter.ToString())));
					await Task.Delay(2000);
					this.Invisible();
					Agent.HNS_EVENT = true;
					await Task.Delay(MainMenu.HNS_TIMETOSEARCH * 1000);
					Agent.HNS_EVENT = false;
					if (Event_Counter < MainMenu.HNS_ROUNDS)
					{
						await Task.Delay(5000);
						goto IL_00ff;
					}
					await Task.Delay(5000);
					AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.HNS_END));
					MainMenu.WriteLine(2, "Hide And Seek Etkinliği sona erdi.!");
				}
				break;
			}
		}
		catch
		{
			MainMenu.WriteLine(3, "[AutoEvent]: Event QA Loading Error");
		}
	}

	public async void GM_Killer_Event()
	{
		try
		{
			MainMenu.WriteLine(1, "[AutoEvent]: GM Killer Event has started!");
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.GMK_START_NOTICE));
			await Task.Delay(30000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.GMK_INFO_NOTICE.Replace("%roundcount%", MainMenu.GMK_ROUND.ToString()).Replace("%time%", MainMenu.GMK_TIMETOWAIT.ToString())));
			await Task.Delay(30000);
			int Event_Counter;
			Event_Counter = 0;
			while (true)
			{
				Event_Counter++;
				this.Warp(MainMenu.GMK_REGIONID, MainMenu.GMK_POSX, MainMenu.GMK_POSY, MainMenu.GMK_POSZ);
				await Task.Delay(5000);
				Packet packet2;
				packet2 = new Packet(29974);
				packet2.WriteUInt8(5);
				this.Security.Send(packet2);
				await Task.Delay(15000);
				AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.GMK_INFORM_NOTICE.Replace("%placename%", MainMenu.GMK_PLACENAME).Replace("%round%", Event_Counter.ToString())));
				await Task.Delay(2000);
				this.Invisible();
				MainMenu.GMK_STATUS = true;
				await Task.Delay(MainMenu.GMK_TIMETOWAIT * 1000);
				MainMenu.GMK_STATUS = false;
				if (Event_Counter >= MainMenu.GMK_ROUND)
				{
					break;
				}
				await Task.Delay(5000);
			}
			await Task.Delay(5000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.GMK_END_NOTICE));
			MainMenu.WriteLine(2, "Hide And Seek Etkinliği sona erdi.!");
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(3, "[AutoEvent]: GM Killer Event start has error! " + ex.ToString());
		}
	}

	public async void Lottery_Gold_Event()
	{
		try
		{
			MainMenu.WriteLine(1, "[AutoEvent]: Lottery Gold Event has started!");
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LG_START_NOTICE));
			await Task.Delay(60000);
			int Event_Counter;
			Event_Counter = 0;
			while (true)
			{
				IL_00e6:
				Event_Counter++;
				await sqlCon.EXEC_QUERY("TRUNCATE TABLE " + MainMenu.FILTER_DB + ".._Event_LG");
				AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LG_TICKETPRICE_NOTICE.Replace("%amount%", DPSWindow.FormatNumber(MainMenu.LG_TICKETPRICE)).Replace("%minute%", (MainMenu.LG_TIMETOWAIT / 60).ToString()).Replace("%round%", Event_Counter.ToString())));
				await Task.Delay(15000);
				MainMenu.LG_REGISTER_STATUS = true;
				AgentContext.SendPackagetoAll(AgentContext.RegStatus(5));
				AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LG_STARTREG_NOTICE));
				await Task.Delay(MainMenu.LG_TIMETOWAIT * 1000);
				AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LG_STOPREG_NOTICE));
				MainMenu.LG_REGISTER_STATUS = false;
				AgentContext.SendPackagetoAll(AgentContext.RegStatus(4));
				await Task.Delay(10000);
				DataTable winner;
				winner = Task.Run(async () => await sqlCon.GetList("EXEC " + MainMenu.FILTER_DB + ".._LG_GetWinner")).Result;
				if (winner == null || winner.Rows.Count <= 0)
				{
					break;
				}
				foreach (DataRow r in winner.Rows)
				{
					string PlayerName;
					PlayerName = Convert.ToString(r["Winner"]);
					int Gold;
					Gold = Convert.ToInt32(r["Gold"]);
					if (PlayerName.Contains("NoWinner"))
					{
						AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LG_END_NOTICE));
						await sqlCon.EXEC_QUERY("TRUNCATE TABLE " + MainMenu.FILTER_DB + ".._Event_LG");
					}
					else
					{
						AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LG_WIN_NOTICE.Replace("%player%", PlayerName).Replace("%amount%", DPSWindow.FormatNumber(Gold))));
						AgentContext.AddGold(PlayerName, Gold);
						await Task.Delay(2000);
						await sqlCon.EXEC_QUERY("TRUNCATE TABLE " + MainMenu.FILTER_DB + ".._Event_LG");
					}
					if (Event_Counter < MainMenu.LG_ROUND)
					{
						await Task.Delay(5000);
						goto IL_00e6;
					}
					await Task.Delay(5000);
					AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LG_ENDD_NOTICE));
					MainMenu.WriteLine(2, "Lottery Gold Etkinliği sona erdi.!");
				}
				break;
			}
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(3, "[AutoEvent]: Error In Lottery Gold Event " + ex.ToString());
		}
	}

	public async void Search_And_Destroy_Event()
	{
		try
		{
			MainMenu.WriteLine(1, MainMenu.SND_STARTNOTICE);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SND_STARTNOTICE));
			await Task.Delay(30000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SND_INFONOTICE.Replace("%roundcount%", MainMenu.SND_ROUNDS.ToString()).Replace("%time%", MainMenu.SND_TIMETOSEARCH.ToString())));
			int Event_Counter;
			Event_Counter = 0;
			while (true)
			{
				IL_00ff:
				Event_Counter++;
				foreach (DataRow r in Task.Run(async () => await sqlCon.GetList("SELECT top 1* FROM " + MainMenu.FILTER_DB + ".._SearchAndDestroyEventRegions ORDER BY NEWID ()")).Result.Rows)
				{
					string RegionID;
					RegionID = Convert.ToString(r["RegionID"]);
					string X;
					X = Convert.ToString(r["X"]);
					string Y;
					Y = Convert.ToString(r["Y"]);
					string Z;
					Z = Convert.ToString(r["Z"]);
					string PlaceName;
					PlaceName = Convert.ToString(r["Place_Name"]);
					int MobID;
					MobID = MainMenu.SND_MOBID;
					this.Warp(RegionID, X, Y, Z);
					await Task.Delay(27000);
					AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, string.Format(arg1: MainMenu.SND_PLACEINFO.Replace("%placename%", PlaceName), format: "Round #{0}, {1}", arg0: Event_Counter)));
					await Task.Delay(3000);
					MainMenu.SND_STATUS = true;
					this.SpawnUnique(MobID, 1);
					await Task.Delay(MainMenu.SND_TIMETOSEARCH * 1000);
					MainMenu.SND_STATUS = false;
					if (Event_Counter < MainMenu.SND_ROUNDS)
					{
						await Task.Delay(5000);
						goto IL_00ff;
					}
					await Task.Delay(5000);
					AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SND_END));
					MainMenu.WriteLine(2, "Hide And Seek Etkinliği sona erdi.!");
				}
				break;
			}
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(3, "[AutoEvent]: Search And Destroy error: " + ex.ToString());
		}
	}

	public async void Lucky_Party_Number()
	{
		MainMenu.WriteLine(1, "[AutoEvent]: Lucky Party Number Event has started");
		AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LPN_START_NOTICE));
		await Task.Delay(50000);
		AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LPN_INFO.Replace("%roundcount%", MainMenu.LPN_ROUNDS.ToString())));
		int Event_Counter;
		Event_Counter = 0;
		while (true)
		{
			Event_Counter++;
			this.CreateParty();
			await Task.Delay(10000);
			MainMenu.LPN_STATUS = true;
			MainMenu.LPN_TARGETPARTYY = new Random().Next(MainMenu.PT_NR_RECORD + MainMenu.PT_MINVALUE, MainMenu.PT_NR_RECORD + MainMenu.PT_MAXVALUE);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LPN_ROUNDINFO.Replace("%time%", MainMenu.LPN_TIMETOWAIT.ToString()).Replace("%targetpartynumber%", MainMenu.LPN_TARGETPARTYY.ToString()).Replace("%round%", Event_Counter.ToString())));
			await Task.Delay(MainMenu.LPN_TIMETOWAIT * 1000);
			MainMenu.LPN_STATUS = false;
			this.CloseParty(Convert.ToUInt32(MainMenu.PT_NR_RECORD));
			if (Event_Counter >= MainMenu.LPN_ROUNDS)
			{
				break;
			}
			await Task.Delay(5000);
		}
		await Task.Delay(5000);
		AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LPN_END));
		MainMenu.WriteLine(1, "[AutoEvent]: Lucky Party Number Event has end. None win");
	}

	public async void LMSEventHandler()
	{
		try
		{
			MainMenu.LMS_PLAYERSLIST.Clear();
			Utils.Kills.Clear();
			await sqlCon.EXEC_QUERY("TRUNCATE TABLE " + MainMenu.FILTER_DB + ".._Event_LMS");
			MainMenu.WriteLine(1, "[AutoEvent]: LMS Event has started!");
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LMS_START_NOTICE));
			await Task.Delay(120000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LMS_INFORM_NOTICE));
			await Task.Delay(120000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LMS_INFO2_NOTICE));
			await Task.Delay(60000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LMS_REGISTERTIME_NOTICE.Replace("{minute}", MainMenu.LMS_REGISTERTIME.ToString())));
			MainMenu.LMS_REGISTERSTATUS = true;
			AgentContext.SendPackagetoAll(AgentContext.RegStatus(1));
			await Task.Delay(MainMenu.LMS_REGISTERTIME * 60000);
			MainMenu.LMS_REGISTERSTATUS = false;
			AgentContext.SendPackagetoAll(AgentContext.RegStatus(0));
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LMS_REGISTERCLOSE_NOTICE));
			await Task.Delay(3000);
			if (MainMenu.LMS_PLAYERSLIST.Count < 2)
			{
				AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LMS_CANCELEVENT_NOTICE));
			}
			else
			{
				MainMenu.LMS_GATE_OPENCLOSE = true;
				MainMenu.LMS_ATTACK_ON_OFF = true;
				AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LMS_GATEOPEN_NOTICE.Replace("%minute%", MainMenu.LMS_GATEWAIT_TIME.ToString())));
			}
			await Task.Delay(MainMenu.LMS_GATEWAIT_TIME * 60000);
			MainMenu.LMS_GATE_OPENCLOSE = false;
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LMS_GATECLOSE_NOTICE));
			foreach (KeyValuePair<Socket, AgentContext> Char2 in AsyncServer.AgentConnections)
			{
				foreach (string charname2 in MainMenu.LMS_PLAYERSLIST.ToList())
				{
					if (MainMenu.LMS_PCLIMIT > 0 && charname2 == Char2.Value.Charname && AsyncServer.GetHwidCountLMS(Char2.Value.HWID) > MainMenu.LMS_PCLIMIT && Char2.Value.INSIDE_LMS)
					{
						AgentContext.SendPackagetoCharname_Client(AgentContext.MessagePacket(1, "WarningYou have reached the maxmimum allowed PC(s) to enter the arena."), Char2.Value.Charname);
						AgentContext.LiveTeleportWithCharName(Char2.Value.Charname, 1, 1, 25000, 982f, 140f, 140f);
					}
				}
			}
			await Task.Delay(10000);
			foreach (KeyValuePair<Socket, AgentContext> Char in AsyncServer.AgentConnections)
			{
				foreach (string item in MainMenu.LMS_PLAYERSLIST.ToList())
				{
					_ = item;
					if (MainMenu.LMS_PLAYERSLIST.Contains(Char.Value.Charname) && !Char.Value.INSIDE_LMS)
					{
						MainMenu.LMS_PLAYERSLIST.Remove(Char.Value.Charname);
					}
				}
			}
			await Task.Delay(3000);
			foreach (string charname in MainMenu.LMS_PLAYERSLIST.ToList())
			{
				await Task.Run(async delegate
				{
					await sqlCon.EXEC_QUERY("INSERT INTO [" + MainMenu.FILTER_DB + "]..[_Event_LMS] Values ('" + charname + "', '1')");
				});
			}
			await Task.Delay(3000);
			new Thread(StartLMS).Start();
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(3, "[AutoEvent]: LMS Event function has error! " + ex.ToString());
		}
	}

	public async void StartLMS()
	{
		DataTable tb;
		tb = Task.Run(async () => await sqlCon.GetList("SELECT CharName FROM " + MainMenu.FILTER_DB + ".._Event_LMS")).Result;
		if (tb.Rows != null && tb.Rows.Count > 1)
		{
			foreach (string item in MainMenu.LMS_PLAYERSLIST.ToList())
			{
				AgentContext.SendCape(item, 5);
			}
			MainMenu.LMS_WAR_START = true;
			await Task.Delay(10000);
			new Thread(RemovePlayerFromList).Start();
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LMS_FIGHTSTART_NOTICE.Replace("%minute", MainMenu.LMS_MATCHTIME.ToString())));
			MainMenu.LMS_ATTACK_ON_OFF = false;
			await Task.Delay(MainMenu.LMS_MATCHTIME * 60000);
			AgentContext.StartTime_LMS_PLAYERSLIST(MainMenu.LMS_MATCHTIME);
			if (!MainMenu.LMS_WAR_START)
			{
				return;
			}
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LMS_END_NOTICE));
			DataTable wtf;
			wtf = Task.Run(async () => await sqlCon.GetList("SELECT CharName FROM " + MainMenu.FILTER_DB + ".._Event_LMS")).Result;
			if (wtf != null && wtf.Rows.Count > 0)
			{
				foreach (DataRow row in wtf.Rows)
				{
					string name;
					name = row["CharName"].ToString();
					MainMenu.LMS_WAR_START = false;
					this.ToTown(name);
				}
				MainMenu.LMS_PLAYERSLIST.Clear();
			}
			await Task.Delay(1000);
			return;
		}
		AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LMS_CANCELEVENT_NOTICE));
		foreach (string item2 in MainMenu.LMS_PLAYERSLIST.ToList())
		{
			this.ToTown(item2);
			MainMenu.LMS_WAR_START = false;
		}
	}

	public async void RemovePlayerFromList()
	{
		List<string> ROS_LivePlayer;
		ROS_LivePlayer = new List<string>();
		try
		{
			while (MainMenu.LMS_WAR_START)
			{
				ROS_LivePlayer.Clear();
				DataTable tb;
				tb = Task.Run(async () => await sqlCon.GetList("SELECT CharName FROM " + MainMenu.FILTER_DB + ".._Event_LMS")).Result;
				if (tb == null || tb.Rows.Count <= 0)
				{
					continue;
				}
				foreach (DataRow row in tb.Rows)
				{
					string name;
					name = row["CharName"].ToString();
					ROS_LivePlayer.Add(name);
					if (tb.Rows.Count == 1)
					{
						MainMenu.LMS_WAR_START = false;
						AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.LMS_WIN_NOTICE.Replace("%player%", name).Replace("%reward%", MainMenu.LMS_ITEMNAME)));
						await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._QuestionReward 0, '{name}', {MainMenu.LMS_ITEMREWARD}, {MainMenu.LMS_ITEMCOUNT}, 0, 0, 'Last Man Standing Event'");
						this.ToTown(name);
					}
				}
				foreach (string charname in MainMenu.LMS_PLAYERSLIST.ToList())
				{
					if (!ROS_LivePlayer.Contains(charname))
					{
						this.ToTown(charname);
						MainMenu.LMS_PLAYERSLIST.Remove(charname);
						AgentContext.SendPackagetoCharname_Client(AgentContext.MessagePacket(4, MainMenu.LMS_ELIMINATED_NOTICE), charname);
					}
				}
			}
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(3, "[AutoEvent]: LMS Check Player Dead function has error! " + ex.ToString());
		}
	}

	public async void SurvivalEvent()
	{
		try
		{
			MainMenu.SURV_PLAYERSLIST.Clear();
			Utils.Kills.Clear();
			await sqlCon.EXEC_QUERY("TRUNCATE TABLE " + MainMenu.FILTER_DB + ".._Event_Surv");
			MainMenu.WriteLine(1, "[AutoEvent]: Survival Event has started!");
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SURV_START_NOTICE));
			await Task.Delay(150000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SURV_INFORM_NOTICE));
			await Task.Delay(150000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SURV_REGISTERTIME_NOTICE.Replace("%minute%", MainMenu.LMS_REGISTERTIME.ToString())));
			MainMenu.SURV_REGISTERSTATUS = true;
			AgentContext.SendPackagetoAll(AgentContext.RegStatus(3));
			await Task.Delay(MainMenu.SURV_REGISTERTIME * 60000);
			MainMenu.SURV_REGISTERSTATUS = false;
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SURV_REGISTERCLOSE_NOTICE));
			AgentContext.SendPackagetoAll(AgentContext.RegStatus(2));
			await Task.Delay(3000);
			if (MainMenu.SURV_PLAYERSLIST.Count < 2)
			{
				AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SURV_CANCELEVENT_NOTICE));
			}
			else
			{
				MainMenu.SURV_GATE_OPENCLOSE = true;
				MainMenu.SURV_ATTACK_ON_OFF = true;
				AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SURV_GATEOPEN_NOTICE.Replace("%minute%", MainMenu.SURV_GATEWAIT_TIME.ToString())));
			}
			await Task.Delay(MainMenu.SURV_GATEWAIT_TIME * 60000);
			MainMenu.SURV_GATE_OPENCLOSE = false;
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SURV_GATECLOSE_NOTICE));
			await Task.Delay(10000);
			foreach (KeyValuePair<Socket, AgentContext> Char in AsyncServer.AgentConnections)
			{
				foreach (string item in MainMenu.SURV_PLAYERSLIST.ToList())
				{
					_ = item;
					if (MainMenu.SURV_PLAYERSLIST.Contains(Char.Value.Charname) && !Char.Value.INSIDE_SURVIVAL)
					{
						MainMenu.SURV_PLAYERSLIST.Remove(Char.Value.Charname);
					}
				}
			}
			await Task.Delay(3000);
			foreach (string charname in MainMenu.SURV_PLAYERSLIST.ToList())
			{
				await Task.Run(async delegate
				{
					await sqlCon.EXEC_QUERY("INSERT INTO [" + MainMenu.FILTER_DB + "]..[_Event_Surv] Values ('" + charname + "', '0')");
				});
			}
			await Task.Delay(3000);
			new Thread(StartSURVI).Start();
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(3, "[AutoEvent]: LMS Event function has error! " + ex.ToString());
		}
	}

	public async void StartSURVI()
	{
		if (Task.Run(async () => await sqlCon.GetList("SELECT CharName FROM " + MainMenu.FILTER_DB + ".._Event_Surv")).Result.Rows.Count > 1)
		{
			foreach (string item in MainMenu.SURV_PLAYERSLIST.ToList())
			{
				AgentContext.SendCape(item, 5);
			}
			MainMenu.SURV_WAR_START = true;
			await Task.Delay(10000);
			AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SURV_FIGHTSTART_NOTICE.Replace("%minute%", MainMenu.SURV_MATCHTIME.ToString())));
			MainMenu.SURV_ATTACK_ON_OFF = false;
			await Task.Delay(MainMenu.SURV_MATCHTIME * 30000);
			AgentContext.StartTime_PLAYERSLIST(MainMenu.SURV_MATCHTIME);
			MainMenu.SURV_WAR_START = false;
			DataTable wtf;
			wtf = Task.Run(async () => await sqlCon.GetList("select  top (3) CharName, Kills from " + MainMenu.FILTER_DB + ".._Event_Surv ORDER BY Kills DESC")).Result;
			int i;
			i = 1;
			if (wtf != null && wtf.Rows.Count > 0)
			{
				foreach (DataRow tbs in wtf.Rows)
				{
					await Task.Delay(2000);
					if (i == 1)
					{
						string name3;
						name3 = tbs["CharName"].ToString();
						string SurvivalKill3;
						SurvivalKill3 = tbs["Kills"].ToString();
						if (Convert.ToInt32(SurvivalKill3) > 0)
						{
							AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, "1st: " + name3 + ", with " + SurvivalKill3 + " kills! Reward : " + MainMenu.SURV_ITEMNAME));
							foreach (KeyValuePair<Socket, AgentContext> Char in AsyncServer.AgentConnections)
							{
								if (Char.Value.Charname == name3)
								{
									await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._AddItemChest 0, '{Char.Value.CharID}', {MainMenu.SURV_ITEMID}, {MainMenu.SURV_ITEMCOUNT}, 0, 0, 'Survival Event'");
								}
							}
						}
					}
					if (i == 2)
					{
						string name2;
						name2 = tbs["CharName"].ToString();
						string SurvivalKill2;
						SurvivalKill2 = tbs["Kills"].ToString();
						if (Convert.ToInt32(SurvivalKill2) > 0)
						{
							AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, "2nd: " + name2 + ", with " + SurvivalKill2 + " kills!"));
						}
					}
					if (i == 3)
					{
						string name;
						name = tbs["CharName"].ToString();
						string SurvivalKill;
						SurvivalKill = tbs["Kills"].ToString();
						if (Convert.ToInt32(SurvivalKill) > 0)
						{
							AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, "3nd: " + name + ", with " + SurvivalKill + " kills!"));
						}
					}
					i++;
				}
			}
			await Task.Delay(2000);
			foreach (string charname in MainMenu.SURV_PLAYERSLIST.ToList())
			{
				if (charname != null)
				{
					this.ToTown(charname);
				}
				await sqlCon.EXEC_QUERY("TRUNCATE TABLE " + MainMenu.FILTER_DB + ".._Event_Surv");
			}
			return;
		}
		AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SURV_CANCELEVENT_NOTICE));
		foreach (string item2 in MainMenu.SURV_PLAYERSLIST.ToList())
		{
			this.ToTown(item2);
			MainMenu.SURV_WAR_START = false;
			await sqlCon.EXEC_QUERY("TRUNCATE TABLE " + MainMenu.FILTER_DB + ".._Event_Surv");
		}
	}

	public async void EXCH_parse(Packet packet)
	{
		try
		{
			byte ExchangeType;
			ExchangeType = packet.ReadUInt8();
			uint ExchangerUniqueID;
			ExchangerUniqueID = packet.ReadUInt32();
			if (ExchangeType != 1)
			{
				return;
			}
			foreach (KeyValuePair<Socket, AgentContext> Char in AsyncServer.AgentConnections)
			{
				if (Agent.HNS_EVENT && Char.Value.UNIQUE_ID == ExchangerUniqueID)
				{
					Agent.HNS_EVENT = false;
					AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.HNS_WIN.Replace("%winner%", Char.Value.Charname).Replace("%reward%", MainMenu.HNS_ITEMNAME.ToString())));
					await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._QuestionReward 0, '{Char.Value.Charname}', {MainMenu.HNS_ITEMREWARD}, {MainMenu.HNS_ITEMCOUNT}, 0, 0, 'Hide And Seek Event'");
					this.Gotown();
				}
			}
		}
		catch
		{
			MainMenu.WriteLine(3, "ERROR EXCHANGE AMK");
		}
	}

	public async void SpawnUnique_Parse(Packet _pck)
	{
		try
		{
			uint SpawnFunc;
			SpawnFunc = _pck.ReadUInt16();
			if (SpawnFunc.ToString("X4") == "0C05")
			{
				_pck.ReadUInt32();
			}
			else if (SpawnFunc.ToString("X4") == "0C06")
			{
				uint MOBID;
				MOBID = _pck.ReadUInt32();
				string PTcharname;
				PTcharname = _pck.ReadAscii();
				int i;
				i = Convert.ToInt32(MOBID);
				if (MainMenu.SND_MOBID == i && MainMenu.SND_STATUS)
				{
					AgentContext.SendPackagetoAll(AgentContext.MessagePacket(1, MainMenu.SND_WIN.Replace("%winner%", PTcharname).Replace("%reward%", MainMenu.SND_ITEMNAME.ToString())));
					await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._QuestionReward 0, '{PTcharname}', {MainMenu.SND_ITEMREWARD}, {MainMenu.SND_ITEMCOUNT}, 0, 0, 'GM Killer Event'");
					this.Gotown();
					MainMenu.WriteLine(1, "[AutoEvent]: Search And Destroy Finished");
				}
			}
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(3, "[AutoEvent]: Error In Search And Destroy Event " + ex.ToString());
		}
	}

	public async void GM_Killer_End()
	{
		Packet pk;
		pk = new Packet(12371);
		pk.WriteUInt8(1);
		this.Security.Send(pk);
		await Task.Delay(3000);
		this.Gotown();
	}

	private void Warp(string RegionID, string x, string y, string z)
	{
		if (!this.isExit)
		{
			this.teleporting = true;
			Packet packet;
			packet = new Packet(28688);
			packet.WriteUInt8(16);
			packet.WriteUInt8(0);
			packet.WriteInt16(RegionID);
			packet.WriteSingle(x);
			packet.WriteSingle(y);
			packet.WriteSingle(z);
			packet.WriteInt8(1);
			packet.WriteUInt8(0);
			this.Send(packet);
		}
	}

	private void Invisible()
	{
		if (!this.isExit)
		{
			if (this.isVisible)
			{
				this.isVisible = false;
			}
			else
			{
				this.isVisible = true;
			}
			Packet packet;
			packet = new Packet(28688);
			packet.WriteUInt16(14);
			this.Send(packet);
		}
	}

	private void SpawnUnique(int mobid, int amount)
	{
		if (!this.isExit)
		{
			Packet packet;
			packet = new Packet(28688);
			packet.WriteUInt8(6);
			packet.WriteUInt8(0);
			packet.WriteUInt32(mobid);
			packet.WriteUInt16(amount);
			packet.WriteUInt8(3);
			this.Security.Send(packet);
		}
	}

	private void CreateParty()
	{
		if (!this.isExit)
		{
			Packet packet;
			packet = new Packet(28777);
			packet.WriteUInt32(0u);
			packet.WriteUInt32(0u);
			packet.WriteUInt8(5);
			packet.WriteUInt8(0);
			packet.WriteUInt8(1);
			packet.WriteUInt8(110);
			packet.WriteAscii("............");
			this.Security.Send(packet);
		}
	}

	private void CloseParty(uint party_number)
	{
		if (!this.isExit)
		{
			Packet packet;
			packet = new Packet(28779);
			packet.WriteUInt32(party_number);
			this.Security.Send(packet);
		}
	}

	public void ToTown(string charname = "")
	{
		if (!this.isExit)
		{
			Thread.Sleep(100);
			Packet packet;
			packet = new Packet(28688);
			packet.WriteUInt16(3);
			packet.WriteAscii(charname);
			this.Security.Send(packet);
		}
	}

	public void Gotown()
	{
		if (!this.isExit)
		{
			Packet packet;
			packet = new Packet(28688, encrypted: true);
			packet.WriteUInt16(3);
			packet.WriteAscii(MainMenu.CLIENT_CharName);
			this.Security.Send(packet);
		}
	}

	private void SendPM(string targetuser, string message)
	{
		if (!this.isExit)
		{
			Packet packet;
			packet = new Packet(28709);
			packet.WriteUInt8(2);
			packet.WriteUInt8(0);
			packet.WriteAscii(targetuser);
			packet.WriteAscii(message);
			this.Security.Send(packet);
		}
	}
}
