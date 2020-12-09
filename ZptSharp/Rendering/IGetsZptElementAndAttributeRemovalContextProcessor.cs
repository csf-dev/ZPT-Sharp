namespace ZptSharp.Rendering
{
    /// <summary>
    /// An object which gets an implementation of <see cref="IProcessesExpressionContext"/> which will remove
    /// ZPT elements and attributes from a DOM context.
    /// </summary>
    public interface IGetsZptElementAndAttributeRemovalContextProcessor
    {
        /// <summary>
        /// Gets the element &amp; attribute removal processor.
        /// </summary>
        /// <returns>The element and attribute removal processor.</returns>
        IProcessesExpressionContext GetElementAndAttributeRemovalProcessor();
    }
}
