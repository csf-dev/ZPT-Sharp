using System;
using System.IO;
using System.Text;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.DocumentProviders
{
  /// <summary>
  /// Implementation of <see cref="IZptDocumentProvider"/> which creates documents based on the HTML Agility pack.
  /// See https://htmlagilitypack.codeplex.com/
  /// </summary>
  public class HtmlZptDocumentProvider : IZptDocumentProvider
  {
    #region methods

    /// <summary>
    /// Creates a document from a HAP document object and information about the source of the document.
    /// </summary>
    /// <returns>The ZPT document implementation.</returns>
    /// <param name="hapDocument">A HTML Agility pack document.</param>
    /// <param name="sourceInfo">The source info.</param>
    public IZptDocument CreateDocument(HtmlAgilityPack.HtmlDocument hapDocument, ISourceInfo sourceInfo)
    {
      if(hapDocument == null)
      {
        throw new ArgumentNullException(nameof(hapDocument));
      }
      if(sourceInfo == null)
      {
        throw new ArgumentNullException(nameof(sourceInfo));
      }

      return new ZptHtmlDocument(hapDocument, sourceInfo);
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
      if(encoding == null)
      {
        throw new ArgumentNullException(nameof(encoding));
      }

      var sourceInfo = new SourceFileInfo(sourceFile);

      var doc = new HtmlAgilityPack.HtmlDocument();
      doc.Load(sourceFile.FullName, encoding);

      return CreateDocument(doc, sourceInfo);
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
      if(encoding == null)
      {
        throw new ArgumentNullException(nameof(encoding));
      }

      var doc = new HtmlAgilityPack.HtmlDocument();
      doc.Load(sourceStream, encoding);

      return CreateDocument(doc, sourceInfo);
    }

    #endregion
  }
}

