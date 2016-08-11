using System;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Encapsulates the available options for rendering a <see cref="ZptDocument"/>.
  /// </summary>
  public class DefaultRenderingOptions : RenderingOptions
  {
    #region constants

    private static readonly IContextVisitor[] DefaultVisitors = new IContextVisitor[] {
      new CSF.Zpt.Metal.MetalVisitor(),
      new CSF.Zpt.Tal.TalVisitor(),
      new CSF.Zpt.Metal.MetalTidyUpVisitor(),
      new CSF.Zpt.Tal.TalTidyUpVisitor(),
    };

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.RenderingOptions"/> class.
    /// </summary>
    /// <param name="addSourceFileAnnotation">Indicates whether or not source file annotation is to be added.</param>
    /// <param name="elementVisitors">The element visitors to use.</param>
    /// <param name="contextFactory">The rendering context factory.</param>
    /// <param name="outputEncoding">The desired output encoding.</param>
    /// <param name="omitXmlDeclaration">Whether or not to omit the XML declaration.</param>
    /// <param name="xmlIndentCharacters">The character(s) to use when indending XML.</param>
    /// <param name="outputIndentedXml">Whether or not to output indent-formatted XML.</param>
    public DefaultRenderingOptions(IContextVisitor[] elementVisitors = null,
                                   IRenderingContextFactory contextFactory = null,
                                   bool addSourceFileAnnotation = false,
                                   System.Text.Encoding outputEncoding = null,
                                   bool omitXmlDeclaration = false,
                                   string xmlIndentCharacters = "  ",
                                   bool outputIndentedXml = true) : base(elementVisitors?? DefaultVisitors,
                                                                         contextFactory?? new TalesRenderingContextFactory(),
                                                                         addSourceFileAnnotation,
                                                                         outputEncoding,
                                                                         omitXmlDeclaration,
                                                                         xmlIndentCharacters,
                                                                         outputIndentedXml) {}

    #endregion
  }
}

