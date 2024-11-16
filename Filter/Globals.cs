using System;
using System.Collections.Generic;

namespace Filter;

internal class Globals
{
	public struct Types_
	{
		public List<string> grab_types;

		public List<string> grabpet_spawn_types;

		public List<string> attack_types;

		public List<string> attack_spawn_types;
	}

	public static ushort StatPoints = 0;

	public static float WalkSpeed = 17.6f;

	public static float RunSpeed = 55f;

	public static float ZerkSpeed = 110f;

	public static uint PHYdef = 0u;

	public static uint MAGdef = 0u;

	public static uint hitRate = 0u;

	public static uint parryRatio = 0u;

	public static uint CurrentHP = 0u;

	public static uint CurrentMP = 0u;

	public static uint MaximumHP = 0u;

	public static uint MaximumMP = 0u;

	public static ulong MaxExp = 0uL;

	public static ulong Exp = 0uL;

	public static ushort STR = 0;

	public static ushort INT = 0;

	public static byte bad_status = 0;

	public static List<string> explist = new List<string>();

	public static string PlayerName;

	public static List<byte> inventoryslot = new List<byte>();

	public static List<uint> inventoryid = new List<uint>();

	public static List<string> inventorytype = new List<string>();

	public static List<ushort> inventorycount = new List<ushort>();

	public static List<uint> inventorydurability = new List<uint>();

	public static List<uint> skillid = new List<uint>();

	public static List<string> skillname = new List<string>();

	public static List<string> skilltype = new List<string>();

	public static List<int> skillwait = new List<int>();

	public static List<DateTime> skilllastuse = new List<DateTime>();

	public static Types_ Types = default(Types_);

	public static void InitializeTypes()
	{
		Globals.Types.grab_types = new List<string>();
		Globals.Types.grab_types.Add("COS_P_SPOT_RABBIT");
		Globals.Types.grab_types.Add("COS_P_RABBIT");
		Globals.Types.grab_types.Add("COS_P_GGLIDER");
		Globals.Types.grab_types.Add("COS_P_MYOWON");
		Globals.Types.grab_types.Add("COS_P_SEOWON");
		Globals.Types.grab_types.Add("COS_P_RACCOONDOG");
		Globals.Types.grab_types.Add("COS_P_CAT");
		Globals.Types.grab_types.Add("COS_P_BROWNIE");
		Globals.Types.grab_types.Add("COS_P_PINKPIG");
		Globals.Types.grab_types.Add("COS_P_GOLDPIG");
		Globals.Types.grab_types.Add("COS_P_FOX");
		Globals.Types.attack_types = new List<string>();
		Globals.Types.attack_types.Add("COS_P_BEAR");
		Globals.Types.attack_types.Add("COS_P_FOX");
		Globals.Types.attack_types.Add("COS_P_PENGUIN");
		Globals.Types.attack_types.Add("COS_P_WOLF_WHITE_SMALL");
		Globals.Types.attack_types.Add("COS_P_WOLF_WHITE");
		Globals.Types.attack_types.Add("COS_P_WOLF");
		Globals.Types.attack_spawn_types = new List<string>();
		Globals.Types.attack_spawn_types.Add("ITEM_COS_P_FOX_SCROLL");
		Globals.Types.attack_spawn_types.Add("ITEM_COS_P_BEAR_SCROLL");
		Globals.Types.attack_spawn_types.Add("ITEM_COS_P_FLUTE");
		Globals.Types.attack_spawn_types.Add("ITEM_COS_P_FLUTE_SILK");
		Globals.Types.attack_spawn_types.Add("ITEM_COS_P_FLUTE_WHITE");
		Globals.Types.attack_spawn_types.Add("ITEM_COS_P_FLUTE_WHITE_SMALL");
		Globals.Types.attack_spawn_types.Add("ITEM_COS_P_PENGUIN_SCROLL");
		Globals.Types.grabpet_spawn_types = new List<string>();
		Globals.Types.grabpet_spawn_types.Add("ITEM_COS_P_SPOT_RABBIT_SCROLL");
		Globals.Types.grabpet_spawn_types.Add("ITEM_COS_P_RABBIT_SCROLL");
		Globals.Types.grabpet_spawn_types.Add("ITEM_COS_P_RABBIT_SCROLL_SILK");
		Globals.Types.grabpet_spawn_types.Add("ITEM_COS_P_GGLIDER_SCROLL");
		Globals.Types.grabpet_spawn_types.Add("ITEM_COS_P_MYOWON_SCROLL");
		Globals.Types.grabpet_spawn_types.Add("ITEM_COS_P_SEOWON_SCROLL");
		Globals.Types.grabpet_spawn_types.Add("ITEM_COS_P_RACCOONDOG_SCROLL");
		Globals.Types.grabpet_spawn_types.Add("ITEM_COS_P_BROWNIE_SCROLL");
		Globals.Types.grabpet_spawn_types.Add("ITEM_COS_P_CAT_SCROLL");
		Globals.Types.grabpet_spawn_types.Add("ITEM_COS_P_PINKPIG_SCROLL");
		Globals.Types.grabpet_spawn_types.Add("ITEM_COS_P_GOLDPIG_SCROLL");
		Globals.Types.grabpet_spawn_types.Add("ITEM_COS_P_GOLDPIG_SCROLL_SILK");
	}
}
