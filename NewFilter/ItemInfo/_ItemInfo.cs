using System.Collections.Generic;

namespace NewFilter.ItemInfo;

public class _ItemInfo
{
	public byte Plus { get; set; } = 0;


	public ulong Variance { get; set; } = 0uL;


	public uint Durability { get; set; } = 0u;


	public byte MagParamNum { get; set; } = 0;


	public byte SocketNum { get; set; } = 0;


	public byte AdvanceNum { get; set; } = 0;


	public List<long> MagicOptions { get; set; } = new List<long>();


	public List<_ItemInfoSocket> SocketOptions { get; set; } = new List<_ItemInfoSocket>();


	public List<_ItemInfoAdvance> AdvanceOptions { get; set; } = new List<_ItemInfoAdvance>();


	public int ItemID { get; set; } = 0;


	public int TypeID2 { get; set; } = 0;


	public int TypeID3 { get; set; } = 0;


	public int TypeID4 { get; set; } = 0;


	public long ID64 { get; set; } = 0L;

}
