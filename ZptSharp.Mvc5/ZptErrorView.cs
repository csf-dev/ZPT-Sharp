using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Rendering;

namespace ZptSharp.Mvc
{
    /// <summary>
    /// A provider for a view which shows a ZptSharp rendering error.
    /// </summary>
    public class ZptErrorView
    {
        readonly Exception ex;
        readonly IServiceProvider serviceProvider;

        IGetsZptDocumentRendererForFilePath RendererFactory
            => serviceProvider.GetRequiredService<IGetsZptDocumentRendererForFilePath>();

        string FilePath => $"{nameof(ZptErrorView)}.pt";

        /// <summary>
        /// Gets a stream which represents the rendered error document.
        /// </summary>
        /// <returns>The error stream.</returns>
        public async Task<Stream> GetErrorStream()
        {
            var documentRenderer = RendererFactory.GetDocumentRenderer(FilePath);
            var docStream = GetTemplateStream();
            var model = GetModel();

            return await documentRenderer.RenderAsync(docStream, model);
        }

        object GetModel()
        {
            return new
            {
                Exception = ex,
                ZptVersion = typeof(IRendersZptDocument).Assembly.GetName().Version.ToString(),
                ZptMvcVersion = typeof(ZptErrorView).Assembly.GetName().Version.ToString(),
            };
        }

        Stream GetTemplateStream() => typeof(ZptErrorView).Assembly.GetManifestResourceStream(FilePath);

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptErrorView"/> class.
        /// </summary>
        /// <param name="ex">The exception which lead to the error.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public ZptErrorView(Exception ex, IServiceProvider serviceProvider)
        {
            this.ex = ex ?? throw new ArgumentNullException(nameof(ex));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}
