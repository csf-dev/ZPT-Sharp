using System;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;

namespace CSF.Zpt.MVC.Tales
{
  public class MvcTalesModel : TalesModel
  {
    #region fields

    private ViewContext _viewContext;

    #endregion

    #region methods

    public virtual ViewContext ViewContext
    {
      get {
        return _viewContext;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException(nameof(value));
        }

        _viewContext = value;
      }
    }

    protected override BuiltinContextsContainer GetBuiltinContexts(IRenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      var originalAttrs = new Lazy<OriginalAttributeValuesCollection>(() => context.GetOriginalAttributes());
      return new MvcContextsContainer(this.GetKeywordOptions(),
                                      this.GetRepetitionSummaries(),
                                      originalAttrs,
                                      ViewContext);
    }

    /// <summary>
    /// Gets a collection of the current model's built-in definitions.
    /// </summary>
    /// <returns>The built-in definitions.</returns>
    protected override IDictionary<string, object> GetBuiltinDefinitions()
    {
      var output = base.GetBuiltinDefinitions();

      var viewsDirectoryPath = ViewContext.HttpContext.Server.MapPath(MvcContextsContainer.VIEWS_VIRTUAL_PATH);
      var viewsDirectory = new TemplateDirectory(new DirectoryInfo(viewsDirectoryPath));

      var appDictionary = MvcContextsContainer.GetApplicationDictionary(ViewContext).Value;

      output.Add(MvcContextsContainer.VIEW_CONTEXT, ViewContext);
      output.Add(MvcContextsContainer.VIEW_DATA_DICTIONARY, new NamedObjectWrapper(ViewContext.ViewData));
      output.Add(MvcContextsContainer.TEMP_DATA_DICTIONARY, new NamedObjectWrapper(ViewContext.TempData));
      output.Add(MvcContextsContainer.APPLICATION_DICTIONARY, new NamedObjectWrapper(appDictionary));
      output.Add(MvcContextsContainer.CACHE_DICTIONARY,ViewContext.HttpContext?.Cache);
      output.Add(MvcContextsContainer.REQUEST, ViewContext.HttpContext?.Request);
      output.Add(MvcContextsContainer.REQUEST_LOWER, ViewContext.HttpContext?.Request);
      output.Add(MvcContextsContainer.RESPONSE, ViewContext.HttpContext?.Response);
      output.Add(MvcContextsContainer.ROUTE_DATA, ViewContext.RouteData);
      output.Add(MvcContextsContainer.SERVER, ViewContext.HttpContext?.Server);
      output.Add(MvcContextsContainer.SESSION_DICTIONARY, ViewContext.HttpContext?.Session);
      output.Add(MvcContextsContainer.TYPED_MODEL, ViewContext.ViewData?.Model);
      output.Add(MvcContextsContainer.VIEWS_DIRECTORY, viewsDirectory);
      output.Add(MvcContextsContainer.FORM_CONTEXT, ViewContext.FormContext);
      output.Add(MvcContextsContainer.VIEW_BAG, ViewContext.ViewBag);

      return output;
    }

    public override IModel CreateChildModel()
    {
      return new MvcTalesModel(this, this.Root, EvaluatorRegistry, model: this.ModelObject) {
        ViewContext = ViewContext
      };
    }

    protected override Model CreateTypedSiblingModel()
    {
      return new MvcTalesModel(this.Parent, this.Root, EvaluatorRegistry, model: this.ModelObject) {
        ViewContext = ViewContext
      };
    }

    #endregion

    #region constructor

    public MvcTalesModel(IEvaluatorSelector evaluatorRegistry,
                         NamedObjectWrapper options = null,
                         IExpressionFactory expressionCreator = null,
                         object model = null) : base(evaluatorRegistry,
                                                     options,
                                                     expressionCreator,
                                                     model) {}

    public MvcTalesModel(IModel parent,
                         IModel root,
                         IEvaluatorSelector evaluatorRegistry,
                         IExpressionFactory expressionCreator = null,
                         object model = null) : base(parent,
                                                     root,
                                                     evaluatorRegistry,
                                                     expressionCreator,
                                                     model) {}

    #endregion
  }
}

