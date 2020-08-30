using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using HtmlAgilityPack;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An implementation of <see cref="IReadsAndWritesDocument"/> which uses the HtmlAgilityPack (aka "HAP") HTML
    /// parsing library. See https://html-agility-pack.net/ and https://github.com/zzzprojects/html-agility-pack for
    /// more information.
    /// </summary>
    public class HapDocumentProvider : IReadsAndWritesDocument
    {
        /// <summary>This matches the default buffer size for a built-in <see cref="StreamWriter"/>.</summary>
        const int BufferSize = 1024;

        /// <summary>
        /// Gets a document instance from the specified input stream.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This implementation is not truly asynchronous, as the APIs involved do not provide async functionality.
        /// </para>
        /// </remarks>
        /// <returns>A task which provides the document which has been read.</returns>
        /// <param name="stream">A stream containing the source of the document.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        public Task<IDocument> GetDocumentAsync(Stream stream, RenderingConfig config, CancellationToken token)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(stream, config.DocumentEncoding);
            IDocument doc = new HapDocument(htmlDoc);
            return Task.FromResult(doc);
        }

        /// <summary>
        /// Writes the specified document to a specified output stream.
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
        public async Task<Stream> WriteDocumentAsync(IDocument document, RenderingConfig config, CancellationToken token)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            if (!(document is HapDocument angleSharpDocument))
                throw new ArgumentException($"The document must be an instance of {nameof(HapDocument)}.", nameof(document));
            token.ThrowIfCancellationRequested();

            var stream = new MemoryStream();
            using(var writer = new StreamWriter(stream, config.DocumentEncoding, BufferSize, true))
            {
                token.ThrowIfCancellationRequested();
                angleSharpDocument.Document.DocumentNode.WriteTo(writer);
                token.ThrowIfCancellationRequested();
                await writer.FlushAsync().ConfigureAwait(false);
            }

            stream.Position = 0;
            return stream;
        }
    }
}
