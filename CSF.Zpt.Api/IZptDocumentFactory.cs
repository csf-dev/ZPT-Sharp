using System;
using System.IO;
using CSF.Zpt.Rendering;
using System.Text;

namespace CSF.Zpt
{
  /// <summary>
  /// Interface for a type which creates instances of <see cref="ZptDocument"/>.
  /// </summary>
  public interface IZptDocumentFactory
  {
    #region methods

    /// <summary>
    /// Gets a value indicating whether or not the current instance can create a <see cref="ZptDocument"/> from a given
    /// source file.
    /// </summary>
    /// <returns><c>true</c> if this instance can create a document from the given file; otherwise, <c>false</c>.</returns>
    /// <param name="sourceFile">The source file.</param>
    bool CanCreateFromFile(FileInfo sourceFile);

    /// <summary>
    /// Creates a document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    ZptDocument CreateDocument(FileInfo sourceFile, Encoding encoding = null);

    /// <summary>
    /// Creates an HTML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    ZptDocument CreateHtmlDocument(FileInfo sourceFile, Encoding encoding = null);

    /// <summary>
    /// Creates an XML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    ZptDocument CreateXmlDocument(FileInfo sourceFile, Encoding encoding = null);

    /// <summary>
    /// Creates an HTML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Information about the source document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    ZptDocument CreateHtmlDocument(Stream sourceStream, SourceFileInfo sourceInfo = null, Encoding encoding = null);

    /// <summary>
    /// Creates an XML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Information about the source document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    ZptDocument CreateXmlDocument(Stream sourceStream, SourceFileInfo sourceInfo = null, Encoding encoding = null);

    #endregion
  }
}

