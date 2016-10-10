using System;
using System.Collections.Generic;

namespace CSF.Zpt
{
  /// <summary>
  /// Interface for a type capable of providing information about installed plugins.
  /// </summary>
  public interface IPluginConfiguration
  {
    /// <summary>
    /// Gets all of the installed document provider types.
    /// </summary>
    /// <returns>The all document provider types.</returns>
    IEnumerable<Type> GetAllDocumentProviderTypes();

    /// <summary>
    /// Gets all of the installed expression evaluator types.
    /// </summary>
    /// <returns>The all expression evaluator types.</returns>
    IEnumerable<Type> GetAllExpressionEvaluatorTypes();

    /// <summary>
    /// Gets the default HTML document provider.
    /// </summary>
    /// <returns>The default HTML document provider.</returns>
    Type GetDefaultHtmlDocumentProvider();

    /// <summary>
    /// Gets the default XML document provider.
    /// </summary>
    /// <returns>The default XML document provider.</returns>
    Type GetDefaultXmlDocumentProvider();

    /// <summary>
    /// Gets the default expression evaluator.
    /// </summary>
    /// <returns>The default expression evaluator.</returns>
    Type GetDefaultExpressionEvaluator();
  }
}

