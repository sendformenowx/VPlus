using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Framework;

public class Security
{
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	private class SecurityFlags
	{
		[FieldOffset(0)]
		public byte none;

		[FieldOffset(1)]
		public byte blowfish;

		[FieldOffset(2)]
		public byte security_bytes;

		[FieldOffset(3)]
		public byte handshake;

		[FieldOffset(4)]
		public byte handshake_response;

		[FieldOffset(5)]
		public byte _6;

		[FieldOffset(6)]
		public byte _7;

		[FieldOffset(7)]
		public byte _8;
	}

	private static uint[] global_security_table = Security.GenerateSecurityTable();

	private static Random random = new Random();

	private uint m_value_x;

	private uint m_value_g;

	private uint m_value_p;

	private uint m_value_A;

	private uint m_value_B;

	private uint m_value_K;

	private uint m_seed_count;

	private uint m_crc_seed;

	private ulong m_initial_blowfish_key;

	private ulong m_handshake_blowfish_key;

	private byte[] m_count_byte_seeds;

	private ulong m_client_key;

	private ulong m_challenge_key;

	private bool m_client_security;

	private byte m_security_flag;

	private SecurityFlags m_security_flags;

	private bool m_accepted_handshake;

	private bool m_started_handshake;

	private byte m_identity_flag;

	private string m_identity_name;

	private List<Packet> m_incoming_packets;

	private List<Packet> m_outgoing_packets;

	private List<ushort> m_enc_opcodes;

	private Blowfish m_blowfish;

	private TransferBuffer m_recv_buffer;

	private TransferBuffer m_current_buffer;

	private ushort m_massive_count;

	private Packet m_massive_packet;

	private object m_class_lock;

	private static SecurityFlags CopySecurityFlags(SecurityFlags flags)
	{
		SecurityFlags securityFlags;
		securityFlags = new SecurityFlags();
		securityFlags.none = flags.none;
		securityFlags.blowfish = flags.blowfish;
		securityFlags.security_bytes = flags.security_bytes;
		securityFlags.handshake = flags.handshake;
		securityFlags.handshake_response = flags.handshake_response;
		securityFlags._6 = flags._6;
		securityFlags._7 = flags._7;
		securityFlags._8 = flags._8;
		return securityFlags;
	}

	private static byte FromSecurityFlags(SecurityFlags flags)
	{
		return (byte)(flags.none | (flags.blowfish << 1) | (flags.security_bytes << 2) | (flags.handshake << 3) | (flags.handshake_response << 4) | (flags._6 << 5) | (flags._7 << 6) | (flags._8 << 7));
	}

	private static SecurityFlags ToSecurityFlags(byte value)
	{
		SecurityFlags securityFlags;
		securityFlags = new SecurityFlags();
		securityFlags.none = (byte)(value & 1u);
		value = (byte)(value >> 1);
		securityFlags.blowfish = (byte)(value & 1u);
		value = (byte)(value >> 1);
		securityFlags.security_bytes = (byte)(value & 1u);
		value = (byte)(value >> 1);
		securityFlags.handshake = (byte)(value & 1u);
		value = (byte)(value >> 1);
		securityFlags.handshake_response = (byte)(value & 1u);
		value = (byte)(value >> 1);
		securityFlags._6 = (byte)(value & 1u);
		value = (byte)(value >> 1);
		securityFlags._7 = (byte)(value & 1u);
		value = (byte)(value >> 1);
		securityFlags._8 = (byte)(value & 1u);
		value = (byte)(value >> 1);
		return securityFlags;
	}

