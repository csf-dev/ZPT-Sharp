using System;
using HtmlAgilityPack;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An implementation of <see cref="IDocument"/> which is based upon
    /// an HTML Agility Pack <see cref="HtmlDocument"/>.
    /// </summary>
    public class HapDocument : DocumentBase
    {
        readonly INode root;

        /// <summary>
        /// Gets the native HTML Agility Pack document object.
        /// </summary>
        /// <value>The native document.</value>
        public HtmlDocument NativeDocument { get; }

        /// <summary>
        /// Gets the root node for the current document.
        /// </summary>
        /// <returns>The root node.</returns>
        public override INode RootNode => root;

        /// <summary>
        /// Where-supported, adds a comment before the first node node in the document.  In cases where
        /// the underlying document implementation does not support this, a workaround is acceptable (such as
        /// commenting immediately inside the first node).
        /// </summary>
        public override void AddCommentToBeginningOfDocument(string commentText)
        {
            var comment = NativeDocument.CreateComment(String.Format(HapNode.CommentFormat, commentText));
            NativeDocument.DocumentNode.PrependChild(comment);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HapDocument"/> class.
        /// </summary>
        /// <param name="document">The native document object.</param>
        /// <param name="source">The source info for the document.</param>
        public HapDocument(HtmlDocument document, IDocumentSourceInfo source) : base(source)
        {
            NativeDocument = document ?? throw new ArgumentNullException(nameof(document));

            var nativeRoot = NativeDocument.DocumentNode;
            var src = new NodeSourceInfo(Source);
            root = new HapNode(nativeRoot, this, sourceInfo: src);
        }
    }
}
