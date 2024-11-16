using System;

namespace SQL;

public class _CharChest
{
	public int ChestID { get; set; } = 0;


	public string CodeName { get; set; } = "";


	public uint ItemID { get; set; } = 0u;


	public int LineNum { get; set; } = 0;


	public int Count { get; set; } = 0;


	public string NameStrID128 { get; set; } = "";


	public byte OptLevel { get; set; } = 0;


	public bool RandomizedStats { get; set; } = false;


	public string From { get; set; } = "";


	public DateTime RegisterTime { get; set; }

	public int TypeStr { get; set; } = 0;

}
