using System.IO;
using System.Collections.Generic;
using graph_algorithms.logging;
using graph_algorithms.data_structures;
using System;

namespace graph_algorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            bool input;

            do
            {
                input = GetUserInput();
            }
            while(input);

            Logger.WriteLine("\nPress enter to exit....");
            Logger.ReadLine();
        }

        static bool GetUserInput()
        {
            Graph<string> graph = new Graph<string>(DirectedType.Undirected);
            Vertex<string> start = new Vertex<string>("A");
            bool invalidInput;
            string allInput;
            char input1 = '?';
            int input;

            do
            {
                invalidInput = false;

                try
                {
                    Logger.WriteLine("\nWhich file would you like to use to create a graph? ");
                    var fileName = Logger.ReadLine();
                    (graph, start) = Graph<string>.BuildGraphFromFile(fileName);
                }
                catch(FileNotFoundException e)
                {
                    Logger.WriteLine("\nFile could not be found. Please try again. \n");
                    invalidInput = true;
                }
                catch(Exception e)
                {
                    Logger.WriteLine("\n" + e.Message + "\n");
                    invalidInput = true;
                }
            }
            while(invalidInput);

            Logger.WriteLine("\nGraph as an Adjacency Matrix: ");
            Logger.WriteLine(graph.ToAdjacencyMatrix());

            do
            {
                invalidInput = false;

                do
                {
                    Logger.WriteLine("\nWhat would you like to do with this graph? (1/2)");
                    Logger.WriteLine("\t1. Run Dijkstra's Algorithm to find shortest path to " + 
                                    "\n\t   each vertex from the starting vertex");
                    Logger.WriteLine("\t2. Run Kruskal's Algorithm to find the minimum " +
                                    "\n\t   spanning tree of the graph");
                    allInput = Logger.ReadLine();
                }
                while(!int.TryParse(allInput, out input) || (input != 1 && input != 2));

                if (input == 1)
                {
                    var paths = ShortestPath<string>.RunDijkstras(graph, start);
                
                    PrintPaths(paths, start);
                }
                else
                {
                    try
                    {
                        KruskalAlgorithm<string>.RunKruskal(graph);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                        invalidInput = true;
                    }
                }
            }
            while(invalidInput);

            do
            {
                Logger.WriteLine("\nWould you like to repeat the program with another graph? (Y/N)");
                allInput = Logger.ReadLine();

                if (allInput.Length > 0)
                {
                    input1 = allInput.ToUpper()[0];
                }
            }
            while(input1 != 'Y' && input1 != 'N');

            return input1 == 'Y';
        }

        static void PrintPaths(Dictionary<string, PathElement<string>> paths, 
                                Vertex<string> start)
        {
            Logger.WriteLine("Paths to each vertex starting from vertex " + start.Element + ": ");

            foreach ((var key, var val) in paths)
            {
                Logger.Write(key + ": ");
                Stack<string> currPath = new Stack<string>();
                var curr = key;

                if (paths[curr].cost != int.MaxValue)
                {
                    while(paths[curr].prevVertex != null)
                    {
                        currPath.Push(paths[curr].currVertex.Element);
                        curr = paths[curr].prevVertex.Element;
                    }

                    currPath.Push(paths[curr].currVertex.Element);

                    while(currPath.Count != 0)
                    {
                        Logger.Write(currPath.Pop() + "  ");
                    }

                    Logger.Write("-  " + val.cost);
                }
                else
                {
                    Logger.Write("Unreachable from vertex " + start.Element);
                }

                Logger.WriteLine();
            }
        }
    }
}
