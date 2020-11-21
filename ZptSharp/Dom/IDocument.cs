namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstraction for a DOM document.
    /// </summary>
    public interface IDocument : IHasDocumentSourceInfo, IHasElements
    {
        /// <summary>
        /// Gets the root element for the current document.
        /// </summary>
        /// <returns>The root element.</returns>
        INode RootElement { get; }

        /// <summary>
        /// Creates and returns a new comment node.
        /// </summary>
        /// <returns>The comment node.</returns>
        /// <param name="commentText">The text for the comment.</param>
        INode CreateComment(string commentText);
    }
}
