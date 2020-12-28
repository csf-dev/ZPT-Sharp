using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Examples
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var services = new ServiceCollection();
        }
    }
}
