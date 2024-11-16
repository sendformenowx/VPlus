using System;
using System.IO;
using System.Windows.Forms;

namespace NewFilter;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new MainMenu());
	}

	public static void WriteError(string msg, string Infor)
	{
		try
		{
			StreamWriter streamWriter;
			streamWriter = new StreamWriter("ErrorLog.txt", append: true);
			streamWriter.WriteLine(">>> " + DateTime.Now.ToString() + " INFOR: [" + Infor + "]\n{");
			streamWriter.WriteLine(msg);
			streamWriter.WriteLine("}\n\n");
			streamWriter.Close();
		}
		catch
		{
		}
	}
}
