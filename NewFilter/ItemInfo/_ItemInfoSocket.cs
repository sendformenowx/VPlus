namespace NewFilter.ItemInfo;

public class _ItemInfoSocket
{
	public ushort ID { get; set; }

	public byte Slot { get; set; }

	public ushort Value { get; set; }

	public uint nParam { get; set; }

	public _ItemInfoSocket(byte Slot, ushort ID)
	{
		this.Slot = Slot;
		this.ID = ID;
	}
}
