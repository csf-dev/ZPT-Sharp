﻿using System;
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
  public class TalesModel : Model
  {
    #region constants

    private const string CONTEXTS = "CONTEXTS";

    #endregion

    #region fields

    private IEvaluatorRegistry _registry;

    #endregion

    #region methods

    /// <summary>
    /// Creates and returns a child <see cref="TalesModel"/> instance.
    /// </summary>
    /// <returns>The child model.</returns>
    public override Model CreateChildModel()
    {
      TalesModel output = new TalesModel(this, this.Root, _registry);
      output.RepetitionInfo = new RepetitionInfoCollection(this.RepetitionInfo);
      return output;
    }

    protected override Model CreateTypedSiblingModel()
    {
      return new TalesModel(this.Parent, this.Root, _registry);
    }

    /// <summary>
    /// Evaluate the specified expression and return the result.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    /// <param name="element">The element for which we are evaluating a result.</param>
    public override ExpressionResult Evaluate(string expression, ZptElement element)
    {
      if(expression == null)
      {
        throw new ArgumentNullException(nameof(expression));
      }

      var talesExpression = new Expression(expression);
      return this.Evaluate(talesExpression, element);
    }

    /// <summary>
    /// Evaluate the specified TALES expression and return the result.
    /// </summary>
    /// <param name="talesExpression">The TALES expression to evaluate.</param>
    /// <param name="element">The element for which we are evaluating a result.</param>
    public virtual ExpressionResult Evaluate(Expression talesExpression, ZptElement element)
    {
      if(talesExpression == null)
      {
        throw new ArgumentNullException(nameof(talesExpression));
      }
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      var evaluator = _registry.GetEvaluator(talesExpression);
      return evaluator.Evaluate(talesExpression, element, this);
    }

    /// <summary>
    /// Attempts to get a object instance from the root of the current model.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if a root item was found matching the given <paramref name="name"/>, <c>false</c> otherwise.
    /// </returns>
    /// <param name="name">The name of the desired item.</param>
    /// <param name="element">The ZPT element for which the item search is being performed.</param>
    /// <param name="result">
    /// Exposes the found object if this method returns <c>true</c>.  The value is undefined if this method returns
    /// <c>false</c>.
    /// </param>
    public virtual bool TryGetRootObject(string name, ZptElement element, out object result)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      bool output;

      if(name == CONTEXTS)
      {
        output = true;
        result = new BuiltinContextsContainer(this.GetKeywordOptions(),
                                              this.GetRepetitionSummaries(element),
                                              element.GetOriginalAttributes());
      }
      else
      {
        output = base.TryGetItem(name, element, out result);
      }

      if(!output)
      {
        var contexts = new BuiltinContextsContainer(this.GetKeywordOptions(),
                                                    this.GetRepetitionSummaries(element),
                                                    element.GetOriginalAttributes());
        output = contexts.HandleTalesPath(name, out result);
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TalesModel"/> class.
    /// </summary>
    /// <param name="evaluatorRegistry">Evaluator registry.</param>
    /// <param name="options">Options.</param>
    public TalesModel(IEvaluatorRegistry evaluatorRegistry,
                      TemplateKeywordOptions options = null) : base(options)
    {
      if(evaluatorRegistry == null)
      {
        throw new ArgumentNullException(nameof(evaluatorRegistry));
      }

      _registry = evaluatorRegistry;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TalesModel"/> class.
    /// </summary>
    /// <param name="parent">The parent model.</param>
    /// <param name="root">The root model.</param>
    /// <param name="evaluatorRegistry">The expression evaluator registry.</param>
    public TalesModel(Model parent,
                      Model root,
                      IEvaluatorRegistry evaluatorRegistry) : base(parent, root)
    {
      if(evaluatorRegistry == null)
      {
        throw new ArgumentNullException(nameof(evaluatorRegistry));
      }

      _registry = evaluatorRegistry;
    }

    #endregion
  }
}

