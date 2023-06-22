using System.Net;
using System.Net.Sockets;
using Shared;

namespace SocketClient;

internal class Client
{
    private readonly Socket _socket;
    private readonly User _identity;
    
    private bool _abortThreads = false;
    public Client(string remoteIpAddress, User identity)
    {
        _socket = Start(remoteIpAddress);
        _identity = identity;
        
        Thread readerThread = new(Reader);    
        Thread writerThread = new(Writer);
        readerThread.Start();
        writerThread.Start();
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
        while (!_abortThreads)
        {
            string message = Console.ReadLine() ?? string.Empty;
            if(message == string.Empty) continue;
            SocketUtils.SendMessage(_socket, new Message(_identity,message));
        }
    }
    
    private void Reader()
    {
        while (!_abortThreads)
        {
            Message? message = SocketUtils.ReadMessage(_socket);
            if (message == null) continue;
            message.Print();

            if (!message.Body.Contains("<EOT>")) continue;
            Close();
            break;
        }
    }
    
    private void Close()
    {
        _socket.Shutdown(SocketShutdown.Both);
        _socket.Close();
        _abortThreads = true;
    }
}