using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZptSharp.Config;

namespace ZptSharp.Mvc
{
    /// <summary>
    /// Implementation of <see cref="IGetsRootContextBuilder"/> which gets a configuration
    /// </summary>
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
            helper.AddToRootContext("ViewContext", viewContext);
            helper.AddToRootContext("ViewData", viewContext.ViewData);
            helper.AddToRootContext("TempData", viewContext.TempData);
            helper.AddToRootContext("Application", GetApplicationDictionary());
            helper.AddToRootContext("Cache", viewContext.HttpContext.Cache);
            helper.AddToRootContext("Request", viewContext.HttpContext.Request);
            helper.AddToRootContext("Response", viewContext.HttpContext.Response);
            helper.AddToRootContext("RouteData", viewContext.RouteData);
            helper.AddToRootContext("Server", viewContext.HttpContext.Server);
            helper.AddToRootContext("Session", viewContext.HttpContext.Session);
            helper.AddToRootContext("Model", viewContext.ViewData?.Model);
            helper.AddToRootContext("request", viewContext.HttpContext.Request);
            helper.AddToRootContext("FormContext", viewContext.FormContext);
            helper.AddToRootContext("ViewBag", viewContext.ViewBag);
            helper.AddToRootContext("Views",  GetViewsObject());
        }

        IDictionary<string, object> GetApplicationDictionary()
        {
            var app = viewContext.HttpContext.Application;
            if(app == null) return new Dictionary<string, object>();
            return app.AllKeys.ToDictionary(k => k, v => app[v]);
        }

        object GetViewsObject()
        {
            var absolutePath = viewContext.HttpContext.Server.MapPath(viewsPath);
            return new TemplateDirectory(absolutePath);
        }

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
