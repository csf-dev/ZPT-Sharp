using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Interface for a service which provides access to instances of <see cref="IExpressionEvaluator"/>.
  /// </summary>
  public interface IEvaluatorRegistry
  {
    /// <summary>
    /// Gets an evaluator implementation suitable for a given expression.
    /// </summary>
    /// <returns>The evaluator.</returns>
    /// <param name="expression">The expression.</param>
    IExpressionEvaluator GetEvaluator(Expression expression);
  }
}

