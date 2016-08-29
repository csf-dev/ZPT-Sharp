using System;
using System.IO;
using System.Text;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Impl
{
  public class HtmlZptDocumentProvider : IZptDocumentProvider
  {
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
      if(encoding == null)
      {
        throw new ArgumentNullException(nameof(encoding));
      }

      var sourceInfo = new SourceFileInfo(sourceFile);
      var doc = new HtmlAgilityPack.HtmlDocument();
      doc.Load(sourceFile.FullName, encoding);

      return new ZptHtmlDocument(doc, sourceInfo);
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

      var doc = new HtmlAgilityPack.HtmlDocument();
      doc.Load(sourceStream, encoding);
      return new ZptHtmlDocument(doc, sourceInfo);
    }

    #endregion
  }
}

