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
    /// Gets a value indicating whether or not the current instance can create an <see cref="IZptDocument"/> from a given
    /// source file using <see cref="RenderingMode.AutoDetect"/>.
    /// </summary>
    /// <returns><c>true</c> if this instance can auto-detect the rendering mode for the given file; otherwise, <c>false</c>.</returns>
    /// <param name="sourceFile">The source file.</param>
    bool CanAutoDetectMode(FileInfo sourceFile);

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
    /// Creates a template file from the given ZPT document.
    /// </summary>
    /// <returns>The template file.</returns>
    /// <param name="document">Document.</param>
    TemplateFile CreateTemplateFile(IZptDocument document);

    #endregion
  }
}

