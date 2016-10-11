﻿using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.Configuration;
using System.IO;
using CSF.IO;

namespace CSF.Zpt
{
  /// <summary>
  /// Implementation of <see cref="IPluginConfiguration"/> using a <c>System.Configuration.ConfigurationSection</c>.
  /// </summary>
  public class PluginConfigurationSection : ConfigurationSection, IPluginConfiguration
  {
    #region fields

    private static IPluginConfiguration _singleton;
    private IEnumerable<Assembly> _cachedPluginAsseblies;

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets a collection of all of the paths to plugin assemblies.
    /// </summary>
    /// <value>The plugins.</value>
    [ConfigurationProperty(@"PluginAssemblies", IsRequired = true)]
    public virtual PluginAssemblyCollection Plugins
    {
      get {
        return (PluginAssemblyCollection) this["PluginAssemblies"];
      }
      set {
        this["PluginAssemblies"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the type name of the default HTML document provider.
    /// </summary>
    /// <value>The default html document provider.</value>
    [ConfigurationProperty(@"DefaultHtmlDocumentProvider", IsRequired = false)]
    public virtual string DefaultHtmlDocumentProvider
    {
      get {
        return (string) this["DefaultHtmlDocumentProvider"];
      }
      set {
        this["DefaultHtmlDocumentProvider"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the type name of the default XML document provider.
    /// </summary>
    /// <value>The default xml document provider.</value>
    [ConfigurationProperty(@"DefaultXmlDocumentProvider", IsRequired = false)]
    public virtual string DefaultXmlDocumentProvider
    {
      get {
        return (string) this["DefaultXmlDocumentProvider"];
      }
      set {
        this["DefaultXmlDocumentProvider"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the type name of the default TALES expression evaluator.
    /// </summary>
    /// <value>The default expression evaluator.</value>
    [ConfigurationProperty(@"DefaultExpressionEvaluator", IsRequired = true)]
    public virtual string DefaultExpressionEvaluator
    {
      get {
        return (string) this["DefaultExpressionEvaluator"];
      }
      set {
        this["DefaultExpressionEvaluator"] = value;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets a collection of all of the registered plugin assemblies.
    /// </summary>
    /// <returns>The plugin assemblies.</returns>
    public IEnumerable<Assembly> GetAllPluginAssemblies()
    {
      if(_cachedPluginAsseblies == null)
      {
        _cachedPluginAsseblies = LoadAllAssemblies();
      }

      return _cachedPluginAsseblies;
    }

    /// <summary>
    /// Gets the name (including namespace) of the <c>System.Type</c> which is the default HTML document provider.
    /// </summary>
    /// <returns>The default HTML document provider type name.</returns>
    public string GetDefaultHtmlDocumentProviderTypeName()
    {
      return this.DefaultHtmlDocumentProvider;
    }

    /// <summary>
    /// Gets the name (including namespace) of the <c>System.Type</c> which is the default XML document provider.
    /// </summary>
    /// <returns>The default XML document provider type name.</returns>
    public string GetDefaultXmlDocumentProviderTypeName()
    {
      return this.DefaultXmlDocumentProvider;
    }

    /// <summary>
    /// Gets the name (including namespace) of the <c>System.Type</c> which is the default TALES expression evaluator.
    /// </summary>
    /// <returns>The default expression evaluator type name.</returns>
    public string GetDefaultExpressionEvaluatorTypeName()
    {
      return this.DefaultExpressionEvaluator;
    }

    private IEnumerable<Assembly> LoadAllAssemblies()
    {
      if(this.Plugins == null)
      {
        return Enumerable.Empty<Assembly>();
      }

      return this.Plugins
        .Cast<Plugin>()
        .Select(x => LoadAssembly(x.Path))
        .ToArray();
    }

    private Assembly LoadAssembly(string path)
    {
      var currentAssemblyDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).GetParent();
      var pluginRelativePath = Path.Combine(currentAssemblyDirectory.FullName, path);

      try
      {
        return Assembly.LoadFrom(pluginRelativePath);
      }
      catch(Exception ex)
      {
        // TODO: Wrap this in a better exception and move message to resource file
        throw new InvalidOperationException("The plugin assembly could not be loaded.", ex);
      }
    }

    #endregion

    #region singleton instance

    /// <summary>
    /// Gets the default instance of the plugin configuration.
    /// </summary>
    /// <returns>The default/singleton instance.</returns>
    public static IPluginConfiguration GetDefault()
    {
      if(_singleton == null)
      {
        _singleton = ConfigurationHelper.GetSection<PluginConfigurationSection>();
      }

      return _singleton?? new EmptyPluginConfiguration();
    }

    #endregion
  }
}

