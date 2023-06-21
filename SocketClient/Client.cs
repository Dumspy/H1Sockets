using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Utils;
using IPAddress = System.Net.IPAddress;

namespace SocketClient;

internal class Client
{
    public Client(string remoteIpAddress)
    {
        Console.ReadKey();
        Socket socket = Start(remoteIpAddress);
    
        while (true)
        {
            string message = Guid.NewGuid().ToString();
            SocketUtils.SendMessage(socket, message);
            
            string response = SocketUtils.ReadMessage(socket);
            Console.WriteLine($"Text received: {response}");
            
            if (response.Contains("Goodbye from server")) break;
        }
    }
    /// <summary>
    /// Starts a socket connection to the server
    /// </summary>
    /// <param name="remoteIpAddress">a string representation of the server's ip address</param>
    /// <returns></returns>
    private Socket Start(string remoteIpAddress)
    {
        IPAddress ipAddress = IPAddress.Parse(remoteIpAddress);
        IPEndPoint remoteEndPoint = new(ipAddress, 6969);
        
        Socket sender = new(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        sender.Connect(remoteEndPoint);
        Console.WriteLine($"Socket connected to {sender.RemoteEndPoint}");
        return sender;
    }
}