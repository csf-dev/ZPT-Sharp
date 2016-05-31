using System;
using System.Web.Mvc;
using CSF.Zpt.Rendering;
using CSF.Zpt.MVC.Rendering;

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

    #region fields

    private RenderingMode _renderingMode;

    #endregion

    #region properties

    public RenderingOptions RenderingOptions
    {
      get;
      set;
    }

    public RenderingMode RenderingMode
    {
      get {
        return _renderingMode;
      }
      set {
        if(!value.IsDefinedValue())
        {
          throw new ArgumentException(Resources.ExceptionMessages.InvalidRenderingMode, nameof(value));
        }

        _renderingMode = value;
      }
    }

    public System.Text.Encoding InputEncoding
    {
      get;
      set;
    }

    #endregion

    #region overrides

    protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
    {
      return new ZptView(controllerContext.HttpContext.Server.MapPath(viewPath)) {
        RenderingOptions = RenderingOptions,
        RenderingMode = RenderingMode,
        InputEncoding = InputEncoding,
      };
    }

    protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
    {
      return new ZptView(controllerContext.HttpContext.Server.MapPath(partialPath)) {
        RenderingOptions = RenderingOptions,
        RenderingMode = RenderingMode,
        InputEncoding = InputEncoding,
      };
    }

    #endregion

    #region constructor

    public ZptViewEngine()
    {
      this.ViewLocationFormats = ViewLocations;
      this.PartialViewLocationFormats = ViewLocations;
      this.RenderingOptions = new DefaultRenderingOptions(contextFactory: new MvcRenderingContextFactory());
    }

    #endregion
  }
}

