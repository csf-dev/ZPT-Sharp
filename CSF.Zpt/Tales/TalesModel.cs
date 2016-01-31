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
  public class TalesModel : Model
  {
    #region fields

    private EvaluatorRegistry _registry;

    #endregion

    #region methods

    /// <summary>
    /// Creates and returns a child <see cref="TalesModel"/> instance.
    /// </summary>
    /// <returns>The child model.</returns>
    public override Model CreateChildModel()
    {
      return new TalesModel(this, this.Root, _registry);
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
        throw new ArgumentNullException("expression");
      }

      var talesExpression = new Expression(expression);
      return this.Evaluate(talesExpression, element);
    }

    /// <summary>
    /// Evaluate the specified TALES expression and return the result.
    /// </summary>
    /// <param name="talesExpression">The TALES expression to evaluate.</param>
    /// <param name="element">The element for which we are evaluating a result.</param>
    public ExpressionResult Evaluate(Expression talesExpression, ZptElement element)
    {
      if(talesExpression == null)
      {
        throw new ArgumentNullException("talesExpression");
      }
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      var evaluator = _registry.GetEvaluator(talesExpression);
      return evaluator.Evaluate(talesExpression, element, this);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TalesModel"/> class.
    /// </summary>
    /// <param name="parent">The parent model.</param>
    /// <param name="root">The root model.</param>
    /// <param name="evaluatorRegistry">The expression evaluator registry.</param>
    public TalesModel(Model parent,
                      Model root,
                      EvaluatorRegistry evaluatorRegistry) : base(parent, root)
    {
      if(evaluatorRegistry == null)
      {
        throw new ArgumentNullException("evaluatorRegistry");
      }

      _registry = evaluatorRegistry;
    }

    #endregion
  }
}

