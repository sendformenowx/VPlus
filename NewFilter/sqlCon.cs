using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Timers;
using Dapper;
using Framework;
using HWANS;
using NewFilter.Clientless;
using NewFilter.CORE;
using NewFilter.ItemInfo;
using NewFilter.NetEngine;
using Ranks;
using ReverseLoc;
using SQL;

namespace NewFilter;

internal class sqlCon
{
	public static string connectionstring;

	public static DataTable EventSchedule = new DataTable();

	public static DataTable AttendanceRewards = new DataTable();

	public static DataTable UniqueRank = new DataTable();

	public static DataTable HonorRank = new DataTable();

	public static DataTable TraderRank = new DataTable();

	public static DataTable HunterRank = new DataTable();

	public static DataTable ThiefRank = new DataTable();

	public static DataTable IconNpc = new DataTable();

	public static DataTable TitleColorNpc = new DataTable();

	public static DataTable NameColorNpc = new DataTable();

	public static DataTable CustomUniqueNotices = new DataTable();

	public static Timer FilterCommandsTimers;

	public static Timer RankTimer;

	public static DataTable Analyzer = new DataTable();

	public static void SetConnectiontType()
	{
		sqlCon.connectionstring = "Data Source=" + MainMenu.SQL_HOST + "; Initial Catalog= " + MainMenu.SHA_DB + "; Integrated Security=false; User ID = " + MainMenu.SQL_USER + "; Password = " + MainMenu.SQL_PASS + "; Pooling=True";
	}

	public static async void LoadEveryThing()
	{
		sqlCon.EventSchedule = Task.Run(async () => await sqlCon.GetList("Select * from " + MainMenu.FILTER_DB + ".._EventSchedule with(nolock)")).Result;
		sqlCon.UniqueRank = Task.Run(async () => await sqlCon.GetList("SELECT TOP 50 ROW_NUMBER() OVER(ORDER BY Points desc) AS Rank, CharName16, Guild, Points FROM " + MainMenu.FILTER_DB + ".._DynamicUniqueRanking with(nolock) ORDER BY Points DESC")).Result;
		sqlCon.HonorRank = Task.Run(async () => await sqlCon.GetList("SELECT TOP 50 ROW_NUMBER() OVER (ORDER BY GraduateCount DESC) AS Rank,\tC.CharName16 as CharName, E.Name as GuildName, F.GraduateCount FROM " + MainMenu.SHA_DB + ".dbo._TrainingCampHonorRank AS A with(nolock) JOIN " + MainMenu.SHA_DB + ".dbo._TrainingCampMember AS B with(nolock) ON A.CampID = B.CampID JOIN " + MainMenu.SHA_DB + ".dbo._Char AS C with(nolock) ON B.CharID = C.CharID JOIN " + MainMenu.SHA_DB + ".dbo._Guild AS E with(nolock) ON E.ID = C.GuildID JOIN  " + MainMenu.SHA_DB + ".dbo._TrainingCamp AS F with(nolock) ON  B.CampID = F.ID WHERE B.MemberClass = 0 ORDER BY F.GraduateCount DESC")).Result;
		sqlCon.TraderRank = Task.Run(async () => await sqlCon.GetList("SELECT TOP 50 ROW_NUMBER() OVER (ORDER BY X.Exp DESC) AS Rank, Y.NickName16, E.Name as GuildName, X.Exp FROM " + MainMenu.SHA_DB + ".dbo._CharTrijob X with(nolock), " + MainMenu.SHA_DB + ".dbo._Char Y with(nolock) JOIN " + MainMenu.SHA_DB + ".dbo._Guild AS E with(nolock) ON E.ID = GuildID WHERE X.CharID=Y.CharID AND X.JobType=1 ORDER BY X.Exp DESC")).Result;
		sqlCon.HunterRank = Task.Run(async () => await sqlCon.GetList("SELECT TOP 50 ROW_NUMBER() OVER (ORDER BY X.Exp DESC) AS Rank, Y.NickName16, E.Name as GuildName, X.Exp FROM " + MainMenu.SHA_DB + ".dbo._CharTrijob X with(nolock), " + MainMenu.SHA_DB + ".dbo._Char Y with(nolock) JOIN " + MainMenu.SHA_DB + ".dbo._Guild AS E with(nolock) ON E.ID = GuildID WHERE X.CharID=Y.CharID AND X.JobType=3 ORDER BY X.Exp DESC")).Result;
		sqlCon.ThiefRank = Task.Run(async () => await sqlCon.GetList("SELECT TOP 50 ROW_NUMBER() OVER (ORDER BY X.Exp DESC) AS Rank, Y.NickName16, E.Name as GuildName, X.Exp FROM " + MainMenu.SHA_DB + ".dbo._CharTrijob X with(nolock), " + MainMenu.SHA_DB + ".dbo._Char Y with(nolock) JOIN " + MainMenu.SHA_DB + ".dbo._Guild AS E with(nolock) ON E.ID = GuildID WHERE X.CharID=Y.CharID AND X.JobType=2 ORDER BY X.Exp DESC")).Result;
		sqlCon.IconNpc = Task.Run(async () => await sqlCon.GetList("Select * from " + MainMenu.FILTER_DB + ".._StandardCharIconListMedia with(nolock)")).Result;
		sqlCon.TitleColorNpc = Task.Run(async () => await sqlCon.GetList("Select * from " + MainMenu.FILTER_DB + ".._NpcTitleColors with(nolock)")).Result;
		sqlCon.NameColorNpc = Task.Run(async () => await sqlCon.GetList("Select * from " + MainMenu.FILTER_DB + ".._NpcNameColors with(nolock)")).Result;
		sqlCon.CustomUniqueNotices = Task.Run(async () => await sqlCon.GetList("Select * from " + MainMenu.FILTER_DB + ".._StandardUniqueNotices with(nolock)")).Result;
		sqlCon.AttendanceRewards = Task.Run(async () => await sqlCon.GetList("Select * from " + MainMenu.FILTER_DB + ".._StandardAttendanceRewards with(nolock)")).Result;
		await sqlCon.LoadCharNameColorList();
		await sqlCon.LoadTitleColorList();
		await sqlCon.CharIconListMediaPk2Left();
		await sqlCon.CharIconListMediaPk2Right();
		await sqlCon.CharIconListMediaPk2Up();
		await sqlCon.CharIconsLeft();
		await sqlCon.CustomTitles();
		await sqlCon.CharIconsRight();
		await sqlCon.CharIconsUp();
		sqlCon.LoadHwans();
		sqlCon.LoadHwansnew();
		sqlCon.LoadObjects();
		await sqlCon.GetNewTitles();
		sqlCon.Timers();
		await sqlCon.EXEC_QUERY("truncate table " + MainMenu.FILTER_DB + ".dbo._FilterCommands");
		if (!(await sqlCon.Get_wRegionID(AgentContext.FORTRESS_REGIONS, "FORT_")))
		{
			MainMenu.WriteLine(2, "failed from GET Fortress Regions.");
		}
		if (!(await sqlCon.Get_wRegionID(AgentContext.BA_REGIONS, "ARENA_")))
		{
			MainMenu.WriteLine(2, "failed to acquire Get Battle Arena.");
		}
		if (!(await sqlCon.Get_wRegionID(AgentContext.JUPITER_REGIONS, "JUPITER")))
		{
			MainMenu.WriteLine(2, "failed to acquire GET_JUPITERwRegionID.");
		}
		if (!(await sqlCon.Get_wRegionID(AgentContext.FGW_REGIONS, "GOD_")))
		{
			MainMenu.WriteLine(2, "failed to acquire GET_BAwRegionID.");
		}
		if (!(await sqlCon.Get_wRegionID(AgentContext.HWT_REGIONS, "Pharaoh")))
		{
			MainMenu.WriteLine(2, "failed to acquire GET_HWTRegionID.");
		}
		if (!(await sqlCon.Get_wRegionID(AgentContext.TEMPLE_REGIONS, "TEMPLE")))
		{
			MainMenu.WriteLine(2, "failed to acquire GET_TEMPLEwRegionID.");
		}
		if (!(await sqlCon.Get_TeleportID(AgentContext.FW_TELEPORT_ID, "FORT")))
		{
			MainMenu.WriteLine(2, "failed to acquire GET_FWTeleportID.");
		}
		if (!(await sqlCon.Get_TeleportID(AgentContext.TEMPLE_TELEPORT_ID, "TEMPLE")))
		{
			MainMenu.WriteLine(2, "failed to acquire GET_TEMPLETeleportID.");
		}
		if (!(await sqlCon.Get_TeleportID(AgentContext.HWT_TELEPORT_ID, "Pharaoh")))
		{
			MainMenu.WriteLine(2, "failed to acquire GET_HWTTeleportID.");
		}
		if (!(await sqlCon.Get_TeleportID(AgentContext.JUB_TELEPORT_ID, "JUPITER")))
		{
			MainMenu.WriteLine(2, "failed to acquire GET_JUBTeleportID.");
		}
	}

