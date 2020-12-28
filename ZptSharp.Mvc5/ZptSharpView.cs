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
        /// The service provider.
        /// </summary>
        /// <value>The service provider.</value>
        public IServiceProvider ServiceProvider { get; }

        IRendersZptFile FileRenderer => ServiceProvider.GetRequiredService<IRendersZptFile>();

        IWritesStreamToTextWriter StreamCopier => ServiceProvider.GetRequiredService<IWritesStreamToTextWriter>();

        RenderingConfig OriginalConfig => ServiceProvider.GetRequiredService<RenderingConfig>();

        IGetsMvcRenderingConfig ConfigProvider => ServiceProvider.GetRequiredService<IGetsMvcRenderingConfig>();

        IGetsErrorStream ErrorStreamProvider => ServiceProvider.GetRequiredService<IGetsErrorStream>();

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
                return await ErrorStreamProvider.GetErrorStreamAsync(ex).ConfigureAwait(false);
            }
        }


        RenderingConfig GetRenderingConfigForMvc(ViewContext viewContext)
            => ConfigProvider.GetMvcRenderingConfig(OriginalConfig, viewContext, ViewsPath);

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptSharpView"/> class.
        /// </summary>
        /// <param name="filePath">The path to the view file which is to be rendered.</param>
        /// <param name="serviceProvider">A service provider.</param>
        /// <param name="viewsPath">The path to the root of the <c>Views</c> directory.</param>
        public ZptSharpView(string filePath,
                            IServiceProvider serviceProvider,
                            string viewsPath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            ViewsPath = viewsPath ?? throw new ArgumentNullException(nameof(viewsPath));
        }
    }
}
