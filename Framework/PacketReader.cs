using System.IO;

namespace Framework;

public class PacketReader : BinaryReader
{
	private byte[] m_input;

	public PacketReader(byte[] input)
		: base(new MemoryStream(input, writable: false))
	{
		this.m_input = input;
	}

	public PacketReader(byte[] input, int index, int count)
		: base(new MemoryStream(input, index, count, writable: false))
	{
		this.m_input = input;
	}
}
