using System;
using CSF.Zpt.Tales;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec
{
  /// <summary>
  /// A service which creates <see cref="ExpressionSpecification"/> instances.
  /// </summary>
  public interface IExpressionSpecificationFactory
  {
    /// <summary>
    /// Creates the expression specification from an expression text and a TALES model.
    /// </summary>
    /// <returns>The expression specification.</returns>
    /// <param name="expressionText">Expression text.</param>
    /// <param name="model">The TALES model.</param>
    ExpressionSpecification CreateExpressionSpecification(string expressionText, ITalesModel model);
  }
}

