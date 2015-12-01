using System;
using System.Xml;
using System.IO;
using CSF.Zpt.Rendering;
using CSF.Zpt.Resources;

namespace CSF.Zpt
{
  /// <summary>
  /// Implementation of <see cref="ZptDocument"/> based on a <c>System.Xml.XmlDocument</c>.
  /// </summary>
  public class ZptXmlDocument : ZptDocument
  {
    #region fields

    private XmlDocument _document;
    private SourceFileInfo _sourceFile;

    #endregion

    #region properties

    /// <summary>
    /// Gets the original XML document.
    /// </summary>
    /// <value>The original XML document.</value>
    public virtual XmlDocument Document
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
    /// Renders the document to an <c>System.Xml.XmlDocument</c> instance.
    /// </summary>
    /// <returns>The rendered XML document.</returns>
    /// <param name="context">The rendering context, containing the object model of data available to the document.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    public XmlDocument RenderXml(RenderingContext context,
                                 RenderingOptions options = null)
    {
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      var opts = this.GetOptions(options);
      var element = this.RenderElement(context, options: opts);

      var output = new XmlDocument();
      output.LoadXml(element.ToString());

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
      var doc = this.RenderXml(context, opts);
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

      var xmlElement = element as ZptXmlElement;
      if(xmlElement == null)
      {
        string message = String.Format(ExceptionMessages.RenderedElementIncorrectType,
                                       typeof(ZptXmlElement).Name);
        throw new ArgumentException(message, "element");
      }

      using(var xmlWriter = new XmlTextWriter(writer))
      {
        xmlElement.Node.WriteTo(xmlWriter);  
      }
    }

    /// <summary>
    /// Creates a rendering model from the current instance.
    /// </summary>
    /// <returns>The rendering model.</returns>
    protected override ZptElement GetRootElement()
    {
      return new Rendering.ZptXmlElement(this.Document.DocumentElement, this.SourceFile, isRoot: true);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptXmlDocument"/> class.
    /// </summary>
    /// <param name="document">An XML document from which to create the current instance.</param>
    /// <param name="sourceFile">Information about the document's source file.</param>
    /// <param name="visitors">A collection of the visitor types to use.</param>
    public ZptXmlDocument(XmlDocument document,
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

