using System;
using System.Collections.Generic;

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
    /// <param name="text">Text.</param>
    /// <param name="variableNames">Variable names.</param>
    CSharpExpression GetExpression(string text, IEnumerable<string> variableNames);
  }
}

