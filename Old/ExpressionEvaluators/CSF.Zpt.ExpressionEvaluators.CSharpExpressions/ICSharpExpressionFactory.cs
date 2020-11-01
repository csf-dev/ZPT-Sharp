using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Represents a factory instance capable of creating <see cref="CSharpExpression"/> instances.
  /// </summary>
  public interface ICSharpExpressionFactory
  {
    /// <summary>
    /// Creates an expression from the given information
    /// </summary>
    /// <param name="model">Information representing the creation of a CSharp expression.</param>
    CSharpExpression Create(ExpressionModel model);
  }
}

