using System;
using System.Text;

namespace GetIPAndSubnet
{

    class IP
    {
        string ipAddress;
        public string IpAddress{
                get { return ipAddress; } set {ipAddress = GetIP();}
                               }
        string[] IPOctet;// = new string[4];
        int IPOctet0;

        byte[] subnetOctet;
        public byte[] SubnetOctet { get { return subnetOctet; } }
        char IPClass;


        public string GetIP()
        {
            Console.WriteLine("Enter the IP Address");
            ipAddress = Console.ReadLine();

            IPOctet = ipAddress.Split('.');
            if (IPOctet.Length != 4)
                return "Error";
            IPOctet0 = Convert.ToInt32(IPOctet[0]);
            if (IPOctet0.CompareTo(254) > 0 ||
                    Convert.ToInt32(IPOctet[1]).CompareTo(254) > 0 || 
                        Convert.ToInt32(IPOctet[2]).CompareTo(254) > 0 ||
                            Convert.ToInt32(IPOctet[3]).CompareTo(254) > 0 )
                return "Error";
            return ipAddress;
        }
        public char GetClass()
        {
            if (IPOctet0.CompareTo(126) <= 0)
            {
                IPClass= 'a';
            }
            else if (IPOctet0.CompareTo(128) >= 0 
                      && IPOctet0.CompareTo(191) <= 0)
            {
                IPClass = 'b';
            }
            else if (IPOctet0.CompareTo(192) >= 0 
                      && IPOctet0.CompareTo(223) <= 0)
            {
                IPClass = 'c';
            }
            else if (IPOctet0.CompareTo(224) >= 0
                      && IPOctet0.CompareTo(239) <= 0)
            {
                IPClass = 'd';
            }
            else if (IPOctet0.CompareTo(240) >= 0
                      && IPOctet0.CompareTo(254) <= 0)
            {
                IPClass = 'e';
            }
            else if (IPOctet0.Equals(127))
            {
                IPClass = 'l';
            }
            else 
                IPClass = '0';

            return IPClass;
        }
        public string ReturnSubnet()
        {
            subnetOctet = new Byte[4];
            subnetOctet[0] = 255;
            switch (IPClass)
            {
                case 'a':
                    subnetOctet[1] = 0;
                    subnetOctet[2] = 0;
                    subnetOctet[3] = 0;
                    break;
                case 'b':
                    subnetOctet[1] = 255;
                    subnetOctet[2] = 0;
                    subnetOctet[3] = 0;
                    break;
                case 'c':
                    subnetOctet[1] = 255;
                    subnetOctet[2] = 255;
                    subnetOctet[3] = 0;
                    break;
                default:
                    return "Error";

            }

            StringBuilder sb = new StringBuilder();
            int pt = 0;
            foreach (byte i in subnetOctet)
            {

                pt = pt + 1;
                sb.Append(i);
                if (pt <= 3)
                {
                    sb.Append('.');
                }
            }
            return sb.ToString();
        }


        public string[] SubnetToBinary()
        {

            string[] SOctet = new string[4];
            for (int i=0;i<4;i++)
            {
                SOctet[i] = Convert.ToString(subnetOctet[i], 2).PadLeft(8,'0');
            }
            return SOctet;
        }
    }
}
