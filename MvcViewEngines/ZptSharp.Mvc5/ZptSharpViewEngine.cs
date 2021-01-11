using System;
using System.Linq;
using System.Web.Mvc;
using ZptSharp.Config;
using ZptSharp.Hosting;
using System.Collections.Generic;
using System.Web;

namespace ZptSharp.Mvc5
{
    /// <summary>
    /// The ZptSharp MVC ViewEngine, used to render MVC views using ZptSharp.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Please note that a number of ASP.NET MVC-related features are unsupported by ZptSharp views, as
    /// they are not relevant in the context of ZptSharp views.  These features includes:
    /// </para>
    /// <list type="bullet">
    /// <item>Master pages</item>
    /// <item>Partial views (MVC5 only)</item>
    /// </list>
    /// <para>
    /// Usages of parameters relating to these unsupported features will be ignored (and a normal
    /// view will be returned regardless).  Usages of methods relating to these unsupported features
    /// will return a normal view.
    /// </para>
    /// </remarks>
    public class ZptSharpViewEngine : IViewEngine
    {
        internal const string DefaultViewsPath = "~/Views/";

        static readonly string[] defaultViewLocationFormats =
        {
            "~/Views/{1}/{0}.pt",
            "~/Views/Shared/{0}.pt",
            "~/Views/{1}/{0}.html",
            "~/Views/Shared/{0}.html",
        };

        readonly string[] viewLocationFormats;

        readonly IHostsZptSharp host;
        readonly string viewsPath;
        readonly RenderingConfig config;
        readonly IGetsMvcRenderingConfig configProvider;
        readonly IGetsErrorStream errorStreamProvider;
        readonly IFindsView defaultViewFinder;

        /// <summary>
        /// Gets a <see cref="ViewEngineResult" /> from an attempt to find a ZptSharp view of the specified
        /// <paramref name="partialViewName" />.
        /// </summary>
        /// <param name="controllerContext">The current controller context.</param>
        /// <param name="partialViewName">The name of the requested view.</param>
        /// <param name="useCache">Indicates whether or not a view-location cache should be used or not.</param>
        /// <returns>A <see cref="ViewEngineResult" /> indicating whether the view was found or not.</returns>
        public ViewEngineResult FindPartialView (ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var controllerName = controllerContext.RouteData.GetRequiredString("controller");
            return FindView(controllerName, partialViewName, controllerContext.HttpContext);
        }

        /// <summary>
        /// Gets a <see cref="ViewEngineResult" /> from an attempt to find a ZptSharp view of the specified
        /// <paramref name="viewName" />.
        /// </summary>
        /// <param name="controllerContext">The current controller context.</param>
        /// <param name="viewName">The name of the requested view.</param>
        /// <param name="masterName">An ignored parameter indicating the name of the master page..</param>
        /// <param name="useCache">Indicates whether or not a view-location cache should be used or not.</param>
        /// <returns>A <see cref="ViewEngineResult" /> indicating whether the view was found or not.</returns>
        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var controllerName = controllerContext.RouteData.GetRequiredString("controller");
            return FindView(controllerName, viewName, controllerContext.HttpContext);
        }

        void IViewEngine.ReleaseView(ControllerContext controllerContext, IView view) { /* Intentional no-op */ }

        ViewEngineResult FindView(string controllerName, string viewName, HttpContextBase context)
        {
            var viewFinder = GetViewFinder(context);
            var findResult = viewFinder.FindView(controllerName, viewName, viewLocationFormats);
            
            if(!findResult.Success)
                return NotFound(findResult.AttemptedLocations);

            var view = new ZptSharpView(findResult.Path, host, viewsPath, config, configProvider, errorStreamProvider);
            return Found(view);
        }

        ViewEngineResult Found(ZptSharpView view)
        {
            return new ViewEngineResult(view, this);
        }

        static ViewEngineResult NotFound(IEnumerable<string> attemptedLocations = null)
        {
            return new ViewEngineResult(attemptedLocations ?? Enumerable.Empty<string>());
        }

        IFindsView GetViewFinder(HttpContextBase context)
        {
            if(defaultViewFinder != null) return defaultViewFinder;
            return new ViewFinder(new ServerLocationMapper(context), new FileExistenceTester());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptSharpViewEngine"/> class.
        /// </summary>
        /// <param name="builderAction">A builder action for creating the ZptSharp hosting environment.</param>
        /// <param name="viewLocationFormats">View location formats.</param>
        /// <param name="viewsPath">The virtual path for the <c>Views</c> context variable.</param>
        /// <param name="config">An optional rendering config instance.</param>
        /// <param name="viewFinder">
        /// An optional view-finder instance.  This parameter is provided primarily for unit-testing
        /// purposes; if omitted or passed as <see langword="null" /> then a default instance will
        /// be used.
        /// </param>
        public ZptSharpViewEngine(Action<IBuildsHostingEnvironment> builderAction,
                                  string[] viewLocationFormats = null,
                                  string viewsPath = DefaultViewsPath,
                                  RenderingConfig config = null,
                                  IFindsView viewFinder = null)
        {
            this.viewsPath = viewsPath ?? throw new ArgumentNullException(nameof(viewsPath));
            this.config = config ?? RenderingConfig.Default;
            defaultViewFinder = viewFinder;
            this.viewLocationFormats = viewLocationFormats ?? defaultViewLocationFormats;

            host = ZptSharpHost.GetHost(builderAction ?? throw new ArgumentNullException(nameof(builderAction)));
            configProvider = new MvcRenderingConfigProvider();
            errorStreamProvider = new ZptSharpErrorView(host.DocumentRendererForPathFactory);
        }
    }
}