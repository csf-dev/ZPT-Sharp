using System.Collections.Generic;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstract base class for an <see cref="IDocument"/>, containing functionality
    /// which is neutral to the specific implementation.
    /// </summary>
    public abstract class DocumentBase : IDocument
    {
        /// <summary>
        /// A field to get the document source info.
        /// </summary>
        protected readonly IDocumentSourceInfo Source;

        IEnumerable<INode> IHasElements.GetChildElements() => new[] { RootElement };

        /// <summary>
        /// Gets information which indicates the original source of the document (for example, a file path).
        /// </summary>
        /// <value>The source info.</value>
        public virtual IDocumentSourceInfo SourceInfo => Source;

        /// <summary>
        /// Where-supported, adds a comment before the first element node in the document.  In cases where
        /// the underlying document implementation does not support this, a workaround is acceptable (such as
        /// commenting immediately inside the first element).
        /// </summary>
        public abstract void AddCommentToBeginningOfDocument(string commentText);

        /// <summary>
        /// Gets the root element for the current document.
        /// </summary>
        /// <returns>The root element.</returns>
        public abstract INode RootElement { get; }

        /// <summary>
        /// Creates and returns a new comment node.
        /// </summary>
        /// <returns>The comment node.</returns>
        /// <param name="commentText">The text for the comment.</param>
        public abstract INode CreateComment(string commentText);

        /// <summary>
        /// Creates and returns a new text node from the specified content.
        /// Even if the content contains valid markup, it is strictly to be treated as text.
        /// </summary>
        /// <returns>A text node.</returns>
        /// <param name="content">The text content for the node.</param>
        public abstract INode CreateTextNode(string content);

        /// <summary>
        /// Parses the specified text <paramref name="markup"/> and returns the resulting nodes.
        /// </summary>
        /// <returns>The parsed nodes.</returns>
        /// <param name="markup">Markup text.</param>
        public abstract IList<INode> ParseAsNodes(string markup);

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentBase"/> class.
        /// </summary>
        /// <param name="source">The source info for the document.</param>
        protected DocumentBase(IDocumentSourceInfo source)
        {
            Source = source ?? throw new System.ArgumentNullException(nameof(source));
        }
    }
}
