using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
#if MVC5
using System.Web.Mvc;
#elif MVCCORE
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
#endif
using ZptSharp.Config;
using ZptSharp.Hosting;

#if MVC5
namespace ZptSharp.Mvc5
#elif MVCCORE
namespace ZptSharp.MvcCore
#endif
{
    /// <summary>
    /// The ZptSharp implementation of an <see cref="IView"/> for either MVC5 or MVC Core.
    /// </summary>
    public class ZptSharpView : IView
    {
        readonly RenderingConfig originalConfig;
        readonly IGetsMvcRenderingConfig configProvider;
        readonly IGetsErrorStream errorStreamProvider;

        /// <summary>
        /// Gets the path of the view as resolved by the <see cref="ZptSharpViewEngine" />
        /// </summary>
        /// <value>The path to the view.</value>
        public string Path { get; }

        /// <summary>
        /// The path for the <c>Views</c> TALES variable.
        /// </summary>
        /// <value>The views path.</value>
        public string ViewsPath { get; }

        /// <summary>
        /// The ZptSharp self-hosting environment, from which dependencies are retrieved.
        /// </summary>
        /// <value>The self-hosting environment.</value>
        public IHostsZptSharp Host { get; }

        IRendersZptFile FileRenderer => Host.FileRenderer;

        IWritesStreamToTextWriter StreamCopier => Host.StreamCopier;

#if MVC5
        /// <summary>
        /// Renders the specified view context by using the specified the writer object.
        /// </summary>
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
#elif MVCCORE
        /// <summary>
        /// Asynchronously renders the view using the specified context.
        /// </summary>
        /// <param name="context">The view context</param>
        /// <returns>A task which completes when rendering is finished.</returns>
        public Task RenderAsync(ViewContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return RenderPrivateAsync(context, context.Writer);
        }
#endif

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
                return await FileRenderer.RenderAsync(Path, viewContext.ViewData?.Model, config).ConfigureAwait(false);
            }
            catch(ZptRenderingException ex)
            {
                viewContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
#if MVC5
                viewContext.HttpContext.Response.StatusDescription = "Internal server error";
#endif
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
        /// <param name="configProvider">A service which gets a modified rendering config, suitable for use with MVC.</param>
        /// <param name="errorStreamProvider">A service which gets a stream for displaying a rendering error.</param>
        public ZptSharpView(string filePath,
                            IHostsZptSharp host,
                            string viewsPath,
                            RenderingConfig config,
                            IGetsMvcRenderingConfig configProvider,
                            IGetsErrorStream errorStreamProvider)
        {
            Path = filePath ?? throw new ArgumentNullException(nameof(filePath));
            Host = host ?? throw new ArgumentNullException(nameof(host));
            ViewsPath = viewsPath ?? throw new ArgumentNullException(nameof(viewsPath));
            originalConfig = config ?? throw new ArgumentNullException(nameof(config));
            this.configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            this.errorStreamProvider = errorStreamProvider ?? throw new ArgumentNullException(nameof(errorStreamProvider));
        }
    }
}
