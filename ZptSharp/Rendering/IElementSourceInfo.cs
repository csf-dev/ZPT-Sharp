using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// Information about the source of a DOM element, including the line number
    /// at which the element appears.
    /// </summary>
    public interface IElementSourceInfo : IEquatable<IElementSourceInfo>
    {
        /// <summary>
        /// Gets source information about the document where this element originated.
        /// </summary>
        /// <value>The document.</value>
        IDocumentSourceInfo Document { get; }

        /// <summary>
        /// Gets the line number for the beginning of the element.
        /// </summary>
        /// <value>The line number.</value>
        int? LineNumber { get; }

        /// <summary>
        /// Creates a child <see cref="IElementSourceInfo"/> which uses the same
        /// document as the current instance, but the specified line number instead.
        /// </summary>
        /// <returns>The child source info.</returns>
        /// <param name="lineNumber">A line number.</param>
        IElementSourceInfo CreateChild(int? lineNumber = null);
    }
}
