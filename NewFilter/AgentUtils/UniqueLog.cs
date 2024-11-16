using System;

namespace NewFilter.AgentUtils;

internal class UniqueLog
{
	public static void UniqueLogspawn(int s_MOBID)
	{
		if (Utils.UniqueLog.Count <= 100)
		{
			if (Utils.UniqueLog.ContainsKey(s_MOBID))
			{
				Utils.UniqueLog[s_MOBID].Status = "<Alive>";
				Utils.UniqueLog[s_MOBID].Killer = "<None>";
				return;
			}
			UniqueMob value;
			value = new UniqueMob
			{
				Status = "<Alive>",
				Killer = "<None>"
			};
			Utils.UniqueLog.TryAdd(s_MOBID, value);
		}
	}

	public static void UniqueLogkill(int MobID, string Charname)
	{
		if (Utils.UniqueLog.ContainsKey(Convert.ToInt32(MobID)))
		{
			Utils.UniqueLog[Convert.ToInt32(MobID)].Status = DateTime.Now.ToShortTimeString();
			Utils.UniqueLog[Convert.ToInt32(MobID)].Killer = Charname;
			return;
		}
		UniqueMob value;
		value = new UniqueMob
		{
			Status = DateTime.Now.ToShortTimeString(),
			Killer = Charname
		};
		Utils.UniqueLog.TryAdd(Convert.ToInt32(MobID), value);
	}

	public static void UniqueLogSpawn(int MobID, string Charname)
	{
		if (Utils.UniqueLog.ContainsKey(Convert.ToInt32(MobID)))
		{
			Utils.UniqueLog[Convert.ToInt32(MobID)].Status = "Not spawn";
			Utils.UniqueLog[Convert.ToInt32(MobID)].Killer = Charname;
			return;
		}
		UniqueMob value;
		value = new UniqueMob
		{
			Status = "Not spawn",
			Killer = Charname
		};
		Utils.UniqueLog.TryAdd(Convert.ToInt32(MobID), value);
	}
}
