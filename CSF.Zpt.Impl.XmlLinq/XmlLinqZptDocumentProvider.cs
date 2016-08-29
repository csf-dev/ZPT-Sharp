using System;
using System.IO;
using System.Text;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Impl
{
  public class XmlLinqZptDocumentProvider : IZptDocumentProvider
  {
    #region fields

    private IXmlUrlResolverFactory _resolverFactory;

    #endregion

    #region methods

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

      System.Xml.Linq.XDocument doc;

      using(var streamReader = new StreamReader(sourceStream, encoding))
      using(var reader = System.Xml.XmlReader.Create(streamReader, settings))
      {
        var options = System.Xml.Linq.LoadOptions.PreserveWhitespace | System.Xml.Linq.LoadOptions.SetLineInfo;
        doc = System.Xml.Linq.XDocument.Load(reader, options);
      }

      return new ZptXmlLinqDocument(doc, sourceInfo);
    }

    #endregion

    #region constructor

    public XmlLinqZptDocumentProvider()
    {
      _resolverFactory = new XmlUrlResolverFactory();
    }

    #endregion
  }
}

