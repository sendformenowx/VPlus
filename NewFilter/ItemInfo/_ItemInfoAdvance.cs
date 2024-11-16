namespace NewFilter.ItemInfo;

public class _ItemInfoAdvance
{
	public uint ID { get; set; }

	public byte Slot { get; set; }

	public uint Value { get; set; }

	public _ItemInfoAdvance(byte Slot, uint ID)
	{
		this.Slot = Slot;
		this.ID = ID;
	}
}
