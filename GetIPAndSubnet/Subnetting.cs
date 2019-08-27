using System;
using System.Text;
namespace GetIPAndSubnet
{
    class Subnetting
    {
        string IpAddress;
        byte[] SubnetMaskOctet;
        byte[] DefaultMaskOctet;
        int TotalSubnets;
        int TotalHosts;
        int Host;// No. of hosts per subnet 


        //Function stores the network details fro the IP passed as arguments
        public void GetNetWorkDetails(IP ip)
        {
            TotalHosts = 1;
            DefaultMaskOctet = new byte[4];
            IpAddress = ip.IpAddress;
            SubnetMaskOctet = ip.SubnetOctet;
            SubnetMaskOctet.CopyTo(DefaultMaskOctet, 0);
            for (int i = 0; i < 4; i++)//Calculate total hosts in the network
            {
                if (DefaultMaskOctet[i] != 255)
                {
                    TotalHosts *= Convert.ToInt32((byte)~DefaultMaskOctet[i]) + 1;
                }
            }
        }


        //Function asks for the Number of subnets and ,rounding of to the nearest power of 2, creates new subnet mask and subnets
        public int SetSubnetMask()
        {
            int i, j;
            Host = 1;
            do
            {
                Console.WriteLine("Enter the number of Subnet");
            } while (!int.TryParse(Console.ReadLine(), out TotalSubnets) || TotalSubnets >= TotalHosts);//Accept number of subnets to create.
                                                                                                        // Loop if input not a number or value greater than the total umber of hosts

            int bit = (1 << Convert.ToString(TotalSubnets - 1, 2).Length) - 1;// Total bits used by subnet
            TotalSubnets = bit + 1;//Total subnets created
            string subnetbits = Convert.ToString(bit, 2);// Bits used in subnet converted to binary
            subnetbits = subnetbits.PadRight(((subnetbits.Length / 8) + 1) * 8, '0');// Adding padding to make size as byte/multiples of byte
            byte[] tempoctet = new byte[subnetbits.Length / 8];//Stores temporary Octet



            //Setting up of the new SubNet mask
            for (i = 0; i < subnetbits.Length / 8; i++)
            {
                tempoctet[i] = Convert.ToByte(subnetbits.Substring(i * 8, 8), 2);
            }

            for (i = 0, j = 0; i < 4 && j < tempoctet.Length; i++)
            {
                if (SubnetMaskOctet[i] != 255)
                {
                    SubnetMaskOctet[i] = tempoctet[j];
                    j++;
                }
            }

            /*
             for (i=0,j=0;i<4;i++)
             {
                 if (SubnetMaskOctet[i]!=255)
                 {
                     for (; j < tempoctet.Length; j++)
                     {
                         SubnetMaskOctet[i] = tempoctet[j];
                         i++;
                     }
                 }
             }
             */

            /*for (i=0;i<subnetbits.Length;)
            {
                char[] temp=new char[8];
                for (j = 0; j < subnetbits.Length / 8; j++)
                {
                    for(int k = 0; k < 8; k++)
                    {
                        temp[k] = subnetbits[i];
                        i++;
                    }
                    Console.WriteLine(new string(temp));
                    Console.WriteLine(Convert.ToByte(new string(temp),2));
                    //tempoctet[j] = Convert.ToByte(temp.ToString());
                    //Console.WriteLine(tempoctet[j]);
                }
            }
            */
            int pt = 0;
            StringBuilder sb = new StringBuilder();
            foreach (byte Octet in SubnetMaskOctet)
            {
                sb.Append(Octet);
                if (pt < 3)
                {
                    sb.Append(".");
                }
                pt++;


            }
            Console.WriteLine("Subnet Mask: {0}", sb.ToString());//Display the new Subnet Mask
            //Total No of hosts in the network/No of hosts per subnet 
            for (i = 0; i < 4; i++)
            {
                if (SubnetMaskOctet[i] != 255)
                {
                    Host *= Convert.ToInt32((byte)~SubnetMaskOctet[i]) + 1;
                }
            }

            Console.WriteLine("Hosts Per Subnet: {0}", Host);

            return 0;
        }

