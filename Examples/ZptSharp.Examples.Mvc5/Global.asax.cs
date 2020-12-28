using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ZptSharp.Examples
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var zptSharpViewEngine = GetZptSharpViewEngine();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(zptSharpViewEngine);
        }

        IViewEngine GetZptSharpViewEngine()
        {
            var services = new ServiceCollection();
            services
                .AddZptSharp()
                .AddHapZptDocuments()
                .AddZptSharpMvc5ViewEngine()
                .AddLogging();

            var provider = services.BuildServiceProvider();
            provider
                .UseHapZptDocuments()
                .UseStandardZptExpressions();

            return provider.GetZptSharpMvc5ViewEngine();
        }
    }
}
