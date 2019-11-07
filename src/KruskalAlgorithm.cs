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

            // While there are still edges to potentially add
            while(!heap.isEmpty())
            {
                // Get the edge at the top of the heap
                var currEdge = heap.RemoveMin();

                // If the end points of the current edge are not part of the same set
                if (disjointSet.Union(currEdge))
                {
                    // Insert the edge into the minimum spanning tree
                    minSpanTree.InsertEdge(currEdge.EndVertices.start, 
                                            currEdge.EndVertices.end, currEdge.Weight);
                }
            }

            Logger.WriteLine("Finished Kruskal's Algorithm. \n");

            // Get the total weight of the minimum spanning tree
            int totalWeight = minSpanTree.Edges.Sum(e => e.Weight);

            // Print the minimum spanning tree as an adjacency matrix with total weight
            Logger.WriteLine("\nMinimum Spanning Tree created by the Kruskal Algorithm: ");
            Logger.WriteLine("The total weight of the MST is " + totalWeight + ". ");
            Logger.WriteLine(minSpanTree.ToAdjacencyMatrix());

            // Return the minimum spanning tree
            return minSpanTree;
        }
    }
}
