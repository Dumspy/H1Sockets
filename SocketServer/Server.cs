using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Shared;

namespace SocketServer;

internal class Server
{
    private readonly User _identity = new(ConsoleColor.Red, "Server");
    private readonly List<Socket> _clients = new();
    public Server() 
    {
        Socket listener = Start();
        
        while (true)
        {
            Socket socket = listener.Accept();
            
            Thread clientThread = new(() => HandleClient(socket));
            clientThread.Start();
            _clients.Add(socket);
            Console.WriteLine($"Clients connected: {_clients.Count}");
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
        
        while (client.Connected)
        {
            Message? message = SocketUtils.ReadMessage(client);
            if (message == null) continue;
            message.Print();
            foreach (Socket var in _clients.Where(var => var.Connected))
            {
                if(var.RemoteEndPoint == client.RemoteEndPoint) continue;
                SocketUtils.SendMessage(var, message);
            }
        }
        
        EndPoint? remoteEndPoint = client.RemoteEndPoint;
        Console.WriteLine($"Closing connection with client: {remoteEndPoint}");
        client.Shutdown(SocketShutdown.Both);
        client.Close();
        Console.WriteLine($"Successfully closed connection with: {remoteEndPoint}");
    }
}