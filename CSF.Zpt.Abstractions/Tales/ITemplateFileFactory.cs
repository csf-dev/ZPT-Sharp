using System;
using System.IO;
using System.Text;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Interface for a type which creates instances of <see cref="TemplateFile"/>.
  /// </summary>
  public interface ITemplateFileFactory
  {
    #region methods

    /// <summary>
    /// Gets a value indicating the <see cref="RenderingMode"/> detected for a given source file, assuming it were
    /// parsed using <see cref="RenderingMode.AutoDetect"/>.
    /// </summary>
    /// <returns><c>true</c> if the <see cref="RenderingMode"/> could be auto-detected; <c>false</c> if not.</returns>
    /// <param name="sourceFile">The source file.</param>
    /// <param name="detectedMode">Exposes the detected rendering mode.</param>
    bool TryDetectMode(FileInfo sourceFile, out RenderingMode detectedMode);

    /// <summary>
    /// Creates a template file from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    /// <param name="renderingMode">The rendering mode to use in creating the output document.</param>
    TemplateFile CreateTemplateFile(FileInfo sourceFile,
                                    Encoding encoding = null,
                                    RenderingMode renderingMode = RenderingMode.AutoDetect);

    /// <summary>
    /// Creates a template file from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="providerType">The <see cref="IZptDocumentProvider"/> type to use for creating the document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    TemplateFile CreateTemplateFile(FileInfo sourceFile,
                                    Type providerType,
                                    Encoding encoding = null);

    /// <summary>
    /// Creates a template file from the given ZPT document.
    /// </summary>
    /// <returns>The template file.</returns>
    /// <param name="document">Document.</param>
    TemplateFile CreateTemplateFile(IZptDocument document);

    #endregion
  }
}

