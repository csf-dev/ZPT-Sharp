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
    }
}
