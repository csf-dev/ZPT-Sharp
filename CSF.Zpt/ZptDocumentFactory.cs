using System;
using System.IO;
using CSF.Zpt.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt
{
  /// <summary>
  /// Default implementation of <see cref="IZptDocumentFactory"/>.
  /// </summary>
  public class ZptDocumentFactory : IZptDocumentFactory
  {
    #region constants

    private static readonly string[]
      HtmlSuffixes = new string[] {
        ".pt",
        ".html",
      },
      XmlSuffixes = new string[] {
        ".xml",
        ".xhtml",
      };

    #endregion

    #region methods

    /// <summary>
    /// Creates a document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    public ZptDocument Create(FileInfo sourceFile)
    {
      return this.Create(sourceFile, System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// Creates a document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    public ZptDocument Create(FileInfo sourceFile, System.Text.Encoding encoding)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }
      if(encoding == null)
      {
        throw new ArgumentNullException(nameof(encoding));
      }

      ZptDocument output;

      var extension = sourceFile.Extension;

      if(XmlSuffixes.Contains(extension))
      {
        output = this.CreateXml(sourceFile, encoding);
      }
      else if(HtmlSuffixes.Contains(extension))
      {
        output = this.CreateHtml(sourceFile, encoding);
      }
      else
      {
        var message = String.Format(Resources.ExceptionMessages.UnsupportedDocumentFilenameExtension,
                                    sourceFile.FullName);
        throw new ArgumentException(message, nameof(sourceFile));
      }

      return output;
    }

    /// <summary>
    /// Creates an HTML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    public ZptDocument CreateHtml(FileInfo sourceFile, System.Text.Encoding encoding)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }
      if(encoding == null)
      {
        throw new ArgumentNullException(nameof(encoding));
      }

      ZptDocument output;
      var sourceInfo = new SourceFileInfo(sourceFile);

      using(var stream = sourceFile.OpenRead())
      {
        output = this.CreateHtml(stream, sourceInfo, encoding);
      }

      return output;
    }

    /// <summary>
    /// Creates an XML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    public ZptDocument CreateXml(FileInfo sourceFile, System.Text.Encoding encoding)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }
      if(encoding == null)
      {
        throw new ArgumentNullException(nameof(encoding));
      }

      ZptDocument output;
      var sourceInfo = new SourceFileInfo(sourceFile);

      using(var stream = sourceFile.OpenRead())
      {
        output = this.CreateXml(stream, sourceInfo, encoding);
      }

      return output;
    }

    /// <summary>
    /// Creates an HTML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    public ZptDocument CreateHtml(FileInfo sourceFile)
    {
      return this.CreateHtml(sourceFile, System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// Creates an XML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    public ZptDocument CreateXml(FileInfo sourceFile)
    {
      return this.CreateXml(sourceFile, System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// Creates an HTML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Optional information about the source document.</param>
    public ZptDocument CreateHtml(Stream sourceStream, SourceFileInfo sourceInfo)
    {
      return this.CreateHtml(sourceStream, sourceInfo, System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// Creates an XML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Optional information about the source document.</param>
    public ZptDocument CreateXml(Stream sourceStream, SourceFileInfo sourceInfo)
    {
      return this.CreateXml(sourceStream, sourceInfo, System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// Creates an HTML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Optional information about the source document.</param>
    public ZptDocument CreateHtml(Stream sourceStream, SourceFileInfo sourceInfo, System.Text.Encoding encoding)
    {
      if(sourceStream == null)
      {
        throw new ArgumentNullException(nameof(sourceStream));
      }
      if(sourceInfo == null)
      {
        throw new ArgumentNullException(nameof(sourceInfo));
      }
      if(encoding == null)
      {
        throw new ArgumentNullException(nameof(encoding));
      }

      var doc = new HtmlAgilityPack.HtmlDocument();
      doc.Load(sourceStream, encoding);

      return new ZptHtmlDocument(doc, sourceInfo);
    }

    /// <summary>
    /// Creates an XML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Optional information about the source document.</param>
    public ZptDocument CreateXml(Stream sourceStream, SourceFileInfo sourceInfo, System.Text.Encoding encoding)
    {
      if(sourceStream == null)
      {
        throw new ArgumentNullException(nameof(sourceStream));
      }
      if(sourceInfo == null)
      {
        throw new ArgumentNullException(nameof(sourceInfo));
      }
      if(encoding == null)
      {
        throw new ArgumentNullException(nameof(encoding));
      }

      var settings = new System.Xml.XmlReaderSettings() {
        XmlResolver = new LocalXhtmlXmlResolver(),
        DtdProcessing = System.Xml.DtdProcessing.Parse,
      };

      var doc = new System.Xml.XmlDocument();

      using(var streamReader = new StreamReader(sourceStream, encoding))
      using(var reader = System.Xml.XmlReader.Create(streamReader, settings))
      {
        doc.Load(reader);
      }

      return new ZptXmlDocument(doc, sourceInfo);
    }

    #endregion

    #region static methods

    /// <summary>
    /// Gets the file extensions which indicate that an <see cref="ZptHtmlDocument"/> should be used.
    /// </summary>
    /// <returns>The HTML extensions.</returns>
    public static IEnumerable<string> GetHtmlExtensions()
    {
      return new HashSet<string>(HtmlSuffixes);
    }

    /// <summary>
    /// Gets the file extensions which indicate that an <see cref="ZptXmlDocument"/> should be used.
    /// </summary>
    /// <returns>The XML extensions.</returns>
    public static IEnumerable<string> GetXmlExtensions()
    {
      return new HashSet<string>(XmlSuffixes);
    }

    #endregion
  }
}

