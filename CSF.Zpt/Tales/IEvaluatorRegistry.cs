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

    /// <summary>
    /// Gets an expression evaluator matching a desired type.
    /// </summary>
    /// <returns>The evaluator instance.</returns>
    /// <typeparam name="TEvaluator">The desired evaluator type.</typeparam>
    IExpressionEvaluator GetEvaluator<TEvaluator>() where TEvaluator : IExpressionEvaluator;

    /// <summary>
    /// Gets an expression evaluator matching a desired type.
    /// </summary>
    /// <returns>The evaluator instance.</returns>
    /// <param name="evaluatorType">The desired evaluator type.</param>
    IExpressionEvaluator GetEvaluator(Type evaluatorType);
  }
}

