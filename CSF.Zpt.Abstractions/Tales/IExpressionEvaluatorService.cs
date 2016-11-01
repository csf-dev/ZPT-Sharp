using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Interface for a service which exposes <see cref="IExpressionEvaluator"/> instances.
  /// </summary>
  public interface IExpressionEvaluatorService
  {
    /// <summary>
    /// Gets an evaluator by its <c>System.Type</c>.
    /// </summary>
    /// <returns>The evaluator.</returns>
    /// <param name="type">Type.</param>
    IExpressionEvaluator GetEvaluator(Type type);

    /// <summary>
    /// Gets an evaluator by its expression prefix.
    /// </summary>
    /// <returns>The evaluator.</returns>
    /// <param name="prefix">Prefix.</param>
    IExpressionEvaluator GetEvaluator(string prefix);

    /// <summary>
    /// Gets the default expression evaluator.
    /// </summary>
    /// <returns>The default evaluator.</returns>
    IExpressionEvaluator GetDefaultEvaluator();
  }
}

