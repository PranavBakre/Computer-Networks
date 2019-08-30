using System;
using System.Collections.Generic;
using System.Text;


namespace DistanceVector
{

    

    class Graph
    {
        int n;
        int[,] graph;
        public Graph(int n)
        {
            this.n = n;
            graph = new int[n,n];
            for(int i = 0; i < n; i++)
            {
                for(int j=0;j<n;j++)
                {
                    graph[i, j] = -1;
                }
            }
        }

        public void init()
        {
            Console.WriteLine("Enter the distances. Enter 999 for infinity\n");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i==j)
                    {
                        graph[i,j] = 0;
                        graph[j,i] = 0;
                    }
                    if(graph[i,j]==-1)
                    {
                        Console.WriteLine($"Enter the distance between {i+1} and {j+1}.");
                        graph[i, j] = graph[j, i] = Convert.ToInt32(Console.ReadLine());

                    }
                    

                }
            }
        }


    }
}
