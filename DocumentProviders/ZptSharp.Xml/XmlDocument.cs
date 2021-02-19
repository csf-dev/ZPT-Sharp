using System;
using System.Xml.Linq;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An implementation of <see cref="IDocument"/> which is based upon
    /// a <c>System.Xml.Linq</c> <see cref="XDocument"/>.
    /// </summary>
    public class XmlDocument : DocumentBase, ICanReplaceRootNode
    {
        INode root;

        /// <summary>
        /// Gets the native HTML Agility Pack document object.
        /// </summary>
        /// <value>The native document.</value>
        public XDocument NativeDocument { get; }

        /// <summary>
        /// Gets the root element for the current document.
        /// </summary>
        /// <returns>The root node.</returns>
        public override INode RootNode => root;

        /// <summary>
        /// Where-supported, adds a comment before the first element node in the document.  In cases where
        /// the underlying document implementation does not support this, a workaround is acceptable (such as
        /// commenting immediately inside the first node).
        /// </summary>
        public override void AddCommentToBeginningOfDocument(string commentText)
        {
            var node = new XComment(commentText);
            NativeDocument.AddFirst(node);
        }

        /// <summary>
        /// Replaces the root element node of the DOM using the specified <paramref name="replacement"/>.
        /// </summary>
        /// <param name="replacement">The replacement node.</param>
        public void ReplaceRootNode(INode replacement)
        {
            if (replacement == null)
                throw new ArgumentNullException(nameof(replacement));
            var nativeReplacement = ((XmlNode)replacement).NativeNode;

            NativeDocument.Root.ReplaceWith(nativeReplacement);
            root = replacement;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDocument"/> class.
        /// </summary>
        /// <param name="document">The native XML document.</param>
        /// <param name="source">The source info for the document.</param>
        public XmlDocument(XDocument document, IDocumentSourceInfo source) : base(source)
        {
            NativeDocument = document ?? throw new ArgumentNullException(nameof(document));

            var nativeRoot = NativeDocument.Root;
            var src = new NodeSourceInfo(Source, XmlNode.GetLineNumber(nativeRoot));
            root = new XmlNode(nativeRoot, this, sourceInfo: src);
        }
    }
}
