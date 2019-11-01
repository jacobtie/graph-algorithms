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

            foreach (var v in paths.Vertices)
            {
                PathElement<T> newElement;

                if (!v.Element.Equals(start.Element))
                {
                    newElement = new PathElement<T>(v, null, int.MaxValue);
                }
                else
                {
                    newElement = new PathElement<T>(v, null, 0);
                }

                costs.Add(v.Element, newElement);
            }

            UpdateCosts(paths, costs[currentElement].currVertex, costs);
            finalized.Add(currentElement);
            currentElement = FindMinCostVertex(paths, costs, finalized);

            do 
            {
                UpdateCosts(paths, costs[currentElement].currVertex, costs);
                finalized.Add(currentElement);

                RemoveEdgesInCloud(paths, finalized, costs);

                currentElement = FindMinCostVertex(paths, costs, finalized);
            }
            while(finalized.Count != paths.NumVertices);

            Logger.WriteLine("\nGraph after Extraneous Edges have been Removed: ");
            Logger.WriteLine(paths.ToAdjacencyMatrix());

            return costs;
        }

        private static void UpdateCosts(Graph<T> graph, Vertex<T> curr, 
                                        Dictionary<T, PathElement<T>> costs)
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

                if (costs[prevElement].cost + e.Weight < costs[currElement].cost)
                {
                    costs[currElement] = new PathElement<T>(costs[currElement].currVertex, 
                                                            costs[prevElement].currVertex, 
                                                            costs[prevElement].cost + e.Weight);
                }
            }
        }

        private static void RemoveEdgesInCloud(Graph<T> graph, List<T> finalized, 
                                                Dictionary<T, PathElement<T>> costs)
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

            PathElement<T> last = costs[finalized.Last()];

            foreach (var edge in adjEdges)
            {
                T element; 

                if (!last.currVertex.Element.Equals(edge.EndVertices.start.Element))
                {
                    element = edge.EndVertices.start.Element;
                }
                else
                {
                    element = edge.EndVertices.end.Element;
                }

                if (finalized.Contains(element) &&
                    (edge.Weight + costs[element].cost > costs[last.currVertex.Element].cost || 
                    (edge.Weight + costs[element].cost == costs[last.currVertex.Element].cost && 
                    !element.Equals(last.prevVertex.Element))))
                {
                    graph.RemoveEdge(edge);
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
