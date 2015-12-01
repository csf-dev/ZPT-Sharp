using System;
using System.IO;
using HtmlAgilityPack;
using CSF.Zpt.Rendering;

namespace CSF.Zpt
{
  /// <summary>
  /// Implementation of <see cref="ZptDocument"/> based on an <c>HtmlAgilityPack.HtmlDocument</c>.
  /// </summary>
  public class ZptHtmlDocument : ZptDocument
  {
    #region fields

    private HtmlDocument _document;
    private SourceFileInfo _sourceFile;

    #endregion

    #region properties

    /// <summary>
    /// Gets the original HTML document.
    /// </summary>
    /// <value>The original HTML document.</value>
    public virtual HtmlDocument Document
    {
      get {
        return _document;
      }
    }

    /// <summary>
    /// Gets information about the document's source file.
    /// </summary>
    /// <value>The source file.</value>
    public virtual SourceFileInfo SourceFile
    {
      get {
        return _sourceFile;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Renders the document to an <c>HtmlAgilityPack.HtmlDocument</c> instance.
    /// </summary>
    /// <returns>The rendered HTML document.</returns>
    /// <param name="context">The rendering context, containing the object model of data available to the document.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    public HtmlDocument RenderHtml(RenderingContext context,
                                   RenderingOptions options = null)
    {
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      var opts = this.GetOptions(options);
      var element = this.RenderElement(context, opts);

      var output = new HtmlDocument();
      output.LoadHtml(element.ToString());

      return output;
    }

    /// <summary>
    /// Renders the document to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="context">The rendering context, containing the object model of data available to the document.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    public override void Render(TextWriter writer,
                                RenderingContext context,
                                RenderingOptions options = null)
    {
      var opts = this.GetOptions(options);

      var doc = this.RenderHtml(context, opts);
      doc.Save(writer);
    }

    /// <summary>
    /// Renders an element to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="element">The element to render.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    protected override void Render(TextWriter writer,
                                   ZptElement element,
                                   RenderingOptions options)
    {
      if(writer == null)
      {
        throw new ArgumentNullException("writer");
      }
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      var htmlElement = element as ZptHtmlElement;
      if(htmlElement == null)
      {
        throw new ArgumentException("Element must be an instance of HtmlElement.", "element");
      }

      htmlElement.Node.WriteTo(writer);
    }

    /// <summary>
    /// Creates a rendering model from the current instance.
    /// </summary>
    /// <returns>The rendering model.</returns>
    protected override ZptElement GetRootElement()
    {
      return new ZptHtmlElement(this.Document.DocumentNode, this.SourceFile, isRoot: true);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptHtmlDocument"/> class.
    /// </summary>
    /// <param name="document">An HTML document from which to create the current instance.</param>
    /// <param name="sourceFile">Information about the document's source file.</param>
    /// <param name="visitors">A collection of the visitor types to use.</param>
    public ZptHtmlDocument(HtmlDocument document,
                           SourceFileInfo sourceFile,
                           ElementVisitor[] visitors = null) : base(visitors)
    {
      if(document == null)
      {
        throw new ArgumentNullException("document");
      }
      if(sourceFile == null)
      {
        throw new ArgumentNullException("sourceFile");
      }

      _document = document;
      _sourceFile = sourceFile;
    }

    #endregion
  }
}

