using System;
using ZptSharp.Config;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    public class ZptDocumentRendererFactory : IGetsZptDocumentRenderer
    {
        readonly IServiceProvider serviceProvider;

        public IRendersZptDocuments GetDocumentRenderer(RenderingConfig config, IReadsAndWritesDocument readerWriter = null)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            return new ZptDocumentRenderer(config, serviceProvider, readerWriter);
        }

        public ZptDocumentRendererFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}
