using System;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;
using System.Collections.Generic;

namespace CSF.Zpt.MVC.Tales
{
  public class MvcContextsContainer : BuiltinContextsContainer
  {
    #region constants

    private const string
      VIEW_DATA = "ViewData",
      TEMP_DATA = "TempData";

    #endregion

    #region fields

    private IDictionary<string,object> _viewData, _tempData;

    #endregion

    #region properties

    public IDictionary<string,object> ViewData
    {
      get {
        return _viewData;
      }
    }

    public IDictionary<string,object> TempData
    {
      get {
        return _tempData;
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
        case VIEW_DATA:
          output = true;
          result = new NamedObjectWrapper(this.ViewData);
          break;

        case TEMP_DATA:
          output = true;
          result = new NamedObjectWrapper(this.TempData);
          break;

        default:
          output = false;
          result = null;
          break;
        }
      }

      return output;
    }

    #endregion

    #region constructor

    public MvcContextsContainer(NamedObjectWrapper options,
                                ContextualisedRepetitionSummaryWrapper repeat,
                                Lazy<OriginalAttributeValuesCollection> attrs,
                                IDictionary<string,object> viewData = null,
                                IDictionary<string,object> tempData = null) : base(options, repeat, attrs)
    {
      _viewData = viewData?? new Dictionary<string, object>();
      _tempData = tempData?? new Dictionary<string, object>();
    }

    #endregion
  }
}

