using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using System.Xml.Linq;
using System.Xml;
using ZptSharp.Rendering;
using System.Linq;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An implementation of <see cref="IReadsAndWritesDocument"/> which uses the XML/Linq API built-into .NET
    /// Framework and dotnet core.
    /// See also: <seealso cref="XDocument"/>
    /// </summary>
    public class XmlDocumentProvider : DocumentReaderWriterBase<XmlDocument>
    {
        static readonly string[] supportedExtensions = new[] { ".xml", ".xhtml" };
        const LoadOptions loadOptions = LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo;

        /// <summary>This matches the default buffer size for a built-in <see cref="StreamWriter"/>.</summary>
        const int BufferSize = 1024;

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
        /// This implementation is not truly asynchronous, as the APIs involved do not provide async functionality.
        /// </para>
        /// </remarks>
        /// <returns>A task which provides the document which has been read.</returns>
        /// <param name="stream">A stream containing the source of the document.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="sourceInfo">Information which identifies the source of the document.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        protected override Task<IDocument> GetDocumentProtectedAsync(Stream stream,
                                                                     RenderingConfig config,
                                                                     IDocumentSourceInfo sourceInfo,
                                                                     CancellationToken token)
        {
            using (var reader = new StreamReader(stream, config.DocumentEncoding))
            {
                var doc = XDocument.Load(reader, loadOptions);

                IDocument xmlDoc = new XmlDocument(doc, sourceInfo ?? new UnknownSourceInfo());
                return Task.FromResult(xmlDoc);
            }
        }

        /// <summary>
        /// Implement this method in a derived class to perform custom logic related to writing a document to a stream.
        /// Derived types should assume that the <paramref name="document"/> &amp; <paramref name="config"/> parameters
        /// have been null-checked. The document will also already be converted to <see cref="XmlDocument"/>.
        /// Finally, the <paramref name="token"/> parameter will have already been checked that it does not request cancellation.
        /// </summary>
        /// <returns>A task which provides the output stream.</returns>
        /// <param name="document">The document to write.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        protected override async Task<Stream> WriteDocumentProtectedAsync(XmlDocument document, RenderingConfig config, CancellationToken token)
        {
            var stream = new MemoryStream();

            using (var writer = new StreamWriter(stream, config.DocumentEncoding, BufferSize, true))
            using (var xmlWriter = GetXmlWriter(config, writer))
            {
                document.NativeDocument.Save(xmlWriter);
                await writer.FlushAsync().ConfigureAwait(false);
            }

            stream.Position = 0;
            return stream;
        }

        XmlWriter GetXmlWriter(RenderingConfig config, TextWriter writer)
        {
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = config.DocumentEncoding;
            settings.OmitXmlDeclaration = config.OmitXmlDeclaration;

            return XmlWriter.Create(writer, settings);
        }
    }
}
