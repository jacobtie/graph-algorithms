using System;
using System.Text;
using System.IO;

namespace graph_algorithms.logging
{
    /// <summary>
    /// Wrapper around Console to log every command the console does
    /// </summary>
    public static class Logger
    {
        private static StringBuilder _sb = new StringBuilder();

        /// <summary>
        /// Writes an object to console and logs it
        /// </summary>
        /// <param name="input">The object to console and log</param>
        /// <typeparam name="T">The type of object inferred by input</typeparam>
        public static void Write<T>(T input)
        {
            if (input != null)
            {
                var inputAsString = input.ToString();
                Console.Write(inputAsString);
                _sb.Append(inputAsString);
            }
        }

        /// <summary>
        /// Writes an object to the console and logs it followed by a carriage return
        /// </summary>
        /// <param name="line">The object to console and log</param>
        /// <typeparam name="T">The type of object inferred by input</typeparam>
        public static void WriteLine<T>(T line)
        {
            var lineAsString = line?.ToString() + "\n" ?? "\n";
            Write(lineAsString);
        }

        /// <summary>
        /// Writes a blank line to the console and logs the blank line
        /// </summary>
        public static void WriteLine()
        {
            Console.WriteLine();
            _sb.Append("\n");
        }

        /// <summary>
        /// Reads a line from the console and logs the input
        /// </summary>
        /// <returns>The input read from the console</returns>
        public static string ReadLine()
        {
            var input = Console.ReadLine();
            _sb.Append(input + "\n");
            return input;
        }

        /// <summary>
        /// Clears the log
        /// </summary>
        public static void ClearLogger()
        {
            _sb.Clear();
        }

        /// <summary>
        /// Asynchronously writes the contents of the log to an output file in the 
        /// logs directory as a txt file with a filename of "log_" followed by the time in milliseconds
        /// </summary>
        /// <returns></returns>
        public static void LogToFile()
        {
            var fileName = $"logs/log_{DateTime.UtcNow.ToFileTimeUtc()}.txt";

            using (var writer = new StreamWriter(fileName))
            {
                writer.Write(_sb);
            }
        }
    }
}
