using System;
using System.Text;
using System.Text.RegularExpressions;
namespace GetIPAndSubnet
{
    class Program
    {
        public static void Main(string[] args)
        {
            IP ip = new IP(); 
            Boolean p = ip.IpAddress.Equals("Error");
            if (!p)
            {
                char ipClass = ip.GetClass();
                string subnet = ip.ReturnSubnet();
                Console.WriteLine("Default Subnet Mask: "+ subnet);
                if (ipClass != '0' && ipClass != 'l')
                    Console.WriteLine($"IP Class: {ipClass.ToString().ToUpper()}");
                if (ipClass == 'l')
                {
                    Console.WriteLine("LocalHost Entered");
                    return;
                }
                if (ipClass == 'a' || ipClass == 'b' || ipClass == 'c')
                {
                    String[] SubnetOctet = ip.SubnetToBinary();
                    Console.Write("Binary Form: ");
                    for (int i = 0; i < 4; i++)
                        Console.Write($"{SubnetOctet[i]} ");
                    Console.WriteLine();


                    Subnetting sb = new Subnetting();
                    sb.GetNetWorkDetails(ip);
                    sb.SetSubnetMask();
                    sb.GetFirstLastIPSubnet();
                    Console.WriteLine();
                }
            }
        }
    }
}