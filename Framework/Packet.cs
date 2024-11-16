using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Framework;

public class Packet
{
	private ushort m_opcode;

	private PacketWriter m_writer;

	public PacketReader m_reader;

	private bool m_encrypted;

	private bool m_massive;

	private bool m_locked;

	private byte[] m_reader_bytes;

	private object m_lock;

	public ushort Opcode => this.m_opcode;

	public bool Encrypted => this.m_encrypted;

	public bool Massive => this.m_massive;

	public bool HasDataToRead => this.m_reader.BaseStream.Position < this.m_reader.BaseStream.Length;

	public Packet(ushort opcode)
	{
		this.m_lock = new object();
		this.m_opcode = opcode;
		this.m_encrypted = false;
		this.m_massive = false;
		this.m_writer = new PacketWriter();
		this.m_reader = null;
		this.m_reader_bytes = null;
	}

	public Packet(ushort opcode, bool encrypted)
	{
		this.m_lock = new object();
		this.m_opcode = opcode;
		this.m_encrypted = encrypted;
		this.m_massive = false;
		this.m_writer = new PacketWriter();
		this.m_reader = null;
		this.m_reader_bytes = null;
	}

	public Packet(ushort opcode, bool encrypted, bool massive)
	{
		if (encrypted && massive)
		{
			throw new Exception("[Packet::Packet] Packets cannot both be massive and encrypted!");
		}
		this.m_lock = new object();
		this.m_opcode = opcode;
		this.m_encrypted = encrypted;
		this.m_massive = massive;
		this.m_writer = new PacketWriter();
		this.m_reader = null;
		this.m_reader_bytes = null;
	}

	public Packet(ushort opcode, bool encrypted, bool massive, byte[] bytes)
	{
		if (encrypted && massive)
		{
			throw new Exception("[Packet::Packet] Packets cannot both be massive and encrypted!");
		}
		this.m_lock = new object();
		this.m_opcode = opcode;
		this.m_encrypted = encrypted;
		this.m_massive = massive;
		this.m_writer = new PacketWriter();
		this.m_writer.Write(bytes);
		this.m_reader = null;
		this.m_reader_bytes = null;
	}

	public Packet(ushort opcode, bool encrypted, bool massive, byte[] bytes, int offset, int length)
	{
		if (encrypted && massive)
		{
			throw new Exception("[Packet::Packet] Packets cannot both be massive and encrypted!");
		}
		this.m_lock = new object();
		this.m_opcode = opcode;
		this.m_encrypted = encrypted;
		this.m_massive = massive;
		this.m_writer = new PacketWriter();
		this.m_writer.Write(bytes, offset, length);
		this.m_reader = null;
		this.m_reader_bytes = null;
	}

