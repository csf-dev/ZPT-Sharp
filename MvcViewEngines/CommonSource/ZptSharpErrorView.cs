using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using ZptSharp.Rendering;

namespace ZptSharp.Mvc
{
    /// <summary>
    /// A provider for a stream which contains a web page displaying a ZptSharp rendering error to an end user.
    /// </summary>
    public class ZptSharpErrorView : IGetsErrorStream
    {
        readonly IGetsZptDocumentRendererForFilePath rendererFactory;

        static string ResourceName => $"{nameof(ZptSharpErrorView)}.pt";

        Assembly MvcAssembly => GetType().Assembly;

        Assembly ZptAssembly => typeof(IRendersZptDocument).Assembly;

        /// <summary>
        /// Gets a stream which represents the rendered error document.
        /// </summary>
        /// <returns>The error stream.</returns>
        /// <param name="exception">The exception which caused this error view to be displayed.</param>
        public async Task<Stream> GetErrorStreamAsync(Exception exception)
        {
            var documentRenderer = rendererFactory.GetDocumentRenderer(ResourceName);
            var docStream = MvcAssembly.GetManifestResourceStream(ResourceName);
            var model = GetModel(exception);

            return await documentRenderer.RenderAsync(docStream, model).ConfigureAwait(false);
        }

        object GetModel(Exception exception)
        {
            var zptAssemblyName = ZptAssembly.GetName(); 
            var mvcAssemblyName = MvcAssembly.GetName();

            return new
            {
                Exception = exception,
                ZptAssemblyName = zptAssemblyName.Name,
                ZptAssemblyVersion = zptAssemblyName.Version.ToString(),
                MvcAssemblyName = mvcAssemblyName.Name,
                MvcAssemblyVersion = mvcAssemblyName.Version.ToString(),
            };
        }

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
