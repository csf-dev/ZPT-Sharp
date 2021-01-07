using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which may read &amp; write <see cref="IDocument"/> instances to/from
    /// streams.  This is the core interface of "a document provider".
    /// </summary>
    /// <remarks>
    /// <para>
    /// A document provider is a required add-on for ZptSharp which allows it to read and write
    /// markup documents.
    /// On its own, the ZptSharp core deals only in an abstraction, the <see cref="IDocument"/> interface
    /// and its members.
    /// The document provider is how a stream may be read as a document and how a modified/rendered
    /// document may be written/saved back to a stream.
    /// </para>
    /// <para>
    /// Implementations of this interface typically use other/3rd-party libraries to read/write the
    /// markup, and "bridge the gap" between the specific document reading/writing API, and the abstractions
    /// which the ZptSharp core uses.
    /// </para>
    /// <para>
    /// In order to have a working/usable ZptSharp environment, at least one document provider
    /// must be registered with dependency injection and must also be registered for use with
    /// <see cref="IRegistersDocumentReaderWriter"/>.
    /// This registration is usually accomplished via extension methods for
    /// <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> and
    /// <see cref="System.IServiceProvider"/> and does not usually need to be done manually.
    /// </para>
    /// </remarks>
    /// <seealso cref="IDocument"/>
    /// <seealso cref="INode"/>
    /// <seealso cref="IAttribute"/>
    /// <seealso cref="IRegistersDocumentReaderWriter"/>
    public interface IReadsAndWritesDocument
    {
        /// <summary>
        /// Gets a value which indicates whether or not the current instance may be used to read &amp; write documents
        /// which have the specified filename.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is the mechanism by which an <see cref="IGetsZptDocumentRendererForFilePath"/>
        /// (and ultimately an <see cref="IRendersZptFile"/>) selects an appropriate document provider implementation
        /// for a specified file.
        /// </para>
        /// <para>
        /// This enables document providers to be used with the strategy pattern -
        /// <see href="https://en.wikipedia.org/wiki/Strategy_pattern" /> - interrogating each of the available providers
        /// to determine which can perform the required task.
        /// </para>
        /// </remarks>
        /// <returns><c>true</c>, if this instance may be used for the specified file name/file path; <c>false</c> if not.</returns>
        /// <param name="filenameOrPath">The file name or file path of a ZPT document.</param>
        /// <seealso cref="IGetsZptDocumentRendererForFilePath"/>
        /// <seealso cref="IRendersZptFile"/>
        bool CanReadWriteForFilename(string filenameOrPath);

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
