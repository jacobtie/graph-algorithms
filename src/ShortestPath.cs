using System;
using System.IO;
using graph_algorithms.logging;
using graph_algorithms.data_structures;
using System.Collections.Generic;
using System.Linq;

namespace graph_algorithms
{
    static class ShortestPath<T>
    {
        public static Graph<T> RunDijkstras(Graph<T> graph, Vertex<T> start)
        {
            var paths = new Graph<T>(graph);
            var finalized = new List<T>();
            var costs = new Dictionary<T, int>(paths.Vertices.Count);
            Vertex<T>? currentVertex = start;

            Console.WriteLine(graph.ToAdjacencyMatrix());

            foreach (var v in paths.Vertices)
            {
                costs.Add(v.Element, int.MaxValue);
            }

            costs[currentVertex.Element] = 0;
            UpdateCosts(paths, currentVertex, costs);
            finalized.Add(currentVertex.Element);
            currentVertex = FindMinCostVertex(paths, costs, finalized);

            do 
            {
                UpdateCosts(paths, currentVertex, costs);
                finalized.Add(currentVertex.Element);

                RemoveEdgesInCloud(paths, finalized, costs);

                currentVertex = FindMinCostVertex(paths, costs, finalized);
            }
            while(currentVertex != null);

            Console.WriteLine(paths.ToAdjacencyMatrix());

            return paths;
        }

        private static void UpdateCosts(Graph<T> graph, Vertex<T> curr, 
                                        Dictionary<T, int> costs)
        {
            List<Edge<T>> adjEdges;

            if (graph.GraphType == DirectedType.Undirected)
            {
                adjEdges = graph.GetIncidentEdges(curr);
            }
            else
            {
                adjEdges = graph.GetOutEdges(curr);
            }

            foreach (var e in adjEdges)
            {
                var currElement = e.EndVertices.end.Element;
                var prevElement = e.EndVertices.start.Element;

                if ((costs[prevElement] + e.Weight) < costs[currElement])
                {
                    costs[currElement] = costs[prevElement] + e.Weight;
                }
            }
        }

        private static void RemoveEdgesInCloud(Graph<T> graph, List<T> finalized, 
                                                Dictionary<T, int> costs)
        {
            List<Edge<T>> adjEdges;
            
            if (graph.GraphType == DirectedType.Undirected)
            {
                adjEdges = graph.GetIncidentEdges(graph.Vertices.Find(vertex => 
                                                vertex.Element.Equals(finalized.Last())));
            }
            else
            {
                adjEdges = graph.GetInEdges(graph.Vertices.Find(vertex => 
                                                vertex.Element.Equals(finalized.Last())));
            }

            foreach (var edge in adjEdges)
            {
                T element; 

                if (!finalized.Last().Equals(edge.EndVertices.start.Element))
                {
                    element = edge.EndVertices.start.Element;
                }
                else
                {
                    element = edge.EndVertices.end.Element;
                }

                if (finalized.Contains(element) &&
                    (edge.Weight + costs[element] > costs[finalized.Last()]))
                {
                    Console.WriteLine();
                    Console.WriteLine(finalized.Last());
                    Console.WriteLine(element);
                    Console.WriteLine(costs[finalized.Last()]);
                    Console.WriteLine(edge.Weight + costs[element]);

                    graph.RemoveEdge(edge);
                }
            }
        }

        private static Vertex<T>? FindMinCostVertex(Graph<T> graph, Dictionary<T, int> costs, 
                                                    List<T> finalized)
        {
            T minKey = default(T);
            int min = int.MaxValue;

            foreach ((T key, int val) in costs)
            {
                if ((val < min && !finalized.Contains(key)) || 
                    (val == int.MaxValue && min == int.MaxValue))
                {
                    minKey = key;
                    min = val;
                }
            }

            return graph.Vertices.Find(vertex => vertex.Element.Equals(minKey));
        }
    }
}
