using System;
using CSF.Zpt.Rendering;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// An implementation of <see cref="Model"/> which makes use of the TALES: Template Attribute Language Expression
  /// Syntax for evaluating expressions.
  /// </summary>
  public class TalesModel : Model, ITalesModel
  {
    #region constants

    private const string CONTEXTS = "CONTEXTS";

    #endregion

    #region fields

    private IEvaluatorSelector _registry;
    private IExpressionFactory _expressionCreator;

    #endregion

    #region properties

    /// <summary>
    /// Gets the evaluator registry.
    /// </summary>
    /// <value>The evaluator registry.</value>
    protected virtual IEvaluatorSelector EvaluatorRegistry
    {
      get {
        return _registry;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Creates and returns a child <see cref="TalesModel"/> instance.
    /// </summary>
    /// <returns>The child model.</returns>
    public override IModel CreateChildModel()
    {
      var output = new TalesModel(this, this.Root, EvaluatorRegistry, modelObject: this.ModelObject);
      return output;
    }

    /// <summary>
    /// Creates an instance of <see cref="TalesModel"/>.
    /// </summary>
    /// <returns>The sibling model.</returns>
    protected override Model CreateTypedSiblingModel()
    {
      return new TalesModel(this.Parent, this.Root, EvaluatorRegistry, modelObject: this.ModelObject);
    }

    /// <summary>
    /// Evaluate the specified expression and return the result.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    /// <param name="context">The rendering context for which we are evaluating a result.</param>
    public override ExpressionResult Evaluate(string expression, IRenderingContext context)
    {
      if(expression == null)
      {
        throw new ArgumentNullException(nameof(expression));
      }

      var talesExpression = _expressionCreator.Create(expression);
      return this.Evaluate(talesExpression, context);
    }

    /// <summary>
    /// Evaluate the specified TALES expression and return the result.
    /// </summary>
    /// <param name="talesExpression">The TALES expression to evaluate.</param>
    /// <param name="context">The rendering context for which we are evaluating a result.</param>
    public virtual ExpressionResult Evaluate(Expression talesExpression, IRenderingContext context)
    {
      if(talesExpression == null)
      {
        throw new ArgumentNullException(nameof(talesExpression));
      }
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      var evaluator = EvaluatorRegistry.GetEvaluator(talesExpression);
      var output = evaluator.Evaluate(talesExpression, context, this);

      ZptConstants.TraceSource.TraceEvent(System.Diagnostics.TraceEventType.Verbose,
                                          4,
                                          Resources.LogMessageFormats.ExpressionEvaluated,
                                          talesExpression.ToString(),
                                          output.Value?? "<null>",
                                          nameof(TalesModel),
                                          nameof(Evaluate));

      return output;
    }

    /// <summary>
    /// Attempts to get a object instance from the root of the current model.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if a root item was found matching the given <paramref name="name"/>, <c>false</c> otherwise.
    /// </returns>
    /// <param name="name">The name of the desired item.</param>
    /// <param name="context">The rendering context for which the item search is being performed.</param>
    /// <param name="result">
    /// Exposes the found object if this method returns <c>true</c>.  The value is undefined if this method returns
    /// <c>false</c>.
    /// </param>
    public virtual bool TryGetRootObject(string name, IRenderingContext context, out object result)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      bool output;

      if(name == CONTEXTS)
      {
        result = GetBuiltinContexts(context);
        output = true;
      }
      else
      {
        output = base.TryGetItem(name, context, out result);
      }

      if(!output)
      {
        var contexts = GetBuiltinContexts(context);
        output = contexts.HandleTalesPath(name, out result, context);
      }

      return output;
    }

    /// <summary>
    /// Attempts to get a object instance from the root of the current model, but only searches local variable
    /// definitions.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if a root item was found matching the given <paramref name="name"/>, <c>false</c> otherwise.
    /// </returns>
    /// <param name="name">The name of the desired item.</param>
    /// <param name="element">The ZPT element.</param>
    /// <param name="result">
    /// Exposes the found object if this method returns <c>true</c>.  The value is undefined if this method returns
    /// <c>false</c>.
    /// </param>
    public virtual bool TryGetLocalRootObject(string name, IZptElement element, out object result)
    {

      bool output;

      if(base.LocalDefinitions.ContainsKey(name))
      {
        output = true;
        result = base.LocalDefinitions[name];
      }
      else
      {
        output = false;
        result = null;
      }

      return output;
    }

    /// <summary>
    /// Gets the builtin contexts, associated with the <c>CONTEXTS</c> root path keyword.
    /// </summary>
    /// <returns>The builtin contexts.</returns>
    /// <param name="context">The current rendering context.</param>
    protected virtual BuiltinContextsContainer GetBuiltinContexts(IRenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      var originalAttrs = new Lazy<OriginalAttributeValuesCollection>(() => context.GetOriginalAttributes());
      return new BuiltinContextsContainer(this.GetKeywordOptions(),
                                          this.GetRepetitionSummaries(),
                                          originalAttrs,
                                          templateFileFactory: context.RenderingOptions.GetTemplateFileFactory(),
                                          model: this.ModelObject);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TalesModel"/> class.
    /// </summary>
    /// <param name="evaluatorRegistry">Evaluator registry.</param>
    /// <param name="options">Options.</param>
    /// <param name="expressionCreator">The expression factory to use.</param>
    /// <param name="modelObject">An object to which the ZPT document is to be applied.</param>
    public TalesModel(IEvaluatorSelector evaluatorRegistry,
                      NamedObjectWrapper options = null,
                      IExpressionFactory expressionCreator = null,
                      object modelObject = null) : base(options, modelObject)
    {
      if(evaluatorRegistry == null)
      {
        throw new ArgumentNullException(nameof(evaluatorRegistry));
      }

      _registry = evaluatorRegistry;
      _expressionCreator = expressionCreator?? new ExpressionFactory();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TalesModel"/> class.
    /// </summary>
    /// <param name="parent">The parent model.</param>
    /// <param name="root">The root model.</param>
    /// <param name="evaluatorRegistry">The expression evaluator registry.</param>
    /// <param name="expressionCreator">The expression factory to use.</param>
    /// <param name="modelObject">An object to which the ZPT document is to be applied.</param>
    public TalesModel(IModel parent,
                      IModel root,
                      IEvaluatorSelector evaluatorRegistry,
                      IExpressionFactory expressionCreator = null,
                      object modelObject = null) : base(parent, root, modelObject)
    {
      if(evaluatorRegistry == null)
      {
        throw new ArgumentNullException(nameof(evaluatorRegistry));
      }

      _registry = evaluatorRegistry;
      _expressionCreator = expressionCreator?? new ExpressionFactory();
    }

    #endregion
  }
}