	public byte[] GetBytes()
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				return this.m_reader_bytes;
			}
			return this.m_writer.GetBytes();
		}
	}

	public byte[] GetBytesFromIndex(int index)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				return this.m_reader_bytes.Skip(index).ToArray();
			}
			return this.m_writer.GetBytes().Skip(index).ToArray();
		}
	}

	public void Lock()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				this.m_reader_bytes = this.m_writer.GetBytes();
				this.m_reader = new PacketReader(this.m_reader_bytes);
				this.m_writer.Close();
				this.m_writer = null;
				this.m_locked = true;
			}
		}
	}

	public T ReadValue<T>()
	{
		bool success;
		return this.ReadValue<T>(out success);
	}

	public T ReadValue<T>(out bool success)
	{
		if (!this.m_locked)
		{
			throw new InvalidOperationException("Packet is in write mode");
		}
		T result;
		result = default(T);
		Type typeFromHandle;
		typeFromHandle = typeof(T);
		success = false;
		try
		{
			if (typeFromHandle == typeof(bool))
			{
				result = (T)Convert.ChangeType(this.m_reader.ReadBoolean(), typeFromHandle);
			}
			if (typeFromHandle == typeof(short))
			{
				result = (T)Convert.ChangeType(this.m_reader.ReadInt16(), typeFromHandle);
			}
			if (typeFromHandle == typeof(ushort))
			{
				result = (T)Convert.ChangeType(this.m_reader.ReadUInt16(), typeFromHandle);
			}
			if (typeFromHandle == typeof(int))
			{
				result = (T)Convert.ChangeType(this.m_reader.ReadInt32(), typeFromHandle);
			}
			if (typeFromHandle == typeof(uint))
			{
				result = (T)Convert.ChangeType(this.m_reader.ReadUInt32(), typeFromHandle);
			}
			if (typeFromHandle == typeof(long))
			{
				result = (T)Convert.ChangeType(this.m_reader.ReadInt64(), typeFromHandle);
			}
			if (typeFromHandle == typeof(ulong))
			{
				result = (T)Convert.ChangeType(this.m_reader.ReadUInt64(), typeFromHandle);
			}
			if (typeFromHandle == typeof(sbyte))
			{
				result = (T)Convert.ChangeType(this.m_reader.ReadSByte(), typeFromHandle);
			}
			if (typeFromHandle == typeof(byte))
			{
				result = (T)Convert.ChangeType(this.m_reader.ReadByte(), typeFromHandle);
			}
			if (typeFromHandle == typeof(float))
			{
				result = (T)Convert.ChangeType(this.m_reader.ReadSingle(), typeFromHandle);
			}
			if (typeFromHandle == typeof(string))
			{
				ushort count;
				count = this.m_reader.ReadUInt16();
				byte[] bytes;
				bytes = this.m_reader.ReadBytes(count);
				result = (T)Convert.ChangeType(Encoding.ASCII.GetString(bytes, 0, count), typeFromHandle);
			}
			success = true;
		}
		catch
		{
		}
		return result;
	}

	public string ReadString(ushort length, int codepage = 1251)
	{
		if (!this.m_locked)
		{
			throw new InvalidOperationException("Cannot Read from an unlocked Packet.");
		}
		byte[] bytes;
		bytes = this.m_reader.ReadBytes(length);
		return Encoding.GetEncoding(codepage).GetString(bytes);
	}

	public List<T> ReadList<T>(int count)
	{
		if (!this.m_locked)
		{
			throw new InvalidOperationException("Packet is in write mode");
		}
		List<T> list;
		list = new List<T>(count);
		for (int i = 0; i < count; i++)
		{
			list.Add(this.ReadValue<T>());
		}
		return list;
	}

	public void WriteValue<T>(dynamic value)
	{
		if (this.m_locked)
		{
			throw new InvalidOperationException("Packet is in read mode");
		}
		value = (T)value;
		if (value is string)
		{
			byte[] bytes;
			bytes = Encoding.ASCII.GetBytes(value as string);
			this.m_writer.Write((ushort)bytes.Length);
			this.m_writer.Write(bytes);
		}
		else
		{
			this.m_writer.Write(value);
		}
	}

	public void WriteList<T>(params dynamic[] values)
	{
		if (this.m_locked)
		{
			throw new InvalidOperationException("Packet is in read mode");
		}
		for (int i = 0; i < values.Length; i++)
		{
			WriteValue<T>(values[i]);
		}
	}

	public void WriteByteArray(byte[] data)
	{
		if (this.m_locked)
		{
			throw new InvalidOperationException("Packet is in read mode");
		}
		this.m_writer.Write(data);
	}

	public long SeekRead(long offset, SeekOrigin orgin)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot SeekRead on an unlocked Packet.");
			}
			return this.m_reader.BaseStream.Seek(offset, orgin);
		}
	}

	public int RemainingRead()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot SeekRead on an unlocked Packet.");
			}
			return (int)(this.m_reader.BaseStream.Length - this.m_reader.BaseStream.Position);
		}
	}

	public byte ReadUInt8()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			return this.m_reader.ReadByte();
		}
	}

	public sbyte ReadInt8()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			return this.m_reader.ReadSByte();
		}
	}

	public ushort ReadUInt16()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			return this.m_reader.ReadUInt16();
		}
	}

	public float ReadFloat()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			return this.m_reader.ReadSingle();
		}
	}

	public short ReadInt16()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			return this.m_reader.ReadInt16();
		}
	}

	public uint ReadUInt32()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			return this.m_reader.ReadUInt32();
		}
	}

	public int ReadInt32()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			return this.m_reader.ReadInt32();
		}
	}

	public ulong ReadUInt64()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			return this.m_reader.ReadUInt64();
		}
	}

	public long ReadInt64()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			return this.m_reader.ReadInt64();
		}
	}

	public float ReadSingle()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			return this.m_reader.ReadSingle();
		}
	}

	public double ReadDouble()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			return this.m_reader.ReadDouble();
		}
	}

	public string ReadAscii()
	{
		return this.ReadAscii(1254);
	}

	public string ReadAscii(int codepage)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			ushort count;
			count = this.m_reader.ReadUInt16();
			byte[] bytes;
			bytes = this.m_reader.ReadBytes(count);
			return Encoding.GetEncoding(codepage).GetString(bytes);
		}
	}

	public string ReadUnicode()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			ushort num;
			num = this.m_reader.ReadUInt16();
			byte[] bytes;
			bytes = this.m_reader.ReadBytes(num * 2);
			return Encoding.Unicode.GetString(bytes);
		}
	}

	public string ReadUnicodes()
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
			}
			ushort num;
			num = this.m_reader.ReadUInt16();
			byte[] bytes;
			bytes = this.m_reader.ReadBytes(num * 2);
			return Encoding.Unicode.GetString(bytes);
		}
	}

	public byte[] ReadUInt8Array(int count)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			byte[] array;
			array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.m_reader.ReadByte();
			}
			return array;
		}
	}

	public sbyte[] ReadInt8Array(int count)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			sbyte[] array;
			array = new sbyte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.m_reader.ReadSByte();
			}
			return array;
		}
	}

	public ushort[] ReadUInt16Array(int count)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			ushort[] array;
			array = new ushort[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.m_reader.ReadUInt16();
			}
			return array;
		}
	}

	public short[] ReadInt16Array(int count)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			short[] array;
			array = new short[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.m_reader.ReadInt16();
			}
			return array;
		}
	}

	public uint[] ReadUInt32Array(int count)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			uint[] array;
			array = new uint[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.m_reader.ReadUInt32();
			}
			return array;
		}
	}

	public int[] ReadInt32Array(int count)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			int[] array;
			array = new int[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.m_reader.ReadInt32();
			}
			return array;
		}
	}

	public ulong[] ReadUInt64Array(int count)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			ulong[] array;
			array = new ulong[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.m_reader.ReadUInt64();
			}
			return array;
		}
	}

	public long[] ReadInt64Array(int count)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			long[] array;
			array = new long[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.m_reader.ReadInt64();
			}
			return array;
		}
	}

	public float[] ReadSingleArray(int count)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			float[] array;
			array = new float[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.m_reader.ReadSingle();
			}
			return array;
		}
	}

	public double[] ReadDoubleArray(int count)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			double[] array;
			array = new double[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.m_reader.ReadDouble();
			}
			return array;
		}
	}

	public string[] ReadAsciiArray(int count)
	{
		return this.ReadAsciiArray(1252);
	}

	public string[] ReadAsciiArray(int codepage, int count)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			string[] array;
			array = new string[count];
			for (int i = 0; i < count; i++)
			{
				ushort count2;
				count2 = this.m_reader.ReadUInt16();
				byte[] bytes;
				bytes = this.m_reader.ReadBytes(count2);
				array[i] = Encoding.UTF7.GetString(bytes);
			}
			return array;
		}
	}

	public string[] ReadUnicodeArray(int count)
	{
		lock (this.m_lock)
		{
			if (!this.m_locked)
			{
				throw new Exception("Cannot Read from an unlocked Packet.");
			}
			string[] array;
			array = new string[count];
			for (int i = 0; i < count; i++)
			{
				ushort num;
				num = this.m_reader.ReadUInt16();
				byte[] bytes;
				bytes = this.m_reader.ReadBytes(num * 2);
				array[i] = Encoding.Unicode.GetString(bytes);
			}
			return array;
		}
	}

	public long SeekWrite(long offset, SeekOrigin orgin)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot SeekWrite on a locked Packet.");
			}
			return this.m_writer.BaseStream.Seek(offset, orgin);
		}
	}

	public void WriteUInt8(byte value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(value);
		}
	}

	public void WriteInt8(sbyte value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(value);
		}
	}

	public void WriteUInt16(ushort value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(value);
		}
	}

	public void WriteInt16(short value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(value);
		}
	}

	public void WriteUInt32(uint value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(value);
		}
	}

	public void WriteInt32(int value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(value);
		}
	}

	public void WriteUInt64(ulong value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(value);
		}
	}

	public void WriteInt64(long value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(value);
		}
	}

	public void WriteSingle(float value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(value);
		}
	}

	public void WriteDouble(double value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(value);
		}
	}

	public void WriteAscii(string value)
	{
		this.WriteAscii(value, 1254);
	}

	public void WriteAscii_Chinese(string value)
	{
		byte[] bytes;
		bytes = Encoding.GetEncoding("gb2312").GetBytes(value);
		this.m_writer.Write((ushort)(bytes.Length + 1));
		this.m_writer.Write(bytes);
		this.m_writer.Write((byte)0);
	}

	public void WriteAsciiA(string value)
	{
		byte[] bytes;
		bytes = Encoding.GetEncoding("gb2312").GetBytes(value);
		this.m_writer.Write((ushort)(bytes.Length + 1));
		this.m_writer.Write(bytes);
		this.m_writer.Write((byte)0);
	}

	public void WriteAscii(string value, int code_page)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			byte[] bytes;
			bytes = Encoding.GetEncoding(code_page).GetBytes(value);
			this.m_writer.Write((ushort)bytes.Length);
			this.m_writer.Write(bytes);
		}
	}

	public void WriteUnicode(string value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			byte[] bytes;
			bytes = Encoding.Unicode.GetBytes(value);
			this.m_writer.Write((ushort)value.ToString().Length);
			this.m_writer.Write(bytes);
		}
	}

	public void WriteUnicodeNew(string value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			byte[] bytes;
			bytes = Encoding.GetEncoding("gb2312").GetBytes(value);
			this.m_writer.Write((ushort)value.ToString().Length);
			this.m_writer.Write(bytes);
		}
	}

	public void WriteUInt8(object value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write((byte)(Convert.ToUInt64(value) & 0xFF));
		}
	}

	public void WriteInt8(object value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write((sbyte)(Convert.ToInt64(value) & 0xFF));
		}
	}

	public void WriteUInt16(object value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write((ushort)(Convert.ToUInt64(value) & 0xFFFF));
		}
	}

	public void WriteInt16(object value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write((ushort)(Convert.ToInt64(value) & 0xFFFF));
		}
	}

	public void WriteUInt32(object value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write((uint)(Convert.ToUInt64(value) & 0xFFFFFFFFu));
		}
	}

	public void WriteInt32(object value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write((int)(Convert.ToInt64(value) & 0xFFFFFFFFu));
		}
	}

	public void WriteUInt64(object value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(Convert.ToUInt64(value));
		}
	}

	public void WriteInt64(object value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(Convert.ToInt64(value));
		}
	}

	public void WriteSingle(object value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(Convert.ToSingle(value));
		}
	}

	public void WriteDouble(object value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			this.m_writer.Write(Convert.ToDouble(value));
		}
	}

	public void WriteAscii(object value)
	{
		this.WriteAscii(value, 1252);
	}

	public void WriteAscii(object value, int code_page)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			byte[] bytes;
			bytes = Encoding.GetEncoding(code_page).GetBytes(value.ToString());
			string @string;
			@string = Encoding.UTF7.GetString(bytes);
			byte[] bytes2;
			bytes2 = Encoding.Default.GetBytes(@string);
			this.m_writer.Write((ushort)bytes2.Length);
			this.m_writer.Write(bytes2);
		}
	}

	public void WriteUnicode(object value)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			byte[] bytes;
			bytes = Encoding.Unicode.GetBytes(value.ToString());
			this.m_writer.Write((ushort)value.ToString().Length);
			this.m_writer.Write(bytes);
		}
	}

	public void WriteUInt8Array(byte[] values)
	{
		if (this.m_locked)
		{
			throw new Exception("Cannot Write to a locked Packet.");
		}
		this.m_writer.Write(values);
	}

	public void WriteUInt8Array(byte[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.m_writer.Write(values[i]);
			}
		}
	}

	public void WriteUInt16Array(ushort[] values)
	{
		this.WriteUInt16Array(values, 0, values.Length);
	}

	public void WriteUInt16Array(ushort[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.m_writer.Write(values[i]);
			}
		}
	}

	public void WriteInt16Array(short[] values)
	{
		this.WriteInt16Array(values, 0, values.Length);
	}

	public void WriteInt16Array(short[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.m_writer.Write(values[i]);
			}
		}
	}

	public void WriteUInt32Array(uint[] values)
	{
		this.WriteUInt32Array(values, 0, values.Length);
	}

	public void WriteUInt32Array(uint[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.m_writer.Write(values[i]);
			}
		}
	}

	public void WriteInt32Array(int[] values)
	{
		this.WriteInt32Array(values, 0, values.Length);
	}

	public void WriteInt32Array(int[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.m_writer.Write(values[i]);
			}
		}
	}

	public void WriteUInt64Array(ulong[] values)
	{
		this.WriteUInt64Array(values, 0, values.Length);
	}

	public void WriteUInt64Array(ulong[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.m_writer.Write(values[i]);
			}
		}
	}

	public void WriteInt64Array(long[] values)
	{
		this.WriteInt64Array(values, 0, values.Length);
	}

	public void WriteInt64Array(long[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.m_writer.Write(values[i]);
			}
		}
	}

	public void WriteSingleArray(float[] values)
	{
		this.WriteSingleArray(values, 0, values.Length);
	}

	public void WriteSingleArray(float[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.m_writer.Write(values[i]);
			}
		}
	}

	public void WriteDoubleArray(double[] values)
	{
		this.WriteDoubleArray(values, 0, values.Length);
	}

	public void WriteDoubleArray(double[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.m_writer.Write(values[i]);
			}
		}
	}

	public void WriteAsciiArray(string[] values, int codepage)
	{
		this.WriteAsciiArray(values, 0, values.Length, codepage);
	}

	public void WriteAsciiArray(string[] values, int index, int count, int codepage)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteAscii(values[i], codepage);
			}
		}
	}

	public void WriteAsciiArray(string[] values)
	{
		this.WriteAsciiArray(values, 0, values.Length, 1252);
	}

	public void WriteAsciiArray(string[] values, int index, int count)
	{
		this.WriteAsciiArray(values, index, count, 1252);
	}

	public void WriteUnicodeArray(string[] values)
	{
		this.WriteUnicodeArray(values, 0, values.Length);
	}

	public void WriteUnicodeArray(string[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteUnicode(values[i]);
			}
		}
	}

	public void WriteUInt8Array(object[] values)
	{
		this.WriteUInt8Array(values, 0, values.Length);
	}

	public void WriteUInt8Array(object[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteUInt8(values[i]);
			}
		}
	}

	public void WriteInt8Array(object[] values)
	{
		this.WriteInt8Array(values, 0, values.Length);
	}

	public void WriteInt8Array(object[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteInt8(values[i]);
			}
		}
	}

	public void WriteUInt16Array(object[] values)
	{
		this.WriteUInt16Array(values, 0, values.Length);
	}

	public void WriteUInt16Array(object[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteUInt16(values[i]);
			}
		}
	}

	public void WriteInt16Array(object[] values)
	{
		this.WriteInt16Array(values, 0, values.Length);
	}

	public void WriteInt16Array(object[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteInt16(values[i]);
			}
		}
	}

	public void WriteUInt32Array(object[] values)
	{
		this.WriteUInt32Array(values, 0, values.Length);
	}

	public void WriteUInt32Array(object[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteUInt32(values[i]);
			}
		}
	}

	public void WriteInt32Array(object[] values)
	{
		this.WriteInt32Array(values, 0, values.Length);
	}

	public void WriteInt32Array(object[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteInt32(values[i]);
			}
		}
	}

	public void WriteUInt64Array(object[] values)
	{
		this.WriteUInt64Array(values, 0, values.Length);
	}

	public void WriteUInt64Array(object[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteUInt64(values[i]);
			}
		}
	}

	public void WriteInt64Array(object[] values)
	{
		this.WriteInt64Array(values, 0, values.Length);
	}

	public void WriteInt64Array(object[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteInt64(values[i]);
			}
		}
	}

	public void WriteSingleArray(object[] values)
	{
		this.WriteSingleArray(values, 0, values.Length);
	}

	public void WriteSingleArray(object[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteSingle(values[i]);
			}
		}
	}

	public void WriteDoubleArray(object[] values)
	{
		this.WriteDoubleArray(values, 0, values.Length);
	}

	public void WriteDoubleArray(object[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteDouble(values[i]);
			}
		}
	}

	public void WriteAsciiArray(object[] values, int codepage)
	{
		this.WriteAsciiArray(values, 0, values.Length, codepage);
	}

	public void WriteAsciiArray(object[] values, int index, int count, int codepage)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteAscii(values[i].ToString(), codepage);
			}
		}
	}

	public void WriteAsciiArray(object[] values)
	{
		this.WriteAsciiArray(values, 0, values.Length, 1252);
	}

	public void WriteAsciiArray(object[] values, int index, int count)
	{
		this.WriteAsciiArray(values, index, count, 1252);
	}

	public void WriteUnicodeArray(object[] values)
	{
		this.WriteUnicodeArray(values, 0, values.Length);
	}

	public void WriteUnicodeArray(object[] values, int index, int count)
	{
		lock (this.m_lock)
		{
			if (this.m_locked)
			{
				throw new Exception("Cannot Write to a locked Packet.");
			}
			for (int i = index; i < index + count; i++)
			{
				this.WriteUnicode(values[i].ToString());
			}
		}
	}
}
