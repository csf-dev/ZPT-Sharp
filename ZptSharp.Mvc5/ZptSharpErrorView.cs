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
    public class ZptSharpErrorView : IGetsErrorStream
    {
        readonly IGetsZptDocumentRendererForFilePath rendererFactory;

        static string FilePath => $"{nameof(ZptSharpErrorView)}.pt";

        /// <summary>
        /// Gets a stream which represents the rendered error document.
        /// </summary>
        /// <returns>The error stream.</returns>
        /// <param name="exception">The exception which caused this error view to be displayed.</param>
        public async Task<Stream> GetErrorStreamAsync(Exception exception)
        {
            var documentRenderer = rendererFactory.GetDocumentRenderer(FilePath);
            var docStream = GetTemplateStream();
            var model = GetModel(exception);

            return await documentRenderer.RenderAsync(docStream, model).ConfigureAwait(false);
        }

        object GetModel(Exception exception)
        {
            return new
            {
                Exception = exception,
                ZptVersion = typeof(IRendersZptDocument).Assembly.GetName().Version.ToString(),
                ZptMvcVersion = typeof(ZptSharpErrorView).Assembly.GetName().Version.ToString(),
            };
        }

        static Stream GetTemplateStream() => typeof(ZptSharpErrorView).Assembly.GetManifestResourceStream(FilePath);

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptSharpErrorView"/> class.
        /// </summary>
        /// <param name="rendererFactory">A renderer factory.</param>
        public ZptSharpErrorView(IGetsZptDocumentRendererForFilePath rendererFactory)
        {
            this.rendererFactory = rendererFactory ?? throw new ArgumentNullException(nameof(rendererFactory));
        }
    }
}
