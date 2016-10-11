using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CSF.Zpt
{
  /// <summary>
  /// Represents empty/nonexistent plugin configuration.
  /// </summary>
  internal sealed class EmptyPluginConfiguration : IPluginConfiguration
  {
    /// <summary>
    /// Gets a collection of all of the registered plugin assemblies.
    /// </summary>
    /// <returns>The plugin assemblies.</returns>
    public IEnumerable<Assembly> GetAllPluginAssemblies()
    {
      return Enumerable.Empty<Assembly>();
    }

    /// <summary>
    /// Gets the name (including namespace) of the <c>System.Type</c> which is the default HTML document provider.
    /// </summary>
    /// <returns>The default HTML document provider type name.</returns>
    public string GetDefaultHtmlDocumentProviderTypeName()
    {
      return null;
    }

    /// <summary>
    /// Gets the name (including namespace) of the <c>System.Type</c> which is the default XML document provider.
    /// </summary>
    /// <returns>The default XML document provider type name.</returns>
    public string GetDefaultXmlDocumentProviderTypeName()
    {
      return null;
    }

    /// <summary>
    /// Gets the name (including namespace) of the <c>System.Type</c> which is the default TALES expression evaluator.
    /// </summary>
    /// <returns>The default expression evaluator type name.</returns>
    public string GetDefaultExpressionEvaluatorTypeName()
    {
      return null;
    }
  }
}

