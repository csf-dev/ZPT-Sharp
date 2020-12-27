using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZptSharp.Config;
using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Mvc
{
    /// <summary>
    /// The ZptSharp implementation of an MVC5 <see cref="IView"/>.
    /// </summary>
    public class ZptSharpView : IView
    {
        readonly string filePath;
        readonly IServiceProvider serviceProvider;
        readonly Func<ViewContext, IGetsRootContextBuilder> contextBuilderFactory;

        IRendersZptFile FileRenderer => serviceProvider.GetRequiredService<IRendersZptFile>();

        IWritesStreamToTextWriter StreamCopier => serviceProvider.GetRequiredService<IWritesStreamToTextWriter>();

        RenderingConfig OriginalConfig => serviceProvider.GetRequiredService<RenderingConfig>();

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
                return await FileRenderer.RenderAsync(filePath, viewContext.ViewData?.Model, config).ConfigureAwait(false);
            }
            catch(ZptRenderingException ex)
            {
                var errorView = new ZptErrorView(ex, serviceProvider);
                viewContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                viewContext.HttpContext.Response.StatusDescription = "Internal server error";
                return await errorView.GetErrorStream().ConfigureAwait(false);
            }
        }


        RenderingConfig GetRenderingConfigForMvc(ViewContext viewContext)
        {
            var contextBuilder = contextBuilderFactory(viewContext);

            var builder = OriginalConfig.CloneToNewBuilder();
            builder.ContextBuilder = contextBuilder.GetRootContextBuilder();
            return builder.GetConfig();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptSharpView"/> class.
        /// </summary>
        /// <param name="filePath">The path to the view file which is to be rendered.</param>
        /// <param name="serviceProvider">A service provider.</param>
        /// <param name="contextBuilderFactory">A context-builder factory.</param>
        public ZptSharpView(string filePath,
                            IServiceProvider serviceProvider,
                            Func<ViewContext,IGetsRootContextBuilder> contextBuilderFactory)
        {
            this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.contextBuilderFactory = contextBuilderFactory ?? throw new ArgumentNullException(nameof(contextBuilderFactory));
        }
    }
}
