using System;
using System.Collections.Generic;

namespace NewFilter.NetEngine;

public class TokenProvider
{
	private static readonly List<TokenProvider> Tokens = new List<TokenProvider>();

	private static readonly object Locker = new object();

	private static int LastTokenID = 0;

	public int ID { get; set; }

	public DateTime Date { get; set; }

	public string ClientIP { get; set; }

	public string AgentIP { get; set; }

	public ushort AgentPort { get; set; }

	public TokenProvider(string clientIP, string agentIP, ushort agentPort)
	{
		this.ClientIP = clientIP;
		this.AgentIP = agentIP;
		this.AgentPort = agentPort;
		this.Date = DateTime.Now;
		lock (TokenProvider.Locker)
		{
			TokenProvider.LastTokenID++;
			this.ID = TokenProvider.LastTokenID;
			TokenProvider.Tokens.Add(this);
		}
	}

	public static TokenProvider GetToken(string clientIP)
	{
		TokenProvider tokenProvider;
		lock (TokenProvider.Locker)
		{
			TokenProvider.Tokens.RemoveAll((TokenProvider t) => t.Date < DateTime.Now.AddSeconds(-10.0));
			tokenProvider = TokenProvider.Tokens.Find((TokenProvider t) => t.ClientIP == clientIP);
			if (tokenProvider != null)
			{
				TokenProvider.Tokens.Remove(tokenProvider);
			}
		}
		return tokenProvider;
	}
}
