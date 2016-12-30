using System;
using System.Configuration;
using System.Collections.Generic;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Configuration section for CSharp expressions.
  /// </summary>
  public class ExpressionConfigurationSection : ConfigurationSection, IExpressionConfiguration
  {
    #region properties

    /// <summary>
    /// Gets or sets the configuration for imported namespaces.
    /// </summary>
    /// <value>The namespaces.</value>
    [ConfigurationProperty(@"Namespaces", IsRequired = true, IsDefaultCollection = false)]
    public virtual NamespaceConfigurationCollection Namespaces
    {
      get {
        return (NamespaceConfigurationCollection) this["Namespaces"];
      }
      set {
        this["Namespaces"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the configuration for referenced assemblies.
    /// </summary>
    /// <value>The assemblies.</value>
    [ConfigurationProperty(@"Assemblies", IsRequired = true, IsDefaultCollection = false)]
    public virtual AssemblyReferenceConfigurationCollection Assemblies
    {
      get {
        return (AssemblyReferenceConfigurationCollection) this["Assemblies"];
      }
      set {
        this["Assemblies"] = value;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets the imported namespaces.
    /// </summary>
    /// <returns>The imported namespaces.</returns>
    public IEnumerable<UsingNamespaceSpecification> GetImportedNamespaces()
    {
      if(Namespaces == null)
      {
        return Enumerable.Empty<UsingNamespaceSpecification>();
      }

      return Namespaces
        .Cast<NamespaceConfigurationElement>()
        .Select(x => new UsingNamespaceSpecification(x.Namespace, x.Alias))
        .ToArray();
    }

    /// <summary>
    /// Gets the referenced assemblies.
    /// </summary>
    /// <returns>The referenced assemblies.</returns>
    public IEnumerable<ReferencedAssemblySpecification> GetReferencedAssemblies()
    {
      if(Assemblies == null)
      {
        return Enumerable.Empty<ReferencedAssemblySpecification>();
      }

      return Assemblies
        .Cast<AssemblyReferenceConfigurationElement>()
        .Select(x => new ReferencedAssemblySpecification(x.Name))
        .ToArray();
    }

    #endregion
  }

  #region other types

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

    /// <summary>
    /// Gets or sets the namespace alias.
    /// </summary>
    /// <value>The alias.</value>
    [ConfigurationProperty(@"Alias", IsRequired = false, DefaultValue = null)]
    public virtual string Alias
    {
      get {
        return (string) this["Alias"];
      }
      set {
        this["Alias"] = value;
      }
    }
  }

  /// <summary>
  /// Assembly reference configuration collection.
  /// </summary>
  public class AssemblyReferenceConfigurationCollection : ConfigurationElementCollection
  {
    /// <summary>
    /// Creates the new element.
    /// </summary>
    /// <returns>The new element.</returns>
    protected override ConfigurationElement CreateNewElement()
    {
      return new AssemblyReferenceConfigurationElement();
    }

    /// <summary>
    /// Gets the element key.
    /// </summary>
    /// <returns>The element key.</returns>
    /// <param name="element">Element.</param>
    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((AssemblyReferenceConfigurationElement) element).Name;
    }
  }

  /// <summary>
  /// Assembly reference configuration element.
  /// </summary>
  public class AssemblyReferenceConfigurationElement : ConfigurationElement
  {
    /// <summary>
    /// Gets or sets the name of the assembly (including the <c>.dll</c> suffix).
    /// </summary>
    /// <value>The assembly name.</value>
    [ConfigurationProperty(@"Name", IsRequired = true)]
    public virtual string Name
    {
      get {
        return (string) this["Name"];
      }
      set {
        this["Name"] = value;
      }
    }
  }

  #endregion
}

