using System;
using System.IO;
using ZptSharp.Rendering;
using ZptSharp.Config;
using System.Threading.Tasks;
using System.Threading;
using ZptSharp.Dom;
using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp
{
    /// <summary>
    /// The main entry-point service for ZPT-Sharp.  This service renders a model to a document stream
    /// and returns the result as a stream.
    /// </summary>
    public class ZptDocumentRenderer : IRendersZptDocument
    {
        readonly IServiceProvider serviceProvider;
        readonly IReadsAndWritesDocument readerWriter;

        /// <summary>
        /// Renders a specified ZPT document from a stream using the specified model.
        /// </summary>
        /// <returns>A stream containing the rendered document.</returns>
        /// <param name="stream">The stream containing the document to render.</param>
        /// <param name="model">The model to use for the rendering process.</param>
        /// <param name="config">An optional rendering configuration object.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        /// <param name="sourceInfo">The source info for the <paramref name="stream"/>.</param>
        public Task<Stream> RenderAsync(Stream stream,
                                        object model,
                                        RenderingConfig config = null,
                                        CancellationToken token = default,
                                        IDocumentSourceInfo sourceInfo = null)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var effectiveConfig = GetEffectiveRenderingConfig(config);
            var request = new RenderZptDocumentRequest(stream, model, sourceInfo);
            var requestRenderer = serviceProvider.GetRequiredService<IRendersRenderingRequest>();

            return requestRenderer.RenderAsync(request, effectiveConfig, token);
        }

        RenderingConfig GetEffectiveRenderingConfig(RenderingConfig config)
        {
            if (config != null && readerWriter == null) return config;

            var builder = config?.CloneToNewBuilder() ?? RenderingConfig.CreateBuilder();

            if (readerWriter != null)
                builder.DocumentProvider = readerWriter;

            return builder.GetConfig();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptDocumentRenderer"/> class.
        /// </summary>
        /// <param name="serviceProvider">A service provider, from which dependencies may be resolved.</param>
        /// <param name="readerWriter">An optional document reader/writer service to use to render the current document.</param>
        public ZptDocumentRenderer(IServiceProvider serviceProvider,
                                   IReadsAndWritesDocument readerWriter = null)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.readerWriter = readerWriter;
        }
    }
}
