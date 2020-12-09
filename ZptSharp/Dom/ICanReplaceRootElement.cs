using System;
namespace ZptSharp.Dom
{
    /// <summary>
    /// Applied to a <see cref="IDocument"/> which may replace the root element of its DOM.
    /// </summary>
    public interface ICanReplaceRootElement
    {
        /// <summary>
        /// Replaces the root element of the DOM using the specified <paramref name="replacement"/>.
        /// </summary>
        /// <param name="replacement">The replacement element.</param>
        void ReplaceRootElement(INode replacement);
    }
}
