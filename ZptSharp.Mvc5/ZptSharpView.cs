using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZptSharp.Config;
using ZptSharp.Hosting;

namespace ZptSharp.Mvc
{
    /// <summary>
    /// The ZptSharp implementation of an MVC5 <see cref="IView"/>.
    /// </summary>
    public class ZptSharpView : IView
    {
        readonly RenderingConfig originalConfig;
        readonly IGetsMvcRenderingConfig configProvider;
        readonly IGetsErrorStream errorStreamProvider;

        /// <summary>
        /// The path to the current view file.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; }

        /// <summary>
        /// The path for the <c>Views</c> TALES variable.
        /// </summary>
        /// <value>The views path.</value>
        public string ViewsPath { get; }

        /// <summary>
        /// The self-hosting environment.
        /// </summary>
        /// <value>The self-hosting environment.</value>
        public IHostsZptSharp Host { get; }

        IRendersZptFile FileRenderer => Host.FileRenderer;

        IWritesStreamToTextWriter StreamCopier => Host.StreamCopier;

        /// <summary>Renders the specified view context by using the specified the writer object.</summary>
        /// <param name="viewContext">The view context.</param>
        /// <param name="writer">The writer object.</param>
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            if (viewContext == null)
                throw new ArgumentNullException(nameof(viewContext));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            RenderPrivateAsync(viewContext, writer).Wait();
        }

        async Task RenderPrivateAsync(ViewContext viewContext, TextWriter writer)
        {
            var config = GetRenderingConfigForMvc(viewContext);

            using (var stream = await GetRenderedStreamAsync(viewContext, config).ConfigureAwait(false))
            {
                await StreamCopier.WriteToTextWriterAsync(stream, writer).ConfigureAwait(false);
            }
        }

        async Task<Stream> GetRenderedStreamAsync(ViewContext viewContext, RenderingConfig config)
        {
            try
            {
                return await FileRenderer.RenderAsync(FilePath, viewContext.ViewData?.Model, config).ConfigureAwait(false);
            }
            catch(ZptRenderingException ex)
            {
                viewContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                viewContext.HttpContext.Response.StatusDescription = "Internal server error";
                return await errorStreamProvider.GetErrorStreamAsync(ex).ConfigureAwait(false);
            }
        }

        RenderingConfig GetRenderingConfigForMvc(ViewContext viewContext)
            => configProvider.GetMvcRenderingConfig(originalConfig, viewContext, ViewsPath);

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptSharpView"/> class.
        /// </summary>
        /// <param name="filePath">The path to the view file which is to be rendered.</param>
        /// <param name="host">The self-hosting ZptSharp environment.</param>
        /// <param name="viewsPath">The path to the root of the <c>Views</c> directory.</param>
        /// <param name="config">A rendering config.</param>
        public ZptSharpView(string filePath,
                            IHostsZptSharp host,
                            string viewsPath,
                            RenderingConfig config,
                            IGetsMvcRenderingConfig configProvider,
                            IGetsErrorStream errorStreamProvider)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            Host = host ?? throw new ArgumentNullException(nameof(host));
            ViewsPath = viewsPath ?? throw new ArgumentNullException(nameof(viewsPath));
            originalConfig = config ?? throw new ArgumentNullException(nameof(config));
            this.configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            this.errorStreamProvider = errorStreamProvider ?? throw new ArgumentNullException(nameof(errorStreamProvider));
        }
    }
}
