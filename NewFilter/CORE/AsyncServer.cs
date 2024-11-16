using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CORE_NETWORKING;
using NewFilter.NetEngine;

namespace NewFilter.CORE;

public static class AsyncServer
{
	public enum MODULE_TYPE
	{
		DownloadServer = 0,
		GatewayServer = 1,
		AgentServer = 2
	}

	public static AutoResetEvent ARE = new AutoResetEvent(initialState: false);

	public static bool ACTIVATION = true;

	public static ConcurrentDictionary<Socket, DownloadComtext> DownloadConnections = new ConcurrentDictionary<Socket, DownloadComtext>();

	public static ConcurrentDictionary<Socket, GatewayContext> GatewayConnections = new ConcurrentDictionary<Socket, GatewayContext>();

	public static ConcurrentDictionary<Socket, AgentContext> AgentConnections = new ConcurrentDictionary<Socket, AgentContext>();

	public static ConcurrentDictionary<Socket, GatewayContext> DISPOSED_GW_SESSIONS = new ConcurrentDictionary<Socket, GatewayContext>();

	public static void InitializeSingleEngine(string BIND_IP, int GW_PORT, int DW_PORT, int AG_PORT)
	{
		try
		{
			CustomSocket GATEWAY_LISTENER;
			GATEWAY_LISTENER = new CustomSocket();
			GATEWAY_LISTENER.Bind(new IPEndPoint(IPAddress.Parse(BIND_IP), GW_PORT));
			GATEWAY_LISTENER.Listen(5);
			CustomSocket DOWNLOAD_LISTENER;
			DOWNLOAD_LISTENER = new CustomSocket();
			DOWNLOAD_LISTENER.Bind(new IPEndPoint(IPAddress.Parse(BIND_IP), DW_PORT));
			DOWNLOAD_LISTENER.Listen(5);
			CustomSocket AGENT_LISTENER;
			AGENT_LISTENER = new CustomSocket();
			AGENT_LISTENER.Bind(new IPEndPoint(IPAddress.Parse(BIND_IP), AG_PORT));
			AGENT_LISTENER.Listen(5);
			if (GATEWAY_LISTENER.IsBound && DOWNLOAD_LISTENER.IsBound && AGENT_LISTENER.IsBound)
			{
				Thread thread;
				thread = new Thread((ThreadStart)delegate
				{
					while (AsyncServer.ACTIVATION)
					{
						DOWNLOAD_LISTENER.AcceptDownloadServerConnections();
						GATEWAY_LISTENER.AcceptGatewayServerConnections();
						AGENT_LISTENER.AcceptAgentServerConnections();
						AsyncServer.ARE.WaitOne();
					}
				});
				thread.Name = "ListenerThread";
				thread.Start();
			}
			MainMenu.WriteLine(1, $"Default asynchronous [{MODULE_TYPE.DownloadServer}] initialized [{BIND_IP}:{DW_PORT}]");
			MainMenu.WriteLine(1, $"Default asynchronous [{MODULE_TYPE.GatewayServer}] initialized [{BIND_IP}:{GW_PORT}]");
			MainMenu.WriteLine(1, $"Default asynchronous [{MODULE_TYPE.AgentServer}] initialized [{BIND_IP}:{AG_PORT}]");
		}
		catch (Exception ex)
		{
			MainMenu.WriteLine(1, "Cannot initialize asynchronous server, proxy initialization failed. " + ex.Message);
		}
	}

