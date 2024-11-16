using System;

namespace Framework;

public class TransferBuffer
{
	private byte[] m_buffer;

	private int m_offset;

	private int m_size;

	private object m_lock;

	public byte[] Buffer
	{
		get
		{
			return this.m_buffer;
		}
		set
		{
			lock (this.m_lock)
			{
				this.m_buffer = value;
			}
		}
	}

	public int Offset
	{
		get
		{
			return this.m_offset;
		}
		set
		{
			lock (this.m_lock)
			{
				this.m_offset = value;
			}
		}
	}

	public int Size
	{
		get
		{
			return this.m_size;
		}
		set
		{
			lock (this.m_lock)
			{
				this.m_size = value;
			}
		}
	}

	public TransferBuffer(TransferBuffer rhs)
	{
		lock (rhs.m_lock)
		{
			this.m_buffer = new byte[rhs.m_buffer.Length];
			System.Buffer.BlockCopy(rhs.m_buffer, 0, this.m_buffer, 0, this.m_buffer.Length);
			this.m_offset = rhs.m_offset;
			this.m_size = rhs.m_size;
			this.m_lock = new object();
		}
	}

	public TransferBuffer()
	{
		this.m_buffer = null;
		this.m_offset = 0;
		this.m_size = 0;
		this.m_lock = new object();
	}

	public TransferBuffer(int length, int offset, int size)
	{
		this.m_buffer = new byte[length];
		this.m_offset = offset;
		this.m_size = size;
		this.m_lock = new object();
	}

	public TransferBuffer(int length)
	{
		this.m_buffer = new byte[length];
		this.m_offset = 0;
		this.m_size = 0;
		this.m_lock = new object();
	}

	public TransferBuffer(byte[] buffer, int offset, int size, bool assign)
	{
		if (assign)
		{
			this.m_buffer = buffer;
		}
		else
		{
			this.m_buffer = new byte[buffer.Length];
			System.Buffer.BlockCopy(buffer, 0, this.m_buffer, 0, buffer.Length);
		}
		this.m_offset = offset;
		this.m_size = size;
		this.m_lock = new object();
	}
}
