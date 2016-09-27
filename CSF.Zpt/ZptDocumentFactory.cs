using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSF.Zpt.Rendering;
using CSF.Configuration;

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

    #endregion

    #region fields

    private IZptDocumentProviderRegistry _providerRegistry;

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

    #region IZptDocumentFactory implementation

    /// <summary>
    /// Gets a value indicating the <see cref="RenderingMode"/> detected for a given source file.
    /// </summary>
    /// <returns><c>true</c> if the <see cref="RenderingMode"/> could be auto-detected; <c>false</c> if not.</returns>
    /// <param name="sourceFile">The source file.</param>
    /// <param name="detectedMode">Exposes the detected rendering mode.</param>
    public bool TryDetectMode(FileInfo sourceFile, out RenderingMode detectedMode)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }

      bool output;
      string extension = sourceFile.Extension;

      if(HtmlSuffixes.Contains(extension))
      {
        output = true;
        detectedMode = RenderingMode.Html;
      }
      else if(XmlSuffixes.Contains(sourceFile.Extension))
      {
        output = true;
        detectedMode = RenderingMode.Xml;
      }
      else
      {
        output = false;
        detectedMode = (RenderingMode) (-1);
      }

      return output;
    }

    /// <summary>
    /// Creates a document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    /// <param name="renderingMode">The rendering mode to use in creating the output document.</param>
    public IZptDocument CreateDocument(FileInfo sourceFile,
                                       Encoding encoding,
                                       RenderingMode? renderingMode)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }
      if(renderingMode.HasValue && !renderingMode.Value.IsDefinedValue())
      {
        throw new ArgumentException(Resources.ExceptionMessages.InvalidRenderingMode, nameof(renderingMode));
      }

      RenderingMode actualMode;

      if(!renderingMode.HasValue)
      {
        bool detectionSuccess = TryDetectMode(sourceFile, out actualMode);
        if(!detectionSuccess)
        {
          var message = String.Format(Resources.ExceptionMessages.UnsupportedDocumentFilenameExtension,
                                      sourceFile.FullName);
          throw new ArgumentException(message, nameof(sourceFile));
        }
      }
      else
      {
        actualMode = renderingMode.Value;
      }

      encoding = encoding?? DefaultEncoding;
      var provider = SelectProvider(actualMode);

      return CreateDocument(provider, sourceFile, encoding);
    }

    /// <summary>
    /// Creates a document from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="providerType">The <see cref="IZptDocumentProvider"/> type to use for creating the document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    public IZptDocument CreateDocument(FileInfo sourceFile,
                                       Type providerType,
                                       Encoding encoding = null)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }
      if(providerType == null)
      {
        throw new ArgumentNullException(nameof(providerType));
      }

      encoding = encoding?? DefaultEncoding;
      var provider = SelectProvider(providerType);

      return CreateDocument(provider, sourceFile, encoding);
    }

    /// <summary>
    /// Creates a document from the given source stream.
    /// </summary>
    /// <param name="source">The stream containing the document to create.</param>
    /// <param name="renderingMode">The rendering mode to use in creating the output document.</param>
    /// <param name="sourceInfo">Optional information about the source of the document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    public IZptDocument CreateDocument(Stream source,
                                       RenderingMode renderingMode,
                                       ISourceInfo sourceInfo = null,
                                       Encoding encoding = null)
    {
      if(source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }
      if(!renderingMode.IsDefinedValue())
      {
        throw new ArgumentException(Resources.ExceptionMessages.InvalidRenderingMode, nameof(renderingMode));
      }

      encoding = encoding?? DefaultEncoding;
      sourceInfo = sourceInfo?? UnknownSourceFileInfo.Instance;
      var provider = SelectProvider(renderingMode);

      return CreateDocument(provider, source, sourceInfo, encoding);
    }

    /// <summary>
    /// Creates a document from the given source file.
    /// </summary>
    /// <param name="source">The stream containing the document to create.</param>
    /// <param name="providerType">The <see cref="IZptDocumentProvider"/> type to use for creating the document.</param>
    /// <param name="sourceInfo">Optional information about the source of the document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    public IZptDocument CreateDocument(Stream source,
                                       Type providerType,
                                       ISourceInfo sourceInfo = null,
                                       Encoding encoding = null)
    {
      if(source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }
      if(providerType == null)
      {
        throw new ArgumentNullException(nameof(providerType));
      }

      encoding = encoding?? DefaultEncoding;
      sourceInfo = sourceInfo?? UnknownSourceFileInfo.Instance;
      var provider = SelectProvider(providerType);

      return CreateDocument(provider, source, sourceInfo, encoding);
    }

    private IZptDocument CreateDocument(IZptDocumentProvider provider,
                                        Stream source,
                                        ISourceInfo sourceInfo,
                                        Encoding encoding)
    {
      if(provider == null)
      {
        throw new ArgumentNullException(nameof(provider));
      }
      if(source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }
      if(sourceInfo == null)
      {
        throw new ArgumentNullException(nameof(sourceInfo));
      }
      if(encoding == null)
      {
        throw new ArgumentNullException(nameof(encoding));
      }

      return provider.CreateDocument(source, sourceInfo, encoding);
    }

    private IZptDocument CreateDocument(IZptDocumentProvider provider,
                                        FileInfo source,
                                        Encoding encoding)
    {
      if(provider == null)
      {
        throw new ArgumentNullException(nameof(provider));
      }
      if(source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }
      if(encoding == null)
      {
        throw new ArgumentNullException(nameof(encoding));
      }

      return provider.CreateDocument(source, encoding);
    }

    private IZptDocumentProvider SelectProvider(RenderingMode mode)
    {
      IZptDocumentProvider output;

      switch(mode)
      {
      case RenderingMode.Html:
        output = _providerRegistry.DefaultHtml;
        break;

      case RenderingMode.Xml:
        output = _providerRegistry.DefaultXml;
        break;

      default:
        throw new ArgumentException("Rendering mode must be a concrete implementation", nameof(mode));
      }

      return output;
    }

    private IZptDocumentProvider SelectProvider(Type type)
    {
      if(type == null)
      {
        throw new ArgumentNullException(nameof(type));
      }

      return _providerRegistry.Get(type);
    }

    #endregion

    #region ITemplateFileFactory implementation

    /// <summary>
    /// Creates a template file from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    /// <param name="renderingMode">The rendering mode to use in creating the output document.</param>
    public Tales.TemplateFile CreateTemplateFile(FileInfo sourceFile,
                                                 Encoding encoding,
                                                 RenderingMode? renderingMode)
    {
      var document = this.CreateDocument(sourceFile, encoding, renderingMode);
      return CreateTemplateFile(document);
    }

    /// <summary>
    /// Creates a template file from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="providerType">The <see cref="IZptDocumentProvider"/> type to use for creating the document.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    public Tales.TemplateFile CreateTemplateFile(FileInfo sourceFile,
                                                 Type providerType,
                                                 Encoding encoding = null)
    {
      var document = this.CreateDocument(sourceFile, providerType, encoding);
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

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptDocumentFactory"/> class.
    /// </summary>
    public ZptDocumentFactory(IZptDocumentProviderRegistry registry = null)
    {
      _providerRegistry = registry?? ZptDocumentProviderRegistry.Default;
    }

    #endregion

    #region static members

    /// <summary>
    /// Gets the file extensions which indicate that an <see cref="RenderingMode.Html"/> should be used.
    /// </summary>
    /// <returns>The HTML extensions.</returns>
    public static IEnumerable<string> GetHtmlExtensions()
    {
      return new HashSet<string>(HtmlSuffixes);
    }

    /// <summary>
    /// Gets the file extensions which indicate that an <see cref="RenderingMode.Xml"/> should be used.
    /// </summary>
    /// <returns>The XML extensions.</returns>
    public static IEnumerable<string> GetXmlExtensions()
    {
      return new HashSet<string>(XmlSuffixes);
    }

    #endregion
  }
}

