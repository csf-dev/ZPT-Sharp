namespace ZptSharp.Dom
{
    /// <summary>
    /// Applied to a <see cref="IDocument"/> which may replace the root node of its DOM.
    /// </summary>
    public interface ICanReplaceRootNode
    {
        /// <summary>
        /// Replaces the root node of the DOM using the specified <paramref name="replacement"/>.
        /// </summary>
        /// <param name="replacement">The replacement node.</param>
        void ReplaceRootNode(INode replacement);
    }
}
