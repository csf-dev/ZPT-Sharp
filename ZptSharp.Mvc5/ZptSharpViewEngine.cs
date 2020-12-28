using System;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Mvc
{
    /// <summary>
    /// The ZptSharp MVC5 ViewEngine.  Note that this view engine does not support partial views or master pages.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If <see cref="CreatePartialView(ControllerContext, string)"/> is used, then this will be rendered and returned
    /// in exactly the same way as a normal view.
    /// </para>
    /// <para>
    /// Additionally, the <c>masterPath</c> parameter of <see cref="CreateView(ControllerContext, string, string)"/>
    /// is ignored and never used.
    /// </para>
    /// <para>
    /// Unfortunately this class has no test coverage.  That's because it is essentially impossible to provide
    /// test coverage for an MVC5 view engine which inherits from <see cref="VirtualPathProviderViewEngine"/>.
    /// Some of the internals of that class are tied to the MVC framework and thus to IIS.  So the barrier to
    /// usefully substituting them with test fakes is too high.
    /// </para>
    /// </remarks>
    public class ZptSharpViewEngine : VirtualPathProviderViewEngine
    {
        static readonly string[] defaultViewLocationFormats =
        {
            "~/Views/{1}/{0}.pt",
            "~/Views/Shared/{0}.pt",
            "~/Views/{1}/{0}.html",
            "~/Views/Shared/{0}.html",
        };

        internal const string DefaultViewsPath = "~/Views/";

        readonly IServiceProvider serviceProvider;
        readonly string viewsPath;

        /// <summary>Creates the specified partial view by using the specified controller context.</summary>
        /// <returns>A reference to the partial view.</returns>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="partialPath">The partial path for the new partial view.</param>
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
            => CreateView(controllerContext, partialPath);

        /// <summary>Creates the specified view by using the controller context, path of the view, and path of the master view.</summary>
        /// <returns>A reference to the view.</returns>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewPath">The path of the view.</param>
        /// <param name="masterPath">The path of the master view.</param>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
            => CreateView(controllerContext, viewPath);

        IView CreateView(ControllerContext controllerContext, string viewPath)
        {
            var filePath = controllerContext.HttpContext.Server.MapPath(viewPath);
            return new ZptSharpView(filePath, serviceProvider, viewPath);
        }

        void InitialiseViewLocations(string[] viewLocationFormats)
        {
            ViewLocationFormats = viewLocationFormats ?? defaultViewLocationFormats;
            PartialViewLocationFormats = viewLocationFormats ?? defaultViewLocationFormats;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptSharpViewEngine"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="viewLocationFormats">View location formats.</param>
        /// <param name="viewsPath">The virtual path for the <c>Views</c> context variable.</param>
        public ZptSharpViewEngine(IServiceProvider serviceProvider,
                                  string[] viewLocationFormats = null,
                                  string viewsPath = DefaultViewsPath)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.viewsPath = viewsPath ?? throw new ArgumentNullException(nameof(viewsPath));
            InitialiseViewLocations(viewLocationFormats);
        }
    }
}
