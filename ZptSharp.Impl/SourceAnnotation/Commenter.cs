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
            // TODO: Write this impl!
        }

        /// <summary>
        /// Adds a comment immediately after the end of a specified <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The element after-which the comment should be added.</param>
        /// <param name="commentText">The text of the DOM comment.</param>
        public void AddCommentAfter(INode element, string commentText)
        {
            // TODO: Write this impl!
        }
    }
}
