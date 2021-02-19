using System;
using System.Collections.Generic;
using AngleSharp.Html.Dom;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An implementation of <see cref="IDocument"/> which is based upon
    /// an AngleSharp <see cref="IHtmlDocument"/>.
    /// </summary>
    public class AngleSharpDocument : DocumentBase
    {
        readonly INode root;

        /// <summary>
        /// Gets the native AngleSharp document object.
        /// </summary>
        /// <value>The native document.</value>
        public IHtmlDocument NativeDocument { get; }

        /// <summary>
        /// Gets the root node for the current document.
        /// </summary>
        /// <returns>The root node.</returns>
        public override INode RootNode => root;

        /// <summary>
        /// Where-supported, adds a comment before the first node in the document.  In cases where
        /// the underlying document implementation does not support this, a workaround is acceptable (such as
        /// commenting immediately inside the first node).
        /// </summary>
        public override void AddCommentToBeginningOfDocument(string commentText)
        {
            var comment = NativeDocument.CreateComment(commentText);
            NativeDocument.Prepend(comment);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AngleSharpDocument"/> class.
        /// </summary>
        /// <param name="document">The native document object.</param>
        /// <param name="source">The source info for the document.</param>
        public AngleSharpDocument(IHtmlDocument document, IDocumentSourceInfo source) : base(source)
        {
            NativeDocument = document ?? throw new ArgumentNullException(nameof(document));

            var nativeRoot = NativeDocument.DocumentElement;
            var src = new NodeSourceInfo(Source, nativeRoot.SourceReference?.Position.Line);
            root = new AngleSharpNode(nativeRoot, this, sourceInfo: src);
        }
    }
}
