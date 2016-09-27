using System;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;
using System.Collections.Generic;
using System.Web.Mvc;

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

    public override IModel CreateChildModel()
    {
      var output = new MvcTalesModel(this, this.Root, EvaluatorRegistry, model: this.ModelObject);
      return output;
    }

    protected override Model CreateTypedSiblingModel()
    {
      return new MvcTalesModel(this.Parent, this.Root, EvaluatorRegistry, model: this.ModelObject);
    }

    #endregion

    #region constructor

    public MvcTalesModel(IEvaluatorRegistry evaluatorRegistry,
                         NamedObjectWrapper options = null,
                         IExpressionFactory expressionCreator = null,
                         object model = null) : base(evaluatorRegistry,
                                                     options,
                                                     expressionCreator,
                                                     model) {}

    public MvcTalesModel(IModel parent,
                         IModel root,
                         IEvaluatorRegistry evaluatorRegistry,
                         IExpressionFactory expressionCreator = null,
                         object model = null) : base(parent,
                                                     root,
                                                     evaluatorRegistry,
                                                     expressionCreator,
                                                     model) {}

    #endregion
  }
}

