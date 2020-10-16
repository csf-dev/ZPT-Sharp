using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which may read &amp; write <see cref="IDocument"/> instances to/from
    /// streams.
    /// </summary>
    public interface IReadsAndWritesDocument
    {
        /// <summary>
        /// Gets a document instance from the specified input stream.
        /// </summary>
        /// <returns>A task which provides the document which has been read.</returns>
        /// <param name="stream">A stream containing the source of the document.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="sourceInfo">Information which identifies the source of the document.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        Task<IDocument> GetDocumentAsync(Stream stream,
                                         RenderingConfig config,
                                         IDocumentSourceInfo sourceInfo = null,
                                         CancellationToken token = default);

        /// <summary>
        /// Writes the specified document to a specified output stream.
        /// </summary>
        /// <returns>A task which provides the output stream.</returns>
        /// <param name="document">The document to write.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        Task<Stream> WriteDocumentAsync(IDocument document,
                                        RenderingConfig config,
                                        CancellationToken token = default);
    }
}
