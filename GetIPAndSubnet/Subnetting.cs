using System;

namespace GetIPAndSubnet
{
    class Subnetting
    {
        string IpAddress;
        byte[] SubnetMaskOctet;
        byte[] DefaultMaskOctet;
        int SubnetNo;
        int host;// No. of hosts per subnet 
        public void GetNetWorkDetails(IP ip)
        {
            DefaultMaskOctet = new byte[4];
            IpAddress = ip.IpAddress;
            SubnetMaskOctet = ip.SubnetOctet;
            SubnetMaskOctet.CopyTo(DefaultMaskOctet,0);
        }

        public int GetSubnetworkNo()//Get Subnet Network No host
        {
            int i, j;
            host = 1;
            do
            {
                Console.WriteLine("Enter the number of Subnet");
                
            }while(!int.TryParse(Console.ReadLine(), out SubnetNo));

            int bit=(1 << Convert.ToString(SubnetNo-1, 2).Length)-1;

            string subnetbits=Convert.ToString(bit,2);
            subnetbits=subnetbits.PadRight(((subnetbits.Length/8)+1)*8,'0');
            byte[] tempoctet = new byte[subnetbits.Length / 8];
            for (i=0;i< subnetbits.Length / 8;i++)
            {
                tempoctet[i] = Convert.ToByte(subnetbits.Substring(i*8,8),2);
            }

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
            for (i = 0; i < 4; i++)
                Console.Write($"{SubnetMaskOctet[i]} ");
            Console.WriteLine();
            //Total No of hosts in the network/No of hosts per subnet 
            for (i=0;i<4;i++)
            {
                if (SubnetMaskOctet[i] != 255)
                {
                    host *= Convert.ToInt32((byte)~SubnetMaskOctet[i])+1;
                }
            }

            Console.WriteLine(host);

            return 0;
        }

        public byte[] GetFirstIP(int subnetno)
        {
            int i,j, subnetbytes = 0, subnetposition = 1;
            byte[] FipOctet = new byte[4];
            string[] IpOctetS = new string[4];
            byte[] IpOctet = new byte[4];

            IpOctetS = IpAddress.Split('.');
            byte[] subnet = new byte[4];
            string[] subnets = new string[4];
            for (i=0;i<4;i++)
            {
                subnet[i] = (byte)(SubnetMaskOctet[i] ^ DefaultMaskOctet[i]);
            }
            for (i = 3; i >=0; i--)
            {
                
                if (subnet[i]!=0)
                {
                    subnets[i] = Convert.ToString(subnet[i], 2);
                    subnetposition = i;
                    subnetbytes++;
                }
            }

            string subnetbits = Convert.ToString(subnetno, 2).PadLeft(Convert.ToString(SubnetNo,2).Length-1,'0').PadRight(subnetbytes*8,'0');
            Console.WriteLine(subnetbits) ;
            

            for (i=0;i<4;i++)
            {
                IpOctet[i] = Convert.ToByte(IpOctetS[i]);
                FipOctet[i] = Convert.ToByte((IpOctet[i] & SubnetMaskOctet[i]));
            }

            for (i = subnetposition,j=0; i < subnetposition + subnetbytes&& j<subnetbytes; i++,j++)
            {
                
                FipOctet[i] |= Convert.ToByte(subnetbits.Substring(j * 8, 8),2);
            }

            return FipOctet;


        }

        public byte[] GetSubnetLastIP()
        {
            int i;
            byte[] LipOctet = new byte[4];
            string[] IpOctetS = new string[4];
            byte[] IpOctet = new byte[4];

            IpOctetS = IpAddress.Split('.');

            for (i = 0; i < 4; i++)
            {
                IpOctet[i] = Convert.ToByte(IpOctetS[i]);
                IpOctet[i] = Convert.ToByte((IpOctet[i] & SubnetMaskOctet[i]));
                LipOctet[i] = 0;
            }

            int hostbitI = host;
            i = 3;
            while(hostbitI>0)
            {
                int vv = hostbitI%256 - 1;
                LipOctet[i] = (byte)(vv);
                hostbitI /= 256;

                i--;
            }

            for (i = 0; i < 4; i++)
            {
                LipOctet[i] = Convert.ToByte(IpOctet[i] | LipOctet[i]);
            }
            return LipOctet;

        }



    }
}
