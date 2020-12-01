using System;
using Microsoft.Extensions.Logging;
using ZptSharp.Dom;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// Default implementation of <see cref="IAddsComment"/> which adds comments before and after element nodes.
    /// </summary>
    public class Commenter : IAddsComment
    {
        readonly ILogger logger;

        /// <summary>
        /// Adds a comment immediately before the beginning of a specified <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The element before-which the comment should be added.</param>
        /// <param name="commentText">The text of the DOM comment.</param>
        public void AddCommentBefore(INode element, string commentText)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (logger.IsEnabled(LogLevel.Trace))
                logger.LogTrace("Adding comment before element: {element}, parent: {parent}", element, element.ParentElement);

            if (element.ParentElement == null)
            {
                element.Document.AddCommentToBeginningOfDocument(commentText);
                return;
            }

            var elementIndex = element.ParentElement.ChildNodes.IndexOf(element);
            var comment = element.CreateComment(commentText);
            element.ParentElement.ChildNodes.Insert(elementIndex, comment);
        }

        /// <summary>
        /// Adds a comment immediately after the end of a specified <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The element after-which the comment should be added.</param>
        /// <param name="commentText">The text of the DOM comment.</param>
        public void AddCommentAfter(INode element, string commentText)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.ParentElement == null)
                throw new ArgumentException(Resources.ExceptionMessage.ElementMustHaveAParent, nameof(element));

            var elementIndex = element.ParentElement.ChildNodes.IndexOf(element);
            var comment = element.CreateComment(commentText);
            element.ParentElement.ChildNodes.Insert(elementIndex + 1, comment);
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
