using System;
using System.IO;
using System.Text;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.DocumentProviders
{
  /// <summary>
  /// Implementation of <see cref="IZptDocumentProvider"/> which creates XML documents based on
  /// <c>System.Xml.XmlDocument</c>.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Generally, you don't want to be using this type if <c>System.Xml.Linq</c> is available (which it usually is if
  /// you're using .NET framework 4.5 or up).
  /// </para>
  /// <para>
  /// The XML-Linq implementation is superior to this one in several ways and produces more standards-compliant output
  /// also.
  /// </para>
  /// </remarks>
  public class LegacyXmlZptDocumentProvider : IZptDocumentProvider
  {
    #region fields

    private IXmlUrlResolverFactory _resolverFactory;

    #endregion

    #region methods

    /// <summary>
    /// Creates a document from an <c>XmlDocument</c> object and information about the source of the document.
    /// </summary>
    /// <returns>The ZPT document implementation.</returns>
    /// <param name="xmlDocument">An XML document.</param>
    /// <param name="sourceInfo">The source info.</param>
    public IZptDocument CreateDocument(System.Xml.XmlDocument xmlDocument, ISourceInfo sourceInfo)
    {
      if(xmlDocument == null)
      {
        throw new ArgumentNullException(nameof(xmlDocument));
      }
      if(sourceInfo == null)
      {
        throw new ArgumentNullException(nameof(sourceInfo));
      }

      return new ZptXmlDocument(xmlDocument, sourceInfo);
    }

    #endregion

    #region IZptDocumentProvider implementation

    /// <summary>
    /// Gets the rendering mode for documents created by this provider.
    /// </summary>
    /// <returns>The rendering mode.</returns>
    public RenderingMode GetRenderingMode()
    {
      return RenderingMode.Html;
    }

    /// <summary>
    /// Creates a document from a source file.
    /// </summary>
    /// <returns>The document.</returns>
    /// <param name="sourceFile">Source file.</param>
    /// <param name="encoding">Encoding.</param>
    public IZptDocument CreateDocument(FileInfo sourceFile, Encoding encoding)
    {
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }

      IZptDocument output;
      var sourceInfo = new SourceFileInfo(sourceFile);

      using(var stream = sourceFile.OpenRead())
      {
        output = this.CreateDocument(stream, sourceInfo, encoding);
      }

      return output;
    }

    /// <summary>
    /// Creates a document from a stream.
    /// </summary>
    /// <returns>The document.</returns>
    /// <param name="sourceStream">Source stream.</param>
    /// <param name="sourceInfo">Source info.</param>
    /// <param name="encoding">Encoding.</param>
    public IZptDocument CreateDocument(Stream sourceStream, ISourceInfo sourceInfo, Encoding encoding)
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
        XmlResolver = _resolverFactory.GetResolver(),
        DtdProcessing = System.Xml.DtdProcessing.Parse,
      };

      var doc = new System.Xml.XmlDocument();

      using(var streamReader = new StreamReader(sourceStream, encoding))
      using(var reader = System.Xml.XmlReader.Create(streamReader, settings))
      {
        doc.Load(reader);
      }

      return CreateDocument(doc, sourceInfo);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.DocumentProviders.LegacyXmlZptDocumentProvider"/> class.
    /// </summary>
    public LegacyXmlZptDocumentProvider()
    {
      _resolverFactory = new XmlUrlResolverFactory();
    }

    #endregion
  }
}

