using System;
using ZptSharp.Config;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A factory which creates instances of <see cref="EffectiveConfigSettingZptDocumentRendererDecorator"/> from a <see cref="RenderingConfig"/>
    /// and optionally a <see cref="System.Type"/> which indicates the <see cref="Dom.IReadsAndWritesDocument"/> to
    /// be used.
    /// </summary>
    public class ZptDocumentRendererFactory : IGetsZptDocumentRenderer
    {
        readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Gets the document renderer.
        /// </summary>
        /// <returns>The document renderer.</returns>
        /// <param name="readerWriterType">A specific document reader/writer implementation type.</param>
        public IRendersZptDocument GetDocumentRenderer(System.Type readerWriterType = null)
            => new EffectiveConfigSettingZptDocumentRendererDecorator(new ZptDocumentRenderer(serviceProvider), readerWriterType);

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
