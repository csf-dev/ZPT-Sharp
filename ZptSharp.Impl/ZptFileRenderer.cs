using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.Rendering;

namespace ZptSharp
{
    /// <summary>
    /// Implementation of <see cref="IRendersZptFile"/> which uses an
    /// <see cref="IRendersZptDocuments"/> from a factory.  It also uses an
    /// <see cref="IGetsDocumentReaderWriterForFile"/> to attempt to get the
    /// appropriate document reader/writer for the file.
    /// </summary>
    public class ZptFileRenderer : IRendersZptFile
    {
        readonly RenderingConfig config;
        readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Renders a specified ZPT document file using the specified model.
        /// </summary>
        /// <returns>A stream containing the rendered document.</returns>
        /// <param name="filePath">The path of the document file to render.</param>
        /// <param name="model">The model to use for the rendering process.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        /// <param name="contextBuilder">The context builder action.</param>
        public Task<Stream> RenderAsync(string filePath,
                                        object model,
                                        CancellationToken token = default,
                                        Action<IConfiguresRootContext> contextBuilder = null)
        {
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var sourceInfo = new FileSourceInfo(filePath);

            var readerWriter = GetDocumentReaderWriter(filePath);
            var documentRenderer = GetDocumentRenderer(readerWriter);

            return documentRenderer.RenderAsync(stream, model, token, contextBuilder, sourceInfo);
        }

        IReadsAndWritesDocument GetDocumentReaderWriter(string filePath)
        {
            var readerWriterFactory = serviceProvider.GetRequiredService<IGetsDocumentReaderWriterForFile>();
            var readerWriter = readerWriterFactory.GetDocumentProvider(filePath);

            if (readerWriter != null) return readerWriter;

            var message = String.Format(Resources.ExceptionMessage.CannotGetReaderWriterForFile,
                                        filePath,
                                        nameof(IRegistersDocumentReaderWriter));
            throw new NoMatchingReaderWriterException(message);
        }

        IRendersZptDocuments GetDocumentRenderer(IReadsAndWritesDocument readerWriter)
        {
            var rendererFactory = serviceProvider.GetRequiredService<IGetsZptDocumentRenderer>();
            return rendererFactory.GetDocumentRenderer(config, readerWriter);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptFileRenderer"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public ZptFileRenderer(RenderingConfig config, IServiceProvider serviceProvider)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}
