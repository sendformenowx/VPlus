using System.Threading;

namespace NewFilter;

public class users
{
	public string Username;

	public string Password;

	public string Character;

	public UserConnections GW;

	public bool DC;

	public ClientlessMode Mode;

	public users(string _username, string _password, string _character)
	{
		this.Username = _username;
		this.Password = _password;
		this.Character = _character;
		this.GW = null;
		this.DC = true;
		new Thread((ThreadStart)delegate
		{
			this.ClientlessThread();
		}).Start();
	}

	private void ClientlessThread()
	{
		while (MainMenu.Lisans && this.DC)
		{
			try
			{
				if (this.GW != null)
				{
					if (!this.GW.Exit)
					{
						this.GW.Exit = true;
						this.GW.Disconnect();
					}
					else
					{
						this.GW = null;
					}
				}
			}
			catch
			{
			}
			if (this.GW == null)
			{
				this.DC = false;
				this.GW = new UserConnections(this);
			}
			Thread.Sleep(5000);
		}
	}
}
