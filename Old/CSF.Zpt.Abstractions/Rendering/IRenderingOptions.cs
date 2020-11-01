using System;
using System.Collections.Generic;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents options from which <see cref="IRenderingSettings"/> may be built.
  /// </summary>
  public interface IRenderingOptions
  {
    #region properties

    /// <summary>
    /// Gets the assembly-qualified type name of the <see cref="IRenderingContextFactory"/> type to use.
    /// </summary>
    /// <value>The type of the rendering context factory.</value>
    string RenderingContextFactoryType { get; }

    /// <summary>
    /// Gets a value indicating whether source file annotations should be added to the rendered output.
    /// </summary>
    /// <value><c>true</c> if source file annotations are to be added; otherwise, <c>false</c>.</value>
    bool AddSourceFileAnnotation { get; }

    /// <summary>
    /// Gets an ordered, semicolon-separated, list of assembly-qualified type names for
    /// <see cref="IContextVisitor"/> types to use.
    /// </summary>
    /// <value>The context visitor types.</value>
    string ContextVisitorTypes { get; }

    /// <summary>
    /// Gets the name of the encoding (eg: UTF-8) for the rendered output.
    /// </summary>
    /// <value>The output encoding.</value>
    string OutputEncodingName { get; }

    /// <summary>
    /// Gets a value indicating whether the XML declaration should be omitted (where applicable).
    /// </summary>
    /// <value><c>true</c> if the XML declaration is to be omitted; otherwise, <c>false</c>.</value>
    bool OmitXmlDeclaration { get; }

    /// <summary>
    /// Gets the assembly-qualified name of the <see cref="ITemplateFileFactory"/> to use when creating documents.
    /// </summary>
    /// <value>The type of the document factory.</value>
    string DocumentFactoryType { get; }

    /// <summary>
    /// Gets the keyword options.
    /// </summary>
    /// <value>The keyword options.</value>
    IDictionary<string,string> KeywordOptions { get; }

    #endregion
  }
}

