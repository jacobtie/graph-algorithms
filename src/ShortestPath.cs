using graph_algorithms.data_structures;
using System.Collections.Generic;
using graph_algorithms.logging;
using System.Linq;

namespace graph_algorithms
{
    static class ShortestPath<T>
    {
        public static Dictionary<T, PathElement<T>> RunDijkstras(Graph<T> graph, Vertex<T> start)
        {
            var paths = new Graph<T>(graph);
            var finalized = new List<T>();
            var costs = new Dictionary<T, PathElement<T>>(paths.Vertices.Count);
            var currentElement = start.Element;

            Logger.WriteLine("\nOriginal Graph as Adjacency Matrix: ");
            Logger.WriteLine(graph.ToAdjacencyMatrix());

            Logger.WriteLine("Beginning Dijkstra's Algorithm... \n");
            foreach (var v in paths.Vertices)
            {
                PathElement<T> newElement;

                if (!v.Element.Equals(start.Element))
                {
                    newElement = new PathElement<T>(v, null, null, int.MaxValue);
                }
                else
                {
                    newElement = new PathElement<T>(v, null, null, 0);
                }

                costs.Add(v.Element, newElement);
            }

            UpdateCosts(paths, costs[currentElement].currVertex, costs);
            finalized.Add(currentElement);
            currentElement = FindMinCostVertex(paths, costs, finalized);
            Logger.WriteLine();

            do 
            {
                UpdateCosts(paths, costs[currentElement].currVertex, costs);
                finalized.Add(currentElement);

                RemoveEdgesInCloud(paths, finalized, costs);

                currentElement = FindMinCostVertex(paths, costs, finalized);

                Logger.WriteLine();
            }
            while(finalized.Count != paths.NumVertices);

            Logger.WriteLine("\nFinished Dijkstra's Algorithm. ");

            Logger.WriteLine("\nGraph after Extraneous Edges have been Removed: ");
            Logger.WriteLine(paths.ToAdjacencyMatrix());

            return costs;
        }

        private static void UpdateCosts(Graph<T> graph, Vertex<T> curr, 
                                        Dictionary<T, PathElement<T>> costs)
        {
            Logger.WriteLine("Current Node being Finalized: " + curr.Element + " - " + costs[curr.Element].cost);

            if (costs[curr.Element].cost == int.MaxValue)
            {
                return;
            }

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
                T nextElement; 

                if (!curr.Element.Equals(e.EndVertices.start.Element))
                {
                    nextElement = e.EndVertices.start.Element;
                }
                else
                {
                    nextElement = e.EndVertices.end.Element;
                }

                if (costs[curr.Element].cost + e.Weight < costs[nextElement].cost)
                {
                    Logger.WriteLine("Current Node being Updated: " + nextElement + " - " + (costs[curr.Element].cost + e.Weight));
                    costs[nextElement] = new PathElement<T>(costs[nextElement].currVertex, curr, e, 
                                                            costs[curr.Element].cost + e.Weight);
                }
            }
        }

        private static void RemoveEdgesInCloud(Graph<T> graph, List<T> finalized, 
                                                Dictionary<T, PathElement<T>> costs)
        {
            List<Edge<T>> adjEdges;
            
            adjEdges = graph.GetIncidentEdges(graph.Vertices.Find(vertex => 
                                                vertex.Element.Equals(finalized.Last())));

            PathElement<T> last = costs[finalized.Last()];

            foreach (var e in adjEdges)
            {
                if (finalized.Contains(e.EndVertices.start.Element) && 
                    finalized.Contains(e.EndVertices.end.Element))
                {
                    if (last.inEdge == null || !e.Equals(last.inEdge))
                    {
                        Logger.WriteLine("Removed Edge: " + e.EndVertices.start.Element + " " + 
                                            e.EndVertices.end.Element + " " + e.Weight);
                        graph.RemoveEdge(e);
                    }
                }
            }
        }

        private static T FindMinCostVertex(Graph<T> graph, 
                                            Dictionary<T, PathElement<T>> costs, 
                                            List<T> finalized)
        {
            T minElement = default(T);
            int min = int.MaxValue;

            foreach ((T key, PathElement<T> val) in costs)
            {
                if ((val.cost < min && !finalized.Contains(key)) || 
                    (val.cost == int.MaxValue && min == int.MaxValue))
                {
                    minElement = key;
                    min = val.cost;
                }
            }

            return minElement;
        }
    }
}
