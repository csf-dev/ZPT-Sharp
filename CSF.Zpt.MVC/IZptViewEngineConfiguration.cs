using System;

namespace CSF.Zpt.MVC
{
  public interface IZptViewEngineConfiguration
  {
    #region properties

    /// <summary>
    /// Gets the type name of a implementation with which to create <see cref="RenderingContext"/> instances.
    /// </summary>
    /// <value>The rendering context factory type name.</value>
    string ContextFactoryTypeName { get; }

    string ContextVisitorTypeNames { get; }

    /// <summary>
    /// Gets or sets a value indicating whether source file annotations should be added to the rendered output.
    /// </summary>
    /// <value><c>true</c> if source file annotations are to be added; otherwise, <c>false</c>.</value>
    bool AddSourceFileAnnotation { get; }

    /// <summary>
    /// Gets the encoding (eg: Unicode) for the rendered output.
    /// </summary>
    /// <value>The output encoding.</value>
    string OutputEncoding { get; }

    /// <summary>
    /// Gets a value indicating whether the XML declaration should be omitted (where applicable).
    /// </summary>
    /// <value><c>true</c> if the XML declaration is to be omitted; otherwise, <c>false</c>.</value>
    bool OmitXmlDeclaration { get; }

    /// <summary>
    /// Gets a string used to indicate a single level of indentation to use when rendering an XML document.
    /// </summary>
    /// <value>The XML indentation characters.</value>
    string XmlIndentationCharacters { get; }

    /// <summary>
    /// Gets a value indicating whether XML documents should be rendered with indentated formatting or not.
    /// </summary>
    /// <value><c>true</c> if the rendering process is to output indented XML; otherwise, <c>false</c>.</value>
    bool OutputIndentedXml { get; }

    string RenderingMode { get; }

    string ForceInputEncoding { get; }

    #endregion
  }
}

