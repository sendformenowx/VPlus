using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Framework;
using HWANS;
using NetFwTypeLib;
using NewFilter.AgentUtils;
using NewFilter.ItemInfo;
using SQL;

namespace NewFilter;

public static class Utils
{
	public static ConcurrentDictionary<int, RefHWANLevel> RefHwan = new ConcurrentDictionary<int, RefHWANLevel>();

	public static ConcurrentDictionary<int, RefNewHwan> RefNewHwan = new ConcurrentDictionary<int, RefNewHwan>();

	public static ConcurrentDictionary<string, string> NewTitles = new ConcurrentDictionary<string, string>();

	public static ConcurrentDictionary<uint, _RefObjCommon> RefObjCommon = new ConcurrentDictionary<uint, _RefObjCommon>();

	public static ConcurrentDictionary<string, int> Kills = new ConcurrentDictionary<string, int>();

	public static ConcurrentDictionary<string, int> FtwPlayerKills = new ConcurrentDictionary<string, int>();

	public static ConcurrentDictionary<string, int> FtwGuildKills = new ConcurrentDictionary<string, int>();

	public static ConcurrentDictionary<string, int> FtwUnionKills = new ConcurrentDictionary<string, int>();

	public static ConcurrentDictionary<int, _ItemInfo> ItemLinkInfo = new ConcurrentDictionary<int, _ItemInfo>();

	public static ConcurrentDictionary<int, UniqueMob> UniqueLog = new ConcurrentDictionary<int, UniqueMob>();

	public static List<int> Available_Avatar = new List<int>();

	public static int ItemLinkInfonum = 0;

	public static List<string> FLOOD_LIST_AGENT = new List<string>();

	public static List<string> FLOOD_LIST_GATEWAY = new List<string>();

	public static List<string> FLOOD_LIST_DOWNLOAD = new List<string>();

	public static List<string> BlockedZerkForRegion = new List<string>();

	public static ConcurrentDictionary<string, string> BlockedScrollForRegion = new ConcurrentDictionary<string, string>();

	public static List<string> BlockedReverseRegion = new List<string>();

	public static ConcurrentDictionary<string, uint> CharNameColorList = new ConcurrentDictionary<string, uint>();

	public static ConcurrentDictionary<string, uint> TitleColorList = new ConcurrentDictionary<string, uint>();

	public static ConcurrentDictionary<int, string> CharIconListMediaLeft = new ConcurrentDictionary<int, string>();

	public static ConcurrentDictionary<int, string> CharIconListMediaRight = new ConcurrentDictionary<int, string>();

	public static ConcurrentDictionary<int, string> CharIconListMediaUp = new ConcurrentDictionary<int, string>();

	public static ConcurrentDictionary<string, int> CharIconLeft = new ConcurrentDictionary<string, int>();

	public static ConcurrentDictionary<string, int> CharIconRight = new ConcurrentDictionary<string, int>();

	public static ConcurrentDictionary<string, int> CharIconUp = new ConcurrentDictionary<string, int>();

	public static ConcurrentDictionary<string, string> CustomTitle = new ConcurrentDictionary<string, string>();

	public static ConcurrentDictionary<string, string> Tokens = new ConcurrentDictionary<string, string>();

	public static ConcurrentDictionary<string, int> SoxDrop = new ConcurrentDictionary<string, int>();

	public static List<ushort> EventSuitRegionIds = new List<ushort>();

	public static ConcurrentDictionary<string, string> RegionSkillBlock = new ConcurrentDictionary<string, string>();

	public static void Ranklistrefresh()
	{
		MainMenu.WriteLine(3, "Rank list yenilendi");
	}