	private static uint[] GenerateSecurityTable()
	{
		uint[] array;
		array = new uint[65536];
		using (MemoryStream input = new MemoryStream(new byte[1024]
		{
			177, 214, 139, 150, 150, 48, 7, 119, 44, 97,
			14, 238, 186, 81, 9, 153, 25, 196, 109, 7,
			143, 244, 106, 112, 53, 165, 99, 233, 163, 149,
			100, 158, 50, 136, 219, 14, 164, 184, 220, 121,
			30, 233, 213, 224, 136, 217, 210, 151, 43, 76,
			182, 9, 189, 124, 177, 126, 7, 45, 184, 231,
			145, 29, 191, 144, 100, 16, 183, 29, 242, 32,
			176, 106, 72, 113, 177, 243, 222, 65, 190, 140,
			125, 212, 218, 26, 235, 228, 221, 109, 81, 181,
			212, 244, 199, 133, 211, 131, 86, 152, 108, 19,
			192, 168, 107, 100, 122, 249, 98, 253, 236, 201,
			101, 138, 79, 92, 1, 20, 217, 108, 6, 99,
			99, 61, 15, 250, 245, 13, 8, 141, 200, 32,
			110, 59, 94, 16, 105, 76, 228, 65, 96, 213,
			114, 113, 103, 162, 209, 228, 3, 60, 71, 212,
			4, 75, 253, 133, 13, 210, 107, 181, 10, 165,
			250, 168, 181, 53, 108, 152, 178, 66, 214, 201,
			187, 219, 64, 249, 188, 172, 227, 108, 216, 50,
			117, 92, 223, 69, 207, 13, 214, 220, 89, 61,
			209, 171, 172, 48, 217, 38, 58, 0, 222, 81,
			128, 81, 215, 200, 22, 97, 208, 191, 181, 244,
			180, 33, 35, 196, 179, 86, 153, 149, 186, 207,
			15, 165, 183, 184, 158, 184, 2, 40, 8, 136,
			5, 95, 178, 217, 236, 198, 36, 233, 11, 177,
			135, 124, 111, 47, 17, 76, 104, 88, 171, 29,
			97, 193, 61, 45, 102, 182, 144, 65, 220, 118,
			6, 113, 219, 1, 188, 32, 210, 152, 42, 16,
			213, 239, 137, 133, 177, 113, 31, 181, 182, 6,
			165, 228, 191, 159, 51, 212, 184, 232, 162, 201,
			7, 120, 52, 249, 160, 15, 142, 168, 9, 150,
			24, 152, 14, 225, 187, 13, 106, 127, 45, 61,
			109, 8, 151, 108, 100, 145, 1, 92, 99, 230,
			244, 81, 107, 107, 98, 97, 108, 28, 216, 48,
			101, 133, 78, 0, 98, 242, 237, 149, 6, 108,
			123, 165, 1, 27, 193, 244, 8, 130, 87, 196,
			15, 245, 198, 217, 176, 99, 80, 233, 183, 18,
			234, 184, 190, 139, 124, 136, 185, 252, 223, 29,
			221, 98, 73, 45, 218, 21, 243, 124, 211, 140,
			101, 76, 212, 251, 88, 97, 178, 77, 206, 81,
			181, 58, 116, 0, 188, 163, 226, 48, 187, 212,
			65, 165, 223, 74, 215, 149, 216, 61, 109, 196,
			209, 164, 251, 244, 214, 211, 106, 233, 105, 67,
			252, 217, 110, 52, 70, 136, 103, 173, 208, 184,
			96, 218, 115, 45, 4, 68, 229, 29, 3, 51,
			95, 76, 10, 170, 201, 124, 13, 221, 60, 113,
			5, 80, 170, 65, 2, 39, 16, 16, 11, 190,
			134, 32, 12, 201, 37, 181, 104, 87, 179, 133,
			111, 32, 9, 212, 102, 185, 159, 228, 97, 206,
			14, 249, 222, 94, 8, 201, 217, 41, 34, 152,
			208, 176, 180, 168, 87, 199, 23, 61, 179, 89,
			129, 13, 180, 62, 59, 92, 189, 183, 173, 108,
			186, 192, 32, 131, 184, 237, 182, 179, 191, 154,
			12, 226, 182, 3, 154, 210, 177, 116, 57, 71,
			213, 234, 175, 119, 210, 157, 21, 38, 219, 4,
			131, 22, 220, 115, 18, 11, 99, 227, 132, 59,
			100, 148, 62, 106, 109, 13, 168, 90, 106, 122,
			11, 207, 14, 228, 157, 255, 9, 147, 39, 174,
			0, 10, 177, 158, 7, 125, 68, 147, 15, 240,
			210, 162, 8, 135, 104, 242, 1, 30, 254, 194,
			6, 105, 93, 87, 98, 247, 203, 103, 101, 128,
			113, 54, 108, 25, 231, 6, 107, 110, 118, 27,
			212, 254, 224, 43, 211, 137, 90, 122, 218, 16,
			204, 74, 221, 103, 111, 223, 185, 249, 249, 239,
			190, 142, 67, 190, 183, 23, 213, 142, 176, 96,
			232, 163, 214, 214, 126, 147, 209, 161, 196, 194,
			216, 56, 82, 242, 223, 79, 241, 103, 187, 209,
			103, 87, 188, 166, 221, 6, 181, 63, 75, 54,
			178, 72, 218, 43, 13, 216, 76, 27, 10, 175,
			246, 74, 3, 54, 96, 122, 4, 65, 195, 239,
			96, 223, 85, 223, 103, 168, 239, 142, 110, 49,
			121, 14, 105, 70, 140, 179, 81, 203, 26, 131,
			99, 188, 160, 210, 111, 37, 54, 226, 104, 82,
			149, 119, 12, 204, 3, 71, 11, 187, 185, 20,
			2, 34, 47, 38, 5, 85, 190, 59, 182, 197,
			40, 11, 189, 178, 146, 90, 180, 43, 4, 106,
			179, 92, 167, 255, 215, 194, 49, 207, 208, 181,
			139, 158, 217, 44, 29, 174, 222, 91, 176, 114,
			100, 155, 38, 242, 227, 236, 156, 163, 106, 117,
			10, 147, 109, 2, 169, 6, 9, 156, 63, 54,
			14, 235, 133, 104, 7, 114, 19, 7, 0, 5,
			130, 72, 191, 149, 20, 122, 184, 226, 174, 43,
			177, 123, 56, 27, 182, 12, 155, 142, 210, 146,
			13, 190, 213, 229, 183, 239, 220, 124, 33, 223,
			219, 11, 148, 210, 211, 134, 66, 226, 212, 241,
			248, 179, 221, 104, 110, 131, 218, 31, 205, 22,
			190, 129, 91, 38, 185, 246, 225, 119, 176, 111,
			119, 71, 183, 24, 224, 90, 8, 136, 112, 106,
			15, 241, 202, 59, 6, 102, 92, 11, 1, 17,
			255, 158, 101, 143, 105, 174, 98, 248, 211, 255,
			107, 97, 69, 207, 108, 22, 120, 226, 10, 160,
			238, 210, 13, 215, 84, 131, 4, 78, 194, 179,
			3, 57, 97, 38, 103, 167, 247, 22, 96, 208,
			77, 71, 105, 73, 219, 119, 110, 62, 74, 106,
			209, 174, 220, 90, 214, 217, 102, 11, 223, 64,
			240, 59, 216, 55, 83, 174, 188, 169, 197, 158,
			187, 222, 127, 207, 178, 71, 233, 255, 181, 48,
			28, 249, 189, 189, 138, 205, 186, 202, 48, 158,
			179, 83, 166, 163, 188, 36, 5, 59, 208, 186,
			163, 6, 215, 205, 233, 87, 222, 84, 191, 103,
			217, 35, 46, 114, 102, 179, 184, 74, 97, 196,
			2, 27, 56, 93, 148, 43, 111, 43, 55, 190,
			203, 180, 161, 142, 204, 195, 27, 223, 13, 90,
			141, 237, 2, 45
		}, writable: false))
		{
			using BinaryReader binaryReader = new BinaryReader(input);
			int num;
			num = 0;
			for (int i = 0; i < 1024; i += 4)
			{
				uint num2;
				num2 = binaryReader.ReadUInt32();
				for (uint num3 = 0u; num3 < 256; num3++)
				{
					uint num4;
					num4 = num3 >> 1;
					if ((num3 & (true ? 1u : 0u)) != 0)
					{
						num4 ^= num2;
					}
					for (int j = 0; j < 7; j++)
					{
						num4 = (((num4 & 1) == 0) ? (num4 >> 1) : ((num4 >> 1) ^ num2));
					}
					array[num++] = num4;
				}
			}
		}
		return array;
	}

