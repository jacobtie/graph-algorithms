namespace graph_algorithms.data_structures
{
        struct PathElement<T>
        {
            public Vertex<T> currVertex;
            public Vertex<T> prevVertex;
            public int cost;

            public PathElement(Vertex<T> c, Vertex<T> p, int cost)
            {
                currVertex = c;
                prevVertex = p;
                this.cost = cost;
            }
        };
}

        