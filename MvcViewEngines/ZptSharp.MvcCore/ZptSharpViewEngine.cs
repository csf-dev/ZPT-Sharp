using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ZptSharp.Config;
using ZptSharp.Hosting;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ZptSharp.Mvc
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
        const string viewPathSanitiserPattern = "^(~/|/)";
        static readonly Regex viewPathSanitiser = new Regex(viewPathSanitiserPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        internal const string DefaultViewsPath = "Views/";

        static readonly string[] defaultViewLocationFormats =
        {
            "Views/{1}/{0}.pt",
            "Views/Shared/{0}.pt",
            "Views/{1}/{0}.html",
            "Views/Shared/{0}.html",
        };

        readonly string[] viewLocationFormats;

        readonly IHostsZptSharp host;
        readonly string viewsPath;
        readonly RenderingConfig config;
        readonly ITestForFileExistence fileTester;
        readonly IFindsView viewFinder;
        readonly IGetsMvcRenderingConfig configProvider;
        readonly IGetsErrorStream errorStreamProvider;
        
        /// <summary>
        /// Gets a <see cref="ViewEngineResult" /> from an attempt to find a ZptSharp view of the specified
        /// <paramref name="viewName" />.
        /// </summary>
        /// <param name="context">The current action context.</param>
        /// <param name="viewName">The name of the requested view.</param>
        /// <param name="isMainPage">An ignored parameter indicating whether the requested view is the main page.</param>
        /// <returns>A <see cref="ViewEngineResult" /> indicating whether the view was found or not.</returns>
        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
        {
            if(!context.ActionDescriptor.RouteValues.TryGetValue("controller", out var controllerName))
                throw new ArgumentException(Resources.ExceptionMessage.ContextMustContainControllerRouteValue, nameof(context));

            var findResult = viewFinder.FindView(controllerName, viewName, viewLocationFormats);
            if(!findResult.Success)
                return NotFound(viewName, findResult.AttemptedLocations);

            var view = new ZptSharpView(findResult.Path, host, viewsPath, config, configProvider, errorStreamProvider);
            return Found(viewName, view);
        }

        /// <summary>
        /// Gets a <see cref="ViewEngineResult" /> from an attempt to get a ZptSharp view at the specified
        /// <paramref name="viewPath" />.
        /// </summary>
        /// <param name="executingFilePath">A path for the executing file.  Used as context in order to get an application-relative path for a non-rooted view path.</param>
        /// <param name="viewPath">The path to the requested view.</param>
        /// <param name="isMainPage">An ignored parameter indicating whether the requested view is the main page.</param>
        /// <returns>A <see cref="ViewEngineResult" /> indicating whether the view was found or not.</returns>
        public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
        {
            if(String.IsNullOrEmpty(viewPath))
                return NotFound(viewPath);
            
            var mappedPath = GetApplicationRelativeViewPath(viewPath, executingFilePath);
            if(!fileTester.DoesFileExist(mappedPath))
                return NotFound(viewPath, new List<string>{ mappedPath });

            var view = new ZptSharpView(mappedPath, host, viewsPath, config, configProvider, errorStreamProvider);
            return Found(viewPath, view);
        }

        static string GetApplicationRelativeViewPath(string viewPath, string referencePath)
        {
            if (IsAlreadyApplicationRelative(viewPath)) return viewPathSanitiser.Replace(viewPath, String.Empty);
            if(string.IsNullOrEmpty(referencePath)) return viewPath;

            var index = referencePath.LastIndexOf('/');
            return referencePath.Substring(0, index + 1) + viewPath;
        }

        static bool IsAlreadyApplicationRelative(string name)
            => viewPathSanitiser.IsMatch(name);

        static ViewEngineResult Found(string viewName, ZptSharpView view)
            => ViewEngineResult.Found(viewName, view);

        static ViewEngineResult NotFound(string viewName, IEnumerable<string> attemptedLocations = null)
            => ViewEngineResult.NotFound(viewName, attemptedLocations ?? Enumerable.Empty<string>());

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptSharpViewEngine"/> class.
        /// </summary>
        /// <param name="builderAction">A builder action for creating the ZptSharp hosting environment.</param>
        /// <param name="viewLocationFormats">View location formats.</param>
        /// <param name="viewsPath">The virtual path for the <c>Views</c> context variable.</param>
        /// <param name="config">An optional rendering config instance.</param>
        /// <param name="fileTester">
        /// An optional file-tester instance.  This parameter is provided primarily for unit-testing
        /// purposes; if omitted or passed as <see langword="null" /> then a default instance will
        /// be used.
        /// </param>
        /// <param name="viewFinder">
        /// An optional view-finder instance.  This parameter is provided primarily for unit-testing
        /// purposes; if omitted or passed as <see langword="null" /> then a default instance will
        /// be used.
        /// </param>
        public ZptSharpViewEngine(Action<IBuildsSelfHostingEnvironment> builderAction,
                                  string[] viewLocationFormats = null,
                                  string viewsPath = DefaultViewsPath,
                                  RenderingConfig config = null,
                                  ITestForFileExistence fileTester = null,
                                  IFindsView viewFinder = null)
        {
            this.viewsPath = viewsPath ?? throw new ArgumentNullException(nameof(viewsPath));
            this.config = config ?? RenderingConfig.Default;
            this.fileTester = fileTester ?? new FileExistenceTester();
            this.viewFinder = viewFinder ?? new ViewFinder(new NoOpLocationMapper(), this.fileTester);
            this.viewLocationFormats = viewLocationFormats ?? defaultViewLocationFormats;

            host = ZptSharpHost.GetHost(builderAction ?? throw new ArgumentNullException(nameof(builderAction)));
            configProvider = new MvcRenderingConfigProvider();
            errorStreamProvider = new ZptSharpErrorView(host.DocumentRendererForPathFactory);
        }
    }
}