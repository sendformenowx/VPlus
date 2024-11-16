using System.Runtime.InteropServices;
using System.Text;

namespace NewFilter;

public class iniFile
{
	public string path;

	public string Varsayilan { get; set; }

	[DllImport("kernel32")]
	private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

	[DllImport("kernel32")]
	private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

	public iniFile(string INIPath)
	{
		try
		{
			this.path = INIPath;
		}
		catch
		{
		}
	}

	public void IniWriteValue(string Section, string Key, string Value)
	{
		try
		{
			iniFile.WritePrivateProfileString(Section, Key, Value, this.path);
		}
		catch
		{
		}
	}

	public string IniReadValue(string Section, string Key)
	{
		try
		{
			StringBuilder stringBuilder;
			stringBuilder = new StringBuilder(512);
			iniFile.GetPrivateProfileString(Section, Key, "", stringBuilder, 512, this.path);
			return stringBuilder.ToString();
		}
		catch
		{
		}
		return string.Empty;
	}

	public void DeleteKey(string Key, string Section = null)
	{
		this.IniWriteValue(Key, null, Section);
	}

	public void DeleteSection(string Section = null)
	{
		this.IniWriteValue(null, null, Section);
	}

	public bool KeyExists(string Key, string Section = null)
	{
		return this.IniReadValue(Key, Section).Length > 0;
	}

	public string Read(string bolum, string ayaradi)
	{
		this.Varsayilan = this.Varsayilan ?? string.Empty;
		StringBuilder stringBuilder;
		stringBuilder = new StringBuilder(256);
		iniFile.GetPrivateProfileString(bolum, ayaradi, this.Varsayilan, stringBuilder, 255, this.path);
		return stringBuilder.ToString();
	}

	public long Write(string bolum, string ayaradi, string deger)
	{
		return iniFile.WritePrivateProfileString(bolum, ayaradi, deger, this.path);
	}
}
