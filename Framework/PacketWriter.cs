using System.IO;

namespace Framework;

internal class PacketWriter : BinaryWriter
{
	private MemoryStream m_ms;

	public PacketWriter()
	{
		this.m_ms = new MemoryStream();
		base.OutStream = this.m_ms;
	}

	public byte[] GetBytes()
	{
		return this.m_ms.ToArray();
	}
}
