using System;
using System.Xml;
using System.IO;

namespace CSF.Zpt
{
  /// <summary>
  /// Implementation of <see cref="ZptDocument"/> based on a <c>System.Xml.XmlDocument</c>.
  /// </summary>
  public class ZptXmlDocument : ZptDocument
  {
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

    #endregion
  }
}

