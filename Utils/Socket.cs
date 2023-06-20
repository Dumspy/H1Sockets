using System.Net.Sockets;
using System.Text;

namespace Utils;

public abstract class SocketUtils
{
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

    public static void SendMessage(Socket socket, string message)
    {
        byte[] msg = Encoding.ASCII.GetBytes(message+"<EOM>");
        socket.Send(msg);
    }
}