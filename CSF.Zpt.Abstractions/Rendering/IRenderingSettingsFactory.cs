using System;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Factory type for creating instances of <see cref="IRenderingSettings"/>.
  /// </summary>
  public interface IRenderingSettingsFactory
  {
    /// <summary>
    /// Creates the settings from the given rendering options.
    /// </summary>
    /// <returns>The settings.</returns>
    /// <param name="options">Options.</param>
    IRenderingSettings CreateSettings(IRenderingOptions options);

    /// <summary>
    /// Creates rendering settings from specified values, using defaults where values are blank.
    /// </summary>
    /// <returns>The settings.</returns>
    /// <param name="contextVisitors">Context visitors.</param>
    /// <param name="contextFactory">Context factory.</param>
    /// <param name="addSourceAnnotation">If set to <c>true</c> add source annotation.</param>
    /// <param name="encoding">Encoding.</param>
    /// <param name="omitXmlDeclaration">If set to <c>true</c> omit xml declaration.</param>
    /// <param name="templateFactory">Template factory.</param>
    IRenderingSettings CreateSettings(IContextVisitor[] contextVisitors = null,
                                      IRenderingContextFactory contextFactory = null,
                                      bool addSourceAnnotation = false,
                                      System.Text.Encoding encoding = null,
                                      bool omitXmlDeclaration = false,
                                      ITemplateFileFactory templateFactory = null);
  }
}

