using System;
using System.IO;
using CSF.Zpt.Rendering;
using System.Collections.Generic;

namespace CSF.Zpt
{
  /// <summary>
  /// Interface for a type which creates instances of <see cref="ZptDocument"/>.
  /// </summary>
  public interface IZptDocumentFactory
  {
    /// <summary>
    /// Gets the supported file extensions.
    /// </summary>
    /// <value>The supported file extensions.</value>
    ISet<string> SupportedFileExtensions { get; }

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
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    ZptDocument Create(FileInfo sourceFile, System.Text.Encoding encoding);

    /// <summary>
    /// Creates an HTML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    ZptDocument CreateHtml(FileInfo sourceFile, System.Text.Encoding encoding);

    /// <summary>
    /// Creates an XML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    ZptDocument CreateXml(FileInfo sourceFile, System.Text.Encoding encoding);

    /// <summary>
    /// Creates an HTML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Optional information about the source document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    ZptDocument CreateHtml(Stream sourceStream, SourceFileInfo sourceInfo, System.Text.Encoding encoding);

    /// <summary>
    /// Creates an XML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Optional information about the source document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    ZptDocument CreateXml(Stream sourceStream, SourceFileInfo sourceInfo, System.Text.Encoding encoding);
  }
}

