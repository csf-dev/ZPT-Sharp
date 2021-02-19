using System;
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
    /// Base implementation of <see cref="IGetsRootContextBuilder"/> which sets up the
    /// 'root variables' which are made available to ZPT MVC views.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class adds a number of predefined global variables to the root of the TALES context,
    /// such that they are all available in ZPT views.  Where possible, these mimic the same variables
    /// which would have been made available to Razor views.  There are some small differences between
    /// MVC versions.
    /// </para>
    /// <list type="bullet">
    /// <item>For ASP.NET MVC5 see <c>ViewPage&lt;TModel&gt;</c> at https://docs.microsoft.com/en-us/dotnet/api/system.web.mvc.viewpage-1?view=aspnet-mvc-5.2</item>
    /// <item>For ASP.NET Core MVC see <c>RazorPage&lt;TModel&gt;</c> at https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.razor.razorpage-1?view=aspnetcore-2.0</item>
    /// </list>
    /// <para>
    /// Some properties which would have been available to Razor views are excluded, especially where they
    /// are meaningless or do not make sense in the context of a ZPT view.
    /// </para>
    /// <para>
    /// Note that the source for this class uses a number of 'compiler flags' (preprocessor directives)
    /// to cater for either MVC5 or MVC Core.
    /// </para>
    /// </remarks>
    public abstract class MvcContextBuilderProviderBase : IGetsRootContextBuilder
    {
        /// <summary>
        /// Gets the view context.
        /// </summary>
        /// <value>The view context.</value>
        protected ViewContext ViewContext { get; }

        /// <summary>
        /// Gets the 'Views' path.
        /// </summary>
        /// <value>The 'Views' path.</value>
        protected string ViewsPath { get; }

        /// <summary>
        /// Gets the root context builder callback.
        /// </summary>
        /// <returns>The root context builder.</returns>
        public Action<IConfiguresRootContext, IServiceProvider> GetRootContextBuilder() => BuildContext;

        /// <summary>
        /// Builds/adds-to the root TALES context using a helper.
        /// </summary>
        /// <param name="helper">The helper object for adding to the root TALES context.</param>
        /// <param name="serviceProvider">A service provider.</param>
        protected virtual void BuildContext(IConfiguresRootContext helper, IServiceProvider serviceProvider)
        {
            helper.AddToRootContext("Context", ViewContext.HttpContext);
            helper.AddToRootContext("Model", ViewContext.ViewData?.Model);
            helper.AddToRootContext("Request", ViewContext.HttpContext.Request);
            helper.AddToRootContext("request", ViewContext.HttpContext.Request);
            helper.AddToRootContext("Response", ViewContext.HttpContext.Response);
            helper.AddToRootContext("RouteData", ViewContext.RouteData);
            helper.AddToRootContext("TempData", ViewContext.TempData);
            helper.AddToRootContext("Url", GetUrl());
            helper.AddToRootContext("User", ViewContext.HttpContext.User);
            helper.AddToRootContext("ViewBag", ViewContext.ViewBag);
            helper.AddToRootContext("ViewContext", ViewContext);
            helper.AddToRootContext("ViewData", ViewContext.ViewData);
            helper.AddToRootContext("Views",  GetViewsObject());
        }

        /// <summary>
        /// Gets the Uri to the current HTTP request.
        /// </summary>
        /// <returns>The request Uri.</returns>
        protected abstract Uri GetUrl();

        /// <summary>
        /// Gets an object which shall be used to fulfil the 'Views' global variable.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The views object provides access to the root directory which contains all of
        /// the views for the application.
        /// </para>
        /// </remarks>
        /// <returns>The views object.</returns>
        protected abstract object GetViewsObject();

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcContextBuilderProviderBase"/> class.
        /// </summary>
        /// <param name="viewContext">View context.</param>
        /// <param name="viewsPath">Views path.</param>
        protected MvcContextBuilderProviderBase(ViewContext viewContext, string viewsPath)
        {
            ViewContext = viewContext ?? throw new ArgumentNullException(nameof(viewContext));
            ViewsPath = viewsPath ?? throw new ArgumentNullException(nameof(viewsPath));
        }
    }
}
