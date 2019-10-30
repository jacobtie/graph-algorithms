using System;
using System.IO;
using graph_algorithms.logging;
using graph_algorithms.data_structures;
using System.Collections.Generic;
using System.Linq;

namespace graph_algorithms
{
    static class ShortestPath<T> where T : struct
    {
        public static Graph<T> RunDijkstras(Graph<T> graph, Vertex<T> start)
        {
            var paths = new Graph<T>(graph.GraphType);
            var finalized = new List<Vertex<T>>();
            var costs = new Dictionary<T, int>(graph.Vertices.Count);
            var currentVertex = start;
            Edge<T> shortest;
            Vertex<T> prevVertex;
            Edge<T> prevEdge;

            foreach (var v in graph.Vertices)
            {
                costs.Add(v.Element, int.MaxValue);
            }

            finalized.Add(currentVertex);
            costs[currentVertex.Element] = 0;
            shortest = FindShortestEdge(graph, currentVertex);
            currentVertex = shortest.EndVertices.end;
            prevVertex = currentVertex;
            prevEdge = shortest;

            do 
            {
                finalized.Add(currentVertex);
                costs[currentVertex.Element] = costs[prevVertex.Element] + prevEdge.Weight;

                RemoveEdgesInCloud(graph, finalized);

                shortest = FindShortestEdge(graph, currentVertex);
                currentVertex = shortest.EndVertices.end;
                prevEdge = shortest;
            }
            while(finalized.Count != graph.Vertices.Count);

            return paths;
        }

        private static Edge<T> FindShortestEdge(Graph<T> graph, Vertex<T> curr)
        {
            var incidentEdges = graph.GetIncidentEdges(curr);
            var minEdge = incidentEdges[0];

            foreach (var edge in incidentEdges)
            {
                if (edge.Weight < minEdge.Weight)
                {
                    minEdge = edge;
                }
            }

            return minEdge;
        }

        private static void RemoveEdgesInCloud(Graph<T> graph, List<Vertex<T>> finalized)
        {
            var adjEdges = graph.GetIncidentEdges(finalized.Last());

            foreach (var edge in adjEdges)
            {
                if (finalized.Contains(edge.EndVertices.end))
                {
                    // Remove edge from graph
                }
            }
        }
    }
}
