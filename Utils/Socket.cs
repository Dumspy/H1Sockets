using System.Net.Sockets;
using System.Text;

namespace Utils;

public abstract class SocketUtils
{
    /// <summary>
    /// Reads a message from the socket
    /// </summary>
    /// <param name="socket">socket to read a message from</param>
    /// <returns></returns>
    public static string ReadMessage(Socket socket)
    {
        string data = string.Empty;
        while (true)
        {
            byte[] bytes = new byte[4096];
            int bytesRec = socket.Receive(bytes);
            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
            if (data.Contains("<EOM>"))
            {
                break;
            }
        }

        return data;
    }
    /// <summary>
    /// Converts the message to bytes and sends it over the socket
    /// </summary>
    /// <param name="socket">socket to send the message over</param>
    /// <param name="message">message to send</param>
    public static void SendMessage(Socket socket, string message)
    {
        byte[] msg = Encoding.ASCII.GetBytes(message+"<EOM>");
        socket.Send(msg);
    }
}