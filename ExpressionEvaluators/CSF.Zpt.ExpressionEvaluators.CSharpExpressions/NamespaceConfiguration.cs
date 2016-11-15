using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Implementation of <see cref="INamespaceConfiguration"/> using a <c>ConfigurationSection</c>.
  /// </summary>
  public class NamespaceConfiguration : ConfigurationSection, INamespaceConfiguration
  {
    #region properties

    /// <summary>
    /// Gets or sets the configured namespaces.
    /// </summary>
    /// <value>The namespaces.</value>
    [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
    public virtual NamespaceConfigurationCollection Namespaces
    {
      get {
        return (NamespaceConfigurationCollection) this[String.Empty];
      }
      set {
        this[String.Empty] = value;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets the namespaces which are imported via <c>using</c> statements, for all CSharp expressions.
    /// </summary>
    /// <returns>The namespaces.</returns>
    public IEnumerable<string> GetNamespaces()
    {
      if(Namespaces == null)
      {
        return Enumerable.Empty<string>();
      }

      return Namespaces
        .Cast<NamespaceConfigurationElement>()
        .Select(x => x.Namespace)
        .ToArray();
    }

    #endregion
  }

  #region containted types

  /// <summary>
  /// Namespace configuration collection.
  /// </summary>
  public class NamespaceConfigurationCollection : ConfigurationElementCollection
  {
    /// <summary>
    /// Creates the new element.
    /// </summary>
    /// <returns>The new element.</returns>
    protected override ConfigurationElement CreateNewElement()
    {
      return new NamespaceConfigurationElement();
    }

    /// <summary>
    /// Gets the element key.
    /// </summary>
    /// <returns>The element key.</returns>
    /// <param name="element">Element.</param>
    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((NamespaceConfigurationElement) element).Namespace;
    }
  }

  /// <summary>
  /// Namespace configuration element.
  /// </summary>
  public class NamespaceConfigurationElement : ConfigurationElement
  {
    /// <summary>
    /// Gets or sets the namespace.
    /// </summary>
    /// <value>The namespace.</value>
    [ConfigurationProperty(@"Namespace", IsRequired = true)]
    public virtual string Namespace
    {
      get {
        return (string) this["Namespace"];
      }
      set {
        this["Namespace"] = value;
      }
    }
  }

  #endregion
}

