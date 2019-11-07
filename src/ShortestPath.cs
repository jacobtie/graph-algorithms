using graph_algorithms.data_structures;
using System.Collections.Generic;
using graph_algorithms.logging;
using System.Linq;

namespace graph_algorithms
{
    static class ShortestPath<T>
    {
        // Method to perform Dijkstra's Algorithm by getting the paths and creating the tree
        public static Dictionary<T, PathElement<T>> RunDijkstras(Graph<T> graph, Vertex<T> start)
        {
            // Create graph to store the tree of shortest paths
            var paths = new Graph<T>(graph);

            // Create List to store the finalized vertices
            var finalized = new List<T>();

            // Create dictionary to store the relation between the vertices and their path
            var costs = new Dictionary<T, PathElement<T>>(paths.Vertices.Count);

            // Create variable to store the current node being finalized
            var currentElement = start.Element;

            // Print the original graph as an adjacency graph
            Logger.WriteLine("\nOriginal Graph as Adjacency Matrix: ");
            Logger.WriteLine(graph.ToAdjacencyMatrix());
            Logger.WriteLine("Beginning Dijkstra's Algorithm... \n");

            // For each vertex in the original graph
            foreach (var v in graph.Vertices)
            {
                // Create new path element
                PathElement<T> newElement;

                // If the current vertex is not the starting vertex
                if (!v.Element.Equals(start.Element))
                {
                    // Set the new path element with max cost
                    newElement = new PathElement<T>(v, null, null, int.MaxValue);
                }
                // Else the current vertex is the starting vertex
                else
                {
                    // Set the new path element with zero cost
                    newElement = new PathElement<T>(v, null, null, 0);
                }

                // Add the path element to the dictionary
                costs.Add(v.Element, newElement);
            }

            // // Update the cost based on the current vertex being finalized
            // UpdateCosts(paths, costs[currentElement].currVertex, costs);

            // // Finalize the current vertex
            // finalized.Add(currentElement);

            // // Set the current element equal to the closest, unfinalized vertex
            // currentElement = FindMinCostVertex(paths, costs, finalized);
            // Logger.WriteLine();

            // Do while all nodes are not finalized
            do
            {
                // Update the cost based on the current vertex being finalized
                UpdateCosts(paths, costs[currentElement].currVertex, costs);

                // Finalize the current vertex
                finalized.Add(currentElement);

                // Remove extraneous edges from the graph 
                RemoveEdgesInCloud(paths, finalized, costs);

                // Set the current element equal to the closest, unfinalized vertex
                currentElement = FindMinCostVertex(paths, costs, finalized);
                Logger.WriteLine();
            }
            while (finalized.Count != paths.NumVertices);

            Logger.WriteLine("\nFinished Dijkstra's Algorithm. ");

            // Print the adjacency matrix of the graph after all extraneous edges have been removed
            Logger.WriteLine("\nGraph after Extraneous Edges have been Removed: ");
            Logger.WriteLine(paths.ToAdjacencyMatrix());

            return costs;
        }

        // Method to update the costs by looking at the neighbors of the current vertex
        private static void UpdateCosts(Graph<T> graph, Vertex<T> curr,
                                        Dictionary<T, PathElement<T>> costs)
        {
            // Print current vertex element and the cost to reach that vertex
            Logger.WriteLine("Current Node being Finalized: " + curr.Element + " - " + costs[curr.Element].cost);

            // If the cost of the current element is the max value
            if (costs[curr.Element].cost == int.MaxValue)
            {
                // Stop the method
                return;
            }

            // Create List to store the adjacent edges of the most-recently finalized vertex
            List<Edge<T>> adjEdges;

            // If the graph is undirected
            if (graph.GraphType == DirectedType.Undirected)
            {
                // Get all edges that lead to or come from the current vertex
                adjEdges = graph.GetIncidentEdges(curr);
            }
            else
            {
                // Get all edges that come from the current vertex
                adjEdges = graph.GetOutEdges(curr);
            }

            // For each edge in the neighboring edges of the current vertex
            foreach (var e in adjEdges)
            {
                // Create variable to store the element of the connected vertex
                T nextElement;

                // If the starting vertex of the current edge is not the current vertex
                if (!curr.Element.Equals(e.EndVertices.start.Element))
                {
                    // Set the next element equal to the starting element of the current edge
                    nextElement = e.EndVertices.start.Element;
                }
                // Else the ending vertex of the current edge is not the current vertex
                else
                {
                    // Set the next element equal to the ending element of the current edge
                    nextElement = e.EndVertices.end.Element;
                }

                // If the costs to reach the next vertex through the current vertex is less than
                // the current cost of the next vertex
                if (costs[curr.Element].cost + e.Weight < costs[nextElement].cost)
                {
                    // Set the previous vertex, previous edge, and cost of the current vertex
                    // equal to the current vertex, current edge, and new cost
                    Logger.WriteLine("Current Node being Updated: " + nextElement + " - " + (costs[curr.Element].cost + e.Weight));
                    costs[nextElement] = new PathElement<T>(costs[nextElement].currVertex, curr, e,
                                                            costs[curr.Element].cost + e.Weight);
                }
            }
        }

        // Method to remove extraneous edges that are not part of the shortest paths
        private static void RemoveEdgesInCloud(Graph<T> graph, List<T> finalized,
                                                Dictionary<T, PathElement<T>> costs)
        {
            // Create List to store the adjacent edges of the most-recently finalized vertex
            List<Edge<T>> adjEdges;

            // Get the path element related to the most-recently finalized vertex
            PathElement<T> last = costs[finalized.Last()];

            // Get all edges that lead to or come from the last vertex
            adjEdges = graph.GetIncidentEdges(last.currVertex);

            // For each edge in the neighboring edges of the last vertex
            foreach (var e in adjEdges)
            {
                // If both vertices of the current edge are finalized
                if (finalized.Contains(e.EndVertices.start.Element) &&
                    finalized.Contains(e.EndVertices.end.Element))
                {
                    // If the previous edge of the last vertex is equal to the current edge
                    if (last.inEdge == null || !e.Equals(last.inEdge))
                    {
                        // Remove the current edge from the graph
                        Logger.WriteLine("Removed Edge: " + e.EndVertices.start.Element + " " +
                                            e.EndVertices.end.Element + " " + e.Weight);
                        graph.RemoveEdge(e);
                    }
                }
            }
        }

        // Method to find the minimum-cost vertex that is not yet finalized
        private static T FindMinCostVertex(Graph<T> graph,
                                            Dictionary<T, PathElement<T>> costs,
                                            List<T> finalized)
        {
            // Create variables to store the minimum element and value of the list of vertices
            T minElement = default(T);
            int min = int.MaxValue;

            // For each key-value pair in the dictionary of costs
            foreach ((T key, PathElement<T> val) in costs)
            {
                // If the current vertex is not finalized and the cost of the current vertex
                // and the min value are both equal the the max value
                if (!finalized.Contains(key) && val.cost == int.MaxValue && min == int.MaxValue)
                {
                    minElement = key;
                    min = val.cost;
                    continue;
                }

                // If the current vertex is not finalized and the cost of the current vertex 
                // is less than the current value of min
                if (!finalized.Contains(key) && val.cost < min)
                {
                    minElement = key;
                    min = val.cost;
                }
            }

            // Return the minimum element
            return minElement;
        }
    }
}
