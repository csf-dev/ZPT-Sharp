using System;
using System.Collections.Generic;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Interface for a type which provides metadata about the available concrete implementations of
  /// <see cref="IExpressionEvaluator"/>.
  /// </summary>
  public interface IExpressionEvaluatorProvider
  {
    /// <summary>
    /// Gets the metadata about all of the available evaluators.
    /// </summary>
    /// <returns>A collection of evaluator metadata instances.</returns>
    IEnumerable<ExpressionEvaluatorMetadata> GetAllEvaluatorMetadata();
  }
}

