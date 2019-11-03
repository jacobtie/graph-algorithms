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
        private List<Vertex<E>> _vertices;
        private List<Edge<E>> _edges;
        /// <summary>
        /// The list of vertices in the graph
        /// </summary>
        /// <value>Graph vertices</value>
        public List<Vertex<E>> Vertices { get => new List<Vertex<E>>(_vertices); }

        /// <summary>
        /// The list of edges in the graph
        /// </summary>
        /// <value>Graph edges</value>
        public List<Edge<E>> Edges { get => new List<Edge<E>>(_edges); }

        /// <summary>
        /// The type of the graph as an enum
        /// </summary>
        /// <value>Graph type enum</value>
        public DirectedType GraphType { get; }

        /// <summary>
        /// The number of vertices in the graph
        /// </summary>
        /// <value>Number of graph vertices</value>
        public int NumVertices { get => _vertices.Count; }

        /// <summary>
        /// The number of edges in the graph
        /// </summary>
        /// <value>Number of graph edges</value>
        public int NumEdges { get => _edges.Count; }

        /// <summary>
        /// Graph constructor which takes in graph type enum
        /// </summary>
        /// <param name="graphType">The type of the graph</param>
        public Graph(DirectedType graphType)
        {
            _vertices = new List<Vertex<E>>();
            _edges = new List<Edge<E>>();

            GraphType = graphType;
        }

        /// <summary>
        /// Copy constructor using another graph
        /// </summary>
        /// <param name="other">Other graph to copy</param>
        public Graph(Graph<E> other)
        {
            _vertices = new List<Vertex<E>>(other._vertices);
            _edges = new List<Edge<E>>(other._edges);

            GraphType = other.GraphType;
        }

        /// <summary>
        /// Inserts a vertex into the graph
        /// </summary>
        /// <param name="element">The element of the vertex to insert</param>
        public void InsertVertex(E element)
        {
            var v = new Vertex<E>(element);
            _vertices.Add(v);
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
            _edges.Add(edgeToInsert);
        }

        /// <summary>
        /// Gets the outward edges for a given vertex
        /// </summary>
        /// <param name="vertex">Source vertex</param>
        /// <returns>Outward edges for given vertex</returns>
        public List<Edge<E>> GetOutEdges(Vertex<E> vertex)
        {
            return _edges.Where(edge => edge.EndVertices.start == vertex).ToList();
        }

        /// <summary>
        /// Inward edges to a given vertex
        /// </summary>
        /// <param name="vertex">Destination vertex</param>
        /// <returns>Inward edges to given vertex</returns>
        public List<Edge<E>> GetInEdges(Vertex<E> vertex)
        {
            return _edges.Where(edge => edge.EndVertices.end == vertex).ToList();
        }

        /// <summary>
        /// Gets all incident edges given a vertex
        /// </summary>
        /// <param name="vertex">The vertex from which to get incidental edges</param>
        /// <returns>A list of incidental edges of the given vertex</returns>
        public List<Edge<E>> GetIncidentEdges(Vertex<E> vertex)
        {
            return _edges.Where(edge => edge.EndVertices.start == vertex || edge.EndVertices.end == vertex).ToList();
        }

        /// <summary>
        /// Get all, unique adjacent vertices to a given vertex
        /// </summary>
        /// <param name="vertex">Given vertex</param>
        /// <returns>Adjacent vertices to given vertex</returns>
        public List<Vertex<E>> GetAdjacentVertices(Vertex<E> vertex)
        {
            var adjacentVerticies = new List<Vertex<E>>();
            adjacentVerticies.AddRange(GetOutEdges(vertex)
                .Select(edge => edge.EndVertices.end));
            adjacentVerticies.AddRange(GetInEdges(vertex)
                .Select(edge => edge.EndVertices.start).Except(adjacentVerticies));
            return adjacentVerticies;
        }

        /// <summary>
        /// Checks whether two vertices are adjacent
        /// </summary>
        /// <param name="v">Vertex v</param>
        /// <param name="w">Vertex w</param>
        /// <returns>Boolean v and w are adjacent</returns>
        public bool AreAdjacent(Vertex<E> v, Vertex<E> w)
        {
            return _edges.Any(edge => (edge.EndVertices.start == v && edge.EndVertices.end == w)
                        || (edge.EndVertices.start == w && edge.EndVertices.end == v));
        }

        /// <summary>
        /// Removes a vertex from a graph and all edges to or from that vertex
        /// </summary>
        /// <param name="vertex">Vertex to remove</param>
        public void RemoveVertex(Vertex<E> vertex)
        {
            _edges = _edges.Where(edge => edge.EndVertices.start != vertex && edge.EndVertices.end != vertex).ToList();
            _vertices.Remove(vertex);
        }

        /// <summary>
        /// Removes an edge from the graph
        /// </summary>
        /// <param name="edge">Edge to remove</param>
        public void RemoveEdge(Edge<E> edge)
        {
            _edges.Remove(edge);
        }

        /// <summary>
        /// Returns whether or not an undirected graph is a connected graph. NOTE that this only works for undirected graphs.
        /// </summary>
        /// <returns>A boolean indicating if an undirected graph is connected</returns>
        public bool IsConnected()
        {
            if (GraphType != DirectedType.Undirected)
            {
                throw new Exception("This method is only implemented for undirected graphs");
            }

            var dfsVertices = _vertices.Select(v => new DFSVertex<E>(v, DFSColor.White)).ToList();

            _dfsVisit(dfsVertices, dfsVertices[0]);

            bool connected = true;
            foreach (var vertex in dfsVertices)
            {
                if (vertex.Color != DFSColor.Black)
                {
                    connected = false;
                    break;
                }
            }

            return connected;
        }

        private void _dfsVisit(List<DFSVertex<E>> vertices, DFSVertex<E> currentVertex)
        {
            currentVertex.Color = DFSColor.Gray;
            foreach (var neighbor in this.GetAdjacentVertices(currentVertex.Vertex))
            {
                var dfsVertex = vertices.Find(dfsV => dfsV.Vertex == neighbor);
                if (dfsVertex.Color == DFSColor.White)
                {
                    _dfsVisit(vertices, dfsVertex);
                }
            }
            currentVertex.Color = DFSColor.Black;
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
                using (var reader = new StreamReader("./input-files/" + inputFile))
                {
                    firstLine = reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line is null)
                        {
                            throw new Exception("No line could be read from the file. ");
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
                throw new Exception("No line could be read from the file. ");
            }

            var firstLineValues = firstLine.Split(' ');

            int numVertices;
            DirectedType graphType;

            if (!int.TryParse(firstLineValues[0], out numVertices))
            {
                throw new FormatException("The number of vertices is not an integer. ");
            }

            if (firstLineValues[2] != "D" && firstLineValues[2] != "U")
            {
                throw new FormatException("The graph type must be D or U. ");
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
                        throw new FormatException("A weight not a positive integer. ");
                    }
                    var startVertex = vertices.Find(v => v.Element == startLetter);
                    var endVertex = vertices.Find(v => v.Element == endLetter);
                    if (startVertex is null || endVertex is null)
                    {
                        throw new Exception("A vertex could not be found. ");
                    }
                    builtGraph.InsertEdge(startVertex, endVertex, weight);
                }
                else if (lineValues.Length == 1)
                {
                    sourceNode = vertices.Find(v => v.Element == lineValues[0]);
                }
                else
                {
                    throw new FormatException("There is an unexpected number of values in the graph input. ");
                }
            }

            return (builtGraph, sourceNode);
        }

        public string ToAdjacencyMatrix()
        {
            string adjMat = "";

            for (int i = 0; i < Vertices.Count * 9; i++)
            {
                adjMat += "-";
            }

            adjMat += "\n";

            adjMat += "\t";

            foreach (var x in Vertices)
            {
                adjMat += x.Element + "\t";
            }

            adjMat += "\n";

            foreach (var x in Vertices)
            {
                adjMat += x.Element + "\t";

                foreach (var y in Vertices)
                {
                    Edge<E> e;

                    if (GraphType == DirectedType.Undirected)
                    {
                        e = Edges.Find(edge => (edge.EndVertices.start.Element.Equals(x.Element) &&
                            edge.EndVertices.end.Element.Equals(y.Element)) ||
                            (edge.EndVertices.start.Element.Equals(y.Element) &&
                            edge.EndVertices.end.Element.Equals(x.Element)));
                    }
                    else
                    {
                        e = Edges.Find(edge => (edge.EndVertices.start.Element.Equals(x.Element) &&
                            edge.EndVertices.end.Element.Equals(y.Element)));
                    }

                    if (e != null)
                    {
                        adjMat += e.Weight + "\t";
                    }
                    else
                    {
                        adjMat += "0\t";
                    }
                }

                adjMat += "\n";
            }

            for (int i = 0; i < Vertices.Count * 9; i++)
            {
                adjMat += "-";
            }

            adjMat += "\n";

            return adjMat;
        }
    }
}
