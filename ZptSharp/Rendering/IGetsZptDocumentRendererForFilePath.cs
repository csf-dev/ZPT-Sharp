using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// An object which may get an instance of <see cref="IRendersZptDocument"/> for a
    /// specified file path.
    /// </summary>
    public interface IGetsZptDocumentRendererForFilePath
    {
        /// <summary>
        /// Gets the document renderer.
        /// </summary>
        /// <returns>The document renderer.</returns>
        /// <param name="filePath">The path to a file which would be rendered by the renderer.</param>
        IRendersZptDocument GetDocumentRenderer(string filePath);
    }
}
