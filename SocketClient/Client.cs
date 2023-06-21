using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Utils;
using IPAddress = System.Net.IPAddress;

namespace SocketClient;

internal class Client
{
    private readonly Thread _readerThread;
    private readonly Thread _writerThread;
    private readonly Socket _socket;
    public Client(string remoteIpAddress)
    {
        Console.ReadKey(); 
        _socket = Start(remoteIpAddress);
        
        _readerThread = new Thread(Reader);    
        _writerThread = new Thread(Writer);
        _readerThread.Start();
        _writerThread.Start();
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
    
    private void Writer()
    {
        while (true)
        {
            string message = Console.ReadLine() ?? string.Empty;
            SocketUtils.SendMessage(_socket, message);
        }
    }
    
    private void Reader()
    {
        while (true)
        {
            string message = SocketUtils.ReadMessage(_socket);
            Console.WriteLine($"Text received: {message}");

            if (!message.Contains("<EOT>")) continue;
            Close();
            break;
        }
    }
    
    private void Close()
    {
        _socket.Shutdown(SocketShutdown.Both);
        _socket.Close();
        _readerThread.Abort(); // TODO Replace abort with a bool for the class that kills the while loop
        _writerThread.Abort();
    }
}