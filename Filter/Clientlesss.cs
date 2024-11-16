using System.Threading;
using Framework;
using NewFilter;

namespace Filter;

public class Clientlesss
{
	public string Username;

	public string Password;

	public string Character;

	public Gateway GW;

	public Agent AG;

	public bool DC;

	public ClientlessMode Mode;

	public Clientlesss(string _username, string _password, string _character)
	{
		this.Username = _username;
		this.Password = _password;
		this.Character = _character;
		this.GW = null;
		this.AG = null;
		this.DC = true;
		this.Mode = ClientlessMode.None;
		new Thread((ThreadStart)delegate
		{
			this.ClientlessThread();
		}).Start();
	}

	private void ClientlessThread()
	{
		while (MainMenu.AUTOEVENT_ENABLE)
		{
			if (this.DC)
			{
				try
				{
					if (this.AG != null)
					{
						if (!this.AG.isExit)
						{
							this.AG.isExit = true;
							this.AG.Disconnect();
						}
						else
						{
							this.AG = null;
						}
					}
				}
				catch
				{
				}
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
				if (this.GW == null && this.AG == null)
				{
					MainMenu.WriteLine(1, "[AutoEvent]: Starting AutoEvent with Character [ " + this.Character + " ]");
					this.DC = false;
					this.GW = new Gateway(this);
				}
				Thread.Sleep(5000);
			}
			else
			{
				if (this.Mode == ClientlessMode.Gateway)
				{
					this.GW.Security.Send(new Packet(8194));
					this.GW.SendToServer();
				}
				else if (this.Mode == ClientlessMode.Agent)
				{
					this.AG.Security.Send(new Packet(8194));
					this.AG.SendToServer();
				}
				Thread.Sleep(8000);
			}
		}
	}
}
