namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which can be used to omit a specified node.  Omitting an element node is
    /// essentially equivalent to replacing that element node with its children.
    /// </summary>
    public interface IOmitsNode
    {
        /// <summary>
        /// Omits the specified element node from its DOM.  The node's parent and children will
        /// still be a part of the DOM, but the element node itself will be removed.
        /// </summary>
        /// <param name="node">The element node to omit.</param>
        void Omit(INode node);
    }
}
