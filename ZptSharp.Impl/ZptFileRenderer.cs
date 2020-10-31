using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.Rendering;

namespace ZptSharp
{
    public class ZptFileRenderer : IRendersZptFile
    {
        readonly RenderingConfig config;
        readonly IServiceProvider serviceProvider;

        public Task<Stream> RenderAsync(string filePath,
                                        object model,
                                        CancellationToken token = default,
                                        Action<IConfiguresRootContext> contextBuilder = null)
        {
            var readerWriter = GetDocumentReaderWriter(filePath);
            var documentRenderer = GetDocumentRenderer(readerWriter);

            var stream = new FileStream(filePath, FileMode.Open);
            var sourceInfo = new FileSourceInfo(filePath);

            return documentRenderer.RenderAsync(stream, model, token, contextBuilder, sourceInfo);
        }

        IReadsAndWritesDocument GetDocumentReaderWriter(string filePath)
        {
            var readerWriterFactory = serviceProvider.Resolve<IGetsDocumentReaderWriterForFile>();
            var readerWriter = readerWriterFactory.GetDocumentProvider(filePath);

            if (readerWriter != null) return readerWriter;

            var message = String.Format(Resources.ExceptionMessage.CannotGetReaderWriterForFile,
                                        filePath,
                                        nameof(IRegistersDocumentReaderWriter));
            throw new NoMatchingReaderWriterException(message);
        }

        IRendersZptDocuments GetDocumentRenderer(IReadsAndWritesDocument readerWriter)
        {
            var rendererFactory = serviceProvider.Resolve<IGetsZptDocumentRenderer>();
            return rendererFactory.GetDocumentRenderer(config, readerWriter);
        }

        public ZptFileRenderer(RenderingConfig config, IServiceProvider serviceProvider)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}
