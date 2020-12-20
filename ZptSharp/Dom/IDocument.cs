namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstraction for a DOM document.
    /// </summary>
    public interface IDocument : IHasDocumentSourceInfo, IHasNodes
    {
        /// <summary>
        /// Gets the root node for the current document.
        /// </summary>
        /// <returns>The root node.</returns>
        INode RootNode { get; }

        /// <summary>
        /// Where-supported, adds a comment before the first node node in the document.  In cases where
        /// the underlying document implementation does not support this, a workaround is acceptable (such as
        /// commenting immediately inside the first node).
        /// </summary>
        void AddCommentToBeginningOfDocument(string commentText);
    }
}
