using System;
using ZptSharp.Dom;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// Default implementation of <see cref="IAddsComment"/> which adds comments before and after element nodes.
    /// </summary>
    public class Commenter : IAddsComment
    {
        /// <summary>
        /// Adds a comment immediately before the beginning of a specified <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The element before-which the comment should be added.</param>
        /// <param name="commentText">The text of the DOM comment.</param>
        public void AddCommentBefore(INode element, string commentText)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (element.ParentElement == null)
            {
                element.Document.AddCommentToBeginningOfDocument(commentText);
                return;
            }

            var elementIndex = element.ParentElement.ChildNodes.IndexOf(element);
            var comment = element.Document.CreateComment(commentText);
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
            var comment = element.Document.CreateComment(commentText);
            element.ParentElement.ChildNodes.Insert(elementIndex + 1, comment);
        }
    }
}
