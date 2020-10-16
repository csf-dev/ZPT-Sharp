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
    }
}
