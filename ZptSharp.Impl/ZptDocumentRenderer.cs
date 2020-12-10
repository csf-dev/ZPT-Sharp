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
    /// An entry-point service for ZPT-Sharp.  This service renders a model to a document stream
    /// and returns the result as a stream.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use this service when you wish to perform the ZPT rendering process upon a <see cref="Stream"/>.
    /// That stream may have come from any source (not neccesarily a file on disk).
    /// </para>
    /// <para>
    /// If you wish to render a file on disk, then you should probably consider using
    /// <see cref="ZptFileRenderer"/> (<see cref="IRendersZptFile"/>) instead, which provides
    /// an API which is more suitable for files, including choosing a rendering backend based
    /// upon the file extension.
    /// </para>
    /// </remarks>
    public class ZptDocumentRenderer : IRendersZptDocument
    {
        readonly IServiceProvider serviceProvider;
        readonly IReadsAndWritesDocument readerWriter;

        /// <summary>
        /// Renders a specified ZPT document from a stream using the specified model.
        /// </summary>
        /// <remarks>
        /// <para>
        /// There are two ways in which an implementation of <see cref="IReadsAndWritesDocument"/>
        /// (aka "the document provider") may be specified for use when executing this method.
        /// One way is to specify a non-null <paramref name="config"/> which has its
        /// <see cref="RenderingConfig.DocumentProvider"/> configuration option set.
        /// The other way is to specify a non-null instance of <see cref="IReadsAndWritesDocument"/>
        /// in the constructor of this <see cref="ZptDocumentRenderer"/> instance.
        /// </para>
        /// <para>
        /// If both mechanisms are specified, then the document provider specified via the
        /// <see cref="ZptDocumentRenderer"/> will take precedence (ignoring the reader/writer specified
        /// in the configuration).
        /// </para>
        /// </remarks>
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
        /// <remarks>
        /// <para>
        /// If a <paramref name="readerWriter"/> is specified in this constructor, then the
        /// <see cref="RenderingConfig.DocumentProvider"/> specified in
        /// <see cref="RenderAsync(Stream, object, RenderingConfig, CancellationToken, IDocumentSourceInfo)"/>
        /// will not be used.  The document provider (aka document reader/writer) provided in
        /// this constructor will be used instead.
        /// </para>
        /// </remarks>
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
