using System;
using System.Configuration;

namespace CSF.Zpt
{
  /// <summary>
  /// Configuration type which represents the collection of plugin assemblies.
  /// </summary>
  public class PluginAssemblyCollection : ConfigurationElementCollection
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
      return new Plugin();
    }

    /// <summary>
    /// Gets the element key.
    /// </summary>
    /// <returns>The element key.</returns>
    /// <param name="element">Element.</param>
    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((Plugin)(element)).Path;
    }

    /// <summary>
    /// Gets the <see cref="CSF.Zpt.Plugin"/> with the specified index.
    /// </summary>
    /// <param name="idx">Index.</param>
    public Plugin this[int idx]
    {
      get
      {
        return (Plugin)BaseGet(idx);
      }
    }
  }

  /// <summary>
  /// Configuration type representing a single plugin assembly.
  /// </summary>
  public class Plugin : ConfigurationElement
  {
    /// <summary>
    /// Gets or sets the relative path to the assembly
    /// </summary>
    /// <value>The path to the plugin assembly.</value>
    [ConfigurationProperty(@"Path", IsRequired = true)]
    public virtual string Path
    {
      get {
        return (string) this["Path"];
      }
      set {
        this["Path"] = value;
      }
    }
  }
}

