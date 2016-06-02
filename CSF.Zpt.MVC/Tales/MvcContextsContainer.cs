using System;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Linq;

namespace CSF.Zpt.MVC.Tales
{
  public class MvcContextsContainer : BuiltinContextsContainer
  {
    #region constants

    private const string
      VIEW_DATA_DICTIONARY    = "ViewData",
      TEMP_DATA_DICTIONARY    = "TempData",
      APPLICATION_DICTIONARY  = "Application",
      CACHE_DICTIONARY        = "Cache",
      REQUEST                 = "Request",
      RESPONSE                = "Response",
      ROUTE_DATA              = "RouteData",
      SERVER                  = "Server",
      SESSION_DICTIONARY      = "Session",
      TYPED_MODEL             = "Model",
      VIEWS_DIRECTORY         = "Views",
      VIEWS_VIRTUAL_PATH      = "~/Views/";

    #endregion

    #region fields

    private ViewContext _viewContext;
    private Lazy<IDictionary<string,object>> _applicationDictionary;

    #endregion

    #region properties

    public ViewContext ViewContext
    {
      get {
        return _viewContext;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets a built-in object by name (a TALES path fragment).
    /// </summary>
    /// <returns><c>true</c> if the path traversal was a success; <c>false</c> otherwise.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    /// <param name="result">Exposes the result if the traversal was a success</param>
    /// <param name="currentContext">Gets the current rendering context.</param>
    public override bool HandleTalesPath(string pathFragment, out object result, RenderingContext currentContext)
    {
      bool output = base.HandleTalesPath(pathFragment, out result, currentContext);

      if(!output)
      {
        switch(pathFragment)
        {
        case VIEW_DATA_DICTIONARY:
          output = ViewContext.ViewData != null;
          result = output? new NamedObjectWrapper(ViewContext.ViewData) : null;
          break;

        case TEMP_DATA_DICTIONARY:
          output = ViewContext.TempData != null;
          result = output? new NamedObjectWrapper(ViewContext.TempData) : null;
          break;

        case APPLICATION_DICTIONARY:
          result = new NamedObjectWrapper(_applicationDictionary.Value);
          output = true;
          break;

        case CACHE_DICTIONARY:
          result = ViewContext.HttpContext?.Cache;
          output = result != null;
          break;

        case REQUEST:
          result = ViewContext.HttpContext?.Request;
          output = result != null;
          break;

        case RESPONSE:
          result = ViewContext.HttpContext?.Response;
          output = result != null;
          break;

        case ROUTE_DATA:
          result = ViewContext.RouteData;
          output = result != null;
          break;

        case SERVER:
          result = ViewContext.HttpContext?.Server;
          output = result != null;
          break;

        case SESSION_DICTIONARY:
          result = ViewContext.HttpContext?.Session;
          output = result != null;
          break;

        case TYPED_MODEL:
          result = ViewContext.ViewData?.Model;
          output = (result != null);
          break;

        case VIEWS_DIRECTORY:
          var viewsDirectoryPath = ViewContext.HttpContext.Server.MapPath(VIEWS_VIRTUAL_PATH);
          result = new TemplateDirectory(new DirectoryInfo(viewsDirectoryPath));
          output = true;
          break;

        default:
          output = false;
          result = null;
          break;
        }
      }

      return output;
    }

    private Lazy<IDictionary<string,object>> GetApplicationDictionary(ViewContext context)
    {
      return new Lazy<IDictionary<string, object>>(() => {
        var app = ViewContext.HttpContext?.Application;
        return (app != null)? app.AllKeys.ToDictionary(k => k, v => app[v]) : new Dictionary<string,object>();
      });
    }

    #endregion

    #region constructor

    public MvcContextsContainer(NamedObjectWrapper options,
                                ContextualisedRepetitionSummaryWrapper repeat,
                                Lazy<OriginalAttributeValuesCollection> attrs,
                                ViewContext viewContext) : base(options, repeat, attrs)
    {
      _viewContext = viewContext?? new ViewContext();
      _applicationDictionary = GetApplicationDictionary(viewContext);
    }

    #endregion
  }
}

