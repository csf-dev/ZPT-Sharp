using System;
using System.IO;
using System.Text;
using CSF.Zpt.Rendering;

namespace CSF.Zpt
{
  /// <summary>
  /// Interface for a type which creates <see cref="IZptDocument"/> implementations.
  /// </summary>
  public interface IZptDocumentProvider
  {
    #region methods

    /// <summary>
    /// Gets the rendering mode for documents created by this provider.
    /// </summary>
    /// <returns>The rendering mode.</returns>
    RenderingMode GetRenderingMode();

    /// <summary>
    /// Creates a document from a source file.
    /// </summary>
    /// <returns>The document.</returns>
    /// <param name="sourceFile">Source file.</param>
    /// <param name="encoding">Encoding.</param>
    IZptDocument CreateDocument(FileInfo sourceFile, Encoding encoding);

    /// <summary>
    /// Creates a document from a stream.
    /// </summary>
    /// <returns>The document.</returns>
    /// <param name="sourceStream">Source stream.</param>
    /// <param name="sourceInfo">Source info.</param>
    /// <param name="encoding">Encoding.</param>
    IZptDocument CreateDocument(Stream sourceStream, ISourceInfo sourceInfo, Encoding encoding);

    #endregion
  }
}

