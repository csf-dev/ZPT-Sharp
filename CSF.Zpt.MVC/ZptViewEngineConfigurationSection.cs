using System;
using System.Configuration;

namespace CSF.Zpt.MVC
{
  public class ZptViewEngineConfigurationSection : ConfigurationSection, IZptViewEngineConfiguration
  {
    #region configuration properties

    /// <summary>
    /// Gets the type name of a implementation with which to create <see cref="RenderingContext"/> instances.
    /// </summary>
    /// <value>The rendering context factory type name.</value>
    [ConfigurationProperty(@"ContextFactoryTypeName", IsRequired = false)]
    public virtual string ContextFactoryTypeName
    {
      get {
        return (string) this["ContextFactoryTypeName"];
      }
      set {
        this["ContextFactoryTypeName"] = value;
      }
    }

    [ConfigurationProperty(@"ContextVisitorTypeNames", IsRequired = false)]
    public virtual string ContextVisitorTypeNames
    {
      get {
        return (string) this["ContextVisitorTypeNames"];
      }
      set {
        this["ContextVisitorTypeNames"] = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether source file annotations should be added to the rendered output.
    /// </summary>
    /// <value><c>true</c> if source file annotations are to be added; otherwise, <c>false</c>.</value>
    [ConfigurationProperty(@"AddSourceFileAnnotation", IsRequired = false, DefaultValue = "False")]
    public virtual bool AddSourceFileAnnotation
    {
      get {
        return (bool) this["AddSourceFileAnnotation"];
      }
      set {
        this["AddSourceFileAnnotation"] = value;
      }
    }

    /// <summary>
    /// Gets the encoding (eg: Unicode) for the rendered output.
    /// </summary>
    /// <value>The output encoding.</value>
    [ConfigurationProperty(@"OutputEncoding", IsRequired = false, DefaultValue = "UTF8")]
    public virtual string OutputEncoding
    {
      get {
        return (string) this["OutputEncoding"];
      }
      set {
        this["OutputEncoding"] = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the XML declaration should be omitted (where applicable).
    /// </summary>
    /// <value><c>true</c> if the XML declaration is to be omitted; otherwise, <c>false</c>.</value>
    [ConfigurationProperty(@"OmitXmlDeclaration", IsRequired = false, DefaultValue = "True")]
    public virtual bool OmitXmlDeclaration
    {
      get {
        return (bool) this["OmitXmlDeclaration"];
      }
      set {
        this["OmitXmlDeclaration"] = value;
      }
    }

    /// <summary>
    /// Gets a string used to indicate a single level of indentation to use when rendering an XML document.
    /// </summary>
    /// <value>The XML indentation characters.</value>
    [ConfigurationProperty(@"XmlIndentationCharacters", IsRequired = false, DefaultValue = "  ")]
    public virtual string XmlIndentationCharacters
    {
      get {
        return (string) this["XmlIndentationCharacters"];
      }
      set {
        this["XmlIndentationCharacters"] = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether XML documents should be rendered with indentated formatting or not.
    /// </summary>
    /// <value><c>true</c> if the rendering process is to output indented XML; otherwise, <c>false</c>.</value>
    [ConfigurationProperty(@"OutputIndentedXml", IsRequired = false, DefaultValue = "True")]
    public virtual bool OutputIndentedXml
    {
      get {
        return (bool) this["OutputIndentedXml"];
      }
      set {
        this["OutputIndentedXml"] = value;
      }
    }

    [ConfigurationProperty(@"RenderingMode", IsRequired = false, DefaultValue = "AutoDetect")]
    public virtual string RenderingMode
    {
      get {
        return (string) this["RenderingMode"];
      }
      set {
        this["RenderingMode"] = value;
      }
    }

    [ConfigurationProperty(@"ForceInputEncoding", IsRequired = false)]
    public virtual string ForceInputEncoding
    {
      get {
        return (string) this["ForceInputEncoding"];
      }
      set {
        this["ForceInputEncoding"] = value;
      }
    }

    #endregion
  }
}

