using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstract base class for implementations of <see cref="IReadsAndWritesDocument"/>.  This class takes care
    /// of some boilerplate logic related to the validation of parameters and raising an exception if the operation
    /// is to be cancelled.
    /// </summary>
    public abstract class DocumentReaderWriterBase<TNativeType> : IReadsAndWritesDocument where TNativeType : class
    {
        /// <summary>
        /// Gets the type of the current instance, for the purpose of future dependency resolution.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Gets the type of the current instance, for the purpose of dependency resolution.
        /// This is not/should not be quite the same as <see cref="System.Object.GetType()"/>, because
        /// a derived implementation might not always be resolved as its precise type, it might be resolved
        /// as a less-derived base type.
        /// </para>
        /// </remarks>
        public abstract System.Type ResolvableType { get; }

        /// <summary>
        /// Gets whether or not the current instance may be used to read &amp; write documents
        /// which have the specified filename.
        /// </summary>
        /// <returns><c>true</c>, if this instance maybe used, <c>false</c> otherwise.</returns>
        /// <param name="filenameOrPath">The filename of a ZPT document.</param>
        public abstract bool CanReadWriteForFilename(string filenameOrPath);

        /// <summary>
        /// Gets a document instance from the specified input stream.  Implementors of this base class
        /// should implement logic in <see cref="GetDocumentProtectedAsync(Stream, RenderingConfig, IDocumentSourceInfo, CancellationToken)"/>.
        /// </summary>
        /// <returns>A task which provides the document which has been read.</returns>
        /// <param name="stream">A stream containing the source of the document.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="sourceInfo">Information which identifies the source of the document.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        public Task<IDocument> GetDocumentAsync(Stream stream, RenderingConfig config, IDocumentSourceInfo sourceInfo = null, CancellationToken token = default)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (config == null) throw new ArgumentNullException(nameof(config));
            token.ThrowIfCancellationRequested();

            return GetDocumentProtectedAsync(stream, config, sourceInfo, token);
        }

        /// <summary>
        /// Writes the specified document to a specified output stream.  Implementors of this base class
        /// should implement logic in <see cref="WriteDocumentProtectedAsync(TNativeType, RenderingConfig, CancellationToken)"/>.
        /// </summary>
        /// <returns>A task which provides the output stream.</returns>
        /// <param name="document">The document to write.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        public Task<Stream> WriteDocumentAsync(IDocument document, RenderingConfig config, CancellationToken token = default)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (!(document is TNativeType nativeDocument))
                throw new ArgumentException($"The document must be an instance of {typeof(TNativeType).Name}.", nameof(document));
            token.ThrowIfCancellationRequested();

            return WriteDocumentProtectedAsync(nativeDocument, config, token);
        }

        /// <summary>
        /// Implement this method in a derived class to perform custom logic related to reading a document from a stream.
        /// Derived types should assume that the <paramref name="stream"/> &amp; <paramref name="config"/> parameters have
        /// been null-checked.  Also, they should assume that the <paramref name="token"/> parameter does not request cancellation.
        /// </summary>
        /// <returns>A task which provides the document which has been read.</returns>
        /// <param name="stream">A stream containing the source of the document.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="sourceInfo">Information which identifies the source of the document.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        protected abstract Task<IDocument> GetDocumentProtectedAsync(Stream stream, RenderingConfig config, IDocumentSourceInfo sourceInfo, CancellationToken token);

        /// <summary>
        /// Implement this method in a derived class to perform custom logic related to writing a document to a stream.
        /// Derived types should assume that the <paramref name="document"/> &amp; <paramref name="config"/> parameters
        /// have been null-checked. The document will also already be converted to <typeparamref name="TNativeType"/>.
        /// Finally, the <paramref name="token"/> parameter will have already been checked that it does not request cancellation.
        /// </summary>
        /// <returns>A task which provides the output stream.</returns>
        /// <param name="document">The document to write.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        protected abstract Task<Stream> WriteDocumentProtectedAsync(TNativeType document, RenderingConfig config, CancellationToken token);
    }
}
