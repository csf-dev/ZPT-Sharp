using System;
using System.Text;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Encapsulates the available options for rendering a <see cref="ZptDocument"/>.
  /// </summary>
  public class RenderingOptions : DefaultRenderingOptions
  {
    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.IRenderingOptions"/> class.
    /// </summary>
    /// <param name="addSourceFileAnnotation">Indicates whether or not source file annotation is to be added.</param>
    /// <param name="elementVisitors">The element visitors to use.</param>
    /// <param name="contextFactory">The rendering context factory.</param>
    /// <param name="outputEncoding">The desired output encoding.</param>
    /// <param name="omitXmlDeclaration">Whether or not to omit the XML declaration.</param>
    /// <param name="xmlIndentCharacters">The character(s) to use when indending XML.</param>
    /// <param name="outputIndentedXml">Whether or not to output indent-formatted XML.</param>
    public RenderingOptions(IContextVisitor[] elementVisitors = null,
                            IRenderingContextFactory contextFactory = null,
                            bool addSourceFileAnnotation = false,
                            System.Text.Encoding outputEncoding = null,
                            bool omitXmlDeclaration = false,
                            string xmlIndentCharacters = "  ",
                            bool outputIndentedXml = true,
                            ITemplateFileFactory documentFactory = null)
      : base(documentFactory,
             elementVisitors,
             contextFactory,
             addSourceFileAnnotation,
             outputEncoding,
             omitXmlDeclaration,
             xmlIndentCharacters,
             outputIndentedXml) {}

    #endregion
  }
}

