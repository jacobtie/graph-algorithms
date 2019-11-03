namespace graph_algorithms.data_structures
{
        struct PathElement<T>
        {
            public Vertex<T> currVertex;
            public Vertex<T> prevVertex;
            public Edge<T> inEdge;
            public int cost;

            public PathElement(Vertex<T> c, Vertex<T> p, Edge<T> e, int cost)
            {
                currVertex = c;
                prevVertex = p;
                inEdge = e;
                this.cost = cost;
            }
        };
}

        