	private static ulong MAKELONGLONG_(uint a, uint b)
	{
		return ((ulong)b << 32) | a;
	}

	private static uint MAKELONG_(ushort a, ushort b)
	{
		return (uint)((b << 16) | a);
	}

	private static ushort MAKEWORD_(byte a, byte b)
	{
		return (ushort)((b << 8) | a);
	}

	private static ushort LOWORD_(uint a)
	{
		return (ushort)(a & 0xFFFFu);
	}

	private static ushort HIWORD_(uint a)
	{
		return (ushort)((a >> 16) & 0xFFFFu);
	}

	private static byte LOBYTE_(ushort a)
	{
		return (byte)(a & 0xFFu);
	}

	private static byte HIBYTE_(ushort a)
	{
		return (byte)((uint)(a >> 8) & 0xFFu);
	}

	private static ulong NextUInt64()
	{
		byte[] array;
		array = new byte[8];
		Security.random.NextBytes(array);
		return BitConverter.ToUInt64(array, 0);
	}

	private static uint NextUInt32()
	{
		byte[] array;
		array = new byte[4];
		Security.random.NextBytes(array);
		return BitConverter.ToUInt32(array, 0);
	}

	private static ushort NextUInt16()
	{
		byte[] array;
		array = new byte[2];
		Security.random.NextBytes(array);
		return BitConverter.ToUInt16(array, 0);
	}

	private static byte NextUInt8()
	{
		return (byte)(Security.NextUInt16() & 0xFFu);
	}

	private uint GenerateValue(ref uint val)
	{
		for (int i = 0; i < 32; i++)
		{
			val = (((((((((((val >> 2) ^ val) >> 2) ^ val) >> 1) ^ val) >> 1) ^ val) >> 1) ^ val) & 1u) | ((((val & 1) << 31) | (val >> 1)) & 0xFFFFFFFEu);
		}
		return val;
	}

	private void SetupCountByte(uint seed)
	{
		if (seed == 0)
		{
			seed = 2596254646u;
		}
		uint val;
		val = seed;
		uint num;
		num = this.GenerateValue(ref val);
		uint num2;
		num2 = this.GenerateValue(ref val);
		uint num3;
		num3 = this.GenerateValue(ref val);
		this.GenerateValue(ref val);
		byte b;
		b = (byte)((val & 0xFFu) ^ (num3 & 0xFFu));
		byte b2;
		b2 = (byte)((num & 0xFFu) ^ (num2 & 0xFFu));
		if (b == 0)
		{
			b = 1;
		}
		if (b2 == 0)
		{
			b2 = 1;
		}
		this.m_count_byte_seeds[0] = (byte)(b ^ b2);
		this.m_count_byte_seeds[1] = b2;
		this.m_count_byte_seeds[2] = b;
	}

	private uint G_pow_X_mod_P(uint P, uint X, uint G)
	{
		long num;
		num = 1L;
		long num2;
		num2 = G;
		if (X == 0)
		{
			return 1u;
		}
		while (X != 0)
		{
			if ((X & (true ? 1u : 0u)) != 0)
			{
				num = num2 * num % (long)P;
			}
			X >>= 1;
			num2 = num2 * num2 % (long)P;
		}
		return (uint)num;
	}

	private void KeyTransformValue(ref ulong val, uint key, byte key_byte)
	{
		byte[] bytes;
		bytes = BitConverter.GetBytes(val);
		bytes[0] ^= (byte)(bytes[0] + Security.LOBYTE_(Security.LOWORD_(key)) + key_byte);
		bytes[1] ^= (byte)(bytes[1] + Security.HIBYTE_(Security.LOWORD_(key)) + key_byte);
		bytes[2] ^= (byte)(bytes[2] + Security.LOBYTE_(Security.HIWORD_(key)) + key_byte);
		bytes[3] ^= (byte)(bytes[3] + Security.HIBYTE_(Security.HIWORD_(key)) + key_byte);
		bytes[4] ^= (byte)(bytes[4] + Security.LOBYTE_(Security.LOWORD_(key)) + key_byte);
		bytes[5] ^= (byte)(bytes[5] + Security.HIBYTE_(Security.LOWORD_(key)) + key_byte);
		bytes[6] ^= (byte)(bytes[6] + Security.LOBYTE_(Security.HIWORD_(key)) + key_byte);
		bytes[7] ^= (byte)(bytes[7] + Security.HIBYTE_(Security.HIWORD_(key)) + key_byte);
		val = BitConverter.ToUInt64(bytes, 0);
	}

