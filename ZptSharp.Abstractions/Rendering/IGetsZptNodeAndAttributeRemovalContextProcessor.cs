namespace ZptSharp.Rendering
{
    /// <summary>
    /// An object which gets an implementation of <see cref="IProcessesExpressionContext"/> which will remove
    /// ZPT nodes and attributes from a DOM context.
    /// </summary>
    public interface IGetsZptNodeAndAttributeRemovalContextProcessor
    {
        /// <summary>
        /// Gets the node &amp; attribute removal processor.
        /// </summary>
        /// <returns>The node and attribute removal processor.</returns>
        IProcessesExpressionContext GetNodeAndAttributeRemovalProcessor();
    }
}
