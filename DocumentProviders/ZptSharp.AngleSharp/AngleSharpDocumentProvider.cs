using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using AngleSharp.Html.Parser;
using AngleSharp.Html;
using ZptSharp.Rendering;
using System.Linq;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An implementation of <see cref="IReadsAndWritesDocument"/> which uses the AngleSharp HTML parsing library.
    /// See https://anglesharp.github.io/ for more information.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Please note that when using <see cref="GetDocumentProtectedAsync(Stream, RenderingConfig, IDocumentSourceInfo, CancellationToken)"/>,
    /// this implementation ignores the <see cref="RenderingConfig.DocumentEncoding"/>.  This is because AngleSharp will
    /// always automatically detect the encoding from the document; there is no way to specify the encoding externally.
    /// When writing a document using <see cref="WriteDocumentProtectedAsync(AngleSharpDocument, RenderingConfig, CancellationToken)"/>,
    /// the configured encoding is honoured.
    /// </para>
    /// </remarks>
    public class AngleSharpDocumentProvider : DocumentReaderWriterBase<AngleSharpDocument>
    {
        static readonly string[] supportedExtensions = new[] { ".pt", ".htm", ".html" };

        /// <summary>This matches the default buffer size for a built-in <see cref="StreamWriter"/>.</summary>
        const int BufferSize = 1024;

        readonly IHtmlParser parser = new HtmlParser();

        /// <summary>
        /// Gets whether or not the current instance may be used to read &amp; write documents
        /// which have the specified filename.
        /// </summary>
        /// <returns><c>true</c>, if this instance maybe used, <c>false</c> otherwise.</returns>
        /// <param name="filenameOrPath">The filename of a ZPT document.</param>
        public override bool CanReadWriteForFilename(string filenameOrPath)
        {
            var extension = new FileInfo(filenameOrPath).Extension;
            return supportedExtensions.Any(x => String.Equals(extension, x, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Implement this method in a derived class to perform custom logic related to reading a document from a stream.
        /// Derived types should assume that the <paramref name="stream"/> &amp; <paramref name="config"/> parameters have
        /// been null-checked.  Also, they should assume that the <paramref name="token"/> parameter does not request cancellation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This implementation ignores <see cref="RenderingConfig.DocumentEncoding"/>; AngleSharp always detects
        /// the document encoding from the HTML document itself.  It is strongly advised to use UTF-8.
        /// </para>
        /// </remarks>
        /// <returns>A task which provides the document which has been read.</returns>
        /// <param name="stream">A stream containing the source of the document.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="sourceInfo">Information which identifies the source of the document.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        protected override async Task<IDocument> GetDocumentProtectedAsync(Stream stream,
                                                                           RenderingConfig config,
                                                                           IDocumentSourceInfo sourceInfo,
                                                                           CancellationToken token)
        {
            var doc = await parser.ParseDocumentAsync(stream, token).ConfigureAwait(false);

            return new AngleSharpDocument(doc)
            {
                SourceInfo = sourceInfo ?? new UnknownSourceInfo()
            };
        }

        /// <summary>
        /// Implement this method in a derived class to perform custom logic related to writing a document to a stream.
        /// Derived types should assume that the <paramref name="document"/> &amp; <paramref name="config"/> parameters
        /// have been null-checked. The document will also already be converted to <see cref="AngleSharpDocument"/>.
        /// Finally, the <paramref name="token"/> parameter will have already been checked that it does not request cancellation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This implementation ignores <see cref="RenderingConfig.OmitXmlDeclaration"/>, as would be expected.
        /// This is not an XML document format.
        /// </para>
        /// </remarks>
        /// <returns>A task which provides the output stream.</returns>
        /// <param name="document">The document to write.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        protected override async Task<Stream> WriteDocumentProtectedAsync(AngleSharpDocument document, RenderingConfig config, CancellationToken token)
        {
            var stream = new MemoryStream();

            using (var writer = new StreamWriter(stream, config.DocumentEncoding, BufferSize, true))
            {
                document.NativeDocument.ToHtml(writer, HtmlMarkupFormatter.Instance);
                await writer.FlushAsync().ConfigureAwait(false);
            }

            stream.Position = 0;
            return stream;
        }
    }
}
