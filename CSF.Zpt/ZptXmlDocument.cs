using System;
using System.Xml;
using System.IO;
using CSF.Zpt.Rendering;

namespace CSF.Zpt
{
  /// <summary>
  /// Implementation of <see cref="ZptDocument"/> based on a <c>System.Xml.XmlDocument</c>.
  /// </summary>
  public class ZptXmlDocument : ZptDocument
  {
    #region fields

    private XmlDocument _document;

    #endregion

    #region properties

    /// <summary>
    /// Gets the original XML document.
    /// </summary>
    /// <value>The original XML document.</value>
    public virtual XmlDocument XmlDocument
    {
      get {
        return _document;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Renders the document to an <c>System.Xml.XmlDocument</c> instance.
    /// </summary>
    /// <returns>The rendered XML document.</returns>
    /// <param name="context">The rendering context, containing the object model of data available to the document.</param>
    public XmlDocument RenderXml(RenderingContext context)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    /// <summary>
    /// Renders the document to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="context">The rendering context, containing the object model of data available to the document.</param>
    public override void Render(TextWriter writer, RenderingContext context)
    {
      var doc = this.RenderXml(context);
      doc.Save(writer);
    }

    /// <summary>
    /// Renders an element to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="element">The element to render.</param>
    protected override void Render(TextWriter writer, Element element)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a rendering model from the current instance.
    /// </summary>
    /// <returns>The rendering model.</returns>
    protected override Element GetRootElement()
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptXmlDocument"/> class.
    /// </summary>
    /// <param name="document">An XML document from which to create the current instance.</param>
    public ZptXmlDocument(XmlDocument document)
    {
      if(document == null)
      {
        throw new ArgumentNullException("document");
      }

      _document = document;
    }

    #endregion
  }
}

