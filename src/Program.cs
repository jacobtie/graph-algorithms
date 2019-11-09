using System.IO;
using System.Collections.Generic;
using graph_algorithms.logging;
using graph_algorithms.data_structures;
using System;

namespace graph_algorithms
{
    class Program
    {
        // Method to take user input and run program
        static void Main(string[] args)
        {
            // Variable to store the result of the user input
            bool input;

            // Do while the user wants to repeat the program
            do
            {
                input = GetUserInput();
            }
            while(input);

            Logger.LogToFile();

            Console.WriteLine("\nPress enter to exit....");
            Console.ReadLine();
        }

        // Method to get user input and choose which algorithm to run
        static bool GetUserInput()
        {
            // Create the variables needed to store the returns of each algorithm
            Graph<string> graph = new Graph<string>(DirectedType.Undirected);
            Vertex<string> start = new Vertex<string>("A");

            // Create the variables needed to store the user input
            bool invalidInput;
            string allInput;
            char input1 = '?';
            int input;

            // Do while there was a problem with the input file
            do
            {
                // Reset boolean to indicate invalid input
                invalidInput = false;

                // Try to preform the following block of code
                try
                {
                    // Get user input for the file name (Searches in input-files folder)
                    Logger.WriteLine("\nWhich file would you like to use to create a graph? ");
                    var fileName = Logger.ReadLine();
                    (graph, start) = Graph<string>.BuildGraphFromFile(fileName);
                }
                // If the file could not be found, print the following error message
                catch(FileNotFoundException e)
                {
                    Logger.WriteLine("\nFile could not be found. Please try again. \n");
                    invalidInput = true;
                }
                // If another problem occurred, print the error message
                catch(Exception e)
                {
                    Logger.WriteLine("\n" + e.Message + "\n");
                    invalidInput = true;
                }
            }
            while(invalidInput);

            // Print the graph as an adjacency matrix
            Logger.WriteLine("\nGraph as an Adjacency Matrix: ");
            Logger.WriteLine(graph.ToAdjacencyMatrix());

            // Do while the program cannot preform Kruskal's on the given graph
            do
            {
                // Reset boolean to indicate invalid input
                invalidInput = false;

                // Do while the input is not equal to one or two
                do
                {
                    // Get user input for choice of algorithm
                    Logger.WriteLine("\nWhat would you like to do with this graph? (1/2)");
                    Logger.WriteLine("\n1. Run Dijkstra's Algorithm to find shortest path to " + 
                                    "\n   each vertex from the starting vertex\n");
                    Logger.WriteLine("2. Run Kruskal's Algorithm to find the minimum " +
                                    "\n   spanning tree of the graph\n");
                    allInput = Logger.ReadLine();
                }
                while(!int.TryParse(allInput, out input) || (input != 1 && input != 2));

                // If the user inputted one
                if (input == 1)
                {
                    // Run Dijkstra's and print the list of paths to each node
                    (var paths, var s) = ShortestPath<string>.RunDijkstras(graph, start);
                    PrintPaths(paths, s);
                }
                // Else the user inputted two
                else
                {
                    // Try to perform the following code
                    try
                    {
                        // Run Kruskal's algorithm
                        KruskalAlgorithm<string>.RunKruskal(graph);
                    }
                    // If an exception was thrown, print the error message
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                        invalidInput = true;
                    }
                }
            }
            while(invalidInput);

            // Do while the user did not input Y or N
            do
            {
                // Get user input to repeat the program
                Logger.WriteLine("\nWould you like to repeat the program with another graph? (Y/N)");
                allInput = Logger.ReadLine();

                // If the length of the input is equal to zero
                if (allInput.Length > 0)
                {
                    // Set the input equal to the first character of the input
                    input1 = allInput.ToUpper()[0];
                }
            }
            while(input1 != 'Y' && input1 != 'N');

            // Return if the user entered Y
            return input1 == 'Y';
        }

        // Method to print the paths given by Dijkstra's Algorithm
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
