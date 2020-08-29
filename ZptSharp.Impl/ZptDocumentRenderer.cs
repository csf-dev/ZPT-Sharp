using System;
using System.IO;
using ZptSharp.Rendering;
using ZptSharp.Config;
using System.Threading.Tasks;
using System.Threading;
using ZptSharp.Bootstrap;

namespace ZptSharp
{
    /// <summary>
    /// The main entry-point service for ZPT-Sharp.  This service renders a model to a document stream
    /// and returns the result as a stream.
    /// </summary>
    public class ZptDocumentRenderer : IRendersZptDocuments
    {
        readonly RenderingConfig config;
        readonly ServiceProviderFactory providerFactory;

        /// <summary>
        /// Renders a specified ZPT document from a stream using the specified model.
        /// </summary>
        /// <returns>A stream containing the rendered document.</returns>
        /// <param name="stream">The stream containing the document to render.</param>
        /// <param name="model">The model to use for the rendering process.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        /// <param name="contextBuilder">The context builder action.</param>
        public Task<Stream> RenderAsync(Stream stream,
                                        object model,
                                        CancellationToken token,
                                        Action<IConfiguresRootContext> contextBuilder = null)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var request = new RenderZptDocumentRequest(stream, model, config, contextBuilder ?? (c => { }));
            var serviceProvider = providerFactory.GetServiceProvider(config);
            var requestRenderer = serviceProvider.Resolve<IRendersRenderingRequest>();
            return requestRenderer.RenderAsync(request, token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptDocumentRenderer"/> class.
        /// </summary>
        /// <param name="config">The configuration to be used by this service.</param>
        public ZptDocumentRenderer(RenderingConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            providerFactory = new ServiceProviderFactory();
        }
    }
}
