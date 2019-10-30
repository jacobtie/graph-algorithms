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
            bool invalidInput = false;

            do
            {
                try
                {
                    string fileName = GetUserInput();
                    Graph<string>.BuildGraphFromFile(fileName);
                }
                catch(FileNotFoundException e)
                {
                    Logger.WriteLine("File could not be found. Please try again. ");
                    invalidInput = true;
                }
            }
            while(invalidInput);

            

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