	private byte GenerateCountByte(bool update)
	{
		byte b;
		b = (byte)(this.m_count_byte_seeds[2] * (~this.m_count_byte_seeds[0] + this.m_count_byte_seeds[1]));
		b = (byte)(b ^ (b >> 4));
		if (update)
		{
			this.m_count_byte_seeds[0] = b;
		}
		return b;
	}

	private byte GenerateCheckByte(byte[] stream, int offset, int length)
	{
		uint num;
		num = uint.MaxValue;
		uint num2;
		num2 = this.m_crc_seed << 8;
		for (int i = offset; i < offset + length; i++)
		{
			num = (num >> 8) ^ Security.global_security_table[num2 + ((stream[i] ^ num) & 0xFF)];
		}
		return (byte)(((num >> 24) & 0xFF) + ((num >> 8) & 0xFF) + ((num >> 16) & 0xFF) + (num & 0xFF));
	}

	private byte GenerateCheckByte(byte[] stream)
	{
		return this.GenerateCheckByte(stream, 0, stream.Length);
	}

	private void GenerateSecurity(SecurityFlags flags)
	{
		this.m_security_flag = Security.FromSecurityFlags(flags);
		this.m_security_flags = flags;
		this.m_client_security = true;
		Packet packet;
		packet = new Packet(20480);
		packet.WriteUInt8(this.m_security_flag);
		if (this.m_security_flags.blowfish == 1)
		{
			this.m_initial_blowfish_key = Security.NextUInt64();
			this.m_blowfish.Initialize(BitConverter.GetBytes(this.m_initial_blowfish_key));
			packet.WriteUInt64(this.m_initial_blowfish_key);
		}
		if (this.m_security_flags.security_bytes == 1)
		{
			this.m_seed_count = Security.NextUInt8();
			this.SetupCountByte(this.m_seed_count);
			this.m_crc_seed = Security.NextUInt8();
			packet.WriteUInt32(this.m_seed_count);
			packet.WriteUInt32(this.m_crc_seed);
		}
		if (this.m_security_flags.handshake == 1)
		{
			this.m_handshake_blowfish_key = Security.NextUInt64();
			this.m_value_x = Security.NextUInt32() & 0x7FFFFFFFu;
			this.m_value_g = Security.NextUInt32() & 0x7FFFFFFFu;
			this.m_value_p = Security.NextUInt32() & 0x7FFFFFFFu;
			this.m_value_A = this.G_pow_X_mod_P(this.m_value_p, this.m_value_x, this.m_value_g);
			packet.WriteUInt64(this.m_handshake_blowfish_key);
			packet.WriteUInt32(this.m_value_g);
			packet.WriteUInt32(this.m_value_p);
			packet.WriteUInt32(this.m_value_A);
		}
		this.m_outgoing_packets.Add(packet);
	}

