using System;
using CSF.Zpt.Tales;
using System.Collections.Generic;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Default implementation of <see cref="IRenderingOptions"/>.
  /// </summary>
  public class RenderingOptions : IRenderingOptions
  {
    #region fields

    private Dictionary<string,string> _keywordOptions;

    #endregion

    #region properties

    /// <summary>
    /// Gets the assembly-qualified type name of the <see cref="IRenderingContextFactory"/> type to use.
    /// </summary>
    /// <value>The type of the rendering context factory.</value>
    public string RenderingContextFactoryType { get; set; }

    /// <summary>
    /// Gets a value indicating whether source file annotations should be added to the rendered output.
    /// </summary>
    /// <value><c>true</c> if source file annotations are to be added; otherwise, <c>false</c>.</value>
    public bool AddSourceFileAnnotation { get; set; }

    /// <summary>
    /// Gets an ordered, semicolon-separated, list of assembly-qualified type names for
    /// <see cref="IContextVisitor"/> types to use.
    /// </summary>
    /// <value>The context visitor types.</value>
    public string ContextVisitorTypes { get; set; }

    /// <summary>
    /// Gets the name of the encoding (eg: UTF-8) for the rendered output.
    /// </summary>
    /// <value>The output encoding.</value>
    public string OutputEncodingName { get; set; }

    /// <summary>
    /// Gets a value indicating whether the XML declaration should be omitted (where applicable).
    /// </summary>
    /// <value><c>true</c> if the XML declaration is to be omitted; otherwise, <c>false</c>.</value>
    public bool OmitXmlDeclaration { get; set; }

    /// <summary>
    /// Gets the assembly-qualified name of the <see cref="ITemplateFileFactory"/> to use when creating documents.
    /// </summary>
    /// <value>The type of the document factory.</value>
    public string DocumentFactoryType { get; set; }

    /// <summary>
    /// Gets the keyword options.
    /// </summary>
    /// <value>The keyword options.</value>
    public IDictionary<string,string> KeywordOptions
    { 
      get {
        return _keywordOptions;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.RenderingOptions"/> class.
    /// </summary>
    public RenderingOptions()
    {
      _keywordOptions = new Dictionary<string, string>();
    }

    #endregion
  }
}
