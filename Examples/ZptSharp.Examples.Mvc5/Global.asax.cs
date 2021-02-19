using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ZptSharp.Examples
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var viewEngine = new ZptSharp.Mvc5.ZptSharpViewEngine(builder => {
                builder
                    .AddHapZptDocuments()
                    .AddStandardZptExpressions();
            });
            ViewEngines.Engines.Insert(0, viewEngine);
        }
    }
}
