using System;
using System.Collections.Generic;
using CSF.Zpt.Tales;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Interface for a service which manages instances of <see cref="CSharpExpression"/>.
  /// </summary>
  public interface ICSharpExpressionService
  {
    /// <summary>
    /// Gets a <see cref="CSharpExpression"/> matching the given expression text and variable names.
    /// </summary>
    /// <returns>The expression.</returns>
    /// <param name="text">The expression text.</param>
    /// <param name="model">The current TALES model.</param>
    CSharpExpression GetExpression(string text, ITalesModel model);
  }
}

