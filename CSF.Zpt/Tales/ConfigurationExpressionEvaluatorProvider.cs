using System;
using System.Collections.Generic;
using CSF.Configuration;
using System.Linq;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Implementation of <see cref="IExpressionEvaluatorProvider"/> which uses the application configuration.
  /// </summary>
  public class ConfigurationExpressionEvaluatorProvider : IExpressionEvaluatorProvider
  {
    #region fields

    private static ExpressionEvaluatorConfiguration _config;

    #endregion

    #region methods

    /// <summary>
    /// Gets the metadata about all of the available evaluators.
    /// </summary>
    /// <returns>A collection of evaluator metadata instances.</returns>
    public IEnumerable<ExpressionEvaluatorMetadata> GetAllEvaluatorMetadata()
    {
      return _config.Implementations
        .Cast<Implementation>()
        .Select(x => new ExpressionEvaluatorMetadata(Type.GetType(x.TypeName), x.IsDefault))
        .ToArray();
    }

    #endregion

    #region constructor

    static ConfigurationExpressionEvaluatorProvider()
    {
      _config = ConfigurationHelper.GetSection<ExpressionEvaluatorConfiguration>();
    }

    #endregion
  }
}

