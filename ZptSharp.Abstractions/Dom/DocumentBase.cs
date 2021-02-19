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

        IEnumerable<INode> IHasNodes.GetChildNodes() => new[] { RootNode };

        /// <summary>
        /// Gets information which indicates the original source of the document (for example, a file path).
        /// </summary>
        /// <value>The source info.</value>
        public virtual IDocumentSourceInfo SourceInfo => Source;

        /// <summary>
        /// Where-supported, adds a comment before the first node node in the document.  In cases where
        /// the underlying document implementation does not support this, a workaround is acceptable (such as
        /// commenting immediately inside the first node).
        /// </summary>
        public abstract void AddCommentToBeginningOfDocument(string commentText);

        /// <summary>
        /// Gets the root node for the current document.
        /// </summary>
        /// <returns>The root node.</returns>
        public abstract INode RootNode { get; }

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
