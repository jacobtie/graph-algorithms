namespace graph_algorithms.data_structures
{
    public class DFSVertex<T>
    {
        public Vertex<T> Vertex { get; set; }
        public DFSColor Color { get; set; }

        public DFSVertex(Vertex<T> vertex, DFSColor color)
        {
            Vertex = vertex;
            Color = color;
        }
    }

    public enum DFSColor
    {
        White,
        Gray,
        Black,
    }
}
