using System;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Config;
using ZptSharp.Mvc;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> and <see cref="IServiceProvider"/> instances.
    /// </summary>
    public static class Mvc5ServicesExtensions
    {
        /// <summary>
        /// Adds services which are required in order to make use of the ZptSharp MVC5 ViewEngine.
        /// </summary>
        /// <returns>The same service collection instance, with registrations added.</returns>
        /// <param name="serviceCollection">A service collection to which registrations will be added.</param>
        public static IServiceCollection AddZptSharpMvc5ViewEngine(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IGetsErrorStream, ZptSharpErrorView>();
            serviceCollection.AddTransient<IGetsMvcRenderingConfig, MvcRenderingConfigProvider>();

            return serviceCollection;
        }

        /// <summary>
        /// Gets an instance of the ZptSharp MVC5 ViewEngine.  This is typically used as a singleton
        /// and registered with the global <see cref="ViewEngines"/> provider.
        /// </summary>
        /// <returns>The ZptSharp MVC5 View Engine.</returns>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="viewLocationFormats">An optional collection of view location formats (used to find view files).</param>
        /// <param name="viewsPath">An optional virtual path which will be the location of the <c>Views</c> TALES variable.</param>
        /// <param name="config">An optional rendering config instance.</param>
        public static IViewEngine GetZptSharpMvc5ViewEngine(this IServiceProvider serviceProvider,
                                                            string[] viewLocationFormats = null,
                                                            string viewsPath = ZptSharpViewEngine.DefaultViewsPath,
                                                            RenderingConfig config = null)
        {
            return new ZptSharpViewEngine(serviceProvider, viewLocationFormats, viewsPath, config);
        }
    }
}
