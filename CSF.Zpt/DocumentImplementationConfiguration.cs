using System;
using System.Configuration;

namespace CSF.Zpt
{
  /// <summary>
  /// Configuration type which lists the avalable document implementation types.
  /// </summary>
  public class DocumentImplementationConfiguration : ConfigurationSection
  {
    /// <summary>
    /// Gets a collection of the available implementations.
    /// </summary>
    /// <value>The implementations.</value>
    [ConfigurationProperty(@"Implementations", IsRequired = true)]
    public virtual ImplementationsCollection Implementations
    {
      get {
        return (ImplementationsCollection) this["Implementations"];
      }
    }
  }

  /// <summary>
  /// Configuration type which represents the collection of implementations.
  /// </summary>
  public class ImplementationsCollection : ConfigurationElementCollection
  {
    internal const string PropertyName = "Implementation";

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
      return new Implementation();
    }

    /// <summary>
    /// Gets the element key.
    /// </summary>
    /// <returns>The element key.</returns>
    /// <param name="element">Element.</param>
    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((Implementation)(element)).TypeName;
    }

    /// <summary>
    /// Gets the <see cref="CSF.Zpt.ImplementationsCollection"/> with the specified index.
    /// </summary>
    /// <param name="idx">Index.</param>
    public Implementation this[int idx]
    {
      get
      {
        return (Implementation)BaseGet(idx);
      }
    }
  }

  /// <summary>
  /// Configuration type representing a single document implementation.
  /// </summary>
  public class Implementation : ConfigurationElement
  {
    /// <summary>
    /// Gets or sets the <c>System.Type</c> of the implementation provider.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Typically this is an assembly-qualified name, as implementation types live in satelite assemblies.
    /// </para>
    /// </remarks>
    /// <value>The name of the type.</value>
    [ConfigurationProperty(@"TypeName", IsRequired = true)]
    public virtual string TypeName
    {
      get {
        return (string) this["TypeName"];
      }
      set {
        this["TypeName"] = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is the default HTML provider.
    /// </summary>
    /// <value><c>true</c> if this instance is the default HTML provider; otherwise, <c>false</c>.</value>
    [ConfigurationProperty(@"IsDefaultHtml", IsRequired = false, DefaultValue = "False")]
    public virtual bool IsDefaultHtml
    {
      get {
        return (bool) this["IsDefaultHtml"];
      }
      set {
        this["IsDefaultHtml"] = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is the default XML provider.
    /// </summary>
    /// <value><c>true</c> if this instance is the default XML provider; otherwise, <c>false</c>.</value>
    [ConfigurationProperty(@"IsDefaultXml", IsRequired = false, DefaultValue = "False")]
    public virtual bool IsDefaultXml
    {
      get {
        return (bool) this["IsDefaultXml"];
      }
      set {
        this["IsDefaultXml"] = value;
      }
    }
  }
}