	public static string SqlInjectFix(string str)
	{
		try
		{
			if (!string.IsNullOrEmpty(str))
			{
				str = str.Replace("'", string.Empty);
				str = str.Replace(";", string.Empty);
				str = str.Replace("-", string.Empty);
				str = str.Replace("\\", string.Empty);
				str = str.Replace("%", string.Empty);
				str = str.Replace("<", string.Empty);
				str = str.Replace(">", string.Empty);
			}
			return str;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"INJECTION_PREFIX failed and returned 0.1 {arg}");
			return string.Empty;
		}
	}

	public static void ExportLog(string methodname, Exception EX, Socket SOCKET = null, string CharName16 = "")
	{
		Task.Factory.StartNew(delegate
		{
			try
			{
			}
			catch (Exception ex)
			{
				MainMenu.WriteLine(1, "ExportLog() failed. " + ex.Message);
			}
		});
	}

	public static int FLOOD_COUNT_DOWNLOAD(string Address)
	{
		try
		{
			int num;
			num = 0;
			foreach (string item in Utils.FLOOD_LIST_DOWNLOAD)
			{
				if (item == Address)
				{
					num++;
				}
			}
			return num + 1;
		}
		catch
		{
			return 0;
		}
	}

	public static int FLOOD_COUNT_AGENT(string Address)
	{
		try
		{
			int num;
			num = 0;
			foreach (string item in Utils.FLOOD_LIST_AGENT)
			{
				if (item == Address)
				{
					num++;
				}
			}
			return num + 1;
		}
		catch
		{
			return 0;
		}
	}

	public static int FLOOD_COUNT_GATEWAY(string Address)
	{
		try
		{
			int num;
			num = 0;
			foreach (string item in Utils.FLOOD_LIST_GATEWAY)
			{
				if (item == Address)
				{
					num++;
				}
			}
			return num + 1;
		}
		catch
		{
			return 0;
		}
	}

	public static void BLOCK_IP(string IP, string Reason)
	{
		Task.Factory.StartNew(delegate
		{
			try
			{
				if (!MainMenu.BanIPList.Contains(IP))
				{
					Type typeFromCLSID;
					typeFromCLSID = Type.GetTypeFromCLSID(new Guid("{E2B3C97F-6AE1-41AC-817A-F6F92166D7DD}"));
					Type typeFromCLSID2;
					typeFromCLSID2 = Type.GetTypeFromCLSID(new Guid("{2C5BC43E-3369-4C33-AB0C-BE9469677AF4}"));
					_ = (INetFwPolicy2)Activator.CreateInstance(typeFromCLSID);
					INetFwRule netFwRule;
					netFwRule = (INetFwRule)Activator.CreateInstance(typeFromCLSID2);
					netFwRule.Name = IP + " Blocked by Predator Filter";
					netFwRule.Description = Reason;
					netFwRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
					netFwRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
					netFwRule.Enabled = true;
					netFwRule.InterfaceTypes = "All";
					netFwRule.RemoteAddresses = IP;
					((INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"))).Rules.Add(netFwRule);
					MainMenu.BanIPList.Add(IP);
					MainMenu.WriteLine(1, "IP:" + IP + " has been blocked via firewall due to rules Reason:" + Reason);
				}
			}
			catch
			{
				MainMenu.WriteLine(1, "Cannot manipulate firewall rules, make sure you run Venom as an administrator!");
			}
		});
	}

	public static void UpdateCounters()
	{
		Task.Factory.StartNew(delegate
		{
		});
	}

	public static double GET_PER_SEC_RATE(ulong TOTAL_TRAFFIC, DateTime ST)
	{
		try
		{
			return Math.Round((double)TOTAL_TRAFFIC / (DateTime.Now - ST).TotalSeconds, 2);
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(1, $"GET_PER_SEC_RATE failed and returned 0.1 {arg}");
			return 0.1;
		}
	}

	public static string Base64Decode(string base64EncodedData)
	{
		return Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
	}

	public static string XOR(string data, char[] key)
	{
		try
		{
			StringBuilder stringBuilder;
			stringBuilder = new StringBuilder(data);
			for (int i = 0; i < stringBuilder.Length; i++)
			{
				stringBuilder[i] = (char)(data[i] ^ key[i]);
			}
			return stringBuilder.ToString();
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(1, $"XOR() failed and returned string.Empty. {arg}");
			return string.Empty;
		}
	}

	public static string DECRYPT_HWID(Packet packet)
	{
		try
		{
			string text;
			text = "WSAStart";
			char[] array;
			array = new char[44]
			{
				'n', 'a', 'z', 't', 'y', 't', 'o', 'a', 'r', 'n',
				'v', 'z', 'r', 'm', 'e', 'n', '1', '2', '3', '4',
				'5', '6', '7', '8', 'v', 'b', 'r', 'g', 'e', 'a',
				'1', 'r', 'z', '4', '5', 'g', '7', 'u', 'n', 'a',
				'z', 't', 'y', 't'
			};
			string text2;
			text2 = Utils.Base64Decode(packet.ReadAscii());
			string text3;
			text3 = Utils.XOR(text2, array);
			if (text2.Length != array.Length)
			{
				MainMenu.WriteLine(1, $"HWID key length error! {text2.Length}.");
				return string.Empty;
			}
			if (!text3.EndsWith(text))
			{
				MainMenu.WriteLine(1, "HWID key endswith error! " + text3);
				return string.Empty;
			}
			return BitConverter.ToString(Encoding.Default.GetBytes(text3.Substring(0, text3.Length - text.Length)));
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(1, $"DECRYPT_HWID() failed and returned string.Empty. {arg}");
			return string.Empty;
		}
	}
}
