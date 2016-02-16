﻿using System;
using CSF.Zpt.Rendering;
using System.Collections;
using System.Linq;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Implementation of <see cref="ExpressionEvaluatorBase"/> which evaluates a 'child' expression, converts the result to
  /// boolean and then negates it.
  /// </summary>
  public class NotExpressionEvaluator : ExpressionEvaluatorBase
  {
    #region constants

    private static readonly string Prefix = "not";

    #endregion

    #region properties

    /// <summary>
    /// Gets the expression prefix handled by the current evaluator instance.
    /// </summary>
    /// <value>The prefix.</value>
    public override string ExpressionPrefix
    {
      get {
        return Prefix;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Evaluate the specified expression, for the given element and model.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    /// <param name="element">The <see cref="ZptElement"/> for which the expression is being evaluated.</param>
    /// <param name="model">The ZPT model, providing the context for evaluation.</param>
    public override ExpressionResult Evaluate(Expression expression, ZptElement element, TalesModel model)
    {
      if(expression == null)
      {
        throw new ArgumentNullException("expression");
      }
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      var result = model.Evaluate(expression.GetContentAsExpression(), element);
      bool booleanResult = this.CoerceToBoolean(result);

      return new ExpressionResult(!booleanResult);
    }

    /// <summary>
    /// Coerces an expression result to a boolean value.
    /// </summary>
    /// <returns><c>true</c>, if the expression result indicates truth, <c>false</c> otherwise.</returns>
    /// <param name="result">An expression result.</param>
    private bool CoerceToBoolean(ExpressionResult result)
    {
      if(result == null)
      {
        throw new ArgumentNullException("result");
      }

      bool output;

      if(result.CancelsAction)
      {
        output = false;
      }
      else if(result.Value == null)
      {
        output = false;
      }
      else if(result.Value is bool)
      {
        output = (bool) result.Value;
      }
      else if(result.Value.Equals(0))
      {
        output = false;
      }
      else if(result.Value is IEnumerable)
      {
        var enumerableValue = ((IEnumerable) result.Value).Cast<object>();
        output = enumerableValue.Any();
      }
      else if(result.Value is ITalesConvertible)
      {
        var convertibleValue = (ITalesConvertible) result.Value;
        output = convertibleValue.AsBoolean();
      }
      else
      {
        output = true;
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.NotExpressionEvaluator"/> class.
    /// </summary>
    /// <param name="registry">Registry.</param>
    public NotExpressionEvaluator(IEvaluatorRegistry registry) : base(registry) {}

    #endregion
  }
}
