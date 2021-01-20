using ZptSharp.Dom;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// An object which adds comments to the DOM, either before or after a specified node.
    /// </summary>
    public interface IAddsComment
    {
        /// <summary>
        /// Adds a comment immediately before the beginning of a specified <paramref name="node"/>.
        /// </summary>
        /// <param name="node">The node before-which the comment should be added.</param>
        /// <param name="commentText">The text of the DOM comment.</param>
        void AddCommentBefore(INode node, string commentText);

        /// <summary>
        /// Adds a comment immediately after the end of a specified <paramref name="node"/>.
        /// </summary>
        /// <param name="node">The node after-which the comment should be added.</param>
        /// <param name="commentText">The text of the DOM comment.</param>
        void AddCommentAfter(INode node, string commentText);
    }
}
