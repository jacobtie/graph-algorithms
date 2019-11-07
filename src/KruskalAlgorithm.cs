using graph_algorithms.data_structures;
using System.Linq;
using graph_algorithms.logging;
using System;

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

            Logger.WriteLine("Running Kruskal's Algorithm... \n");

            // Create min heap to store all of the edges in the graph
            var heap = new MinHeap<Edge<T>>();

            var minSpanTree = new Graph<T>(graph.Vertices);

            var disjointSet = new DisjointSet<T>(graph.Vertices);

            // For each edge in the graph
            foreach (var e in graph.Edges)
            {
                // Add the current edge to the heap
                heap.Add(e);
            }

            while(!heap.isEmpty())
            {
                var currEdge = heap.RemoveMin();

                if (!disjointSet.Union(currEdge))
                {
                    minSpanTree.InsertEdge(currEdge.EndVertices.start, 
                                            currEdge.EndVertices.end, currEdge.Weight);
                }
            }

            Logger.WriteLine("Finished Kruskal's Algorithm. \n");

            int totalWeight = minSpanTree.Edges.Sum(e => e.Weight);

            // Print the minimum spanning tree as an adjacency matrix
            Logger.WriteLine("\nMinimum Spanning Tree created by the Kruskal Algorithm: ");
            Logger.WriteLine("The total weight of the MST is " + totalWeight + ". ");
            Logger.WriteLine(minSpanTree.ToAdjacencyMatrix());
            var totalCost = minSpanTree.Edges.Aggregate(0, (sum, edge) => sum += edge.Weight);
            Logger.WriteLine($"Total Graph Edge Cost: {totalCost}");

            // Return the minimum spanning tree
            return minSpanTree;
        }
    }
}
