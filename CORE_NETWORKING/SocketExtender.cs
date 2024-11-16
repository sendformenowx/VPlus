using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NewFilter;
using NewFilter.CORE;
using NewFilter.NetEngine;

namespace CORE_NETWORKING;

public static class SocketExtender
{
	public static Task<Socket> AcceptDownloadServerConnections(this Socket LISTENER_SOCKET)
	{
		TaskCompletionSource<Socket> tcs;
		tcs = new TaskCompletionSource<Socket>();
		LISTENER_SOCKET.BeginAccept(delegate(IAsyncResult result)
		{
			AsyncServer.ARE.Set();
			try
			{
				Socket socket;
				socket = (result.AsyncState as Socket).EndAccept(result);
				tcs.SetResult(socket);
				if (AsyncServer.DownloadConnections.TryAdd(socket, new DownloadComtext(socket)))
				{
					Utils.UpdateCounters();
				}
			}
			catch (Exception exception)
			{
				tcs.SetException(exception);
			}
		}, LISTENER_SOCKET);
		return tcs.Task;
	}

	public static Task<Socket> AcceptGatewayServerConnections(this Socket LISTENER_SOCKET)
	{
		TaskCompletionSource<Socket> tcs;
		tcs = new TaskCompletionSource<Socket>();
		LISTENER_SOCKET.BeginAccept(delegate(IAsyncResult result)
		{
			AsyncServer.ARE.Set();
			try
			{
				Socket socket;
				socket = (result.AsyncState as Socket).EndAccept(result);
				tcs.SetResult(socket);
				if (AsyncServer.GatewayConnections.TryAdd(socket, new GatewayContext(socket)))
				{
					Utils.UpdateCounters();
				}
			}
			catch (Exception exception)
			{
				tcs.SetException(exception);
			}
		}, LISTENER_SOCKET);
		return tcs.Task;
	}

	public static Task<Socket> AcceptAgentServerConnections(this Socket LISTENER_SOCKET)
	{
		TaskCompletionSource<Socket> tcs;
		tcs = new TaskCompletionSource<Socket>();
		LISTENER_SOCKET.BeginAccept(delegate(IAsyncResult result)
		{
			AsyncServer.ARE.Set();
			try
			{
				Socket socket;
				socket = (result.AsyncState as Socket).EndAccept(result);
				tcs.SetResult(socket);
				if (AsyncServer.AgentConnections.TryAdd(socket, new AgentContext(socket)))
				{
					Utils.UpdateCounters();
				}
			}
			catch (Exception exception)
			{
				tcs.SetException(exception);
			}
		}, LISTENER_SOCKET);
		return tcs.Task;
	}

	public static Task<int> RecvFromSocket(this Socket RECV_SOCKET, byte[] TransferBuffer, int Length)
	{
		TaskCompletionSource<int> tcs;
		tcs = new TaskCompletionSource<int>();
		if (RECV_SOCKET != null && RECV_SOCKET.Connected)
		{
			RECV_SOCKET.BeginReceive(TransferBuffer, 0, Length, SocketFlags.None, out SocketError SOCKET_ERROR_HANDLER, delegate (IAsyncResult result)
            {
				try
				{
					Socket socket;
					socket = result.AsyncState as Socket;
					if (socket.Connected)
					{
						tcs.SetResult(socket.EndReceive(result, out SOCKET_ERROR_HANDLER));
					}
					else
					{
						tcs.SetResult(0);
					}
				}
				catch (Exception exception)
				{
					tcs.SetException(exception);
				}
			}, RECV_SOCKET);
		}
		else
		{
			tcs.SetResult(0);
		}
		return tcs.Task;
	}

	public static Task<int> SendToSocket(this Socket TARGET_SOCKET, byte[] TransferBuffer, int Length)
	{
		TaskCompletionSource<int> tcs;
		tcs = new TaskCompletionSource<int>();
		if (TARGET_SOCKET != null && TARGET_SOCKET.Connected)
		{
			TARGET_SOCKET.BeginSend(TransferBuffer, 0, Length, SocketFlags.None, out SocketError SOCKET_ERROR_HANDLER, delegate (IAsyncResult result)
            {
				try
				{
					Socket socket;
					socket = result.AsyncState as Socket;
					if (socket.Connected)
					{
						tcs.SetResult(socket.EndSend(result, out SOCKET_ERROR_HANDLER));
                    }
					else
					{
						tcs.SetResult(0);
					}
				}
				catch (Exception exception)
				{
					tcs.SetException(exception);
				}
			}, TARGET_SOCKET);
		}
		else
		{
			tcs.SetResult(0);
		}
		return tcs.Task;
	}

	public static Task<Socket> ConnectToSocket(this Socket TARGET_SOCKET, string Host, int Port)
	{
		TaskCompletionSource<Socket> tcs;
		tcs = new TaskCompletionSource<Socket>();
		if (TARGET_SOCKET != null && !TARGET_SOCKET.Connected)
		{
			TARGET_SOCKET.BeginConnect(Host, Port, delegate(IAsyncResult result)
			{
				try
				{
					Socket socket;
					socket = result.AsyncState as Socket;
					if (socket.Connected)
					{
						socket.EndConnect(result);
						tcs.SetResult(socket);
					}
					else
					{
						tcs.SetResult(null);
					}
				}
				catch (Exception exception)
				{
					tcs.SetException(exception);
				}
			}, TARGET_SOCKET);
		}
		else
		{
			tcs.SetResult(null);
		}
		return tcs.Task;
	}

	public static void DisconnectFromSocket(this Socket TARGET_SOCKET)
	{
		if (TARGET_SOCKET != null && TARGET_SOCKET.Connected)
		{
			TARGET_SOCKET.Shutdown(SocketShutdown.Both);
			TARGET_SOCKET.Close();
		}
	}
}
