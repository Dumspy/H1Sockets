using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Shared;

public abstract class SocketUtils
{
    /// <summary>
    /// Reads a message from the socket
    /// </summary>
    /// <param name="socket">socket to read a message from</param>
    /// <returns></returns>
    public static Message? ReadMessage(Socket socket)
    {
        string data = string.Empty;
        while (socket.Connected && !data.Contains("<EOM>"))
        {
            try
            {
                byte[] bytes = new byte[4096];
                int bytesRec = socket.Receive(bytes);
                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
            }
            catch
            {
                Console.WriteLine("Connection lost");
                break;
            }
        }

        //const string pattern = @"<EOM>|<EOT>";
        const string pattern = @"<EOM>";
        data = Regex.Replace(data, pattern, "");
        try
        {
            return JsonConvert.DeserializeObject<Message>(data);
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// Converts the message to bytes and sends it over the socket
    /// </summary>
    /// <param name="socket">socket to send the message over</param>
    /// <param name="message">message to send</param>
    public static void SendMessage(Socket socket, Message message)
    {
        if(!socket.Connected) return;
        string json = JsonConvert.SerializeObject(message);
        byte[] msg = Encoding.UTF8.GetBytes(json+"<EOM>");
        socket.Send(msg);
    }
}