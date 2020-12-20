namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which can be used to omit a specified node.  Omitting an node node is
    /// essentially equivalent to replacing that node node with its children.
    /// </summary>
    public interface IOmitsNode
    {
        /// <summary>
        /// Omits the specified node node from its DOM.  The node's parent and children will
        /// still be a part of the DOM, but the node node itself will be removed.
        /// </summary>
        /// <param name="node">The node node to omit.</param>
        void Omit(INode node);
    }
}
