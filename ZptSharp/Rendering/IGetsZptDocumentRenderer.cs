using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// An object which may get an instance of <see cref="IRendersZptDocument"/> for a
    /// specified implementation of <see cref="IReadsAndWritesDocument"/>.
    /// </summary>
    public interface IGetsZptDocumentRenderer
    {
        /// <summary>
        /// Gets the document renderer.
        /// </summary>
        /// <returns>The document renderer.</returns>
        /// <param name="readerWriter">A specific document reader/writer implementation.</param>
        IRendersZptDocument GetDocumentRenderer(IReadsAndWritesDocument readerWriter = null);
    }
}
