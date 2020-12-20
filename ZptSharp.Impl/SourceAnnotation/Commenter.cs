using System;
using Microsoft.Extensions.Logging;
using ZptSharp.Dom;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// Default implementation of <see cref="IAddsComment"/> which adds comments before and after node nodes.
    /// </summary>
    public class Commenter : IAddsComment
    {
        readonly ILogger logger;

        /// <summary>
        /// Adds a comment immediately before the beginning of a specified <paramref name="node"/>.
        /// </summary>
        /// <param name="node">The node before-which the comment should be added.</param>
        /// <param name="commentText">The text of the DOM comment.</param>
        public void AddCommentBefore(INode node, string commentText)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (logger.IsEnabled(LogLevel.Trace))
                logger.LogTrace("Adding comment before node: {node}, parent: {parent}", node, node.ParentNode);

            if (node.ParentNode == null)
            {
                node.Document.AddCommentToBeginningOfDocument(commentText);
                return;
            }

            var nodeIndex = node.ParentNode.ChildNodes.IndexOf(node);
            var comment = node.CreateComment(commentText);
            node.ParentNode.ChildNodes.Insert(nodeIndex, comment);
        }

        /// <summary>
        /// Adds a comment immediately after the end of a specified <paramref name="node"/>.
        /// </summary>
        /// <param name="node">The node after-which the comment should be added.</param>
        /// <param name="commentText">The text of the DOM comment.</param>
        public void AddCommentAfter(INode node, string commentText)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (node.ParentNode == null)
                throw new ArgumentException(Resources.ExceptionMessage.ElementMustHaveAParent, nameof(node));

            var nodeIndex = node.ParentNode.ChildNodes.IndexOf(node);
            var comment = node.CreateComment(commentText);
            node.ParentNode.ChildNodes.Insert(nodeIndex + 1, comment);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Commenter"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public Commenter(ILogger<Commenter> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
