#if MVC5
using System.Web.Mvc;
#elif MVCCORE
using Microsoft.AspNetCore.Mvc.Rendering;
#endif
using ZptSharp.Config;

#if MVC5
namespace ZptSharp.Mvc5
#elif MVCCORE
namespace ZptSharp.MvcCore
#endif
{
    /// <summary>
    /// An object which can provide a <see cref="RenderingConfig"/> instance
    /// which is suitable for use with ASP.NET MVC (either MVC5 or MVC Core).
    /// </summary>
    public interface IGetsMvcRenderingConfig
    {
        /// <summary>
        /// Gets the rendering configuration instance.
        /// </summary>
        /// <returns>The MVC rendering config.</returns>
        /// <param name="originalConfig">Original config.</param>
        /// <param name="viewContext">View context.</param>
        /// <param name="viewsPath">The path to the root of the <c>Views</c> directory.</param>
        RenderingConfig GetMvcRenderingConfig(RenderingConfig originalConfig, ViewContext viewContext, string viewsPath);
    }
}
