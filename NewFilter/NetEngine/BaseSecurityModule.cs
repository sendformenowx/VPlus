using System.Net.Sockets;
using Framework;

namespace NewFilter.NetEngine;

public abstract class BaseSecurityModule
{
	public Socket PROXY_SOCKET { get; set; }

	public Socket CLIENT_SOCKET { get; set; }

	public Security LOCAL_SECURITY { get; set; }

	public Security REMOTE_SECURITY { get; set; }

	public TransferBuffer LOCAL_BUFFER { get; set; }

	public TransferBuffer REMOTE_BUFFER { get; set; }

	public abstract void ConnectToServerModules();

	public abstract void ReceiveFromClient();

	public abstract void ReceiveFromModule();

	public abstract void SendToClient();

	public abstract void SendToModule();
}
