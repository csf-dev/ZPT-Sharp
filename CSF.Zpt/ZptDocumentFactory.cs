using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSF.Zpt.Rendering;

namespace CSF.Zpt
{
  /// <summary>
  /// Default implementation of <see cref="IZptDocumentFactory"/>.
  /// </summary>
  public class ZptDocumentFactory : IZptDocumentFactory, Tales.ITemplateFileFactory
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
    private static readonly ISet<string> _supportedExtensions = new HashSet<string>(HtmlSuffixes.Union(XmlSuffixes));

    private static readonly Encoding DefaultEncoding = Encoding.UTF8;

    private static readonly Type
      HtmlDocumentType = typeof(ZptHtmlDocument),
      XmlDocumentType = typeof(ZptXmlDocument);

    #endregion

    #region fields

    private Type _forceDocumentType;

    #endregion

    #region properties

    /// <summary>
    /// Gets the supported file extensions.
    /// </summary>
    /// <value>The supported file extensions.</value>
    public virtual IEnumerable<string> SupportedFileExtensions
    {
      get {
        return _supportedExtensions;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets a value indicating whether or not the current instance can create an <see cref="IZptDocument"/> from a given
    /// source file using <see cref="RenderingMode.AutoDetect"/>.
    /// </summary>
    /// <returns><c>true</c> if this instance can auto-detect the rendering mode for the given file; otherwise, <c>false</c>.</returns>
    /// <param name="sourceFile">The source file.</param>
    public bool CanAutoDetectMode(FileInfo sourceFile)
    {
      return (_forceDocumentType != null || _supportedExtensions.Contains(sourceFile.Extension));
    }

    /// <summary>
    /// Creates a document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    /// <param name="renderingMode">The rendering mode to use in creating the output document.</param>
    public IZptDocument CreateDocument(FileInfo sourceFile,
                                       Encoding encoding,
                                       RenderingMode renderingMode)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }
      if(!renderingMode.IsDefinedValue())
      {
        throw new ArgumentException(Resources.ExceptionMessages.InvalidRenderingMode, nameof(renderingMode));
      }

      IZptDocument output;

      encoding = encoding?? DefaultEncoding;
      var extension = sourceFile.Extension;

      if(_forceDocumentType == HtmlDocumentType
         || renderingMode == RenderingMode.Html
         || (_forceDocumentType == null
             && renderingMode == RenderingMode.AutoDetect
             && HtmlSuffixes.Contains(extension)))
      {
        output = this.CreateHtmlDocument(sourceFile, encoding);
      }
      else if(_forceDocumentType == XmlDocumentType
              || renderingMode == RenderingMode.Xml
              || (_forceDocumentType == null
                  && renderingMode == RenderingMode.AutoDetect
                  && XmlSuffixes.Contains(extension)))
      {
        output = this.CreateXmlDocument(sourceFile, encoding);
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
    /// Creates a template file from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    /// <param name="renderingMode">The rendering mode to use in creating the output document.</param>
    public Tales.TemplateFile CreateTemplateFile(FileInfo sourceFile,
                                                 Encoding encoding,
                                                 RenderingMode renderingMode)
    {
      var document = this.CreateDocument(sourceFile, encoding, renderingMode);
      return CreateTemplateFile(document);
    }

    /// <summary>
    /// Creates a template file from the given ZPT document.
    /// </summary>
    /// <returns>The template file.</returns>
    /// <param name="document">Document.</param>
    public Tales.TemplateFile CreateTemplateFile(IZptDocument document)
    {
      return new Tales.TemplateFile(document);
    }


    /// <summary>
    /// Creates an HTML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    public IZptDocument CreateHtmlDocument(FileInfo sourceFile, Encoding encoding)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }

      var sourceInfo = new SourceFileInfo(sourceFile);
      encoding = encoding?? DefaultEncoding;

      var doc = new HtmlAgilityPack.HtmlDocument();
      doc.Load(sourceFile.FullName, encoding);

      return new ZptHtmlDocument(doc, sourceInfo);
    }

    /// <summary>
    /// Creates an XML document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    public IZptDocument CreateXmlDocument(FileInfo sourceFile, Encoding encoding)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }

      IZptDocument output;
      var sourceInfo = new SourceFileInfo(sourceFile);
      encoding = encoding?? DefaultEncoding;

      using(var stream = sourceFile.OpenRead())
      {
        output = this.CreateXmlDocument(stream, sourceInfo, encoding);
      }

      return output;
    }


    /// <summary>
    /// Creates an HTML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Information about the source document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    public IZptDocument CreateHtmlDocument(Stream sourceStream, ISourceInfo sourceInfo, Encoding encoding)
    {
      if(sourceStream == null)
      {
        throw new ArgumentNullException(nameof(sourceStream));
      }

      sourceInfo = sourceInfo?? UnknownSourceFileInfo.Instance;
      encoding = encoding?? DefaultEncoding;

      var doc = new HtmlAgilityPack.HtmlDocument();
      doc.Load(sourceStream, encoding);

      return new ZptHtmlDocument(doc, sourceInfo);
    }

    /// <summary>
    /// Creates an XML document from a stream exposing the source document, and optional information about the source.
    /// </summary>
    /// <param name="sourceStream">A stream exposing the document content.</param>
    /// <param name="sourceInfo">Information about the source document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    public IZptDocument CreateXmlDocument(Stream sourceStream, ISourceInfo sourceInfo, Encoding encoding)
    {
      if(sourceStream == null)
      {
        throw new ArgumentNullException(nameof(sourceStream));
      }

      sourceInfo = sourceInfo?? UnknownSourceFileInfo.Instance;
      encoding = encoding?? DefaultEncoding;

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

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptDocumentFactory"/> class.
    /// </summary>
    /// <param name="forceDocumentType">
    /// An optional <c>System.Type</c> indicating the type to create when using <see cref="CreateDocument"/> or
    /// <see cref="M:CreateTemplateFile(FileInfo, Encoding, RenderingMode)"/>.
    /// </param>
    public ZptDocumentFactory(Type forceDocumentType = null)
    {
      _forceDocumentType = forceDocumentType;
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

