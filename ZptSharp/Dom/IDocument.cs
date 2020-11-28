using System.Collections.Generic;

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
        /// Where-supported, adds a comment before the first element node in the document.  In cases where
        /// the underlying document implementation does not support this, a workaround is acceptable (such as
        /// commenting immediately inside the first element).
        /// </summary>
        void AddCommentToBeginningOfDocument(string commentText);

        /// <summary>
        /// Creates and returns a new comment node.
        /// </summary>
        /// <returns>The comment node.</returns>
        /// <param name="commentText">The text for the comment.</param>
        INode CreateComment(string commentText);

        /// <summary>
        /// Creates and returns a new text node from the specified content.
        /// Even if the content contains valid markup, it is strictly to be treated as text.
        /// </summary>
        /// <returns>A text node.</returns>
        /// <param name="content">The text content for the node.</param>
        INode CreateTextNode(string content);

        /// <summary>
        /// Parses the specified text <paramref name="markup"/> and returns the resulting nodes.
        /// </summary>
        /// <returns>The parsed nodes.</returns>
        /// <param name="markup">Markup text.</param>
        IList<INode> ParseAsNodes(string markup);
    }
}
