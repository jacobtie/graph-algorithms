using System;
using System.Text;
using System.IO;

namespace graph_algorithms.logging
{
    public static class Logger
    {
        private static StringBuilder _sb = new StringBuilder();

        public static void Write<T>(T input)
        {
            var inputAsString = input?.ToString() ?? "";
            Console.Write(inputAsString);
            _sb.Append(inputAsString);
        }

        public static void WriteLine<T>(T line)
        {
            var lineAsString = line?.ToString() + "\n" ?? "\n";
            Write(lineAsString);
        }

        public static void WriteLine()
        {
            Console.WriteLine();
            _sb.Append("\n");
        }

        public static void ClearLogger()
        {
            _sb.Clear();
        }

        public static async void LogToFileAsync()
        {
            var fileName = $"logs/log_{DateTime.UtcNow.ToFileTimeUtc()}.txt";

            using (var writer = new StreamWriter(fileName))
            {
                await writer.WriteAsync(_sb);
            }
        }
    }
}
