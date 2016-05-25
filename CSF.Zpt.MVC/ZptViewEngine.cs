using System;
using System.Web.Mvc;

namespace CSF.Zpt.MVC
{
  public class ZptViewEngine : VirtualPathProviderViewEngine
  {
    #region constants

    private static readonly string[] ViewLocations = new [] {
      "~/Views/{1}/{0}.pt",
      "~/Views/Shared/{0}.pt",
      "~/Views/{1}/{0}.xml",
      "~/Views/Shared/{0}.xml",
    };

    #endregion

    #region overrides

    protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
    {
      return new ZptView(controllerContext.HttpContext.Server.MapPath(viewPath));
    }

    protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
    {
      return new ZptView(controllerContext.HttpContext.Server.MapPath(partialPath));
    }

    #endregion

    #region constructor

    public ZptViewEngine()
    {
      this.ViewLocationFormats = ViewLocations;
      this.PartialViewLocationFormats = ViewLocations;
    }

    #endregion
  }
}

