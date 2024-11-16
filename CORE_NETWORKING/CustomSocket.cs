using System.Net.Sockets;

namespace CORE_NETWORKING;

public class CustomSocket : Socket
{
	public CustomSocket()
		: base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
	{
		base.NoDelay = true;
		new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		base.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, new LingerOption(enable: false, 0));
	}
}
