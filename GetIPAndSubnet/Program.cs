using System;
using System.Text;
using System.Text.RegularExpressions;
namespace GetIPAndSubnet
{
    class Program
    {
        public static void Main(string[] args)
        {
            IP ip = new IP(); ;
            Boolean p = ip.GetIP().Equals("Error");
            if (!p)
            {
                char ipClass = ip.GetClass();
                string subnet = ip.ReturnSubnet();
                Console.WriteLine(subnet);
                String[] SubnetOctet = ip.SubnetToBinary();
                for (int i = 0; i < 4; i++)
                    Console.Write($"{SubnetOctet[i]} ");
                Console.WriteLine();
                if (ipClass != '0' && ipClass != 'l')
                    Console.WriteLine($"Class {ipClass.ToString().ToUpper()}");
                if (ipClass == 'l')
                    Console.WriteLine("LocalHost");
                Subnetting sb = new Subnetting();
                sb.GetNetWorkDetails(ip);
                sb.GetSubnetworkNo();
                byte[] FIP=sb.GetFirstIP();
                byte[] LIP = sb.GetSubnetLastIP();
                for(int i=0;i<4;i++)
                {
                    Console.Write(FIP[i] + " ");
                }
                Console.WriteLine();
                for (int i = 0; i < 4; i++)
                {
                    Console.Write(LIP[i] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}