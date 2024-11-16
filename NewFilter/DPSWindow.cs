using System;
using System.Data;

namespace NewFilter;

internal class DPSWindow
{
	public static DataTable UniqueAtacker = new DataTable();

	public static void addcolomn()
	{
		if (DPSWindow.UniqueAtacker.Columns.Count == 0)
		{
			DataColumn dataColumn;
			dataColumn = new DataColumn();
			dataColumn.DataType = Type.GetType("System.String");
			dataColumn.ColumnName = "charname";
			DPSWindow.UniqueAtacker.Columns.Add(dataColumn);
			dataColumn = new DataColumn();
			dataColumn.DataType = Type.GetType("System.Int32");
			dataColumn.ColumnName = "Atackvalue";
			DPSWindow.UniqueAtacker.Columns.Add(dataColumn);
		}
	}

	public static void addrows(string charname, int Atackvalue)
	{
		DataRow dataRow;
		dataRow = DPSWindow.UniqueAtacker.NewRow();
		dataRow["charname"] = charname;
		dataRow["Atackvalue"] = Atackvalue;
		DPSWindow.UniqueAtacker.Rows.Add(dataRow);
		DPSWindow.UniqueAtacker.DefaultView.Sort = "Atackvalue DESC";
		DPSWindow.UniqueAtacker = DPSWindow.UniqueAtacker.DefaultView.ToTable(true);
	}

	public static string FormatNumber(long num)
	{
		if (num >= 100000000)
		{
			return ((double)num / 1000000.0).ToString("0.#M");
		}
		if (num >= 1000000)
		{
			return ((double)num / 1000000.0).ToString("0.##M");
		}
		if (num >= 100000)
		{
			return ((double)num / 1000.0).ToString("0.#k");
		}
		if (num >= 10000)
		{
			return ((double)num / 1000.0).ToString("0.##k");
		}
		return num.ToString("#,0");
	}
}