        public byte[] GetSubnetFirstIP(int subnetno)
        {
            int i, j, subnetbytes = 0, subnetposition = 1;
            byte[] FipOctet = new byte[4];
            string[] IpOctetS;
            byte[] IpOctet = new byte[4];

            IpOctetS = IpAddress.Split('.');//Retrieving the octets from the ip address
            byte[] subnet = new byte[4];
            for (i = 0; i < 4; i++)
            {
                subnet[i] = (byte)(SubnetMaskOctet[i] ^ DefaultMaskOctet[i]);//Identifying the subnet value added to the default mask
            }
            for (i = 3; i >= 0; i--)//Identify the starting position of the first subnet bit and the span of the bits
            {

                if (subnet[i] != 0)
                {
                    subnetposition = i;
                    subnetbytes++;
                }
            }
            //Converting the subnet to binary
            string subnetbits = Convert.ToString(subnetno, 2).PadLeft(Convert.ToString(TotalSubnets, 2).Length - 1, '0').PadRight(subnetbytes * 8, '0');
            for (i = 0; i < 4; i++)
            {
                IpOctet[i] = Convert.ToByte(IpOctetS[i]);
                FipOctet[i] = Convert.ToByte((IpOctet[i] & DefaultMaskOctet[i]));
            }

            for (i = subnetposition, j = 0; i < subnetposition + subnetbytes && j < subnetbytes; i++, j++)
            {

                FipOctet[i] |= Convert.ToByte(subnetbits.Substring(j * 8, 8), 2);
            }

            return FipOctet;


        }

        public byte[] GetSubnetLastIP(int subnetno)
        {
            int i, j, subnetbytes = 0, subnetposition = 1;
            byte[] LipOctet = new byte[4];
            string[] IpOctetS = new string[4];
            byte[] IpOctet = new byte[4];
            byte[] subnet = new byte[4];
            for (i = 0; i < 4; i++)
            {
                subnet[i] = (byte)(SubnetMaskOctet[i] ^ DefaultMaskOctet[i]);
            }
            for (i = 3; i >= 0; i--)
            {

                if (subnet[i] != 0)
                {
                    subnetposition = i;
                    subnetbytes++;
                }
            }

            string subnetbits = Convert.ToString(subnetno, 2).PadLeft(Convert.ToString(TotalSubnets, 2).Length - 1, '0').PadRight(subnetbytes * 8, '0');
            IpOctetS = IpAddress.Split('.');

            for (i = 0; i < 4; i++)
            {
                IpOctet[i] = Convert.ToByte(IpOctetS[i]);
                IpOctet[i] = Convert.ToByte((IpOctet[i] & DefaultMaskOctet[i]));
                LipOctet[i] = 0;
            }

            int hostbitI = Host;
            i = 3;
            while (hostbitI > 0)
            {

                LipOctet[i] = (byte)(hostbitI % 256 - 1);
                hostbitI /= 256;

                i--;
            }

            for (i = 0; i < 4; i++)
            {
                LipOctet[i] = Convert.ToByte(IpOctet[i] | LipOctet[i]);
            }

            for (i = subnetposition, j = 0;j < subnetbytes; i++, j++)
            {

                LipOctet[i] |= Convert.ToByte(subnetbits.Substring(j * 8, 8), 2);
            }

            return LipOctet;

        }


        public void GetFirstLastIPSubnet()//Get the first and Last IP Address for every subnet
        {
            for (int IPiterator = 0; IPiterator < TotalSubnets; IPiterator++)
            {
                Console.WriteLine($"\n\n\nSubnet : {IPiterator + 1}\n");
                byte[] FIP = this.GetSubnetFirstIP(IPiterator);
                byte[] LIP = this.GetSubnetLastIP(IPiterator);
                Console.Write("1st IP Address:");
                for (int i = 0; i < 4; i++)
                {
                    Console.Write(FIP[i] + " ");
                }
                Console.Write("\nLast IP Address:");
                for (int i = 0; i < 4; i++)
                {
                    Console.Write(LIP[i] + " ");
                }
                Console.WriteLine();
            }
        }


    }
}
