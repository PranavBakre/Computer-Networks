using System;

namespace DistanceVector
{
    class Program
    {
        static void Main(string[] args)
        {

            int NodeNumber;
            Console.WriteLine("Enter the number of nodes");
            NodeNumber=Convert.ToInt32(Console.ReadLine());

            Graph G = new Graph(NodeNumber);
            G.init();
        }
    }
}
