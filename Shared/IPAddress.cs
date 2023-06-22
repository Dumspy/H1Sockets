using System;
using System.Net;

namespace Shared;

public abstract class IpAddressUtils
{
    /// <summary>
    /// A simple gui to select an ip address from a list of ip addresses
    /// </summary>
    /// <param name="addresses">list of ip addresses</param>
    /// <returns></returns>
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