	private void Handshake(ushort packet_opcode, PacketReader packet_data, bool packet_encrypted)
	{
		if (packet_encrypted)
		{
			throw new Exception("[SecurityAPI::Handshake] Received an illogical (encrypted) handshake packet.");
		}
		if (this.m_client_security)
		{
			if (this.m_security_flags.handshake == 0)
			{
				switch (packet_opcode)
				{
				case 36864:
					if (this.m_accepted_handshake)
					{
						throw new Exception("[SecurityAPI::Handshake] Received an illogical handshake packet (duplicate 0x9000).");
					}
					this.m_accepted_handshake = true;
					break;
				case 20480:
					throw new Exception("[SecurityAPI::Handshake] Received an illogical handshake packet (0x5000 with no handshake).");
				default:
					throw new Exception("[SecurityAPI::Handshake] Received an illogical handshake packet (programmer error).");
				}
				return;
			}
			switch (packet_opcode)
			{
			case 36864:
				if (!this.m_started_handshake)
				{
					throw new Exception("[SecurityAPI::Handshake] Received an illogical handshake packet (out of order 0x9000).");
				}
				if (this.m_accepted_handshake)
				{
					throw new Exception("[SecurityAPI::Handshake] Received an illogical handshake packet (duplicate 0x9000).");
				}
				this.m_accepted_handshake = true;
				break;
			case 20480:
			{
				if (this.m_started_handshake)
				{
					throw new Exception("[SecurityAPI::Handshake] Received an illogical handshake packet (duplicate 0x5000).");
				}
				this.m_started_handshake = true;
				this.m_value_B = packet_data.ReadUInt32();
				this.m_client_key = packet_data.ReadUInt64();
				this.m_value_K = this.G_pow_X_mod_P(this.m_value_p, this.m_value_x, this.m_value_B);
				ulong val;
				val = Security.MAKELONGLONG_(this.m_value_A, this.m_value_B);
				this.KeyTransformValue(ref val, this.m_value_K, (byte)(Security.LOBYTE_(Security.LOWORD_(this.m_value_K)) & 3u));
				this.m_blowfish.Initialize(BitConverter.GetBytes(val));
				this.m_client_key = BitConverter.ToUInt64(this.m_blowfish.Decode(BitConverter.GetBytes(this.m_client_key)), 0);
				val = Security.MAKELONGLONG_(this.m_value_B, this.m_value_A);
				this.KeyTransformValue(ref val, this.m_value_K, (byte)(Security.LOBYTE_(Security.LOWORD_(this.m_value_B)) & 7u));
				if (this.m_client_key != val)
				{
					throw new Exception("[SecurityAPI::Handshake] Client signature error.");
				}
				val = Security.MAKELONGLONG_(this.m_value_A, this.m_value_B);
				this.KeyTransformValue(ref val, this.m_value_K, (byte)(Security.LOBYTE_(Security.LOWORD_(this.m_value_K)) & 3u));
				this.m_blowfish.Initialize(BitConverter.GetBytes(val));
				this.m_challenge_key = Security.MAKELONGLONG_(this.m_value_A, this.m_value_B);
				this.KeyTransformValue(ref this.m_challenge_key, this.m_value_K, (byte)(Security.LOBYTE_(Security.LOWORD_(this.m_value_A)) & 7u));
				this.m_challenge_key = BitConverter.ToUInt64(this.m_blowfish.Encode(BitConverter.GetBytes(this.m_challenge_key)), 0);
				this.KeyTransformValue(ref this.m_handshake_blowfish_key, this.m_value_K, 3);
				this.m_blowfish.Initialize(BitConverter.GetBytes(this.m_handshake_blowfish_key));
				SecurityFlags securityFlags;
				securityFlags = new SecurityFlags();
				securityFlags.handshake_response = 1;
				byte value;
				value = Security.FromSecurityFlags(securityFlags);
				Packet packet;
				packet = new Packet(20480);
				packet.WriteUInt8(value);
				packet.WriteUInt64(this.m_challenge_key);
				this.m_outgoing_packets.Add(packet);
				break;
			}
			default:
				throw new Exception("[SecurityAPI::Handshake] Received an illogical handshake packet (programmer error).");
			}
			return;
		}
		if (packet_opcode != 20480)
		{
			throw new Exception("[SecurityAPI::Handshake] Received an illogical handshake packet (programmer error).");
		}
		byte b;
		b = packet_data.ReadByte();
		SecurityFlags securityFlags2;
		securityFlags2 = Security.ToSecurityFlags(b);
		if (this.m_security_flag == 0)
		{
			this.m_security_flag = b;
			this.m_security_flags = securityFlags2;
		}
		if (securityFlags2.blowfish == 1)
		{
			this.m_initial_blowfish_key = packet_data.ReadUInt64();
			this.m_blowfish.Initialize(BitConverter.GetBytes(this.m_initial_blowfish_key));
		}
		if (securityFlags2.security_bytes == 1)
		{
			this.m_seed_count = packet_data.ReadUInt32();
			this.m_crc_seed = packet_data.ReadUInt32();
			this.SetupCountByte(this.m_seed_count);
		}
		if (securityFlags2.handshake == 1)
		{
			this.m_handshake_blowfish_key = packet_data.ReadUInt64();
			this.m_value_g = packet_data.ReadUInt32();
			this.m_value_p = packet_data.ReadUInt32();
			this.m_value_A = packet_data.ReadUInt32();
			this.m_value_x = Security.NextUInt32() & 0x7FFFFFFFu;
			this.m_value_B = this.G_pow_X_mod_P(this.m_value_p, this.m_value_x, this.m_value_g);
			this.m_value_K = this.G_pow_X_mod_P(this.m_value_p, this.m_value_x, this.m_value_A);
			ulong val2;
			val2 = Security.MAKELONGLONG_(this.m_value_A, this.m_value_B);
			this.KeyTransformValue(ref val2, this.m_value_K, (byte)(Security.LOBYTE_(Security.LOWORD_(this.m_value_K)) & 3u));
			this.m_blowfish.Initialize(BitConverter.GetBytes(val2));
			this.m_client_key = Security.MAKELONGLONG_(this.m_value_B, this.m_value_A);
			this.KeyTransformValue(ref this.m_client_key, this.m_value_K, (byte)(Security.LOBYTE_(Security.LOWORD_(this.m_value_B)) & 7u));
			this.m_client_key = BitConverter.ToUInt64(this.m_blowfish.Encode(BitConverter.GetBytes(this.m_client_key)), 0);
		}
		if (securityFlags2.handshake_response == 1)
		{
			this.m_challenge_key = packet_data.ReadUInt64();
			ulong val3;
			val3 = Security.MAKELONGLONG_(this.m_value_A, this.m_value_B);
			this.KeyTransformValue(ref val3, this.m_value_K, (byte)(Security.LOBYTE_(Security.LOWORD_(this.m_value_A)) & 7u));
			ulong num;
			num = BitConverter.ToUInt64(this.m_blowfish.Encode(BitConverter.GetBytes(val3)), 0);
			if (this.m_challenge_key != num)
			{
				throw new Exception("[SecurityAPI::Handshake] Server signature error.");
			}
			this.KeyTransformValue(ref this.m_handshake_blowfish_key, this.m_value_K, 3);
			this.m_blowfish.Initialize(BitConverter.GetBytes(this.m_handshake_blowfish_key));
		}
		if (securityFlags2.handshake == 1 && securityFlags2.handshake_response == 0)
		{
			if (this.m_started_handshake || this.m_accepted_handshake)
			{
				throw new Exception("[SecurityAPI::Handshake] Received an illogical handshake packet (duplicate 0x5000).");
			}
			Packet packet2;
			packet2 = new Packet(20480);
			packet2.WriteUInt32(this.m_value_B);
			packet2.WriteUInt64(this.m_client_key);
			this.m_outgoing_packets.Insert(0, packet2);
			this.m_started_handshake = true;
			return;
		}
		if (this.m_accepted_handshake)
		{
			throw new Exception("[SecurityAPI::Handshake] Received an illogical handshake packet (duplicate 0x5000).");
		}
		Packet item;
		item = new Packet(36864);
		Packet packet3;
		packet3 = new Packet(8193, encrypted: true, massive: false);
		packet3.WriteAscii(this.m_identity_name);
		packet3.WriteUInt8(this.m_identity_flag);
		this.m_outgoing_packets.Insert(0, packet3);
		this.m_outgoing_packets.Insert(0, item);
		this.m_started_handshake = true;
		this.m_accepted_handshake = true;
	}

