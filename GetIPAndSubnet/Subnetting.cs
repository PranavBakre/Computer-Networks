using System;

namespace GetIPAndSubnet
{
    class Subnetting
    {
        string IpAddress;
        byte[] SubnetMaskOctet;
        int SubnetNo;
        int host;// No. of hosts per subnet 
        public void GetNetWorkDetails(IP ip)
        {
            SubnetMaskOctet = new byte[4];
            IpAddress = ip.IpAddress;
            SubnetMaskOctet = ip.SubnetOctet;
            
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

        public byte[] GetFirstIP()
        {
            int i;
            byte[] FipOctet = new byte[4];
            string[] IpOctetS = new string[4];
            byte[] IpOctet = new byte[4];

            IpOctetS = IpAddress.Split('.');
            
            for (i=0;i<4;i++)
            {
                IpOctet[i] = Convert.ToByte(IpOctetS[i]);
                FipOctet[i] = Convert.ToByte((IpOctet[i] & SubnetMaskOctet[i]));
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
            i = 4;
            while(hostbitI>0)
            {

                LipOctet[i] = Convert.ToByte((hostbitI%256)-1);
                hostbitI /= 256;

                i--;
            }

            for (i = 0; i < 4; i++)
            {
                LipOctet[i] = Convert.ToByte(IpOctet[i] & LipOctet[i]);
            }
            return LipOctet;

        }



    }
}
