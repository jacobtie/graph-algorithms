using System;
using System.IO;
using System.Collections.Generic;
using graph_algorithms.logging;
using graph_algorithms.data_structures;

namespace graph_algorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph<string> graph = new Graph<string>(DirectedType.Undirected);
            Vertex<string> start = new Vertex<string>("A");
            bool invalidInput = false;

            do
            {
                try
                {
                    string fileName = GetUserInput();
                    (graph, start) = Graph<string>.BuildGraphFromFile(fileName);
                }
                catch(FileNotFoundException e)
                {
                    Logger.WriteLine("File could not be found. Please try again. ");
                    invalidInput = true;
                }
            }
            while(invalidInput);

            var paths = ShortestPath<string>.RunDijkstras(graph, start);
            
            Logger.WriteLine("Paths to each vertex starting from vertex " + start.Element + ": ");

            foreach ((var key, var val) in paths)
            {
                Logger.Write(key + ": ");
                Stack<string> currPath = new Stack<string>();
                var curr = key;

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

                Logger.WriteLine();
            }

            Logger.WriteLine("\nPress enter to exit....");
            Logger.ReadLine();
        }

        static string GetUserInput()
        {
            Logger.WriteLine("Which file would you like to use to create a graph? ");
            return Logger.ReadLine();
        }
    }
}
