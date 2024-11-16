using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Framework;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Utility
{
	public static string HexDump(byte[] buffer)
	{
		return Utility.HexDump(buffer, 0, buffer.Length);
	}

	public static string HexDump(byte[] buffer, int offset, int count)
	{
		StringBuilder stringBuilder;
		stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2;
		stringBuilder2 = new StringBuilder();
		int num;
		num = count;
		if (num % 16 != 0)
		{
			num += 16 - num % 16;
		}
		for (int i = 0; i <= num; i++)
		{
			if (i % 16 == 0)
			{
				if (i > 0)
				{
					stringBuilder.AppendFormat("  {0}{1}", stringBuilder2.ToString(), Environment.NewLine);
					stringBuilder2.Clear();
				}
				if (i != num)
				{
					stringBuilder.AppendFormat("{0:d10}   ", i);
				}
			}
			if (i < count)
			{
				stringBuilder.AppendFormat("{0:X2} ", buffer[offset + i]);
				char c;
				c = (char)buffer[offset + i];
				if (!char.IsControl(c))
				{
					stringBuilder2.AppendFormat("{0}", c);
				}
				else
				{
					stringBuilder2.Append(".");
				}
			}
			else
			{
				stringBuilder.Append("   ");
				stringBuilder2.Append(".");
			}
		}
		return stringBuilder.ToString();
	}
}
