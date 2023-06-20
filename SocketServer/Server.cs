using System.Net;
using System.Net.Sockets;
using System.Text;
using Utils;

namespace SocketServer;

internal class Server
{
    public Server()
    {
        Socket listener = Start();
        while (true)
        {
            AcceptConnection(listener);
        }
    }
    
    private Socket Start()
    {
        // Dns.GetHostEntry(Dns.GetHostName(), AddressFamily.InterNetwork); // still returns some IPv6 addresses on my machine
        IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName()); 
        // manuel filtering on dns.GetHostEntry().AddressList to get only IPv4 addresses
        IPAddress[] filteredAddresses = ipHostEntry.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToArray(); 
        IPAddress ipAddress = IpAddressUtils.SelectIp(filteredAddresses);
        IPEndPoint localEndPoint = new(ipAddress, 6969);
        
        Socket listener = new(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(localEndPoint);
        listener.Listen(10);
        Console.WriteLine($"Listening on {localEndPoint}");

        return listener;
    }

    /// <summary>
    /// Accepts a connection and reads the message from the client
    /// then sends a message back to the client and closes the connection
    /// </summary>
    /// <param name="listener">an active socket listener</param>
    private void AcceptConnection(Socket listener)
    {
        Socket handler = listener.Accept();
        Console.WriteLine($"Socket connected to {handler.RemoteEndPoint}");

        Console.WriteLine($"Text received: {SocketUtils.ReadMessage(handler)}");
        
        SocketUtils.SendMessage(handler, "Hello from server");
        
        Console.WriteLine($"Closing with client: {handler.RemoteEndPoint}");
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
        Console.WriteLine("Closed connection successfully");
    }
}