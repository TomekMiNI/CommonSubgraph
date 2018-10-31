using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAIO
{
    class Graph
    {
        public List<int> V = new List<int>();
        public List<Edge> E = new List<Edge>();
        public Graph Copy()
        {
            return new Graph() { V = new List<int>(V), E = new List<Edge>(E) };
        }
        public override string ToString()
        {
            string verticles = "V: ";
            foreach (int v in V)
                verticles += v + ", ";
            string edges = "\nE: ";
            foreach (Edge e in E)
            {
                edges += e + ", ";
            }
            return verticles + edges + "\n";
        }
        public static Graph convertFromMatrix(int[,] matrix)
        {
            Graph g = new Graph();
            int size = matrix.GetLength(0);
            if (size > 0)
            {
                g.V.Add(0);
                for (int i = 1; i < matrix.GetLength(0); i++)
                {
                    g.V.Add(i);
                    for (int j = 0; j < i; j++)
                    {
                        if (matrix[i, j] != 0)
                        {
                            g.E.Add(new Edge(i, j));
                        }
                    }
                }
            }
            return g;
        }
    }
}
