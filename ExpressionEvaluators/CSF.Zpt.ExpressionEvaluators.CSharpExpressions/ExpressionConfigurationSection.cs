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
        .Select(x => new UsingNamespaceSpecification(x.Name, x.Alias))
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
    internal const string PropertyName = "Namespace";

    /// <summary>
    /// Gets the configuration collection type.
    /// </summary>
    /// <value>The type of the collection.</value>
    public override ConfigurationElementCollectionType CollectionType
    {
      get
      {
        return ConfigurationElementCollectionType.BasicMapAlternate;
      }
    }

    /// <summary>
    /// Gets the name of the element.
    /// </summary>
    /// <value>The name of the element.</value>
    protected override string ElementName
    {
      get
      {
        return PropertyName;
      }
    }

    /// <summary>
    /// Determines whether the given string matches the <see cref="ElementName"/>.
    /// </summary>
    /// <returns><c>true</c> if the string matches the element name; otherwise, <c>false</c>.</returns>
    /// <param name="elementName">Element name.</param>
    protected override bool IsElementName(string elementName)
    {
      return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Determines whether this instance is read only.
    /// </summary>
    /// <returns><c>true</c> if this instance is read only; otherwise, <c>false</c>.</returns>
    public override bool IsReadOnly()
    {
      return false;
    }

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
      return ((NamespaceConfigurationElement) element).Name;
    }

    /// <summary>
    /// Gets the <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.NamespaceConfigurationElement"/> with the specified index.
    /// </summary>
    /// <param name="idx">Index.</param>
    public NamespaceConfigurationElement this[int idx]
    {
      get
      {
        return (NamespaceConfigurationElement)BaseGet(idx);
      }
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
    internal const string PropertyName = "Assembly";

    /// <summary>
    /// Gets the configuration collection type.
    /// </summary>
    /// <value>The type of the collection.</value>
    public override ConfigurationElementCollectionType CollectionType
    {
      get
      {
        return ConfigurationElementCollectionType.BasicMapAlternate;
      }
    }

    /// <summary>
    /// Gets the name of the element.
    /// </summary>
    /// <value>The name of the element.</value>
    protected override string ElementName
    {
      get
      {
        return PropertyName;
      }
    }

    /// <summary>
    /// Determines whether the given string matches the <see cref="ElementName"/>.
    /// </summary>
    /// <returns><c>true</c> if the string matches the element name; otherwise, <c>false</c>.</returns>
    /// <param name="elementName">Element name.</param>
    protected override bool IsElementName(string elementName)
    {
      return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Determines whether this instance is read only.
    /// </summary>
    /// <returns><c>true</c> if this instance is read only; otherwise, <c>false</c>.</returns>
    public override bool IsReadOnly()
    {
      return false;
    }

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

    /// <summary>
    /// Gets the <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.AssemblyReferenceConfigurationElement"/> with the specified index.
    /// </summary>
    /// <param name="idx">Index.</param>
    public AssemblyReferenceConfigurationElement this[int idx]
    {
      get
      {
        return (AssemblyReferenceConfigurationElement)BaseGet(idx);
      }
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

