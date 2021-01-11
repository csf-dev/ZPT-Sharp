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
    /// A document provider is an add-on (albeit a required one) which allows ZptSharp to read &amp; write
    /// markup documents.
    /// The ZptSharp core manipulates DOM documents via an abstraction; this is the <see cref="IDocument"/>
    /// interface and related types.
    /// Implementations of this interface allow reading/writing those abstract documents from/to instances
    /// of <see cref="Stream"/>.
    /// In practical terms, this is the "load" and "save" logic of ZptSharp.
    /// </para>
    /// <para>
    /// In order to have a working/usable ZptSharp environment, at least one document provider
    /// must be registered with dependency injection.  This should be performed during application
    /// start-up, using extension methods for <see cref="Hosting.IBuildsHostingEnvironment"/>.
    /// </para>
    /// <para>
    /// Document provider implementations typically use 3rd-party libraries/APIs to perform a native read &amp;
    /// write of the DOM documents.  They then provide their own implementations of <see cref="IDocument"/>,
    /// <see cref="INode"/> &amp; <see cref="IAttribute"/> which wrap the DOM nodes returned by that 3rd-party
    /// implementation.  This is essentially an application of the <see href="https://en.wikipedia.org/wiki/Adapter_pattern"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="IDocument"/>
    /// <seealso cref="INode"/>
    /// <seealso cref="IAttribute"/>
    /// <seealso cref="Hosting.IBuildsHostingEnvironment"/>
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
        /// Gets a document instance from the specified input stream which contains text markup.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is used to 'load' a ZptSharp <see cref="IDocument"/> from a <paramref name="stream"/>.
        /// Streams are used rather than files so that ZptSharp is not limited to only loading documents
        /// from files on disk.
        /// In most circumstances the <paramref name="sourceInfo"/> will have been specified and this
        /// object will indicate the source of the stream, which should be passed to the returned document.
        /// </para>
        /// <para>
        /// The API of this method is asynchronous (returning a task) although it is recognised that not all
        /// implementations will fully support asynchronous loading of documents.  This also means that there is
        /// no certainty that implementations will honour the usage of the <paramref name="token"/> to cancel/abort
        /// the operation before it completes.
        /// </para>
        /// <para>
        /// Finally, implementations should honour the rendering <paramref name="config"/> passed to this method,
        /// but are permitted not to honour settings which are either irrelevant or which cannot be supported.
        /// </para>
        /// </remarks>
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
        /// Writes the document to text markup and returns a stream containing the rendered document.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is used to 'save' a ZptSharp <see cref="IDocument"/> to a <see cref="Stream"/>.
        /// Streams are used rather than files so that ZptSharp is not limited to only saving documents
        /// as files on disk.
        /// </para>
        /// <para>
        /// The API of this method is asynchronous (returning a task) although it is recognised that not all
        /// implementations will fully support asynchronous writing of documents.  This also means that there is
        /// no certainty that implementations will honour the usage of the <paramref name="token"/> to cancel/abort
        /// the operation before it completes.
        /// </para>
        /// <para>
        /// Finally, implementations should honour the rendering <paramref name="config"/> passed to this method,
        /// but are permitted not to honour settings which are either irrelevant or which cannot be supported.
        /// </para>
        /// </remarks>
        /// <returns>A task which provides the output stream.</returns>
        /// <param name="document">The document to write.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        Task<Stream> WriteDocumentAsync(IDocument document,
                                        RenderingConfig config,
                                        CancellationToken token = default);
    }
}
