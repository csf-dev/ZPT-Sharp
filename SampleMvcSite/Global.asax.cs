
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CraigFowler.Web.ZPT.Mvc;
using CraigFowler.Web.ZPT;
using CraigFowler.Samples.Mvc.Views.Home;

namespace CraigFowler.Samples.Mvc
{
  public class MvcApplication : System.Web.HttpApplication
  {
    public static void RegisterRoutes (RouteCollection routes)
    {
      routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");
      
      routes.MapRoute ("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = "" });
      
    }

    protected void Application_Start ()
    {
      RegisterRoutes (RouteTable.Routes);
      
      ZptMetadata.RegisterDocumentClass(typeof(MacroView));
      
      ViewEngines.Engines.Clear();
      ViewEngines.Engines.Add(new ZptViewEngine());
    }
  }
}

