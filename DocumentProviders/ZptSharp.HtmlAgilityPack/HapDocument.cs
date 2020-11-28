using System;
using System.Collections.Generic;
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
        const string commentFormat = "<!--{0}-->";

        readonly INode root;

        /// <summary>
        /// Gets the native HTML Agility Pack document object.
        /// </summary>
        /// <value>The native document.</value>
        public HtmlDocument NativeDocument { get; }

        /// <summary>
        /// Gets the root element for the current document.
        /// </summary>
        /// <returns>The root element.</returns>
        public override INode RootElement => root;

        /// <summary>
        /// Creates and returns a new comment node.
        /// </summary>
        /// <returns>The comment node.</returns>
        /// <param name="commentText">The text for the comment.</param>
        public override INode CreateComment(string commentText)
        {
            if (commentText == null)
                throw new ArgumentNullException(nameof(commentText));

            var node = NativeDocument.CreateComment(String.Format(commentFormat, commentText));
            return new HapElement(node, this);
        }

        /// <summary>
        /// Creates and returns a new text node from the specified content.
        /// Even if the content contains valid markup, it is strictly to be treated as text.
        /// </summary>
        /// <returns>A text node.</returns>
        /// <param name="content">The text content for the node.</param>
        public override INode CreateTextNode(string content) => throw new NotImplementedException();

        /// <summary>
        /// Parses the specified text <paramref name="markup"/> and returns the resulting nodes.
        /// </summary>
        /// <returns>The parsed nodes.</returns>
        /// <param name="markup">Markup text.</param>
        public override IList<INode> ParseAsNodes(string markup) => throw new NotImplementedException();

        /// <summary>
        /// Where-supported, adds a comment before the first element node in the document.  In cases where
        /// the underlying document implementation does not support this, a workaround is acceptable (such as
        /// commenting immediately inside the first element).
        /// </summary>
        public override void AddCommentToBeginningOfDocument(string commentText)
        {
            var comment = NativeDocument.CreateComment(String.Format(commentFormat, commentText));
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
            var src = new ElementSourceInfo(Source);
            root = new HapElement(nativeRoot, this, sourceInfo: src);
        }
    }
}
