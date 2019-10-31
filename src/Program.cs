using System;
using System.IO;
using graph_algorithms.logging;
using graph_algorithms.data_structures;

namespace graph_algorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph<string> graph = new Graph<string>(DirectedType.Undirected);
            Vertex<string>? start = new Vertex<string>("A");
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
                    Logger.WriteLine(e);
                    Logger.WriteLine("File could not be found. Please try again. ");
                    invalidInput = true;
                }
            }
            while(invalidInput);

            ShortestPath<string>.RunDijkstras(graph, start);

            Console.WriteLine("Press enter to exit....");
            Console.ReadLine();
        }

        static string GetUserInput()
        {
            Logger.WriteLine("Which file would you like to use to create a graph? ");
            return Logger.ReadLine();
        }
    }
}