	public static async Task<DataTable> GetList(string query)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			await con.OpenAsync().ConfigureAwait(continueOnCapturedContext: false);
			using SqlCommand command = new SqlCommand(query, con);
			using SqlDataReader read = await command.ExecuteReaderAsync().ConfigureAwait(continueOnCapturedContext: false);
			DataTable dataTable;
			dataTable = new DataTable();
			dataTable.Load(read);
			return dataTable;
		}
		catch (NullReferenceException)
		{
			MainMenu.WriteLine(2, "SQL Error -> Function GetString(" + query + "), exception catched : Null exception");
			return null;
		}
		catch (SqlException ex2)
		{
			MainMenu.WriteLine(2, "SQL Error -> Function GetInt(" + query + "), exception catched : " + ex2.ToString());
		}
		return null;
	}

	public static string clean(string query)
	{
		query = query.Replace("'", string.Empty);
		query = query.Replace("\"", string.Empty);
		return query;
	}

	public static bool querycheck(string query)
	{
		if (query.Contains("'") || query.Contains("\""))
		{
			return true;
		}
		return false;
	}

	public static string[] getSingleArray(string query)
	{
		try
		{
			SqlDataAdapter sqlDataAdapter;
			sqlDataAdapter = new SqlDataAdapter();
			using SqlConnection connection = new SqlConnection(sqlCon.connectionstring);
			sqlDataAdapter.SelectCommand = new SqlCommand(query, connection);
			DataSet dataSet;
			dataSet = new DataSet();
			sqlDataAdapter.Fill(dataSet);
			DataTable dataTable;
			dataTable = dataSet.Tables[0];
			if (dataTable.Rows.Count != 0)
			{
				string[] array;
				array = InitializeArrays.InitStringArray(new string[dataTable.Rows[0].ItemArray.Length]);
				DataRow dataRow;
				dataRow = dataTable.Rows[0];
				for (int i = 0; i < dataTable.Rows[0].ItemArray.Length; i++)
				{
					array[i] = dataRow[i].ToString();
				}
				return array;
			}
		}
		catch (SqlException ex)
		{
			MainMenu.WriteLine(2, "MSSQL Error -> Function getStringArray(" + query + "), exception catched : " + ex.ToString());
		}
		return null;
	}

	public static async Task<long> checkchargold(string UserID)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand("select RemainGold from " + MainMenu.SHA_DB + ".._Char where CharName16 = '" + UserID + "'", con);
			await con.OpenAsync();
			return Convert.ToInt64(await cmd.ExecuteScalarAsync());
		}
		catch
		{
			MainMenu.WriteLine(2, "ReturnHwid opeation failed and returned string.Empty");
			return 0L;
		}
	}

	public static async Task<int> prod_int(string query)
	{
		int value;
		value = 0;
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand(query, con);
			await con.OpenAsync();
			return Convert.ToInt32(await cmd.ExecuteScalarAsync());
		}
		catch (NullReferenceException)
		{
			MainMenu.WriteLine(2, "SQL Error -> Function prod_int(" + query + "), exception catched : Null exception");
			return 0;
		}
		catch (SqlException ex2)
		{
			MainMenu.WriteLine(2, "SQL Error -> Function GetInt(" + query + "), exception catched : " + ex2.ToString());
		}
		return value;
	}

	public static async Task<string> prod_string(string query)
	{
		string value;
		value = "";
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand(query, con);
			await con.OpenAsync();
			return Convert.ToString(await cmd.ExecuteScalarAsync());
		}
		catch (NullReferenceException)
		{
			MainMenu.WriteLine(2, "SQL Error -> Function prod_string(" + query + "), exception catched : Null exception");
			return "";
		}
		catch (SqlException ex2)
		{
			MainMenu.WriteLine(2, "SQL Error -> Function GetInt(" + query + "), exception catched : " + ex2.ToString());
		}
		return value;
	}

	public static async Task EXEC_QUERY(string query)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand(query, con);
			await con.OpenAsync();
			await cmd.ExecuteNonQueryAsync();
		}
		catch
		{
			MainMenu.WriteLine(3, "ERROR QUERY EXEC");
		}
	}

	private static async void FilterCommandsTimer(object source, ElapsedEventArgs e)
	{
		try
		{
			sqlCon.FilterCommandsTimers.Stop();
			try
			{
				await Task.Factory.StartNew((Func<Task>)async delegate
				{
					await sqlCon.FilterCommands();
				});
			}
			catch
			{
			}
			sqlCon.FilterCommandsTimers.Start();
		}
		catch
		{
			MainMenu.WriteLine(1, "FilterCommandsTimer failed.");
		}
	}

	private static async void RankTimers(object source, ElapsedEventArgs e)
	{
		try
		{
			sqlCon.RankTimer.Stop();
			try
			{
				await Task.Run((Func<Task>)delegate {
                    sqlCon.UniqueRank.Clear();
                    sqlCon.HonorRank.Clear();
                    sqlCon.TraderRank.Clear();
                    sqlCon.HunterRank.Clear();
                    sqlCon.ThiefRank.Clear();
                    sqlCon.UniqueRank = Task.Run(async () => await sqlCon.GetList("SELECT TOP 50 ROW_NUMBER() OVER(ORDER BY Points desc) AS Rank, CharName16, Guild, Points FROM " + MainMenu.FILTER_DB + ".._DynamicUniqueRanking with(nolock) ORDER BY Points DESC")).Result;
                    sqlCon.HonorRank = Task.Run(async () => await sqlCon.GetList("SELECT TOP 50 ROW_NUMBER() OVER (ORDER BY GraduateCount DESC) AS Rank,\tC.CharName16 as CharName, E.Name as GuildName, F.GraduateCount FROM " + MainMenu.SHA_DB + ".dbo._TrainingCampHonorRank AS A with(nolock) JOIN " + MainMenu.SHA_DB + ".dbo._TrainingCampMember AS B with(nolock) ON A.CampID = B.CampID JOIN " + MainMenu.SHA_DB + ".dbo._Char AS C with(nolock) ON B.CharID = C.CharID JOIN " + MainMenu.SHA_DB + ".dbo._Guild AS E with(nolock) ON E.ID = C.GuildID JOIN  " + MainMenu.SHA_DB + ".dbo._TrainingCamp AS F with(nolock) ON  B.CampID = F.ID WHERE B.MemberClass = 0 ORDER BY F.GraduateCount DESC")).Result;
                    sqlCon.TraderRank = Task.Run(async () => await sqlCon.GetList("SELECT TOP 50 ROW_NUMBER() OVER (ORDER BY X.Exp DESC) AS Rank, Y.NickName16, E.Name as GuildName, X.Exp FROM " + MainMenu.SHA_DB + ".dbo._CharTrijob X with(nolock), " + MainMenu.SHA_DB + ".dbo._Char Y with(nolock) JOIN " + MainMenu.SHA_DB + ".dbo._Guild AS E with(nolock) ON E.ID = GuildID WHERE X.CharID=Y.CharID AND X.JobType=1 ORDER BY X.Exp DESC")).Result;
                    sqlCon.HunterRank = Task.Run(async () => await sqlCon.GetList("SELECT TOP 50 ROW_NUMBER() OVER (ORDER BY X.Exp DESC) AS Rank, Y.NickName16, E.Name as GuildName, X.Exp FROM " + MainMenu.SHA_DB + ".dbo._CharTrijob X with(nolock), " + MainMenu.SHA_DB + ".dbo._Char Y with(nolock) JOIN " + MainMenu.SHA_DB + ".dbo._Guild AS E with(nolock) ON E.ID = GuildID WHERE X.CharID=Y.CharID AND X.JobType=3 ORDER BY X.Exp DESC")).Result;
                    sqlCon.ThiefRank = Task.Run(async () => await sqlCon.GetList("SELECT TOP 50 ROW_NUMBER() OVER (ORDER BY X.Exp DESC) AS Rank, Y.NickName16, E.Name as GuildName, X.Exp FROM " + MainMenu.SHA_DB + ".dbo._CharTrijob X with(nolock), " + MainMenu.SHA_DB + ".dbo._Char Y with(nolock) JOIN " + MainMenu.SHA_DB + ".dbo._Guild AS E with(nolock) ON E.ID = GuildID WHERE X.CharID=Y.CharID AND X.JobType=2 ORDER BY X.Exp DESC")).Result;
                    return Task.CompletedTask;
                });
			}
			catch
			{
			}
			sqlCon.RankTimer.Start();
		}
		catch
		{
			MainMenu.WriteLine(1, "Rank Timer failed.");
		}
	}

	public static void SendRanks(byte type, ConcurrentDictionary<string, DynamicRank> List)
	{
		Task.Factory.StartNew(delegate
		{
			if (List.Count > 0)
			{
				Packet packet;
				packet = new Packet(6171);
				packet.WriteUInt8(type);
				packet.WriteUInt8(List.Count);
				foreach (KeyValuePair<string, DynamicRank> item in List)
				{
					packet.WriteUInt8(item.Value.LineNum);
					packet.WriteUnicode(item.Key);
					packet.WriteUnicode(item.Value.Guild);
					packet.WriteUnicode(item.Value.Points.ToString());
				}
				AgentContext.SendPackagetoAll(packet);
			}
		});
	}

	public static async Task<bool> FilterCommands()
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using (SqlCommand cmd = new SqlCommand("select top 50* from " + MainMenu.FILTER_DB + ".dbo._FilterCommands with(nolock) order by ID asc", con))
			{
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					int ID;
					ID = Convert.ToInt32(reader[0]);
					byte CommandID;
					CommandID = Convert.ToByte(reader[1]);
					string CommandString;
					CommandString = Convert.ToString(reader[2]);
					string Data1;
					Data1 = Convert.ToString(reader[3]);
					string Data2;
					Data2 = Convert.ToString(reader[4]);
					string Data3;
					Data3 = Convert.ToString(reader[5]);
					string Data4;
					Data4 = Convert.ToString(reader[6]);
					string Data5;
					Data5 = Convert.ToString(reader[7]);
					string Data6;
					Data6 = Convert.ToString(reader[8]);
					string Data7;
					Data7 = Convert.ToString(reader[9]);
					if (Data1.Length == 0)
					{
						sqlCon.DeleteFromFilterCommands(ID);
						MainMenu.WriteLine(1, "data1 hatalı giriş yapıldı deersin :D:D");
						continue;
					}
					try
					{
						switch (CommandID)
						{
						case 1:
						{
							int.TryParse(Data2, out var operatorID);
							if (Data1.ToLower().Contains("sendall"))
							{
								AgentContext.SendPackagetoAll(AgentContext.MessagePacket(operatorID, Data3));
								sqlCon.DeleteFromFilterCommands(ID);
							}
							if (Data1.ToLower().Contains("sendchar") && Data4.Length > 0)
							{
								AgentContext.SendPackagetoCharname_Client(AgentContext.MessagePacket(operatorID, Data3), Data4);
								sqlCon.DeleteFromFilterCommands(ID);
							}
							break;
						}
						case 2:
							if (!Utils.CharNameColorList.ContainsKey(Data1))
							{
								Utils.CharNameColorList.TryAdd(Data1, uint.Parse(Data2.Replace("#", ""), NumberStyles.HexNumber));
							}
							else
							{
								Utils.CharNameColorList[Data1] = uint.Parse(Data2.Replace("#", ""), NumberStyles.HexNumber);
							}
							AgentContext.SendPackagetoAll(AgentContext.UpdateCharNameColor(Data1, uint.Parse(Data2.Replace("#", ""), NumberStyles.HexNumber)));
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						case 3:
							if (Utils.CharNameColorList.ContainsKey(Data1))
							{
								Utils.CharNameColorList.TryRemove(Data1, out var _);
								AgentContext.SendPackagetoAll(AgentContext.RemoveCharNameColor(Data1));
							}
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						case 4:
							if (!Utils.TitleColorList.ContainsKey(Data1))
							{
								Utils.TitleColorList.TryAdd(Data1, uint.Parse(Data2.Replace("#", ""), NumberStyles.HexNumber));
							}
							else
							{
								Utils.TitleColorList[Data1] = uint.Parse(Data2.Replace("#", ""), NumberStyles.HexNumber);
							}
							AgentContext.SendPackagetoAll(AgentContext.UpdateTitleColor(Data1, uint.Parse(Data2.Replace("#", ""), NumberStyles.HexNumber)));
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						case 5:
							if (Utils.TitleColorList.ContainsKey(Data1))
							{
								Utils.TitleColorList.TryRemove(Data1, out var _);
								AgentContext.SendPackagetoAll(AgentContext.RemoveTitleColor(Data1));
							}
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						case 6:
						{
							int.TryParse(Data2, out var TitleID);
							AgentContext.SendPackagetoAll(AgentContext.LiveUpdateNewTitles(Data1, Convert.ToInt32(TitleID)));
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						}
						case 7:
							AgentContext.SendPackagetoAll(AgentContext.RemoveNewTitles(Data1));
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						case 8:
						{
							AgentContext Agent;
							Agent = AsyncServer.AgentConnections.LastOrDefault((KeyValuePair<Socket, AgentContext> x) => x.Value.Charname.ToLower() == Data1.ToLower()).Value;
							await sqlCon.UpdateNewTitles(Agent);
							if (Agent.CharTitlesNew.Count > 0)
							{
								Packet Infonew;
								Infonew = new Packet(20738);
								Infonew.WriteUInt8(Agent.CharTitlesNew.Count);
								foreach (KeyValuePair<byte, string> line in Agent.CharTitlesNew.OrderBy((KeyValuePair<byte, string> x) => x.Key))
								{
									Infonew.WriteUInt8(line.Key);
									Infonew.WriteAscii(line.Value);
								}
								Agent.LOCAL_SECURITY.Send(Infonew);
								Agent.SendToClient();
							}
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						}
						case 9:
						{
							AgentContext Agentt;
							Agentt = AsyncServer.AgentConnections.LastOrDefault((KeyValuePair<Socket, AgentContext> x) => x.Value.Charname.ToLower() == Data1.ToLower()).Value;
							await sqlCon.UpdateHwanList(Agentt);
							if (Agentt.CharTitles.Count > 0)
							{
								Packet Infonew2;
								Infonew2 = new Packet(20737);
								Infonew2.WriteUInt8(Agentt.CharTitles.Count);
								foreach (KeyValuePair<byte, string> line2 in Agentt.CharTitles.OrderBy((KeyValuePair<byte, string> x) => x.Key))
								{
									Infonew2.WriteUInt8(line2.Key);
									Infonew2.WriteAscii(line2.Value);
								}
								Agentt.LOCAL_SECURITY.Send(Infonew2);
								Agentt.SendToClient();
							}
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						}
						case 10:
						{
							AgentContext Agentt2;
							Agentt2 = AsyncServer.AgentConnections.LastOrDefault((KeyValuePair<Socket, AgentContext> x) => x.Value.Charname.ToLower() == Data1.ToLower()).Value;
							byte.TryParse(Data2, out var TitleID2);
							Agentt2.LiveTitleUpdate(TitleID2);
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						}
						case 11:
						{
							int.TryParse(Data2, out var UpIconID);
							if (!Utils.CharIconUp.ContainsKey(Data1))
							{
								Utils.CharIconUp.TryAdd(Data1, UpIconID);
							}
							else
							{
								Utils.CharIconUp[Data1] = UpIconID;
							}
							AgentContext.SendPackagetoAll(AgentContext.UpdateUpIcons(Data1, UpIconID));
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						}
						case 12:
							if (Utils.CharIconUp.ContainsKey(Data1))
							{
								Utils.CharIconUp.TryRemove(Data1, out var _);
								AgentContext.SendPackagetoAll(AgentContext.RemoveUpIcons(Data1));
							}
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						case 13:
						{
							int.TryParse(Data2, out var UpIconID2);
							if (!Utils.CharIconRight.ContainsKey(Data1))
							{
								Utils.CharIconRight.TryAdd(Data1, UpIconID2);
							}
							else
							{
								Utils.CharIconRight[Data1] = UpIconID2;
							}
							AgentContext.SendPackagetoAll(AgentContext.UpdateRightIcons(Data1, UpIconID2));
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						}
						case 14:
							if (Utils.CharIconRight.ContainsKey(Data1))
							{
								Utils.CharIconRight.TryRemove(Data1, out var _);
								AgentContext.SendPackagetoAll(AgentContext.RemoveRightIcons(Data1));
							}
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						case 15:
						{
							int.TryParse(Data2, out var UpIconID3);
							if (!Utils.CharIconLeft.ContainsKey(Data1))
							{
								Utils.CharIconLeft.TryAdd(Data1, UpIconID3);
							}
							else
							{
								Utils.CharIconLeft[Data1] = UpIconID3;
							}
							AgentContext.SendPackagetoAll(AgentContext.UpdateLeftIcons(Data1, UpIconID3));
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						}
						case 16:
							if (Utils.CharIconLeft.ContainsKey(Data1))
							{
								Utils.CharIconLeft.TryRemove(Data1, out var _);
								AgentContext.SendPackagetoAll(AgentContext.RemoveLeftIcons(Data1));
							}
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						case 17:
						{
							int.TryParse(Data1, out var ChestIDs);
							int.TryParse(Data2, out var CharID);
							uint.TryParse(Data3, out var RefItemID);
							int.TryParse(Data4, out var Count);
							byte.TryParse(Data5, out var OptLevel);
							bool.TryParse(Data6, out var RandomStat);
							AgentContext module;
							module = null;
							foreach (KeyValuePair<Socket, AgentContext> item in AsyncServer.AgentConnections)
							{
								if (item.Value.CharID == CharID)
								{
									module = item.Value;
									break;
								}
							}
							if (module != null && Utils.RefObjCommon.ContainsKey(RefItemID))
							{
								string code;
								code = Utils.RefObjCommon[RefItemID].CodeName128;
								_CharChest charChest;
								charChest = new _CharChest
								{
									ChestID = ChestIDs,
									CodeName = code,
									ItemID = RefItemID,
									Count = Count,
									From = Data7,
									NameStrID128 = Utils.RefObjCommon[RefItemID].NameStrID128,
									OptLevel = OptLevel,
									RandomizedStats = RandomStat,
									RegisterTime = DateTime.Now,
									LineNum = module.CharChest.Count + 1
								};
								module.CharChest.Add(charChest);
								Packet Info;
								Info = new Packet(20632);
								Info.WriteAscii(charChest.ChestID.ToString());
								Info.WriteInt32(charChest.LineNum);
								Info.WriteAscii(charChest.NameStrID128);
								Info.WriteAscii(code);
								Info.WriteAscii(charChest.Count.ToString());
								Info.WriteInt8(charChest.RandomizedStats);
								Info.WriteAscii(charChest.OptLevel.ToString());
								Info.WriteAscii(charChest.From);
								Info.WriteAscii(charChest.RegisterTime.ToShortDateString());
								module.LOCAL_SECURITY.Send(Info);
								module.SendToClient();
							}
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						}
						case 18:
						{
							int CharID2;
							CharID2 = int.Parse(Data2);
							uint RefItemIDR;
							RefItemIDR = uint.Parse(Data3);
							AgentContext module3;
							module3 = null;
							foreach (KeyValuePair<Socket, AgentContext> item3 in AsyncServer.AgentConnections)
							{
								if (item3.Value.CharID == CharID2)
								{
									module3 = item3.Value;
									break;
								}
							}
							if (module3 != null && Utils.RefObjCommon.ContainsKey(RefItemIDR))
							{
								_RefObjCommon item2;
								item2 = Utils.RefObjCommon[RefItemIDR];
								foreach (_CharChest chest in module3.CharChest)
								{
									if (chest.ChestID.ToString() == item2.CodeName128)
									{
										module3.CharChest.Remove(chest);
										break;
									}
								}
								Packet removeitemfromchest;
								removeitemfromchest = new Packet(20633);
								removeitemfromchest.WriteUInt8(1);
								removeitemfromchest.WriteAscii(item2.CodeName128);
								module3.LOCAL_SECURITY.Send(removeitemfromchest);
								module3.SendToClient();
							}
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						}
						case 19:
							switch (CommandString)
							{
							case "addkill":
								AgentContext.AddKill(Data1);
								break;
							case "addftwplayerkill":
								AgentContext.AddFtwPlayerKill(Data1);
								break;
							case "addftwguildkill":
								AgentContext.AddFtwGuildKill(Data1);
								break;
							case "addftwunionkill":
								AgentContext.AddFtwUnionKill(Data1);
								break;
							case "clearftw":
								Utils.FtwPlayerKills.Clear();
								Utils.FtwGuildKills.Clear();
								Utils.FtwUnionKills.Clear();
								break;
							case "clearkillcounter":
								Utils.Kills.Clear();
								break;
							}
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						case 20:
							AgentContext.UpgradeItem(CommandString, Convert.ToInt32(Data1), Data2, Convert.ToUInt32(Data3), Convert.ToUInt32(Data4));
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						case 21:
						{
							int.TryParse(Data2, out var TokenID);
							AgentContext module2;
							module2 = null;
							int newtoken;
							newtoken = 0;
							foreach (KeyValuePair<Socket, AgentContext> item4 in AsyncServer.AgentConnections)
							{
								if (item4.Value.Charname == Data1)
								{
									module2 = item4.Value;
									newtoken = item4.Value.CharToken + TokenID;
									item4.Value.CharToken = newtoken;
									break;
								}
							}
							await sqlCon.EXEC_QUERY($"EXEC {MainMenu.FILTER_DB}.._UpdateTokens '{Data1}', {newtoken}");
							if (module2 != null)
							{
								Packet Info2;
								Info2 = new Packet(4913);
								Info2.WriteUInt32(newtoken);
								module2.LOCAL_SECURITY.Send(Info2);
								module2.SendToClient();
							}
							sqlCon.DeleteFromFilterCommands(ID);
							break;
						}
						}
					}
					catch
					{
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"FilterCommands returned false and failed: {arg}");
			return false;
		}
	}

	private static async void DeleteFromFilterCommands(int ID)
	{
		await sqlCon.EXEC_QUERY($"DELETE FROM {MainMenu.FILTER_DB}.dbo._FilterCommands  WHERE ID={ID}");
	}

	public static void InitializeTimerObject(ref Timer T, double TickingInterval, bool OneTime, ElapsedEventHandler ElapsedFunc)
	{
		T = new Timer(TickingInterval);
		T.Elapsed += ElapsedFunc;
		T.AutoReset = !OneTime;
		T.Enabled = true;
	}

	public static void Timers()
	{
		if (sqlCon.FilterCommandsTimers == null)
		{
			sqlCon.InitializeTimerObject(ref sqlCon.FilterCommandsTimers, 1000.0, OneTime: false, FilterCommandsTimer);
		}
		else
		{
			sqlCon.FilterCommandsTimers.Dispose();
			sqlCon.InitializeTimerObject(ref sqlCon.FilterCommandsTimers, 1000.0, OneTime: false, FilterCommandsTimer);
		}
		if (sqlCon.RankTimer == null)
		{
			sqlCon.InitializeTimerObject(ref sqlCon.RankTimer, 21600000.0, OneTime: false, RankTimers);
			return;
		}
		sqlCon.RankTimer.Dispose();
		sqlCon.InitializeTimerObject(ref sqlCon.RankTimer, 21600000.0, OneTime: false, RankTimers);
	}

	public static async Task<int> GetWorldID(int CharID)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand($"SELECT WorldID FROM {MainMenu.SHA_DB}.dbo._Char with(nolock) where CharID = '{CharID}'", con);
			await con.OpenAsync();
			return Convert.ToInt32(await cmd.ExecuteScalarAsync());
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Get_WorldID_by_CharID failed and returned 0: {arg}");
			return 0;
		}
	}

	public static async Task<bool> CharIDHaveInUserInfo(int CharID)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand($"SELECT CharID FROM {MainMenu.FILTER_DB}.dbo._UserInfo with (nolock) where CharID = '{CharID}';", con);
			await con.OpenAsync();
			return !string.IsNullOrEmpty(Convert.ToString(await cmd.ExecuteScalarAsync()));
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Is_CharID_Has_Online_Record returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<int> Get_TelRegionID_by_CharIDD(int CharID)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand($"SELECT TelRegion FROM {MainMenu.SHA_DB}.dbo._Char with (nolock) where CharID = {CharID}", con);
			await con.OpenAsync();
			return Convert.ToInt32(await cmd.ExecuteScalarAsync());
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Get_TelRegionID_by_CharID failed and returned 0: {arg}");
			return 0;
		}
	}

	public static async Task<int> Get_DeadRegionID_by_CharIDD(int CharID)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand($"SELECT DiedRegion FROM {MainMenu.SHA_DB}.dbo._Char with(nolock) where CharID = {CharID}", con);
			await con.OpenAsync();
			return Convert.ToInt32(await cmd.ExecuteScalarAsync());
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Get_DeadRegionID_by_CharID failed and returned 0: {arg}");
			return 0;
		}
	}

	public static async Task InsertPlayerInfo(int CharID, string CharName16, string StrUserID, string IP, string HWID)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand($"INSERT INTO {MainMenu.FILTER_DB}.dbo._UserInfo (CharID,CharName16,StrUserID,IP,HWID,cur_status,last_seen) values ({CharID},'{CharName16}','{StrUserID}','{IP}','{HWID}',1,GETDATE());", con);
			await con.OpenAsync();
			await cmd.ExecuteNonQueryAsync();
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"INSERT_PLAYER_STATUS failed: {arg}");
		}
	}

	public static async Task UpdatePlayerInfo(string StrUserID, bool status, string curIP = "", string curHWID = "")
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand("UPDATE " + MainMenu.FILTER_DB + ".dbo._UserInfo SET cur_status = " + (status ? ("1, HWID = '" + curHWID + "', IP = '" + curIP + "'") : "0") + ", last_seen = GetDate() WHERE StrUserID = '" + StrUserID + "';", con);
			await con.OpenAsync();
			await cmd.ExecuteNonQueryAsync();
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"UPDATE_PLAYER_STATUS failed: {arg}");
		}
	}

	public static async Task<string> ReturnHWID(int CharID)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand($"Select HWID FROM {MainMenu.FILTER_DB}.dbo._UserInfo with(nolock) where CharID = '{CharID}'", con);
			await con.OpenAsync();
			return Convert.ToString(await cmd.ExecuteScalarAsync());
		}
		catch
		{
			MainMenu.WriteLine(2, "ReturnHwid opeation failed and returned string.Empty");
			return string.Empty;
		}
	}

	public static async Task<bool> Get_TeleportID(List<uint> LIST, string keyword)
	{
		try
		{
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select distinct TargetTeleport from " + MainMenu.SHA_DB + ".dbo._RefTeleLink with (nolock) WHERE OwnerTeleport IN (Select ID from " + MainMenu.SHA_DB + ".dbo._RefTeleport where CodeName128 like '%" + keyword + "%' AND Service = 1)", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					LIST.Add(Convert.ToUInt32(reader[0]));
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Get_TeleportID returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> Get_wRegionID(List<short> LIST, string keyword)
	{
		try
		{
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select distinct wRegionID from " + MainMenu.SHA_DB + ".dbo._RefRegion with(nolock) where ContinentName like '%" + keyword + "%';", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					short.TryParse(reader[0].ToString(), out var Region);
					LIST.Add(Region);
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Get_wRegionID returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> Get_wRegionID(List<string> LIST, string keyword, bool T = false)
	{
		try
		{
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select distinct wRegionID from " + MainMenu.SHA_DB + ".dbo._RefRegion  with (nolock) where ContinentName like '" + keyword + "';", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					LIST.Add(reader[0].ToString());
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Get_wRegionID returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<string> GlobalColor(string ITEMID)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand("select ColorCode from " + MainMenu.FILTER_DB + ".._StandardGlobalColor where GlobalID='" + ITEMID + "'", con);
			await con.OpenAsync();
			string Basic_Code;
			Basic_Code = Convert.ToString(await cmd.ExecuteScalarAsync());
			if (Basic_Code == "")
			{
				Basic_Code = "#ffe600";
			}
			con.Close();
			return Basic_Code;
		}
		catch (SqlException ex)
		{
			MainMenu.WriteLine(2, "SQL Error -> Function GetInt(), exception catched : " + ex.ToString());
			return "#0";
		}
	}

	public static async Task<string> CharJobName(string Charname)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand("Select NickName16 from " + MainMenu.SHA_DB + ".dbo._Char where CharName16 = '" + Charname + "';", con);
			await con.OpenAsync();
			string Basic_Code;
			Basic_Code = Convert.ToString(await cmd.ExecuteScalarAsync());
			con.Close();
			return Basic_Code;
		}
		catch
		{
			MainMenu.WriteLine(1, "SP_BALANCE operation has failed and returned 0");
			return string.Empty;
		}
	}

	public static async Task<int> Get_CharID_by_CharName16(string CharName16)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand("Select CharID from " + MainMenu.SHA_DB + ".dbo._Char with(nolock) where CharName16 = '" + CharName16 + "';", con);
			await con.OpenAsync();
			return Convert.ToInt32(await cmd.ExecuteScalarAsync());
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Get_CharID_by_CharName16 failed and returned 0: {arg}");
			return 0;
		}
	}

	public static async Task<string> GetRefItemID(string CharName16, string slot)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand("select RefItemID from " + MainMenu.SHA_DB + ".._Items where ID64 in (select ItemID from " + MainMenu.SHA_DB + ".._Inventory where CharID in(select CharID from " + MainMenu.SHA_DB + ".._Char where CharName16='" + CharName16 + "') and Slot='" + slot + "');", con);
			await con.OpenAsync();
			string Basic_Code;
			Basic_Code = Convert.ToString(await cmd.ExecuteScalarAsync());
			con.Close();
			return Basic_Code;
		}
		catch (SqlException ex)
		{
			MainMenu.WriteLine(2, "SQL Error -> Function GetInt(), exception catched : " + ex.ToString());
			return string.Empty;
		}
	}

	public static async Task<string> GetOptLevel(string CharName16, string slot)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand("select OptLevel from " + MainMenu.SHA_DB + ".._Items where ID64 in (select ItemID from " + MainMenu.SHA_DB + ".._Inventory where CharID in(select CharID from " + MainMenu.SHA_DB + ".._Char where CharName16='" + CharName16 + "') and Slot='" + slot + "');", con);
			await con.OpenAsync();
			string Basic_Code;
			Basic_Code = Convert.ToString(await cmd.ExecuteScalarAsync());
			con.Close();
			return Basic_Code;
		}
		catch (SqlException ex)
		{
			MainMenu.WriteLine(2, "SQL Error -> Function GetInt(), exception catched : " + ex.ToString());
			return string.Empty;
		}
	}

	public static void LoadObjects()
	{
		try
		{
			using IDbConnection dbConnection = new SqlConnection(sqlCon.connectionstring);
			dbConnection.Open();
			SqlMapper.QueryAsync<_RefObjCommon>(dbConnection, "SELECT ID,CodeName128,NameStrID128,Bionic,TypeID1,TypeID2,TypeID3,TypeID4,Rarity FROM " + MainMenu.SHA_DB + ".dbo._RefObjCommon with(nolock) where service =1", (object)null, (IDbTransaction)null, (int?)null, (CommandType?)null).Result.ToList().ForEach(delegate(_RefObjCommon Com)
			{
				Utils.RefObjCommon.TryAdd(Com.ID, Com);
			});
		}
		catch
		{
			MainMenu.WriteLine(2, "LoadItems operation has failed");
		}
	}

	public static void LoadCharAnalyzer(string CharName)
	{
		try
		{
			sqlCon.Analyzer = Task.Run(async () => await sqlCon.GetList("SELECT * FROM " + MainMenu.FILTER_DB + ".dbo._CharacterAnalyzer with(nolock) where CharName16 ='" + CharName + "'")).Result;
		}
		catch
		{
		}
	}

	public static void LoadHwans()
	{
		try
		{
			using IDbConnection dbConnection = new SqlConnection(sqlCon.connectionstring);
			dbConnection.Open();
			SqlMapper.QueryAsync<RefHWANLevel>(dbConnection, "SELECT HwanLevel,Title_CH70,Title_EU70 FROM " + MainMenu.SHA_DB + ".dbo._RefHWANLevel with(nolock)", (object)null, (IDbTransaction)null, (int?)null, (CommandType?)null).Result.ToList().ForEach(delegate(RefHWANLevel Hawn)
			{
				Utils.RefHwan.TryAdd(Hawn.HwanLevel, Hawn);
			});
		}
		catch
		{
			MainMenu.WriteLine(2, "LoadHwans operation has failed");
		}
	}

	public static void LoadHwansnew()
	{
		try
		{
			using IDbConnection dbConnection = new SqlConnection(sqlCon.connectionstring);
			dbConnection.Open();
			SqlMapper.QueryAsync<RefNewHwan>(dbConnection, "SELECT TitleID,TitleName FROM " + MainMenu.FILTER_DB + ".dbo._StandardTitleNameNewTitles with(nolock)", (object)null, (IDbTransaction)null, (int?)null, (CommandType?)null).Result.ToList().ForEach(delegate(RefNewHwan Hawn1)
			{
				Utils.RefNewHwan.TryAdd(Hawn1.TitleID, Hawn1);
			});
		}
		catch
		{
			MainMenu.WriteLine(2, "LoadHwansewew operation has failed");
		}
	}

	public static async Task<bool> UpdateHwanList(AgentContext Session)
	{
		try
		{
			Session.CharTitles.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select * from " + MainMenu.FILTER_DB + ".dbo._TitleManager with(nolock) where CharName = '" + Session.Charname + "';", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					byte titl;
					titl = Convert.ToByte(reader[2]);
					if (!Session.CharTitles.ContainsKey(titl) && Utils.RefHwan.ContainsKey(titl))
					{
						if (Session.EUChar)
						{
							Session.CharTitles.Add(titl, Utils.RefHwan[titl].Title_EU70);
						}
						else if (Session.CHChar)
						{
							Session.CharTitles.Add(titl, Utils.RefHwan[titl].Title_CH70);
						}
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"UpdateHwanList returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> UpdateNewTitles(AgentContext Session)
	{
		try
		{
			Session.CharTitlesNew.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select * from " + MainMenu.FILTER_DB + ".dbo._TitleManagerNewTitles with (nolock) where CharName = '" + Session.Charname + "';", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					byte Title;
					Title = Convert.ToByte(reader[2]);
					if (!Session.CharTitlesNew.ContainsKey(Title) && Utils.RefNewHwan.ContainsKey(Title))
					{
						Session.CharTitlesNew.Add(Title, Utils.RefNewHwan[Title].TitleName);
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"NewHwans returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> GetNewTitles()
	{
		try
		{
			Utils.NewTitles.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select * from " + MainMenu.FILTER_DB + ".dbo._iSroTitles with(nolock)", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					string charname;
					charname = Convert.ToString(reader[0]);
					byte Title;
					Title = Convert.ToByte(reader[1]);
					if (!Utils.NewTitles.ContainsKey(charname))
					{
						Utils.NewTitles.TryAdd(charname, Utils.RefNewHwan[Title].TitleName);
					}
					else
					{
						Utils.NewTitles[charname] = Utils.RefNewHwan[Title].TitleName;
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"iSro Titles Table returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> UpdateCharChest(AgentContext Session)
	{
		try
		{
			Session.CharChest.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand($"SELECT TOP 100 ROW_NUMBER() OVER(ORDER BY ID asc),* from {MainMenu.FILTER_DB}.dbo._CharChest with (nolock) where CharID = '{Session.CharID}';", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					int linenum;
					linenum = Convert.ToInt32(reader[0]);
					int chestid;
					chestid = Convert.ToInt32(reader[1]);
					uint RefItemID;
					RefItemID = Convert.ToUInt32(reader[3]);
					int Count;
					Count = Convert.ToInt32(reader[4]);
					byte OptLevel;
					OptLevel = Convert.ToByte(reader[5]);
					bool RandomStat;
					RandomStat = Convert.ToBoolean(reader[6]);
					string From;
					From = Convert.ToString(reader[7]);
					int TypeStr;
					TypeStr = Convert.ToInt32(reader[8]);
					DateTime RegTime;
					RegTime = Convert.ToDateTime(reader[9]);
					if (Utils.RefObjCommon.ContainsKey(RefItemID))
					{
						_RefObjCommon item;
						item = Utils.RefObjCommon[RefItemID];
						_CharChest item2;
						item2 = new _CharChest
						{
							ChestID = chestid,
							CodeName = item.CodeName128,
							ItemID = RefItemID,
							LineNum = linenum,
							Count = Count,
							NameStrID128 = item.NameStrID128,
							OptLevel = OptLevel,
							RandomizedStats = RandomStat,
							From = From,
							RegisterTime = RegTime,
							TypeStr = TypeStr
						};
						Session.CharChest.Add(item2);
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"UpdateCharChest returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<int> ReturnEmptySlotCount(int CharID)
	{
		try
		{
			int invtsize;
			invtsize = await sqlCon.ReturnInvtSize(CharID);
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand($"select COUNT(*) from {MainMenu.SHA_DB}.dbo._Inventory with (nolock) where charid = {CharID} and slot >= 13 and slot < {invtsize} and itemid = 0", con);
			await con.OpenAsync();
			return Convert.ToInt32(await cmd.ExecuteScalarAsync());
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"ReturnEmptySlotCount failed: {arg}");
			return 0;
		}
	}

	public static async Task<int> ReturnInvtSize(int CharID)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand($"Select InventorySize from {MainMenu.SHA_DB}.dbo._Char with (nolock) where CharID = {CharID}", con);
			await con.OpenAsync();
			return Convert.ToInt32(await cmd.ExecuteScalarAsync());
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"ReturnInvtSize failed: {arg}");
			return 0;
		}
	}

	public static async Task<_ItemInfo> Get_ItemInfo_by_Slot(int Slot, int CharID)
	{
		try
		{
			List<long> MagParams;
			MagParams = new List<long>();
			_ItemInfo Item;
			Item = new _ItemInfo();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand($"Select I.OptLevel,I.Variance,I.Data,I.MagParamNum,I.MagParam1,I.MagParam2,I.MagParam3,I.MagParam4,I.MagParam5,I.MagParam6,I.MagParam7,I.MagParam8,I.MagParam9,I.MagParam10,I.MagParam11,I.MagParam12,R.TypeID2,R.TypeID3,R.TypeID4,I.ID64, I.RefItemID from {MainMenu.SHA_DB}.dbo._Items I JOIN {MainMenu.SHA_DB}.dbo._Inventory C ON I.ID64 = C.ItemID JOIN {MainMenu.SHA_DB}.dbo._RefObjCommon R ON R.ID = I.RefItemID WHERE c.Slot = {Slot} and C.CharID = {CharID}", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					byte Plus;
					Plus = ((reader[0] != DBNull.Value) ? Convert.ToByte(reader[0]) : Convert.ToByte(0));
					ulong Variance;
					Variance = ((reader[1] != DBNull.Value) ? Convert.ToUInt64(reader[1]) : Convert.ToUInt64(0));
					uint Durability;
					Durability = ((reader[2] != DBNull.Value) ? Convert.ToUInt32(reader[2]) : Convert.ToUInt32(0));
					byte MagParamNum;
					MagParamNum = ((reader[3] != DBNull.Value) ? Convert.ToByte(reader[3]) : Convert.ToByte(0));
					MagParams.Add((reader[4] != DBNull.Value) ? Convert.ToInt64(reader[4]) : 0);
					MagParams.Add((reader[5] != DBNull.Value) ? Convert.ToInt64(reader[5]) : 0);
					MagParams.Add((reader[6] != DBNull.Value) ? Convert.ToInt64(reader[6]) : 0);
					MagParams.Add((reader[7] != DBNull.Value) ? Convert.ToInt64(reader[7]) : 0);
					MagParams.Add((reader[8] != DBNull.Value) ? Convert.ToInt64(reader[8]) : 0);
					MagParams.Add((reader[9] != DBNull.Value) ? Convert.ToInt64(reader[9]) : 0);
					MagParams.Add((reader[10] != DBNull.Value) ? Convert.ToInt64(reader[10]) : 0);
					MagParams.Add((reader[11] != DBNull.Value) ? Convert.ToInt64(reader[11]) : 0);
					MagParams.Add((reader[12] != DBNull.Value) ? Convert.ToInt64(reader[12]) : 0);
					MagParams.Add((reader[13] != DBNull.Value) ? Convert.ToInt64(reader[13]) : 0);
					MagParams.Add((reader[14] != DBNull.Value) ? Convert.ToInt64(reader[14]) : 0);
					MagParams.Add((reader[15] != DBNull.Value) ? Convert.ToInt64(reader[15]) : 0);
					int TypeID2;
					TypeID2 = ((reader[16] != DBNull.Value) ? Convert.ToInt32(reader[16]) : Convert.ToInt32(0));
					int TypeID3;
					TypeID3 = ((reader[17] != DBNull.Value) ? Convert.ToInt32(reader[17]) : Convert.ToInt32(0));
					int TypeID4;
					TypeID4 = ((reader[18] != DBNull.Value) ? Convert.ToInt32(reader[18]) : Convert.ToInt32(0));
					long ID64;
					ID64 = ((reader[19] != DBNull.Value) ? Convert.ToInt64(reader[19]) : Convert.ToInt64(0));
					int RefItemID;
					RefItemID = ((reader[18] != DBNull.Value) ? Convert.ToInt32(reader[20]) : Convert.ToInt32(0));
					Item.Plus = Plus;
					Item.Variance = Variance;
					Item.Durability = Durability;
					Item.MagParamNum = MagParamNum;
					Item.MagicOptions = MagParams;
					Item.TypeID2 = TypeID2;
					Item.TypeID3 = TypeID3;
					Item.TypeID4 = TypeID4;
					Item.ID64 = ID64;
					Item.ItemID = RefItemID;
				}
				reader.Close();
			}
			return Item;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Get_ItemInfo_by_Slot failed: {arg}");
			return new _ItemInfo();
		}
	}

	public static async Task<_ItemInfo> Get_BindingInfo_by_Slot(_ItemInfo Item)
	{
		try
		{
			List<_ItemInfoSocket> Sockets;
			Sockets = new List<_ItemInfoSocket>();
			List<_ItemInfoAdvance> Advances;
			Advances = new List<_ItemInfoAdvance>();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand($"Select bOptType,nSlot,nOptID,nOptLvl,nOptValue,nParam1 from {MainMenu.SHA_DB}.dbo._BindingOptionWithItem  WHERE nItemDBID = {Item.ID64}", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					byte Type;
					Type = ((reader[0] != DBNull.Value) ? Convert.ToByte(reader[0]) : Convert.ToByte(0));
					byte nSlot;
					nSlot = ((reader[1] != DBNull.Value) ? Convert.ToByte(reader[1]) : Convert.ToByte(0));
					switch (Type)
					{
					case 1:
					{
						ushort nOptValue;
						nOptValue = ((reader[3] != DBNull.Value) ? Convert.ToUInt16(reader[3]) : Convert.ToUInt16(0));
						ushort nOptID;
						nOptID = ((reader[2] != DBNull.Value) ? Convert.ToUInt16(reader[2]) : Convert.ToUInt16(0));
						uint nParam;
						nParam = ((reader[5] != DBNull.Value) ? Convert.ToUInt32(reader[5]) : Convert.ToUInt32(0));
						Sockets.Add(new _ItemInfoSocket(nSlot, nOptID)
						{
							Value = nOptValue,
							nParam = nParam
						});
						break;
					}
					case 2:
					{
						uint nOptID2;
						nOptID2 = ((reader[2] != DBNull.Value) ? Convert.ToUInt32(reader[2]) : Convert.ToUInt32(0));
						uint nOptValue2;
						nOptValue2 = ((reader[4] != DBNull.Value) ? Convert.ToUInt32(reader[4]) : Convert.ToUInt32(0));
						Advances.Add(new _ItemInfoAdvance(nSlot, nOptID2)
						{
							Value = nOptValue2
						});
						break;
					}
					}
					Item.SocketOptions = Sockets;
					Item.AdvanceOptions = Advances;
				}
				reader.Close();
			}
			return Item;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Get_ItemInfo_by_Slot failed: {arg}");
			return Item;
		}
	}

	public static async Task<bool> LoadSavedLocations(AgentContext Session = null)
	{
		try
		{
			Session.SavedLocations.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand($"SELECT * FROM {MainMenu.FILTER_DB}.dbo._SavedLocations with (nolock) WHERE CharID = '{Session.CharID}'", con);
				await con.OpenAsync();
				using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						byte BIndex;
						BIndex = Convert.ToByte(reader[2]);
						short region;
						region = Convert.ToInt16(reader[3]);
						int x;
						x = Convert.ToInt32(reader[4]);
						int y;
						y = Convert.ToInt32(reader[5]);
						int z;
						z = Convert.ToInt32(reader[6]);
						int wID;
						wID = Convert.ToInt32(reader[7]);
						string location;
						location = Convert.ToString(reader[8]);
						SavedLocation value;
						value = new SavedLocation
						{
							RegionID = region,
							PosX = x,
							PosY = y,
							PosZ = z,
							WorldID = wID,
							LocationName = location
						};
						Session.SavedLocations.Add(BIndex, value);
					}
					reader.Close();
				}
				con.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"LOAD_SAVED_LOCATIONS returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task ReverseLocations(int charid, byte locationID, int region, int x, int y, int z, int worldID, string location)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand($"INSERT INTO {MainMenu.FILTER_DB}.dbo._SavedLocations VALUES ({charid},{locationID},{region},{x},{y},{z}, {worldID},'{Utils.SqlInjectFix(location)}')", con);
			await con.OpenAsync();
			await cmd.ExecuteNonQueryAsync();
			con.Close();
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"ADD_NEW_LOCATION returned false and failed: {arg}");
		}
	}

	public static async Task<string> GIVE_SILK(string StrUserID, int SilkAmount)
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			string aCC_DB;
			aCC_DB = MainMenu.ACC_DB;
			int? num;
			num = await sqlCon.Get_LastOrderID();
			using SqlCommand cmd = new SqlCommand("EXEC " + aCC_DB + ".CGI.CGI_WebPurchaseSilk '" + (int?)num + "','" + StrUserID + "','1','" + SilkAmount + "',1", con);
			await con.OpenAsync();
			string result;
			result = (string)(await cmd.ExecuteScalarAsync());
			con.Close();
			return result;
		}
		catch
		{
			MainMenu.WriteLine(1, "GIVE_SILK opeation has has returned string.Empty");
			return string.Empty;
		}
	}

	public static async Task<int?> Get_LastOrderID()
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand("Select Max(cast(OrderNumber as int)) + 1 from " + MainMenu.ACC_DB + ".dbo.SK_SilkBuyList;", con);
			await con.OpenAsync();
			int? sb;
			sb = Convert.ToInt32(await cmd.ExecuteScalarAsync());
			if (!sb.HasValue)
			{
				await sqlCon.Insert_LastOrderID_Dummy();
				return await sqlCon.Get_LastOrderID();
			}
			return sb;
		}
		catch (Exception EX)
		{
			await sqlCon.Insert_LastOrderID_Dummy();
			MainMenu.WriteLine(1, $"Get_LastOrderID failed: {EX}");
			return 0;
		}
	}

	public static async Task Insert_LastOrderID_Dummy()
	{
		try
		{
			using SqlConnection con = new SqlConnection(sqlCon.connectionstring);
			using SqlCommand cmd = new SqlCommand("INSERT " + MainMenu.ACC_DB + ".dbo.SK_SilkBuyList(UserJID,Silk_Type,Silk_Reason,Silk_Offset,Silk_Remain,ID,BuyQuantity,OrderNumber,SlipPaper,RegDate) VALUES ( 0,0,0,0,0,0,0,0,'dummy',GETDATE())", con);
			await con.OpenAsync();
			await cmd.ExecuteNonQueryAsync();
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(1, $"Insert_LastOrderID_Dummy failed: {arg}");
		}
	}

	public static async Task<bool> LoadCharNameColorList()
	{
		try
		{
			Utils.CharNameColorList.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select * from " + MainMenu.FILTER_DB + ".._CharNameColors with(nolock)", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					string charname;
					charname = Convert.ToString(reader[0]);
					uint color;
					color = uint.Parse(Convert.ToString(reader[1]).Replace("#", ""), NumberStyles.HexNumber);
					if (!Utils.CharNameColorList.ContainsKey(charname))
					{
						Utils.CharNameColorList.TryAdd(charname, color);
					}
					else
					{
						Utils.CharNameColorList[charname] = color;
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"UpdateCharnameColor returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> LoadTitleColorList()
	{
		try
		{
			Utils.TitleColorList.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select * from " + MainMenu.FILTER_DB + ".._CharTitleColors with(nolock)", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					string charname;
					charname = Convert.ToString(reader[0]);
					uint color;
					color = uint.Parse(Convert.ToString(reader[1]).Replace("#", ""), NumberStyles.HexNumber);
					if (!Utils.TitleColorList.ContainsKey(charname))
					{
						Utils.TitleColorList.TryAdd(charname, color);
					}
					else
					{
						Utils.TitleColorList[charname] = color;
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Load Title Color List returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> CharIconListMediaPk2Left()
	{
		try
		{
			Utils.CharIconListMediaLeft.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select * from " + MainMenu.FILTER_DB + ".._StandardCharIconListMedia with(nolock)", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					int ID;
					ID = Convert.ToInt32(reader[0]);
					string IconPath;
					IconPath = Convert.ToString(reader[1]);
					if (!Utils.CharIconListMediaLeft.ContainsKey(ID))
					{
						Utils.CharIconListMediaLeft.TryAdd(ID, IconPath);
					}
					else
					{
						Utils.CharIconListMediaLeft[ID] = IconPath;
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Load Char Icon List From Media Left returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> CharIconListMediaPk2Right()
	{
		try
		{
			Utils.CharIconListMediaRight.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select * from " + MainMenu.FILTER_DB + ".._StandardCharIconListMediaRight with(nolock)", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					int ID;
					ID = Convert.ToInt32(reader[0]);
					string IconPath;
					IconPath = Convert.ToString(reader[1]);
					if (!Utils.CharIconListMediaRight.ContainsKey(ID))
					{
						Utils.CharIconListMediaRight.TryAdd(ID, IconPath);
					}
					else
					{
						Utils.CharIconListMediaRight[ID] = IconPath;
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Load Char Icon List From Media Right returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> CharIconListMediaPk2Up()
	{
		try
		{
			Utils.CharIconListMediaUp.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select * from " + MainMenu.FILTER_DB + ".._StandardCharIconListMediaUp with(nolock)", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					int ID;
					ID = Convert.ToInt32(reader[0]);
					string IconPath;
					IconPath = Convert.ToString(reader[1]);
					if (!Utils.CharIconListMediaUp.ContainsKey(ID))
					{
						Utils.CharIconListMediaUp.TryAdd(ID, IconPath);
					}
					else
					{
						Utils.CharIconListMediaUp[ID] = IconPath;
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Load Char Icon List From Media Up returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> CustomTitles()
	{
		try
		{
			Utils.CustomTitle.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select * from " + MainMenu.FILTER_DB + ".._CustomTitles with(nolock)", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					string charname;
					charname = Convert.ToString(reader[0]);
					string titlename;
					titlename = Convert.ToString(reader[1]);
					if (!Utils.CustomTitle.ContainsKey(charname))
					{
						Utils.CustomTitle.TryAdd(charname, titlename);
					}
					else
					{
						Utils.CustomTitle[charname] = titlename;
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Load custom titles returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> CharIconsLeft()
	{
		try
		{
			Utils.CharIconLeft.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select * from " + MainMenu.FILTER_DB + ".._CharIcons with(nolock)", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					string charname;
					charname = Convert.ToString(reader[0]);
					int ID;
					ID = Convert.ToInt32(reader[1]);
					if (!Utils.CharIconLeft.ContainsKey(charname))
					{
						Utils.CharIconLeft.TryAdd(charname, ID);
					}
					else
					{
						Utils.CharIconLeft[charname] = ID;
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Load Char Icon Left returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> CharIconsRight()
	{
		try
		{
			Utils.CharIconRight.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select * from " + MainMenu.FILTER_DB + ".._CharIconsRight with(nolock)", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					string charname;
					charname = Convert.ToString(reader[0]);
					int ID;
					ID = Convert.ToInt32(reader[1]);
					if (!Utils.CharIconRight.ContainsKey(charname))
					{
						Utils.CharIconRight.TryAdd(charname, ID);
					}
					else
					{
						Utils.CharIconRight[charname] = ID;
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Load Char Icon Right returned false and failed: {arg}");
			return false;
		}
	}

	public static async Task<bool> CharIconsUp()
	{
		try
		{
			Utils.CharIconUp.Clear();
			using (SqlConnection con = new SqlConnection(sqlCon.connectionstring))
			{
				using SqlCommand cmd = new SqlCommand("Select * from " + MainMenu.FILTER_DB + ".._CharIconsUp with(nolock)", con);
				await con.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					string charname;
					charname = Convert.ToString(reader[0]);
					int ID;
					ID = Convert.ToInt32(reader[1]);
					if (!Utils.CharIconUp.ContainsKey(charname))
					{
						Utils.CharIconUp.TryAdd(charname, ID);
					}
					else
					{
						Utils.CharIconUp[charname] = ID;
					}
				}
				reader.Close();
			}
			return true;
		}
		catch (Exception arg)
		{
			MainMenu.WriteLine(2, $"Load Char Icon Up returned false and failed: {arg}");
			return false;
		}
	}
}
