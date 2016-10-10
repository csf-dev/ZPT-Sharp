using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF.Zpt
{
  /// <summary>
  /// Implementation of <see cref="IPluginConfiguration"/> using a <c>System.Configuration.ConfigurationSection</c>.
  /// </summary>
  public class PluginConfigurationSection : ConfigurationSection, IPluginConfiguration
  {
    #region fields

    private IEnumerable<Type> _documentProviderTypes, _expressionEvaluatorTypes;
    private Type _defaultHtmlType, _defaultXmlType, _defaultExpressionEvaluator;

    #endregion

    #region properties

    [ConfigurationProperty(@"Plugins", IsRequired = true)]
    public virtual PluginAssemblyCollection Plugins
    {
      get {
        return (PluginAssemblyCollection) this["Plugins"];
      }
      set {
        this["Plugins"] = value;
      }
    }

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
    /// Gets all of the installed document provider types.
    /// </summary>
    /// <returns>The all document provider types.</returns>
    public IEnumerable<Type> GetAllDocumentProviderTypes()
    {
      if(_documentProviderTypes == null)
      {
        _documentProviderTypes = GetAllConcretePluginTypes(typeof(IZptDocumentProvider));
      }

      return _documentProviderTypes;
    }

    /// <summary>
    /// Gets all of the installed expression evaluator types.
    /// </summary>
    /// <returns>The all expression evaluator types.</returns>
    public IEnumerable<Type> GetAllExpressionEvaluatorTypes()
    {
      if(_expressionEvaluatorTypes == null)
      {
        _expressionEvaluatorTypes = GetAllConcretePluginTypes(typeof(Tales.IExpressionEvaluator));
      }

      return _expressionEvaluatorTypes;
    }

    /// <summary>
    /// Gets the default HTML document provider.
    /// </summary>
    /// <returns>The default HTML document provider.</returns>
    public Type GetDefaultHtmlDocumentProvider()
    {
      if(String.IsNullOrEmpty(this.DefaultHtmlDocumentProvider))
      {
        return null;
      }

      return GetAllDocumentProviderTypes().SingleOrDefault(x => x.FullName == this.DefaultHtmlDocumentProvider);
    }

    /// <summary>
    /// Gets the default XML document provider.
    /// </summary>
    /// <returns>The default XML document provider.</returns>
    public Type GetDefaultXmlDocumentProvider()
    {
      if(String.IsNullOrEmpty(this.DefaultXmlDocumentProvider))
      {
        return null;
      }

      return GetAllDocumentProviderTypes().SingleOrDefault(x => x.FullName == this.DefaultXmlDocumentProvider);
    }

    /// <summary>
    /// Gets the default expression evaluator.
    /// </summary>
    /// <returns>The default expression evaluator.</returns>
    public Type GetDefaultExpressionEvaluator()
    {
      if(String.IsNullOrEmpty(this.DefaultExpressionEvaluator))
      {
        // TODO: Move this message to a resource file
        throw new InvalidOperationException("The default TALES expression evaluator must be configured.");
      }

      // TODO: Wrap this in a more suitable exception
      return GetAllExpressionEvaluatorTypes().Single(x => x.FullName == this.DefaultExpressionEvaluator);
    }

    /// <summary>
    /// Gets all of the types which are contained within installed plugins and which implement a base class.
    /// </summary>
    /// <returns>The concrete plugin types.</returns>
    /// <param name="baseType">Base type.</param>
    private IEnumerable<Type> GetAllConcretePluginTypes(Type baseType)
    {
      if(baseType == null)
      {
        throw new ArgumentNullException(nameof(baseType));
      }

      return (from plugin in Plugins.Cast<Plugin>()
              let assembly = LoadAssembly(plugin.Path)
              from type in assembly.GetExportedTypes()
              where
                type.IsClass
                && !type.IsAbstract
                && baseType.IsAssignableFrom(type)
                && type.GetConstructor(Type.EmptyTypes) != null
              select type);
    }

    private Assembly LoadAssembly(string path)
    {
      try
      {
        return Assembly.LoadFrom(path);
      }
      catch(Exception ex)
      {
        // TODO: Wrap this in a better exception and move message to resource file
        throw new InvalidOperationException("The plugin assembly could not be loaded.", ex);
      }
    }

    #endregion
  }
}

