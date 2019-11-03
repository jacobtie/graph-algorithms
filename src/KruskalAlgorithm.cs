using graph_algorithms.data_structures;
using System.Collections.Generic;
using graph_algorithms.logging;
using System;

namespace graph_algorithms
{
    static class KruskalAlgorithm<T>
    {
        public static Graph<T> RunKruskal(Graph<T> graph)
        {
            Logger.WriteLine("\nOriginal Graph as Adjacency Matrix: ");
            Logger.WriteLine(graph.ToAdjacencyMatrix());

            if (graph.GraphType != DirectedType.Undirected)
            {
                throw new Exception("Kruskal's Algorithm cannot be performed on a " + 
                                    "directed graph. ");
            }

            var minSpanTree = new Graph<T>(graph);
            var heap = new MinHeap<Edge<T>>();
            var cloud = new List<T>();

            foreach (var e in graph.Edges)
            {
                heap.Add(e);
            }

            while(!heap.isEmpty())
            {
                var min = heap.RemoveMin();

                if (cloud.Contains(min.EndVertices.start.Element) && 
                    cloud.Contains(min.EndVertices.end.Element))
                {
                    minSpanTree.RemoveEdge(min);
                    continue;
                }

                if (!cloud.Contains(min.EndVertices.start.Element))
                {
                    cloud.Add(min.EndVertices.start.Element);
                }

                if (!cloud.Contains(min.EndVertices.end.Element))
                {
                    cloud.Add(min.EndVertices.end.Element);
                }
            }

            Logger.WriteLine("\nMinimum Spanning Tree created by the Kruskal Algorithm: ");
            Logger.WriteLine(minSpanTree.ToAdjacencyMatrix());

            return minSpanTree;
        }
    }
}