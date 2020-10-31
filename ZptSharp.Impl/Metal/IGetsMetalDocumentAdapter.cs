using System;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An object which gets an instance of <see cref="MetalDocumentAdapter"/> for a specified
    /// document.
    /// </summary>
    public interface IGetsMetalDocumentAdapter
    {
        /// <summary>
        /// Gets the METAL document adapter for the specified <paramref name="document"/>.
        /// </summary>
        /// <returns>The METAL document adapter.</returns>
        /// <param name="document">A document.</param>
        MetalDocumentAdapter GetMetalDocumentAdapter(IDocument document);
    }
}
