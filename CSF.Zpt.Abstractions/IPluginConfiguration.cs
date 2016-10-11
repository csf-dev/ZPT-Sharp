using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSF.Zpt
{
  /// <summary>
  /// Interface for a type capable of providing information about installed plugins.
  /// </summary>
  public interface IPluginConfiguration
  {
    /// <summary>
    /// Gets a collection of all of the registered plugin assemblies.
    /// </summary>
    /// <returns>The plugin assemblies.</returns>
    IEnumerable<Assembly> GetAllPluginAssemblies();

    /// <summary>
    /// Gets the name (including namespace) of the <c>System.Type</c> which is the default HTML document provider.
    /// </summary>
    /// <returns>The default HTML document provider type name.</returns>
    string GetDefaultHtmlDocumentProviderTypeName();

    /// <summary>
    /// Gets the name (including namespace) of the <c>System.Type</c> which is the default XML document provider.
    /// </summary>
    /// <returns>The default XML document provider type name.</returns>
    string GetDefaultXmlDocumentProviderTypeName();

    /// <summary>
    /// Gets the name (including namespace) of the <c>System.Type</c> which is the default TALES expression evaluator.
    /// </summary>
    /// <returns>The default expression evaluator type name.</returns>
    string GetDefaultExpressionEvaluatorTypeName();
  }
}

