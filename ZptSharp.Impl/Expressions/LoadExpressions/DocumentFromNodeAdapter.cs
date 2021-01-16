using System.Collections.Generic;
using ZptSharp.Dom;
using ZptSharp.Rendering;

namespace ZptSharp.Expressions.LoadExpressions
{
    /// <summary>
    /// A 'fake' implementation of <see cref="IDocument"/> which really just wraps an <see cref="INode"/>
    /// and allows that node to be used as if it were a complete document.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In this case, the node used to construct instances of this type becomes the root node of the resulting
    /// 'document'.  All other functionality of this interface is redirected into the node.
    /// </para>
    /// </remarks>
    public class DocumentFromNodeAdapter : IDocument
    {
        readonly INode node;

        /// <summary>
        /// Gets the root node for the current document.
        /// </summary>
        /// <returns>The root node.</returns>
        public INode RootNode => node;

        /// <summary>
        /// Gets information which indicates the original source of the document (for example, a file path).
        /// </summary>
        /// <value>The source info.</value>
        public IDocumentSourceInfo SourceInfo => node.SourceInfo.Document;

        /// <summary>
        /// Where-supported, adds a comment before the first node node in the document.  In cases where
        /// the underlying document implementation does not support this, a workaround is acceptable (such as
        /// commenting immediately inside the first node).
        /// </summary>
        public void AddCommentToBeginningOfDocument(string commentText)
        {
            var comment = node.CreateComment(commentText);
            if (node.ParentNode == null)
            {
                node.ChildNodes.Insert(0, comment);
            }
            else
            {
                var index = node.ParentNode.ChildNodes.IndexOf(node);
                node.ParentNode.ChildNodes.Insert(index, comment);
            }
        }

        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <returns>The child nodes.</returns>
        public IEnumerable<INode> GetChildNodes() => new[] { node };

        /// <summary>
        /// Initializes a new instance of <see cref="DocumentFromNodeAdapter"/>.
        /// </summary>
        /// <param name="node">The node upon which the current instance is based.</param>
        public DocumentFromNodeAdapter(INode node)
        {
            this.node = node ?? throw new System.ArgumentNullException(nameof(node));
        }
    }
}