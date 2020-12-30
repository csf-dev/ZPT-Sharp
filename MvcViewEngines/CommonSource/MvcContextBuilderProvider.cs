using System;
#if MVC5
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
#elif MVCCORE
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http.Extensions;
#endif
using ZptSharp.Config;

namespace ZptSharp.Mvc
{
    /// <summary>
    /// Implementation of <see cref="IGetsRootContextBuilder"/> which sets up the
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
    public class MvcContextBuilderProvider : IGetsRootContextBuilder
    {
        readonly ViewContext viewContext;
        readonly string viewsPath;

        /// <summary>
        /// Gets the root context builder callback.
        /// </summary>
        /// <returns>The root context builder.</returns>
        public Action<IConfiguresRootContext, IServiceProvider> GetRootContextBuilder() => BuildContext;

        void BuildContext(IConfiguresRootContext helper, IServiceProvider serviceProvider)
        {
            helper.AddToRootContext("Context", viewContext.HttpContext);
            helper.AddToRootContext("Model", viewContext.ViewData?.Model);
            helper.AddToRootContext("Request", viewContext.HttpContext.Request);
            helper.AddToRootContext("request", viewContext.HttpContext.Request);
            helper.AddToRootContext("Response", viewContext.HttpContext.Response);
            helper.AddToRootContext("RouteData", viewContext.RouteData);
            helper.AddToRootContext("TempData", viewContext.TempData);
            helper.AddToRootContext("Url", GetUrl());
            helper.AddToRootContext("User", viewContext.HttpContext.User);
            helper.AddToRootContext("ViewBag", viewContext.ViewBag);
            helper.AddToRootContext("ViewContext", viewContext);
            helper.AddToRootContext("ViewData", viewContext.ViewData);
            helper.AddToRootContext("Views",  GetViewsObject());

#if MVC5
            AddMvc5OnlyVariables(helper);
#endif
        }

        Uri GetUrl()
        {
#if MVC5
            return viewContext.HttpContext.Request.Url;
#elif MVCCORE
            return new Uri(viewContext.HttpContext.Request.GetEncodedUrl());
#endif
        }

        object GetViewsObject()
        {
#if MVC5
            var absolutePath = viewContext.HttpContext.Server.MapPath(viewsPath);
#elif MVCCORE
            var absolutePath = viewsPath;
#endif
            return new TemplateDirectory(absolutePath);
        }

#if MVC5
        void AddMvc5OnlyVariables(IConfiguresRootContext helper)
        {
            helper.AddToRootContext("Application", GetApplicationDictionary());
            helper.AddToRootContext("Cache", viewContext.HttpContext.Cache);
            helper.AddToRootContext("Server", viewContext.HttpContext.Server);
            helper.AddToRootContext("Session", viewContext.HttpContext.Session);
        }

        IDictionary<string, object> GetApplicationDictionary()
        {
            var app = viewContext.HttpContext.Application;
            if(app == null) return new Dictionary<string, object>();
            return app.AllKeys.ToDictionary(k => k, v => app[v]);
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcContextBuilderProvider"/> class.
        /// </summary>
        /// <param name="viewContext">View context.</param>
        /// <param name="viewsPath">Views path.</param>
        public MvcContextBuilderProvider(ViewContext viewContext, string viewsPath)
        {
            this.viewContext = viewContext ?? throw new ArgumentNullException(nameof(viewContext));
            this.viewsPath = viewsPath ?? throw new ArgumentNullException(nameof(viewsPath));
        }
    }
}
