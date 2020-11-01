using System;
using ZptSharp.Config;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A factory which creates instances of <see cref="ZptDocumentRenderer"/> from a <see cref="RenderingConfig"/>
    /// and optionally a <see cref="IReadsAndWritesDocument"/>.
    /// </summary>
    public class ZptDocumentRendererFactory : IGetsZptDocumentRenderer
    {
        readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Gets the document renderer.
        /// </summary>
        /// <returns>The document renderer.</returns>
        /// <param name="config">Config.</param>
        /// <param name="readerWriter">A specific document reader/writer implementation.</param>
        public IRendersZptDocuments GetDocumentRenderer(RenderingConfig config, IReadsAndWritesDocument readerWriter = null)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            return new ZptDocumentRenderer(config, serviceProvider, readerWriter);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptDocumentRendererFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        public ZptDocumentRendererFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}
