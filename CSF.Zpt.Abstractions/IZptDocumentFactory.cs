using System;
using System.IO;
using CSF.Zpt.Rendering;
using System.Text;

namespace CSF.Zpt
{
  /// <summary>
  /// Interface for a type which creates instances of <see cref="IZptDocument"/>.
  /// </summary>
  public interface IZptDocumentFactory
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
    /// Creates a document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    /// <param name="renderingMode">The rendering mode to use in creating the output document.</param>
    IZptDocument CreateDocument(FileInfo sourceFile,
                                Encoding encoding = null,
                                RenderingMode renderingMode = RenderingMode.AutoDetect);

    /// <summary>
    /// Creates an HTML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Information about the source document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    IZptDocument CreateHtmlDocument(Stream sourceStream, ISourceInfo sourceInfo = null, Encoding encoding = null);

    /// <summary>
    /// Creates an XML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Information about the source document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    IZptDocument CreateXmlDocument(Stream sourceStream, ISourceInfo sourceInfo = null, Encoding encoding = null);

    /// <summary>
    /// Creates an XML Linq document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Information about the source document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    IZptDocument CreateXmlLinqDocument(Stream sourceStream, ISourceInfo sourceInfo = null, Encoding encoding = null);

    #endregion
  }
}

