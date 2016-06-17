
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CSF.Zpt.MVC;
using CSF.Zpt.Rendering;
using CSF.Zpt.MVC.Rendering;

namespace CSF.Zpt.MVCSample
{
  public class MvcApplication : System.Web.HttpApplication
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(
        "Default",
        "{controller}/{action}/{id}",
        new { controller = "Home", action = "Index", id = "" }
      );

    }

    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }

    protected void Application_Start()
    {
      log4net.Config.XmlConfigurator.Configure();

      AreaRegistration.RegisterAllAreas();
      RegisterGlobalFilters(GlobalFilters.Filters);
      RegisterRoutes(RouteTable.Routes);

      ViewEngines.Engines.Clear();
      ViewEngines.Engines.Add(new ZptViewEngine());
    }
  }
}
