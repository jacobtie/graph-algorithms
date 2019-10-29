using System;

namespace graph_algorithms.data_structures
{
    /// <summary>
    /// Represents a weighted edge in a graph
    /// either directed or undirected
    /// </summary>
    /// <typeparam name="T">The type stored in the graph's vertices</typeparam>
    public class Edge<T>
    {
        /// <summary>
        /// The type of the edge, either directed or undirected 
        /// from the graph
        /// </summary>
        /// <value>Either directed or undirected</value>
        public DirectedType EdgeType { get; set; }

        /// <summary>
        /// The weight of the edge
        /// </summary>
        /// <value>Edge weight</value>
        public int Weight { get; set; }

        /// <summary>
        /// The start and end vertices of the edge as a tuple
        /// </summary>
        /// <value>Tuple of start and end verticies</value>
        public (Vertex<T> start, Vertex<T> end) EndVertices { get; }

        /// <summary>
        /// Constructor with start vertex, end vertex, weight, and type
        /// </summary>
        /// <param name="start">Start vertex of the edge</param>
        /// <param name="end">End vertex of the edge</param>
        /// <param name="weight">Weight of the edge</param>
        /// <param name="edgeType">Type of the edge, either directed or undirected</param>
        public Edge(Vertex<T> start, Vertex<T> end, int weight, DirectedType edgeType)
        {
            EndVertices = (start, end);
            Weight = weight;
            EdgeType = edgeType;
        }
    }
}
