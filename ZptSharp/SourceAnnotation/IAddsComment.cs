using ZptSharp.Dom;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// An object which adds comments to the DOM, either before or after a specified element.
    /// </summary>
    public interface IAddsComment
    {
        /// <summary>
        /// Adds a comment immediately before the beginning of a specified <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The element before-which the comment should be added.</param>
        /// <param name="commentText">The text of the DOM comment.</param>
        void AddCommentBefore(INode element, string commentText);

        /// <summary>
        /// Adds a comment immediately after the end of a specified <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The element after-which the comment should be added.</param>
        /// <param name="commentText">The text of the DOM comment.</param>
        void AddCommentAfter(INode element, string commentText);
    }
}
