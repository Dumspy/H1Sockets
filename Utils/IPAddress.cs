using System.Net;

namespace Utils;

public abstract class IpAddressUtils
{
    public static IPAddress SelectIp(IPAddress[] addresses)
    {
        int i = 0;
        foreach (IPAddress address in addresses)
            Console.WriteLine($"[{i++}] {address}");

        int j;
        
        do
        {
            Console.Write("Input index of wanted address: ");
        }
        while (!int.TryParse(Console.ReadLine(), out j) || j < 0 || j >= addresses.Length);
        
        return addresses[j];
    }
}