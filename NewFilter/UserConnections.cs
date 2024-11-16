using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Framework;

namespace NewFilter;

public class UserConnections
{
	public string Charname = string.Empty;

	public byte[] Buffer;

	public Socket Socket;

	public Security Security;

	public users Clientless;

	public bool Exit;

	public bool Success;

	public UserConnections(users _clientless)
	{
		this.Socket = null;
		this.Security = new Security();
		this.Buffer = new byte[4096];
		this.Clientless = _clientless;
		this.Exit = false;
		this.Success = false;
		try
		{
			this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.Socket.BeginConnect(MainMenu.LisansIP, MainMenu.LisansPort, ConnectCallback, null);
			MainMenu.MobilinGames.TryAdd(this.Socket, this);
		}
		catch (Exception ex)
		{
			if (!this.Exit)
			{
				this.Exit = true;
				MainMenu.WriteLine(3, "[AutoEvent]: UserConnection connecting error " + ex.ToString());
				this.Disconnect();
			}
		}
	}

	private void ConnectCallback(IAsyncResult ar)
	{
		try
		{
			this.Socket.EndConnect(ar);
		}
		catch (Exception)
		{
			if (!this.Exit)
			{
				this.Exit = true;
				MainMenu.WriteLine(3, "Sunucu ile bağlantu kurulamıyor");
				this.Disconnect();
			}
		}
		this.BeginReceive();
	}

	private void BeginReceive()
	{
		if (this.Exit)
		{
			return;
		}
		try
		{
			this.Socket.BeginReceive(this.Buffer, 0, 4096, SocketFlags.None, RecieveCallback, null);
		}
		catch (Exception)
		{
			if (!this.Exit)
			{
				this.Exit = true;
				this.Disconnect();
			}
		}
	}

	private void RecieveCallback(IAsyncResult ar)
	{
		int num;
		num = 0;
		try
		{
			if (this.Socket != null)
			{
				num = this.Socket.EndReceive(ar);
			}
		}
		catch (Exception)
		{
			if (!this.Exit)
			{
				this.Exit = true;
				MainMenu.WriteLine(3, "Sunucu ile bağlantu koptu..");
				this.Disconnect();
			}
		}
		if (this.Exit)
		{
			return;
		}
		if (num > 0)
		{
			try
			{
				this.Security.Recv(this.Buffer, 0, num);
			}
			catch (Exception ex2)
			{
				if (!this.Exit)
				{
					this.Exit = true;
					MainMenu.WriteLine(3, "[AutoEvent]: Agent connecting error " + ex2.ToString());
					this.Disconnect();
				}
			}
			if (this.Exit)
			{
				return;
			}
			try
			{
				List<Packet> list;
				list = this.Security.TransferIncoming();
				if (list != null)
				{
					foreach (Packet item in list)
					{
						if (item.Opcode == 20481)
						{
							MainMenu.WriteLine(1, "Lisans Başarılı ");
						}
					}
				}
			}
			catch (Exception ex3)
			{
				if (!this.Exit)
				{
					this.Exit = true;
					MainMenu.WriteLine(3, "[AutoEvent]: Agent connecting error " + ex3.ToString());
					this.Disconnect();
				}
			}
			if (!this.Exit)
			{
				this.SendToServer();
			}
		}
		else if (!this.Exit)
		{
			this.Exit = true;
			this.Disconnect();
		}
		this.BeginReceive();
	}

	public void SendToServer()
	{
		try
		{
			List<KeyValuePair<TransferBuffer, Packet>> list;
			list = this.Security.TransferOutgoing();
			if (list == null)
			{
				return;
			}
			foreach (KeyValuePair<TransferBuffer, Packet> item in list)
			{
				this.Socket.BeginSend(item.Key.Buffer, item.Key.Offset, item.Key.Size, SocketFlags.None, SendCallback, null);
			}
		}
		catch (Exception ex)
		{
			if (!this.Exit)
			{
				this.Exit = true;
				MainMenu.WriteLine(3, "[AutoEvent]: UserConnection connecting error " + ex.ToString());
				this.Disconnect();
			}
		}
	}

	private void SendCallback(IAsyncResult ar)
	{
		try
		{
			this.Socket.EndSend(ar);
		}
		catch (Exception ex)
		{
			if (!this.Exit)
			{
				this.Exit = true;
				MainMenu.WriteLine(3, "[AutoEvent]: UserConnection connecting error " + ex.ToString());
				this.Disconnect();
			}
		}
	}

	public void Disconnect(int send = 0)
	{
		if (!this.Success)
		{
			this.Clientless.DC = true;
		}
		if (this.Clientless.Mode == ClientlessMode.UserConnection)
		{
			this.Clientless.Mode = ClientlessMode.None;
		}
		try
		{
			if (MainMenu.MobilinGames.TryRemove(this.Socket, out var _))
			{
				this.Socket.Shutdown(SocketShutdown.Both);
				this.Socket.Close();
				MainMenu.LisansUser.Clear();
				MainMenu.MobilinGames.Clear();
				MainMenu.Lisans = false;
			}
		}
		catch
		{
		}
		try
		{
			if (this.Socket != null)
			{
				this.Socket = null;
			}
			if (this.Security != null)
			{
				this.Security = null;
			}
			if (this.Buffer != null)
			{
				this.Buffer = null;
			}
		}
		catch
		{
		}
	}
}
