using System;
using System.Web.Mvc;
using ZptSharp.Config;

namespace ZptSharp.Mvc
{
    /// <summary>
    /// Implementation of <see cref="IGetsMvcRenderingConfig"/> which gets a configuration based upon another config,
    /// but with MVC-specific information added.
    /// </summary>
    public class MvcRenderingConfigProvider : IGetsMvcRenderingConfig
    {
        /// <summary>
        /// Gets the rendering configuration instance.
        /// </summary>
        /// <returns>The MVC rendering config.</returns>
        /// <param name="originalConfig">Original config.</param>
        /// <param name="viewContext">View context.</param>
        /// <param name="viewsPath">The path to the root of the <c>Views</c> directory.</param>
        public RenderingConfig GetMvcRenderingConfig(RenderingConfig originalConfig, ViewContext viewContext, string viewsPath)
        {
            var contextBuilder = GetContextBuilder(viewContext, viewsPath);

            var builder = originalConfig.CloneToNewBuilder();
            builder.ContextBuilder = contextBuilder.GetRootContextBuilder();
            return builder.GetConfig();
        }

        static IGetsRootContextBuilder GetContextBuilder(ViewContext viewContext, string viewsPath)
            => new MvcContextBuilderProvider(viewContext, viewsPath);
    }
}
