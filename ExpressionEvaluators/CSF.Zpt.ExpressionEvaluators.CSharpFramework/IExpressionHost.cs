using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpFramework
{
  /// <summary>
  /// Interface for the dynamically-compiled (generated) type which hosts the CSharp expression logic.s
  /// </summary>
  public interface IExpressionHost
  {
    /// <summary>
    /// Sets the value of a single variable (a value which is in-scope for the evaluation of the current expression).
    /// </summary>
    /// <param name="name">The variable name.</param>
    /// <param name="value">The value.</param>
    void SetVariableValue(string name, object value);

    /// <summary>
    /// Evaluates this expression and returns the result.
    /// </summary>
    object Evaluate();
  }
}

