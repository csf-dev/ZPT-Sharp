using System;
using System.Text;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Encapsulates the available options for rendering a <see cref="ZptDocument"/>.
  /// </summary>
  public class RenderingSettings : DefaultRenderingSettings
  {
    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.IRenderingSettings"/> class.
    /// </summary>
    /// <param name="addSourceFileAnnotation">Indicates whether or not source file annotation is to be added.</param>
    /// <param name="elementVisitors">The element visitors to use.</param>
    /// <param name="contextFactory">The rendering context factory.</param>
    /// <param name="outputEncoding">The desired output encoding.</param>
    /// <param name="omitXmlDeclaration">Whether or not to omit the XML declaration.</param>
    /// <param name="documentFactory">An optional non-default implementation of <see cref="ITemplateFileFactory"/> to use.</param>
    public RenderingSettings(IContextVisitor[] elementVisitors = null,
                             IRenderingContextFactory contextFactory = null,
                             bool addSourceFileAnnotation = false,
                             System.Text.Encoding outputEncoding = null,
                             bool omitXmlDeclaration = false,
                             ITemplateFileFactory documentFactory = null)
      : base(documentFactory,
             elementVisitors,
             contextFactory,
             addSourceFileAnnotation,
             outputEncoding,
             omitXmlDeclaration) {}

    #endregion
  }
}

