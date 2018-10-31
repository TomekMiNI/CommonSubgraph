using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAIO
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] G1 = null, G2 = null;
            if(args.Any())
                readArgs(args, out G1, out G2);
            else
            {
                //G1 = new int[,] {
                //{0,1,1,0 },
                //{ 1,0,1,1},
                //{1,1,0,1 },
                //{0,1,1,0 }};
                //G2 = new int[,] {
                //{0,1,1,0 },
                //{ 1,0,0,1},
                //{1,0,0,1 },
                //{0,1,1,0 }};
                G1 = new int[,] {
                    {0, 1,0,0 },
                    { 1,0,1,1},
                    {0,1,0,1 },
                    {0,1,1,0 }};
                G2 = new int[,] {
                    {0,1,1,0 },
                    { 1,0,0,1},
                    {1,0,0,1 },
                    {0,1,1,0 }};
            }
            Console.Write(Graph.convertFromMatrix(G1));
            Console.Write(Graph.convertFromMatrix(G2));
            Graph gMax = new Graph(), gC = new Graph();
            int maxCount = calculateMaxOfEdges(G1, G2);
            MCS(G1, G2, new Graph(), new Graph(), ref gMax, ref gC, maxCount);
            Console.Write("Final graph: \n{0}", gMax);
            Console.Write("Corresponding: \n{0}", gC);
        }
        public static void MCS(int[,] G1, int[,] G2, Graph SG1, Graph SG2, ref Graph theBEST, ref Graph corresponding, int max)
        {
            Graph copySG1 = SG1.Copy();
            Graph copySG2 = SG2.Copy();
            int countOfAddedV1, countOfAddedV2;
            foreach (var e in neighbours(G1, SG1))
            {
                if (e == null) break;
                //if (Edge.Equals(e, new Edge(1, 3)))
                //   Console.Write("tell me cuando cuando cuando");
                //Console.WriteLine(e + " to SG1 added");
                copySG1.E.Add(e);
                countOfAddedV1 = addedVerticles(e, copySG1);
                foreach (var e2 in neighbours(G2, SG2))
                {
                    if (e2 == null) break;
                    //Console.WriteLine(e2 + " to SG2 added");
                    copySG2.E.Add(e2);
                    countOfAddedV2 = addedVerticles(e2, copySG2);
                    if (pairIsFeasible(copySG1, e, copySG2, e2, countOfAddedV1, countOfAddedV2))
                    {
                        //Console.WriteLine("they correspond themselves!");
                        MCS(G1, G2, copySG1, copySG2, ref theBEST, ref corresponding, max);
                        if (theBEST.E.Count < copySG1.E.Count)
                        {
                            Console.Write("Poprawka\n");
                            theBEST = copySG1.Copy();
                            corresponding = copySG2.Copy();
                            Console.WriteLine(theBEST);
                            Console.WriteLine(corresponding);
                        }
                        if (theBEST.E.Count == max)
                            return;
                    }
                    copySG2 = SG2.Copy();
                }
                copySG1 = SG1.Copy();
            }
            return;
        }
        public static IEnumerable<Edge> neighbours(int[,] G, Graph SG)
        {
            if (SG.V.Count == 0)
            {
                for (int i = 0; i < G.GetLength(0); i++)
                    for (int j = 0; j < G.GetLength(0); j++)
                    {
                        if (G[i, j] != 0)
                            yield return new Edge(i, j);
                    }
            }
            else
            {
                foreach (int v in SG.V)
                {
                    for (int i = 0; i < G.GetLength(0); i++)
                    {
                        if (v != i && G[v, i] != 0 && !SG.E.Contains(new Edge(v, i)))
                        {
                            yield return new Edge(v, i);
                        }
                    }
                }
            }

            yield return null;
        }
        public static int addedVerticles(Edge e, Graph G)
        {
            int count = 0;
            if (!G.V.Contains(e.v1))
            {
                G.V.Add(e.v1);
                count++;
            }
            if (!G.V.Contains(e.v2))
            {
                G.V.Add(e.v2);
                count++;
            }
            return count;
        }
        public static bool pairIsFeasible(Graph SG1, Edge e1, Graph SG2, Edge e2, int addedV1, int addedV2)
        {
            if (SG1.V.Count != SG2.V.Count)
                return false;
            //if (addedV1 == 0) //case when edge can bind not corresponding verticles
            //{
            //get number of binded v2 in SG1 and check it in SG2
            //int number = 0;
            //foreach(int v2 in SG1.V)
            //{
            //    if(v2 == e1.v2)
            //    {
            //        if (SG2.E[number].v2 != e2.v2)
            //            return false;
            //        else
            //        {
            //            number = 0;
            //            foreach(int v1 in SG1.V)
            //            {
            //                if(v1 == e1.v1)
            //                {
            //                    if (SG2.E[number].v1 != e2.v1)
            //                        return false;
            //                }
            //                number++;
            //            }
            //        }
            //    }
            //    number++;
            //}
            //or maybe simply check count neighbours verticles of v2
            //return (calculateNeighbours(e1.v1, SG1) == calculateNeighbours(e2.v1, SG2)) && (calculateNeighbours(e1.v2, SG1) == calculateNeighbours(e2.v2, SG2));
            Tuple<int, int> t1, t2;
            t1 = calculateNeighboursE(e1, SG1);
            t2 = calculateNeighboursE(e2, SG2);
            return t1.Item1 == t2.Item1 && t1.Item2 == t2.Item2;
        }
        //private static int calculateNeighbours(int v, Graph g)
        //{
        //    int count = 0;
        //    foreach (Edge e in g.E)
        //        if (e.v1 == v || e.v2 == v)
        //            count++;
        //    return count;
        //}
        private static Tuple<int,int> calculateNeighboursE(Edge e, Graph g)
        {
            int count1 = 0, count2 = 0;
            foreach (Edge ee in g.E)
            {
                if (e.v1 == ee.v1 || e.v2 == ee.v1)
                    count1++;
                if (e.v2 == ee.v2 || e.v2 == ee.v2)
                    count2++;
            }
            return new Tuple<int,int>(count1, count2);
        }
        private static void readArgs(string[] args, out int[,] G1, out int[,] G2)
        {
            if (args.Length == 2)
            {
                if (File.Exists(args[0]) && File.Exists(args[1]))
                {
                    loadGraph(args[0], out G1);
                    loadGraph(args[1], out G2);
                    return;
                }
            }
            throw new Exception();
        }
        private static void loadGraph(string path, out int[,] G)
        {
            using (var reader = new StreamReader(@path))
            {
                List<string[]> list = new List<string[]>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    list.Add(values);
                }
                if (list.Count > 0)
                {
                    G = new int[list.Count, list.Count];
                    for (int i = 0; i < list.Count; i++)
                        for (int j = 0; j < list.Count; j++)
                            if (!int.TryParse(list[i][j], out G[i, j]))
                                throw new Exception();
                }
                else
                    throw new Exception();
            }
        }
        private static int calculateMaxOfEdges(int[,] G1, int[,] G2)
        {
            int count1 = 0, count2 = 0;
            for (int i = 1; i < G1.GetLength(0); i++)
                for (int j = 0; j < i; j++)
                    if (G1[i, j] == 1) count1++;
            for (int i = 1; i < G2.GetLength(0); i++)
                for (int j = 0; j < i; j++)
                    if (G2[i, j] == 1) count2++;
            return Math.Max(count1, count2);
        }
    }
}
