using System.IO;
using System.Collections.Generic;
using graph_algorithms.logging;
using graph_algorithms.data_structures;
using System;

namespace graph_algorithms.data_structures
{
    class DisjointSet<T>
    {
        private Dictionary<T, T> parents;

        // Constructor to create a disjoint set
        public DisjointSet(List<Vertex<T>> vertices)
        {
            // Create new dictionary of parents
            parents = new Dictionary<T, T>();

            // Populate the dictionary with single-vertex sets
            foreach (var v in vertices)
            {
                // Each vertex is a parent of itself
                parents.Add(v.Element, v.Element);
            }

            Logger.WriteLine("Original List of Roots for the Set of each Vertex: ");
            Logger.WriteLine("---------------------------------");
            Logger.WriteLine(ToString());
        }

        // Method to recursively find the root for a set of a particular vertex
        public T Find(T element)
        {
            // If the given element is not the parent of itself (root of the set)
            if (!parents[element].Equals(element))
            {
                // Call the Find method again until the root of the set is found
                return Find(parents[element]);
            }

            // Return the root of the set
            return parents[element];
        }

        // Method to combine the two sets if they are different for each end point of the edge 
        // or simply leave both end points in the same set
        public bool Union(Edge<T> edge)
        {
            // Ged the end points of the edge
            var element1 = edge.EndVertices.start.Element;
            var element2 = edge.EndVertices.end.Element;

            // Get the roots of the respective sets of each end point
            var root1 = Find(element1);
            var root2 = Find(element2);

            // If both end points are not part of the same set
            if (!root1.Equals(root2))
            {
                // Combine the two sets 
                parents[root2] = root1;

                Logger.WriteLine("Added Edge to the Graph: " + element1 + " " + element2 + 
                                " " + edge.Weight);
                Logger.WriteLine("List of Roots for the Set of each Vertex: ");
                Logger.WriteLine("---------------------------------");
                Logger.WriteLine(ToString());
                Logger.WriteLine();

                // Return false to indicate that each end point is part of its own set
                return false;
            }

            // Return true to indicate that both end points are part of the same set
            return true;
        }

        public override string ToString()
        {
            string sets = "|Vertex\t|Root\t|\n";

            foreach((var v, var p) in parents)
            {
                sets += "|" + v + " \t|" + Find(v) + "\t|\n";
            }

            return sets;
        }
    }


}