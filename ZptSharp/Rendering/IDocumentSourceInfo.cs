using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// Information about the source of a document, such as whether it came from a file, an
    /// abstract location or an unknown source.
    /// </summary>
    public interface IDocumentSourceInfo : IEquatable<IDocumentSourceInfo>
    {
        /// <summary>
        /// Gets the name of the source.
        /// </summary>
        /// <value>The name of the document source.</value>
        string Name { get; }
    }
}
