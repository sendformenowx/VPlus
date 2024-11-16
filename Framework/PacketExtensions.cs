using System;
using System.Collections.Generic;

namespace Framework;

public static class PacketExtensions
{
	private delegate dynamic PacketReadHandler(Packet packet);

	private delegate void PacketWriteHandler(Packet packet, dynamic value);

	private static readonly Dictionary<Type, PacketReadHandler> _enumReaders = new Dictionary<Type, PacketReadHandler>
	{
		{
			typeof(byte),
			(Packet reader) => reader.ReadValue<byte>()
		},
		{
			typeof(sbyte),
			(Packet reader) => reader.ReadValue<sbyte>()
		},
		{
			typeof(ushort),
			(Packet reader) => reader.ReadValue<ushort>()
		},
		{
			typeof(short),
			(Packet reader) => reader.ReadValue<short>()
		},
		{
			typeof(uint),
			(Packet reader) => reader.ReadValue<uint>()
		},
		{
			typeof(int),
			(Packet reader) => reader.ReadValue<int>()
		},
		{
			typeof(ulong),
			(Packet reader) => reader.ReadValue<ulong>()
		},
		{
			typeof(long),
			(Packet reader) => reader.ReadValue<long>()
		}
	};

	private static readonly Dictionary<Type, PacketWriteHandler> _enumWriters = new Dictionary<Type, PacketWriteHandler>
	{
		{
			typeof(byte),
			delegate(Packet packet, dynamic value)
			{
				packet.WriteValue<byte>(value);
			}
		},
		{
			typeof(sbyte),
			delegate(Packet packet, dynamic value)
			{
				packet.WriteValue<sbyte>(value);
			}
		},
		{
			typeof(ushort),
			delegate(Packet packet, dynamic value)
			{
				packet.WriteValue<ushort>(value);
			}
		},
		{
			typeof(short),
			delegate(Packet packet, dynamic value)
			{
				packet.WriteValue<short>(value);
			}
		},
		{
			typeof(uint),
			delegate(Packet packet, dynamic value)
			{
				packet.WriteValue<uint>(value);
			}
		},
		{
			typeof(int),
			delegate(Packet packet, dynamic value)
			{
				packet.WriteValue<int>(value);
			}
		},
		{
			typeof(ulong),
			delegate(Packet packet, dynamic value)
			{
				packet.WriteValue<ulong>(value);
			}
		},
		{
			typeof(long),
			delegate(Packet packet, dynamic value)
			{
				packet.WriteValue<long>(value);
			}
		}
	};

	public static TEnum ReadEnum<TEnum>(this Packet packet) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		if (!typeof(TEnum).IsEnum)
		{
			throw new ArgumentException("Given type is not enumaration.");
		}
		if (!PacketExtensions._enumReaders.TryGetValue(Enum.GetUnderlyingType(typeof(TEnum)), out var value))
		{
			throw new ArgumentException("Reader for type " + typeof(TEnum).Name + " does not exist.");
		}
		return (TEnum)value(packet);
	}

	public static void WriteEnum<TEnum>(this Packet packet, TEnum @enum) where TEnum : struct, IComparable, IFormattable, IConvertible
	{
		if (!typeof(TEnum).IsEnum)
		{
			throw new ArgumentException("Given type " + typeof(TEnum).Name + " is not enumeration.");
		}
		if (!PacketExtensions._enumWriters.TryGetValue(Enum.GetUnderlyingType(typeof(TEnum)), out var value))
		{
			throw new ArgumentException("Writer for type " + typeof(TEnum).Name + " does not exist.");
		}
		value(packet, @enum);
	}
}
