using graph_algorithms.data_structures;
using System.Collections.Generic;
using graph_algorithms.logging;
using System;
using System.Linq;

namespace graph_algorithms
{
    static class KruskalAlgorithm<T>
    {
        // Method to run Kruskal's Algorithm and return the minimum spanning tree of the graph
        public static Graph<T> RunKruskal(Graph<T> graph)
        {
            // Print the original graph as an adjacency matrix
            Logger.WriteLine("\nOriginal Graph as Adjacency Matrix: ");
            Logger.WriteLine(graph.ToAdjacencyMatrix());

            // If the graph is not undirected
            if (graph.GraphType != DirectedType.Undirected)
            {
                // Throw an exception
                throw new Exception("Kruskal's Algorithm cannot be performed on a " +
                                    "directed graph. ");
            }

            // If the graph is not connected
            if (!graph.IsConnected())
            {
                // Throw an exception
                throw new Exception("This implementation of Kruskal's Algorithm can only " +
                                    "be performed on connected graphs.");
            }

            // Create Graph to store the minimum spanning tree
            var minSpanTree = new Graph<T>(graph);

            // Create min heap to store all of the edges in the graph
            var heap = new MinHeap<Edge<T>>();

            // Create List to store the vertices in the cloud 
            var cloud = new List<T>();

            // For each edge in the graph
            foreach (var e in graph.Edges)
            {
                // Add the current edge to the heap
                heap.Add(e);
            }

            // While the heap is not empty
            while (!heap.isEmpty())
            {
                // Create variable to store the top edge of the heap
                var min = heap.RemoveMin();

                // If the cloud contains the start and end vertices of the current edge
                if (cloud.Contains(min.EndVertices.start.Element) &&
                    cloud.Contains(min.EndVertices.end.Element))
                {
                    // Remove the current edge from the graph
                    minSpanTree.RemoveEdge(min);

                    // Continue to the next iteration of the foreach loop
                    continue;
                }

                // If the cloud does not contain the starting vertex of the current edge
                if (!cloud.Contains(min.EndVertices.start.Element))
                {
                    // Add the starting vertex of the current edge to the cloud
                    cloud.Add(min.EndVertices.start.Element);
                }

                // If the cloud does not contain the ending vertex of the current edge
                if (!cloud.Contains(min.EndVertices.end.Element))
                {
                    // Add the ending vertex of the current edge to the cloud
                    cloud.Add(min.EndVertices.end.Element);
                }
            }

            // Print the minimum spanning tree as an adjacency matrix
            Logger.WriteLine("\nMinimum Spanning Tree created by the Kruskal Algorithm: ");
            Logger.WriteLine(minSpanTree.ToAdjacencyMatrix());
            var totalCost = minSpanTree.Edges.Aggregate(0, (sum, edge) => sum += edge.Weight);
            Logger.WriteLine($"Total Graph Edge Cost: {totalCost}");

            // Return the minimum spanning tree
            return minSpanTree;
        }
    }
}
