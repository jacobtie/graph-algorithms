using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace graph_algorithms.data_structures
{
    /// <summary>
    /// A generic weighted graph which can be directed or undirected 
    /// with the option to build a string type weighted graph either 
    /// directed or undirected from a file
    /// </summary>
    /// <typeparam name="E">The type of elements in the verticies of the graph</typeparam>
    public class Graph<E>
    {
        /// <summary>
        /// The list of vertices in the graph
        /// </summary>
        /// <value>Graph vertices</value>
        public List<Vertex<E>> Vertices { get; }

        /// <summary>
        /// The list of edges in the graph
        /// </summary>
        /// <value>Graph edges</value>
        public List<Edge<E>> Edges { get; }

        /// <summary>
        /// The type of the graph as an enum
        /// </summary>
        /// <value>Graph type enum</value>
        public DirectedType GraphType { get; set; }

        /// <summary>
        /// The number of vertices in the graph
        /// </summary>
        /// <value>Number of graph vertices</value>
        public int NumVertices { get => Vertices.Count; }

        /// <summary>
        /// The number of edges in the graph
        /// </summary>
        /// <value>Number of graph edges</value>
        public int NumEdges { get => Edges.Count; }

        /// <summary>
        /// Graph constructor which takes in graph type enum
        /// </summary>
        /// <param name="graphType">The type of the graph</param>
        public Graph(DirectedType graphType)
        {
            Vertices = new List<Vertex<E>>();
            Edges = new List<Edge<E>>();
            GraphType = graphType;
        }

        /// <summary>
        /// Inserts a vertex into the graph
        /// </summary>
        /// <param name="element">The element of the vertex to insert</param>
        public void InsertVertex(E element)
        {
            var v = new Vertex<E>(element);
            Vertices.Add(v);
        }

        /// <summary>
        /// Inserts an edge into the graph
        /// </summary>
        /// <param name="v">The start vertex</param>
        /// <param name="w">The end vertex</param>
        /// <param name="weight">The weight of the edge</param>
        public void InsertEdge(Vertex<E> v, Vertex<E> w, int weight)
        {
            var edgeToInsert = new Edge<E>(v, w, weight, GraphType);
            Edges.Add(edgeToInsert);
        }

        /// <summary>
        /// Gets all incident edges given a vertex
        /// </summary>
        /// <param name="vertex">The vertex from which to get incidental edges</param>
        /// <returns>A list of incidental edges of the given vertex</returns>
        public List<Edge<E>> GetIncidentEdges(Vertex<E> vertex)
        {
            return Edges.Where(edge => edge.EndVertices.start == vertex || edge.EndVertices.end == vertex).ToList();
        }

        /// <summary>
        /// Builds a graph using an input file
        /// </summary>
        /// <param name="inputFile">The filepath from which to build the graph</param>
        /// <returns>A tuple of the created graph and the source node</returns>
        /// <exception cref="FormatException">There is a problem with the file input</exception>
        /// <exception cref="Exception">An error occurred while parsing the file</exception>
        public static (Graph<string>, Vertex<string>?) BuildGraphFromFile(string inputFile)
        {
            string? firstLine;
            var lines = new List<string>();

            try
            {
                using (var reader = new StreamReader(inputFile))
                {
                    firstLine = reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line is null)
                        {
                            throw new Exception("Line not read");
                        }
                        lines.Add(line);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }

            if (firstLine is null)
            {
                throw new Exception("Line not read");
            }

            var firstLineValues = firstLine.Split(' ');

            int numVertices;
            DirectedType graphType;

            if (!int.TryParse(firstLineValues[0], out numVertices))
            {
                throw new FormatException("NumVertices not an integer");
            }

            if (firstLineValues[2] != "D" && firstLineValues[2] != "U")
            {
                throw new FormatException("Graph type must be D or U");
            }

            graphType = firstLineValues[2] switch
            {
                "D" => DirectedType.Directed,
                "U" => DirectedType.Undirected,
                _ => DirectedType.Undirected,
            };

            Graph<string> builtGraph = new Graph<string>(graphType);
            Vertex<string>? sourceNode = null;
            char letter = 'A';
            for (int i = 0; i < numVertices; i++)
            {
                builtGraph.InsertVertex(letter.ToString());
                letter++;
            }

            var vertices = builtGraph.Vertices;
            foreach (var line in lines)
            {
                var lineValues = line.Split(' ');
                if (lineValues.Length == 3)
                {
                    var startLetter = lineValues[0];
                    var endLetter = lineValues[1];
                    int weight;
                    if (!int.TryParse(lineValues[2], out weight) && weight <= 0)
                    {
                        throw new FormatException("Weight not a positive integer");
                    }
                    var startVertex = vertices.Find(v => v.Element == startLetter);
                    var endVertex = vertices.Find(v => v.Element == endLetter);
                    if (startVertex is null || endVertex is null)
                    {
                        throw new Exception("Vertex not found");
                    }
                    builtGraph.InsertEdge(startVertex, endVertex, weight);
                }
                else if (lineValues.Length == 1)
                {
                    sourceNode = vertices.Find(v => v.Element == lineValues[0]);
                }
                else
                {
                    throw new FormatException("Unexpected number of values in graph input");
                }
            }

            return (builtGraph, sourceNode);
        }

    }
}
