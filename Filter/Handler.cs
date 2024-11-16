using System.Threading;
using Framework;
using NewFilter;

namespace Filter;

public static class Handler
{
	public enum ReturnType
	{
		Continue = 0,
		Break = 1
	}

	public static ReturnType Gateway(Clientlesss C, Packet P)
	{
		ReturnType result;
		result = ReturnType.Continue;
		switch (P.Opcode)
		{
		case 8193:
			result = Handler.GATEWAY_GLOBAL_IDENTIFICATION(C, P);
			break;
		case 41216:
			result = Handler.GATEWAY_PATCH_RESPONSE(C, P);
			break;
		case 41217:
			result = Handler.GATEWAY_SERVERLIST_RESPONSE(C, P);
			break;
		case 41218:
			result = Handler.GATEWAY_LOGIN_RESPONSE(C, P);
			break;
		case 8994:
			result = Handler.GATEWAY_LOGIN_IBUV_CHALLENGE(C, P);
			break;
		}
		return result;
	}

	public static ReturnType Agent(Clientlesss C, Packet P)
	{
		ReturnType result;
		result = ReturnType.Continue;
		switch (P.Opcode)
		{
		case 8193:
			result = Handler.AGENT_GLOBAL_IDENTIFICATION(C, P);
			break;
		case 41219:
			result = Handler.AGENT_LOGIN_RESPONSE(C, P);
			break;
		case 45063:
			result = Handler.AGENT_CHARACTER_SCREEN(C, P);
			break;
		case 12320:
			result = Handler.AGENT_CELESTIAL_POSITION(C, P);
			break;
		}
		return result;
	}

	public static ReturnType GATEWAY_GLOBAL_IDENTIFICATION(Clientlesss C, Packet P)
	{
		if (P.ReadAscii() == "GatewayServer")
		{
			C.Mode = ClientlessMode.Gateway;
			Packet packet;
			packet = new Packet(24832, encrypted: true, massive: false);
			packet.WriteUInt8(MainMenu.CLIENT_LOCALE);
			packet.WriteAscii("SR_Client");
			packet.WriteUInt32(MainMenu.CLIENT_VERSION);
			C.GW.Security.Send(packet);
		}
		return ReturnType.Continue;
	}

	public static ReturnType GATEWAY_PATCH_RESPONSE(Clientlesss C, Packet P)
	{
		if (P.ReadUInt8() == 1)
		{
			C.GW.Security.Send(new Packet(24833, encrypted: true));
			return ReturnType.Continue;
		}
		if ((uint)(P.ReadUInt8() - 1) > 4u)
		{
		}
		MainMenu.WriteLine(3, "[AutoEvent]: Invalid Version");
		if (!C.GW.Exit)
		{
			C.GW.Exit = true;
			C.GW.Disconnect();
		}
		return ReturnType.Break;
	}

	public static ReturnType GATEWAY_SERVERLIST_RESPONSE(Clientlesss C, Packet P)
	{
		Packet packet;
		packet = new Packet(24834);
		packet.WriteUInt8(MainMenu.CLIENT_LOCALE);
		packet.WriteAscii(C.Username);
		packet.WriteAscii(C.Password);
		packet.WriteUInt16(64);
		C.GW.Security.Send(packet);
		return ReturnType.Continue;
	}

	public static ReturnType GATEWAY_LOGIN_RESPONSE(Clientlesss C, Packet P)
	{
		switch (P.ReadUInt8())
		{
		case 1:
		{
			uint id;
			id = P.ReadUInt32();
			string ip;
			ip = P.ReadAscii();
			ushort port;
			port = P.ReadUInt16();
			C.GW.Success = true;
			C.AG = new Agent(C, id, ip, port);
			if (!C.GW.Exit)
			{
				C.GW.Exit = true;
				C.GW.Disconnect();
			}
			return ReturnType.Break;
		}
		case 2:
			switch (P.ReadUInt8())
			{
			case 1:
			{
				byte b2;
				b2 = P.ReadUInt8();
				P.ReadUInt8();
				P.ReadUInt8();
				P.ReadUInt8();
				MainMenu.WriteLine(3, $"[AutoEvent]: Invalid Password [{P.ReadUInt8()} / {b2}]");
				break;
			}
			case 2:
			{
				byte b;
				b = P.ReadUInt8();
				string text;
				text = "";
				switch (b)
				{
				case 1:
					text = P.ReadAscii();
					break;
				}
				MainMenu.WriteLine(3, "[AutoEvent]: Account Blocked cause : " + text);
				break;
			}
			case 3:
				MainMenu.WriteLine(3, "[AutoEvent]: user is already connected.");
				break;
			default:
				MainMenu.WriteLine(3, "[AutoEvent]: Gateway Login Error");
				break;
			}
			break;
		}
		if (!C.GW.Exit)
		{
			C.GW.Exit = true;
			C.GW.Disconnect();
		}
		return ReturnType.Break;
	}

