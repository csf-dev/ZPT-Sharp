using System;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;
using System.Collections.Generic;

namespace CSF.Zpt.MVC.Tales
{
  public class MvcTalesModel : TalesModel
  {
    #region fields

    private bool _viewDataSet, _tempDataSet;

    #endregion

    #region properties

    protected virtual IDictionary<string,object> ViewData
    {
      get;
      set;
    }

    protected virtual IDictionary<string,object> TempData
    {
      get;
      set;
    }

    #endregion

    #region methods

    public virtual void SetViewData(IDictionary<string,object> data)
    {
      if(_viewDataSet)
      {
        throw new InvalidOperationException();
      }

      if(data != null)
      {
        foreach(var key in data.Keys)
        {
          AddGlobal(key, data[key]);
        }
      }
      ViewData = data;

      _viewDataSet = true;
    }

    public virtual void SetTempData(IDictionary<string,object> data)
    {
      if(_tempDataSet)
      {
        throw new InvalidOperationException();
      }

      if(data != null)
      {
        foreach(var key in data.Keys)
        {
          AddGlobal(key, data[key]);
        }
      }
      TempData = data;

      _tempDataSet = true;
    }

    protected override BuiltinContextsContainer GetBuiltinContexts(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      var originalAttrs = new Lazy<OriginalAttributeValuesCollection>(() => context.GetOriginalAttributes());
      return new MvcContextsContainer(this.GetKeywordOptions(),
                                      this.GetRepetitionSummaries(context.Element),
                                      originalAttrs,
                                      ViewData,
                                      TempData);
    }

    public override IModel CreateChildModel()
    {
      var output = new MvcTalesModel(this, this.Root, EvaluatorRegistry);
      output.RepetitionInfo = new RepetitionInfoCollection(this.RepetitionInfo);
      return output;
    }

    protected override Model CreateTypedSiblingModel()
    {
      return new MvcTalesModel(this.Parent, this.Root, EvaluatorRegistry);
    }

    #endregion

    #region constructor

    public MvcTalesModel(IEvaluatorRegistry evaluatorRegistry,
                         NamedObjectWrapper options = null,
                         IExpressionFactory expressionCreator = null) : base(evaluatorRegistry,
                                                                             options,
                                                                             expressionCreator) {}

    public MvcTalesModel(IModel parent,
                         IModel root,
                         IEvaluatorRegistry evaluatorRegistry,
                         IExpressionFactory expressionCreator = null) : base(parent,
                                                                             root,
                                                                             evaluatorRegistry,
                                                                             expressionCreator) {}

    #endregion
  }
}

