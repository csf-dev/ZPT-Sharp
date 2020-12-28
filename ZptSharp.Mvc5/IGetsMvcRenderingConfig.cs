using System;
using System.Web.Mvc;
using ZptSharp.Config;

namespace ZptSharp.Mvc
{
    /// <summary>
    /// An object which can provide a <see cref="RenderingConfig"/> instance
    /// which is suitable for use with ASP.NET MVC5.
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
