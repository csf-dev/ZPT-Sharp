using System;
using System.IO;
using CSF.Zpt.Rendering;

namespace CSF.Zpt
{
  /// <summary>
  /// Interface for a type which creates instances of <see cref="ZptDocument"/>.
  /// </summary>
  public interface IZptDocumentFactory
  {
    /// <summary>
    /// Creates a document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    ZptDocument Create(FileInfo sourceFile);

    /// <summary>
    /// Creates an HTML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    ZptDocument CreateHtml(FileInfo sourceFile);

    /// <summary>
    /// Creates an XML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    ZptDocument CreateXml(FileInfo sourceFile);

    /// <summary>
    /// Creates an HTML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Optional information about the source document.</param>
    ZptDocument CreateHtml(Stream sourceStream, SourceFileInfo sourceInfo);

    /// <summary>
    /// Creates an XML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Optional information about the source document.</param>
    ZptDocument CreateXml(Stream sourceStream, SourceFileInfo sourceInfo);

    /// <summary>
    /// Creates a document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    ZptDocument Create(FileInfo sourceFile, System.Text.Encoding encoding);

    /// <summary>
    /// Creates an HTML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    ZptDocument CreateHtml(FileInfo sourceFile, System.Text.Encoding encoding);

    /// <summary>
    /// Creates an XML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    ZptDocument CreateXml(FileInfo sourceFile, System.Text.Encoding encoding);

    /// <summary>
    /// Creates an HTML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Optional information about the source document.</param>
    ZptDocument CreateHtml(Stream sourceStream, SourceFileInfo sourceInfo, System.Text.Encoding encoding);

    /// <summary>
    /// Creates an XML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Optional information about the source document.</param>
    ZptDocument CreateXml(Stream sourceStream, SourceFileInfo sourceInfo, System.Text.Encoding encoding);
  }
}

