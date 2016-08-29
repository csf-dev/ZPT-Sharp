using System;
using System.Configuration;

namespace CSF.Zpt
{
  public class DocumentImplementationConfiguration : ConfigurationSection
  {
    [ConfigurationProperty(@"Implementations", IsRequired = true)]
    public virtual ImplementationsCollection Implementations
    {
      get {
        return (ImplementationsCollection) this["Implementations"];
      }
    }
  }

  public class ImplementationsCollection : ConfigurationElementCollection
  {
    internal const string PropertyName = "Implementation";

    public override ConfigurationElementCollectionType CollectionType
    {
      get
      {
        return ConfigurationElementCollectionType.BasicMapAlternate;
      }
    }
    protected override string ElementName
    {
      get
      {
        return PropertyName;
      }
    }

    protected override bool IsElementName(string elementName)
    {
      return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
    }


    public override bool IsReadOnly()
    {
      return false;
    }


    protected override ConfigurationElement CreateNewElement()
    {
      return new Implementation();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((Implementation)(element)).TypeName;
    }

    public Implementation this[int idx]
    {
      get
      {
        return (Implementation)BaseGet(idx);
      }
    }
  }

  public class Implementation : ConfigurationElement
  {
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