	private byte[] FormatPacket(ushort opcode, byte[] data, bool encrypted)
	{
		if (data.Length >= 32768)
		{
			throw new Exception("[SecurityAPI::FormatPacket] Payload is too large!");
		}
		ushort num;
		num = (ushort)data.Length;
		PacketWriter packetWriter;
		packetWriter = new PacketWriter();
		packetWriter.Write(num);
		packetWriter.Write(opcode);
		packetWriter.Write((ushort)0);
		packetWriter.Write(data);
		packetWriter.Flush();
		if (encrypted && (this.m_security_flags.blowfish == 1 || (this.m_security_flags.security_bytes == 1 && this.m_security_flags.blowfish == 0)))
		{
			long offset;
			offset = packetWriter.BaseStream.Seek(0L, SeekOrigin.Current);
			ushort value;
			value = (ushort)(num | 0x8000u);
			packetWriter.BaseStream.Seek(0L, SeekOrigin.Begin);
			packetWriter.Write(value);
			packetWriter.Flush();
			packetWriter.BaseStream.Seek(offset, SeekOrigin.Begin);
		}
		if (!this.m_client_security && this.m_security_flags.security_bytes == 1)
		{
			long offset2;
			offset2 = packetWriter.BaseStream.Seek(0L, SeekOrigin.Current);
			byte value2;
			value2 = this.GenerateCountByte(update: true);
			packetWriter.BaseStream.Seek(4L, SeekOrigin.Begin);
			packetWriter.Write(value2);
			packetWriter.Flush();
			byte value3;
			value3 = this.GenerateCheckByte(packetWriter.GetBytes());
			packetWriter.BaseStream.Seek(5L, SeekOrigin.Begin);
			packetWriter.Write(value3);
			packetWriter.Flush();
			packetWriter.BaseStream.Seek(offset2, SeekOrigin.Begin);
		}
		if (encrypted && this.m_security_flags.blowfish == 1)
		{
			byte[] bytes;
			bytes = packetWriter.GetBytes();
			byte[] buffer;
			buffer = this.m_blowfish.Encode(bytes, 2, bytes.Length - 2);
			packetWriter.BaseStream.Seek(2L, SeekOrigin.Begin);
			packetWriter.Write(buffer);
			packetWriter.Flush();
		}
		else if (encrypted && this.m_security_flags.security_bytes == 1 && this.m_security_flags.blowfish == 0)
		{
			long offset3;
			offset3 = packetWriter.BaseStream.Seek(0L, SeekOrigin.Current);
			packetWriter.BaseStream.Seek(0L, SeekOrigin.Begin);
			packetWriter.Write(num);
			packetWriter.Flush();
			packetWriter.BaseStream.Seek(offset3, SeekOrigin.Begin);
		}
		return packetWriter.GetBytes();
	}

	private bool HasPacketToSend()
	{
		if (this.m_outgoing_packets.Count == 0)
		{
			return false;
		}
		if (this.m_accepted_handshake)
		{
			return true;
		}
		Packet packet;
		packet = this.m_outgoing_packets[0];
		if (packet.Opcode == 20480 || packet.Opcode == 36864)
		{
			return true;
		}
		return false;
	}

	private KeyValuePair<TransferBuffer, Packet> GetPacketToSend()
	{
		if (this.m_outgoing_packets.Count == 0)
		{
			throw new Exception("[SecurityAPI::GetPacketToSend] No packets are avaliable to send.");
		}
		Packet packet;
		packet = this.m_outgoing_packets[0];
		this.m_outgoing_packets.RemoveAt(0);
		if (packet.Massive)
		{
			ushort num;
			num = 0;
			PacketWriter packetWriter;
			packetWriter = new PacketWriter();
			PacketWriter packetWriter2;
			packetWriter2 = new PacketWriter();
			byte[] bytes;
			bytes = packet.GetBytes();
			new PacketReader(bytes);
			TransferBuffer transferBuffer;
			transferBuffer = new TransferBuffer(4089, 0, bytes.Length);
			while (transferBuffer.Size > 0)
			{
				PacketWriter packetWriter3;
				packetWriter3 = new PacketWriter();
				int num2;
				num2 = ((transferBuffer.Size > 4089) ? 4089 : transferBuffer.Size);
				packetWriter3.Write((byte)0);
				packetWriter3.Write(bytes, transferBuffer.Offset, num2);
				transferBuffer.Offset += num2;
				transferBuffer.Size -= num2;
				packetWriter2.Write(this.FormatPacket(24589, packetWriter3.GetBytes(), encrypted: false));
				num = (ushort)(num + 1);
			}
			PacketWriter packetWriter4;
			packetWriter4 = new PacketWriter();
			packetWriter4.Write((byte)1);
			packetWriter4.Write((short)num);
			packetWriter4.Write(packet.Opcode);
			packetWriter.Write(this.FormatPacket(24589, packetWriter4.GetBytes(), encrypted: false));
			packetWriter.Write(packetWriter2.GetBytes());
			byte[] bytes2;
			bytes2 = packetWriter.GetBytes();
			packet.Lock();
			return new KeyValuePair<TransferBuffer, Packet>(new TransferBuffer(bytes2, 0, bytes2.Length, assign: true), packet);
		}
		bool encrypted;
		encrypted = packet.Encrypted;
		if (!this.m_client_security && this.m_enc_opcodes.Contains(packet.Opcode))
		{
			encrypted = true;
		}
		byte[] array;
		array = this.FormatPacket(packet.Opcode, packet.GetBytes(), encrypted);
		packet.Lock();
		return new KeyValuePair<TransferBuffer, Packet>(new TransferBuffer(array, 0, array.Length, assign: true), packet);
	}