	public static void DisconnectFromModule(Socket KEY_CLIENT_SOCKET, MODULE_TYPE MT)
	{
		try
		{
			switch (MT)
			{
			case MODULE_TYPE.DownloadServer:
			{
				if (!AsyncServer.DownloadConnections.TryRemove(KEY_CLIENT_SOCKET, out var value2))
				{
					break;
				}
				if (KEY_CLIENT_SOCKET != null && KEY_CLIENT_SOCKET.Connected)
				{
					KEY_CLIENT_SOCKET.Shutdown(SocketShutdown.Both);
					KEY_CLIENT_SOCKET.Close();
				}
				if (value2.PROXY_SOCKET != null && value2.PROXY_SOCKET.Connected)
				{
					if (value2.PacketTimer != null)
					{
						value2.PacketTimer.Dispose();
						value2.PacketTimer = null;
					}
					Utils.FLOOD_LIST_DOWNLOAD.Remove(value2.SOCKET_IP.Split(':')[0]);
					value2.PROXY_SOCKET.Shutdown(SocketShutdown.Both);
					value2.PROXY_SOCKET.Close();
				}
				Utils.UpdateCounters();
				break;
			}
			case MODULE_TYPE.GatewayServer:
			{
				if (!AsyncServer.GatewayConnections.TryRemove(KEY_CLIENT_SOCKET, out var value3))
				{
					break;
				}
				if (KEY_CLIENT_SOCKET != null && KEY_CLIENT_SOCKET.Connected)
				{
					KEY_CLIENT_SOCKET.Shutdown(SocketShutdown.Both);
					KEY_CLIENT_SOCKET.Close();
				}
				if (value3.PROXY_SOCKET != null && value3.PROXY_SOCKET.Connected)
				{
					if (value3.PacketTimer != null)
					{
						value3.PacketTimer.Dispose();
						value3.PacketTimer = null;
					}
					Utils.FLOOD_LIST_GATEWAY.Remove(value3.SOCKET_IP.Split(':')[0]);
					value3.PROXY_SOCKET.Shutdown(SocketShutdown.Both);
					value3.PROXY_SOCKET.Close();
				}
				Utils.UpdateCounters();
				AsyncServer.DISPOSED_GW_SESSIONS.TryAdd(KEY_CLIENT_SOCKET, value3);
				break;
			}
			case MODULE_TYPE.AgentServer:
			{
				if (MainMenu.SURV_WAR_START)
				{
					foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections.Where((KeyValuePair<Socket, AgentContext> x) => x.Value.CLIENT_SOCKET == KEY_CLIENT_SOCKET))
					{
						string charname;
						charname = item.Value.Charname;
						Task.Run(async delegate
						{
							await sqlCon.EXEC_QUERY("Delete from " + MainMenu.FILTER_DB + ".._Event_Surv where CharName = '" + charname + "'");
						});
					}
				}
				if (!AsyncServer.AgentConnections.TryRemove(KEY_CLIENT_SOCKET, out var value))
				{
					break;
				}
				if (KEY_CLIENT_SOCKET != null && KEY_CLIENT_SOCKET.Connected)
				{
					KEY_CLIENT_SOCKET.Shutdown(SocketShutdown.Both);
					KEY_CLIENT_SOCKET.Close();
				}
				if (value.PROXY_SOCKET != null && value.PROXY_SOCKET.Connected)
				{
					if (value.PacketTimer != null)
					{
						value.PacketTimer.Dispose();
						value.PacketTimer = null;
					}
					Utils.FLOOD_LIST_AGENT.Remove(value.SOCKET_IP.Split(':')[0]);
					value.PROXY_SOCKET.Shutdown(SocketShutdown.Both);
					value.PROXY_SOCKET.Close();
				}
				Utils.UpdateCounters();
				break;
			}
			}
		}
		catch (Exception ex)
		{
			Utils.ExportLog($"DISCONNECT() failed: {ex}", ex);
		}
	}

	public static int GetHWIDCount(string hwid)
	{
		int num;
		num = 0;
		foreach (AgentContext value in AsyncServer.AgentConnections.Values)
		{
			if (value.HWID == hwid)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetHWIDCountJob(string HWID)
	{
		int num;
		num = 0;
		foreach (AgentContext item in AsyncServer.AgentConnections.Values.Where((AgentContext x1) => x1.JobType != 4 || x1.JOB_YELLOW_LINE))
		{
			if (item.HWID == HWID)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetHwidCountBattleArena(string HWID)
	{
		int num;
		num = 0;
		foreach (AgentContext item in AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_BA))
		{
			if (item.HWID == HWID)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetHwidCountCustomRegion(string HWID, string Region)
	{
		int num;
		num = 0;
		foreach (AgentContext item in AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.CurRegion.ToString() == Region))
		{
			if (item.HWID == HWID)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetHwidCountFortress(string HWID)
	{
		int num;
		num = 0;
		foreach (AgentContext item in AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_FTW))
		{
			if (item.HWID == HWID)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetHwidCountSurvival(string HWID)
	{
		int num;
		num = 0;
		foreach (AgentContext item in AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_SURVIVAL))
		{
			if (item.HWID == HWID)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetHwidCountCTF(string HWID)
	{
		int num;
		num = 0;
		foreach (AgentContext item in AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_CTF))
		{
			if (item.HWID == HWID)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetHwidCountFGW(string HWID)
	{
		int num;
		num = 0;
		foreach (AgentContext item in AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_FGW))
		{
			if (item.HWID == HWID)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetHwidCountJupiter(string HWID)
	{
		int num;
		num = 0;
		foreach (AgentContext item in AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_JUPITER))
		{
			if (item.HWID == HWID)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetHwidCountLMS(string HWID)
	{
		int num;
		num = 0;
		foreach (AgentContext item in AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_LMS))
		{
			if (item.HWID == HWID)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetHwidCountJobTemple(string HWID)
	{
		int num;
		num = 0;
		foreach (AgentContext item in AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_TEMPLE))
		{
			if (item.HWID == HWID)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetHwidCountHolyWaterTemple(string HWID)
	{
		int num;
		num = 0;
		foreach (AgentContext item in AsyncServer.AgentConnections.Values.Where((AgentContext x) => x.INSIDE_HWT))
		{
			if (item.HWID == HWID)
			{
				num++;
			}
		}
		return num;
	}
}
