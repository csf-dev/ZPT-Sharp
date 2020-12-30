using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZptSharp.Config;

namespace ZptSharp.Mvc
{
    /// <summary>
    /// Implementation of <see cref="IGetsRootContextBuilder"/> which sets up the
    /// 'root variables' which are made available to ZPT MVC5 views.
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
    public class MvcContextBuilderProvider : MvcContextBuilderProviderBase
    {
        /// <summary>
        /// Builds/adds-to the root TALES context using a helper.
        /// </summary>
        /// <param name="helper">The helper object for adding to the root TALES context.</param>
        /// <param name="serviceProvider">A service provider.</param>
        protected override void BuildContext(IConfiguresRootContext helper, IServiceProvider serviceProvider)
        {
            base.BuildContext(helper, serviceProvider);

            helper.AddToRootContext("Application", GetApplicationDictionary());
            helper.AddToRootContext("Cache", ViewContext.HttpContext.Cache);
            helper.AddToRootContext("Server", ViewContext.HttpContext.Server);
            helper.AddToRootContext("Session", ViewContext.HttpContext.Session);
        }

        /// <summary>
        /// Gets the Uri to the current HTTP request.
        /// </summary>
        /// <returns>The request Uri.</returns>
        protected override Uri GetUrl() => ViewContext.HttpContext.Request.Url;

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
        protected override object GetViewsObject()
            => new TemplateDirectory(ViewContext.HttpContext.Server.MapPath(ViewsPath));

        IDictionary<string, object> GetApplicationDictionary()
        {
            var app = ViewContext.HttpContext.Application;
            if(app == null) return new Dictionary<string, object>();
            return app.AllKeys.ToDictionary(k => k, v => app[v]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcContextBuilderProvider"/> class.
        /// </summary>
        /// <param name="viewContext">View context.</param>
        /// <param name="viewsPath">Views path.</param>
        public MvcContextBuilderProvider(ViewContext viewContext, string viewsPath) : base(viewContext, viewsPath) {}
    }
}