	public Security()
	{
		this.m_value_x = 0u;
		this.m_value_g = 0u;
		this.m_value_p = 0u;
		this.m_value_A = 0u;
		this.m_value_B = 0u;
		this.m_value_K = 0u;
		this.m_seed_count = 0u;
		this.m_crc_seed = 0u;
		this.m_initial_blowfish_key = 0uL;
		this.m_handshake_blowfish_key = 0uL;
		this.m_count_byte_seeds = new byte[3];
		this.m_count_byte_seeds[0] = 0;
		this.m_count_byte_seeds[1] = 0;
		this.m_count_byte_seeds[2] = 0;
		this.m_client_key = 0uL;
		this.m_challenge_key = 0uL;
		this.m_client_security = false;
		this.m_security_flag = 0;
		this.m_security_flags = new SecurityFlags();
		this.m_accepted_handshake = false;
		this.m_started_handshake = false;
		this.m_identity_flag = 0;
		this.m_identity_name = "SR_Client";
		this.m_outgoing_packets = new List<Packet>();
		this.m_incoming_packets = new List<Packet>();
		this.m_enc_opcodes = new List<ushort>();
		this.m_enc_opcodes.Add(8193);
		this.m_enc_opcodes.Add(24832);
		this.m_enc_opcodes.Add(24833);
		this.m_enc_opcodes.Add(24834);
		this.m_enc_opcodes.Add(24835);
		this.m_enc_opcodes.Add(24839);
		this.m_blowfish = new Blowfish();
		this.m_recv_buffer = new TransferBuffer(8192);
		this.m_current_buffer = null;
		this.m_massive_count = 0;
		this.m_massive_packet = null;
		this.m_class_lock = new object();
	}

	public void ChangeIdentity(string name, byte flag)
	{
		lock (this.m_class_lock)
		{
			this.m_identity_name = name;
			this.m_identity_flag = flag;
		}
	}

	public void GenerateSecurity(bool blowfish, bool security_bytes, bool handshake)
	{
		lock (this.m_class_lock)
		{
			SecurityFlags securityFlags;
			securityFlags = new SecurityFlags();
			if (blowfish)
			{
				securityFlags.none = 0;
				securityFlags.blowfish = 1;
			}
			if (security_bytes)
			{
				securityFlags.none = 0;
				securityFlags.security_bytes = 1;
			}
			if (handshake)
			{
				securityFlags.none = 0;
				securityFlags.handshake = 1;
			}
			if (!blowfish && !security_bytes && !handshake)
			{
				securityFlags.none = 1;
			}
			this.GenerateSecurity(securityFlags);
		}
	}

	public void AddEncryptedOpcode(ushort opcode)
	{
		lock (this.m_class_lock)
		{
			if (!this.m_enc_opcodes.Contains(opcode))
			{
				this.m_enc_opcodes.Add(opcode);
			}
		}
	}

	public void Send(Packet packet)
	{
		if (packet.Opcode == 20480 || packet.Opcode == 36864)
		{
			throw new Exception("[SecurityAPI::Send] Handshake packets cannot be sent through this function.");
		}
		lock (this.m_class_lock)
		{
			this.m_outgoing_packets.Add(packet);
		}
	}

	public void Recv(byte[] buffer, int offset, int length)
	{
		this.Recv(new TransferBuffer(buffer, offset, length, assign: true));
	}

