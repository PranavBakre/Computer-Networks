using System;
using System.Collections.Generic;
using System.Text;

namespace GetIPAndSubnet
{
    class Subnetting
    {
        public string IpAddress;
        public byte[] DefaultMaskOctet;
        public int SubnetNo;
        public void GetNetWorkDetails(IP ip)
        {
            DefaultMaskOctet = new byte[4];
            IpAddress = ip.IpAddress;
            DefaultMaskOctet = ip.SubnetOctet;
            
        }

        public int GetSubnetWorkNo()//Get Subnet Network No host
        {
            int i,j, host=1;
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
                Console.WriteLine(tempoctet[i]);
            }

            for (i=0;i<4;i++)
            {
                if (DefaultMaskOctet[i]!=255)
                {
                    for (j = 0; j < tempoctet.Length; j++)
                    {
                        DefaultMaskOctet[i] = tempoctet[j];
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
                Console.Write($"{DefaultMaskOctet[i]} ");
            Console.WriteLine();
            //Total No of hosts in the network/No of hosts per subnet 
            for (i=0;i<4;i++)
            {
                if (DefaultMaskOctet[i] != 255)
                {
                    host *= Convert.ToInt32((byte)~DefaultMaskOctet[i])+1;
                }
            }

            Console.WriteLine(host);

            return 0;
        }





    }
}
