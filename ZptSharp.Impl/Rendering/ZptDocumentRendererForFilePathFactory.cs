using System;
using ZptSharp.Dom;
using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// Implementation of <see cref="IGetsZptDocumentRendererForFilePath"/> which uses a file path to
    /// get a suitable implementation of <see cref="IRendersZptDocument"/>.
    /// </summary>
    public class ZptDocumentRendererForFilePathFactory : IGetsZptDocumentRendererForFilePath
    {
        readonly IServiceProvider serviceProvider;

        IGetsDocumentReaderWriterForFile ReaderWriterFactory
            => serviceProvider.GetRequiredService<IGetsDocumentReaderWriterForFile>();

        IGetsZptDocumentRenderer RendererFactory
            => serviceProvider.GetRequiredService<IGetsZptDocumentRenderer>();

        /// <summary>
        /// Gets the document renderer.
        /// </summary>
        /// <returns>The document renderer.</returns>
        /// <param name="filePath">The path to a file which would be rendered by the renderer.</param>
        public IRendersZptDocument GetDocumentRenderer(string filePath)
        {
            var readerWriterType = GetDocumentReaderWriterType(filePath);
            return RendererFactory.GetDocumentRenderer(readerWriterType);
        }

        System.Type GetDocumentReaderWriterType(string filePath)
        {
            var readerWriter = ReaderWriterFactory.GetDocumentProvider(filePath);

            if (readerWriter != null) return readerWriter.GetType();

            var message = String.Format(Resources.ExceptionMessage.CannotGetReaderWriterForFile,
                                        filePath,
                                        nameof(Hosting.EnvironmentRegistry));
            throw new NoMatchingReaderWriterException(message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptDocumentRendererForFilePathFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        public ZptDocumentRendererForFilePathFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}
