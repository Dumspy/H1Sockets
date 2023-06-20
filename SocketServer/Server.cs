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
            Socket socket = listener.Accept();
            
            Thread clientThread = new(() => HandleClient(socket));
            clientThread.Start();
        }
    }
    /// <summary>
    /// Starts a socket listener on port 6969
    /// </summary>
    /// <returns></returns>
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
    /// Reads a message from the client and
    /// then attempts to sends a message back and closes the connection
    /// </summary>
    /// <param name="client">an active socket of an client</param>
    private void HandleClient(Socket client)
    {
        Console.WriteLine($"Socket connected to {client.RemoteEndPoint}");

        Console.WriteLine($"Text received: {SocketUtils.ReadMessage(client)}");
        
        SocketUtils.SendMessage(client, "Hello from server");
        
        EndPoint? remoteEndPoint = client.RemoteEndPoint;
        
        Console.WriteLine($"Closing connection with client: {remoteEndPoint}");
        client.Shutdown(SocketShutdown.Both);
        client.Close();
        Console.WriteLine($"Successfully closed connection with: {remoteEndPoint}");
    }
}