	public void Recv(TransferBuffer raw_buffer)
	{
		List<TransferBuffer> list;
		list = new List<TransferBuffer>();
		lock (this.m_class_lock)
		{
			int num;
			num = raw_buffer.Size - raw_buffer.Offset;
			int num2;
			num2 = 0;
			while (num > 0)
			{
				int num3;
				num3 = num;
				int num4;
				num4 = this.m_recv_buffer.Buffer.Length - this.m_recv_buffer.Size;
				if (num3 > num4)
				{
					num3 = num4;
				}
				num -= num3;
				raw_buffer.Buffer.CopyTo(this.m_recv_buffer.Buffer, raw_buffer.Offset + num2);
				this.m_recv_buffer.Size += num3;
				num2 += num3;
				while (this.m_recv_buffer.Size > 0)
				{
					if (this.m_current_buffer == null)
					{
						if (this.m_recv_buffer.Size < 2)
						{
							break;
						}
						int num5;
						num5 = (this.m_recv_buffer.Buffer[1] << 8) | this.m_recv_buffer.Buffer[0];
						if ((num5 & 0x8000) > 0)
						{
							num5 &= 0x7FFF;
							num5 = ((this.m_security_flags.blowfish != 1) ? (num5 + 6) : (2 + this.m_blowfish.GetOutputLength(num5 + 4)));
						}
						else
						{
							num5 += 6;
						}
						this.m_current_buffer = new TransferBuffer(num5, 0, num5);
					}
					int num6;
					num6 = this.m_current_buffer.Size - this.m_current_buffer.Offset;
					if (num6 > this.m_recv_buffer.Size)
					{
						num6 = this.m_recv_buffer.Size;
					}
					Buffer.BlockCopy(this.m_recv_buffer.Buffer, 0, this.m_current_buffer.Buffer, this.m_current_buffer.Offset, num6);
					this.m_current_buffer.Offset += num6;
					this.m_recv_buffer.Size -= num6;
					if (this.m_recv_buffer.Size > 0)
					{
						Buffer.BlockCopy(this.m_recv_buffer.Buffer, num6, this.m_recv_buffer.Buffer, 0, this.m_recv_buffer.Size);
					}
					if (this.m_current_buffer.Size == this.m_current_buffer.Offset)
					{
						this.m_current_buffer.Offset = 0;
						list.Add(this.m_current_buffer);
						this.m_current_buffer = null;
						continue;
					}
					break;
				}
			}
			if (list.Count <= 0)
			{
				return;
			}
			foreach (TransferBuffer item in list)
			{
				bool flag;
				flag = false;
				int num7;
				num7 = (item.Buffer[1] << 8) | item.Buffer[0];
				if ((num7 & 0x8000) > 0)
				{
					if (this.m_security_flags.blowfish == 1)
					{
						num7 &= 0x7FFF;
						flag = true;
					}
					else
					{
						num7 &= 0x7FFF;
					}
				}
				if (flag)
				{
					byte[] src;
					src = this.m_blowfish.Decode(item.Buffer, 2, item.Size - 2);
					byte[] array;
					array = new byte[6 + num7];
					Buffer.BlockCopy(BitConverter.GetBytes((ushort)num7), 0, array, 0, 2);
					Buffer.BlockCopy(src, 0, array, 2, 4 + num7);
					item.Buffer = null;
					item.Buffer = array;
				}
				PacketReader packetReader;
				packetReader = new PacketReader(item.Buffer);
				num7 = packetReader.ReadUInt16();
				ushort num8;
				num8 = packetReader.ReadUInt16();
				byte b;
				b = packetReader.ReadByte();
				byte b2;
				b2 = packetReader.ReadByte();
				if (this.m_client_security && this.m_security_flags.security_bytes == 1)
				{
					if (b != this.GenerateCountByte(update: true))
					{
						throw new Exception("[SecurityAPI::Recv] Count byte mismatch.");
					}
					if ((flag || (this.m_security_flags.security_bytes == 1 && this.m_security_flags.blowfish == 0)) && (flag || this.m_enc_opcodes.Contains(num8)))
					{
						num7 |= 0x8000;
						Buffer.BlockCopy(BitConverter.GetBytes((ushort)num7), 0, item.Buffer, 0, 2);
					}
					item.Buffer[5] = 0;
					if (b2 != this.GenerateCheckByte(item.Buffer))
					{
						throw new Exception("[SecurityAPI::Recv] CRC byte mismatch.");
					}
					item.Buffer[4] = 0;
					if ((flag || (this.m_security_flags.security_bytes == 1 && this.m_security_flags.blowfish == 0)) && (flag || this.m_enc_opcodes.Contains(num8)))
					{
						num7 &= 0x7FFF;
						Buffer.BlockCopy(BitConverter.GetBytes((ushort)num7), 0, item.Buffer, 0, 2);
					}
				}
				if (num8 == 20480 || num8 == 36864)
				{
					this.Handshake(num8, packetReader, flag);
					Packet packet;
					packet = new Packet(num8, flag, massive: false, item.Buffer, 6, num7);
					packet.Lock();
					this.m_incoming_packets.Add(packet);
					continue;
				}
				if (this.m_client_security && !this.m_accepted_handshake)
				{
					throw new Exception("[SecurityAPI::Recv] The client has not accepted the handshake.");
				}
				if (num8 == 24589)
				{
					if (packetReader.ReadByte() == 1)
					{
						this.m_massive_count = packetReader.ReadUInt16();
						this.m_massive_packet = new Packet(packetReader.ReadUInt16(), flag, massive: true);
						continue;
					}
					if (this.m_massive_packet == null)
					{
						throw new Exception("[SecurityAPI::Recv] A malformed 0x600D packet was received.");
					}
					this.m_massive_packet.WriteUInt8Array(packetReader.ReadBytes(num7 - 1));
					this.m_massive_count--;
					if (this.m_massive_count == 0)
					{
						this.m_massive_packet.Lock();
						this.m_incoming_packets.Add(this.m_massive_packet);
						this.m_massive_packet = null;
					}
				}
				else
				{
					Packet packet2;
					packet2 = new Packet(num8, flag, massive: false, item.Buffer, 6, num7);
					packet2.Lock();
					this.m_incoming_packets.Add(packet2);
				}
			}
		}
	}

	public List<KeyValuePair<TransferBuffer, Packet>> TransferOutgoing()
	{
		List<KeyValuePair<TransferBuffer, Packet>> list;
		list = new List<KeyValuePair<TransferBuffer, Packet>>();
		lock (this.m_class_lock)
		{
			if (this.HasPacketToSend())
			{
				list = new List<KeyValuePair<TransferBuffer, Packet>>();
				while (this.HasPacketToSend())
				{
					list.Add(this.GetPacketToSend());
				}
			}
		}
		return list;
	}

	public List<Packet> TransferIncoming()
	{
		List<Packet> result;
		result = new List<Packet>();
		lock (this.m_class_lock)
		{
			if (this.m_incoming_packets.Count > 0)
			{
				result = this.m_incoming_packets;
				this.m_incoming_packets = new List<Packet>();
			}
		}
		return result;
	}
}
