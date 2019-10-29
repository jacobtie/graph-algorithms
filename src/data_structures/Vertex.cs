namespace graph_algorithms.data_structures
{
    /// <summary>
    /// Represents a vertex in the graph 
    /// which wraps a generic element
    /// </summary>
    /// <typeparam name="T">The type of element stored in the vertex</typeparam>
    public class Vertex<T>
    {
        /// <summary>
        /// The element wrapped by the vertex
        /// </summary>
        /// <value></value>
        public T Element { get; set; }

        /// <summary>
        /// Constructor to create a vertex wrapper around an element
        /// </summary>
        /// <param name="element">The element</param>
        public Vertex(T element)
        {
            Element = element;
        }
    }
}