	public static ReturnType GATEWAY_LOGIN_IBUV_CHALLENGE(Clientlesss C, Packet P)
	{
		Packet packet;
		packet = new Packet(25379);
		packet.WriteAscii(MainMenu.CLIENT_CAPTCHA_VALUE);
		C.GW.Security.Send(packet);
		return ReturnType.Continue;
	}

	public static ReturnType AGENT_GLOBAL_IDENTIFICATION(Clientlesss C, Packet P)
	{
		if (P.ReadAscii() != "GatewayServer")
		{
			C.Mode = ClientlessMode.Agent;
			Packet packet;
			packet = new Packet(24835);
			packet.WriteUInt32(C.AG.loginID);
			packet.WriteAscii(C.Username);
			packet.WriteAscii(C.Password);
			packet.WriteUInt8(MainMenu.CLIENT_LOCALE);
			packet.WriteUInt32(0u);
			packet.WriteUInt16(0);
			C.AG.Security.Send(packet);
		}
		return ReturnType.Continue;
	}

	public static ReturnType AGENT_LOGIN_RESPONSE(Clientlesss C, Packet P)
	{
		if (P.ReadUInt8() == 1)
		{
			Packet packet;
			packet = new Packet(28679);
			packet.WriteUInt8(2);
			C.AG.Security.Send(packet);
			return ReturnType.Continue;
		}
		MainMenu.WriteLine(3, "[AutoEvent]: Agent Login Error");
		if (!C.AG.isExit)
		{
			C.AG.isExit = true;
			C.AG.Disconnect();
		}
		return ReturnType.Break;
	}

	public static ReturnType AGENT_CHARACTER_SCREEN(Clientlesss C, Packet P)
	{
		if (P.ReadUInt8() == 2)
		{
			if (P.ReadUInt8() == 1)
			{
				bool flag;
				flag = false;
				byte b;
				b = P.ReadUInt8();
				if (b > 0)
				{
					for (int i = 0; i < b; i++)
					{
						P.ReadUInt32();
						string text;
						text = P.ReadAscii();
						if (text == C.Character)
						{
							flag = true;
							Thread.Sleep(2000);
							Packet packet;
							packet = new Packet(28673);
							packet.WriteAscii(text);
							C.AG.Security.Send(packet);
							break;
						}
						P.ReadUInt8();
						P.ReadUInt8();
						P.ReadUInt64();
						P.ReadUInt16();
						P.ReadUInt16();
						P.ReadUInt16();
						P.ReadUInt32();
						P.ReadUInt32();
						if (P.ReadUInt8() == 1)
						{
							P.ReadUInt32();
						}
						P.ReadUInt16();
						P.ReadUInt8();
						byte b2;
						b2 = P.ReadUInt8();
						for (int j = 0; j < b2; j++)
						{
							P.ReadUInt32();
							P.ReadUInt8();
						}
						byte b3;
						b3 = P.ReadUInt8();
						for (int j = 0; j < b3; j++)
						{
							P.ReadUInt32();
							P.ReadUInt8();
						}
					}
				}
				if (!flag)
				{
					if (b >= 4 && !C.GW.Exit)
					{
						C.GW.Exit = true;
						C.GW.Disconnect();
						MainMenu.WriteLine(3, "Too many characters on " + C.Username + " account, please remove some!");
					}
					C.AG.isCreatingCharacter = true;
					MainMenu.WriteLine(1, "[AutoEvent]: Creatting Character [ " + C.Character + " ]");
					Thread.Sleep(3000);
					Packet packet2;
					packet2 = new Packet(28679);
					packet2.WriteUInt8(1);
					packet2.WriteAscii(C.Character);
					packet2.WriteUInt32(1907u);
					packet2.WriteUInt8(34);
					packet2.WriteUInt32(3640u);
					packet2.WriteUInt32(3641u);
					packet2.WriteUInt32(3642u);
					packet2.WriteUInt32(3636u);
					C.AG.Security.Send(packet2);
				}
			}
		}
		else if (P.ReadUInt8() == 1 && C.AG.isCreatingCharacter)
		{
			Packet packet3;
			packet3 = new Packet(28679);
			packet3.WriteUInt8(2);
			C.AG.Security.Send(packet3);
			C.AG.isCreatingCharacter = false;
		}
		return ReturnType.Continue;
	}

	public static ReturnType AGENT_CELESTIAL_POSITION(Clientlesss C, Packet P)
	{
		CharStrings.UniqueID = P.ReadUInt32();
		C.AG.Security.Send(new Packet(12306));
		return ReturnType.Continue;
	}
}
