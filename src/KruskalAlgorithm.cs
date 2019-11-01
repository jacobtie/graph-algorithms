using graph_algorithms.data_structures;
using System.Collections.Generic;
using System;

namespace graph_algorithms
{
    static class KruskalAlgorithm<T>
    {
        public static Graph<T> RunKruskal(Graph<T> graph)
        {
            if (graph.GraphType != DirectedType.Undirected)
            {
                throw new Exception("Directed Graphs are not supported for this " +
                                    "implementation of the Kruskal Algorithm. ");
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

                if (!cloud.Contains(min.EndVertices.start.Element))
                {
                    cloud.Add(min.EndVertices.start.Element);
                }
                else if (!cloud.Contains(min.EndVertices.end.Element))
                {
                    cloud.Add(min.EndVertices.end.Element);
                }
                else
                {
                    minSpanTree.RemoveEdge(min);
                }
            }

            return minSpanTree;
        }
    }
}