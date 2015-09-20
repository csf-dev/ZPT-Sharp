using System;
using HtmlAgilityPack;
using System.IO;

namespace CSF.Zpt
{
  /// <summary>
  /// Implementation of <see cref="ZptDocument"/> based on an <c>HtmlAgilityPack.HtmlDocument</c>.
  /// </summary>
  public class ZptHtmlDocument : ZptDocument
  {
    #region methods

    /// <summary>
    /// Renders the document to an <c>HtmlAgilityPack.HtmlDocument</c> instance.
    /// </summary>
    /// <returns>The rendered HTML document.</returns>
    /// <param name="context">The rendering context, containing the object model of data available to the document.</param>
    public HtmlDocument RenderHtml(RenderingContext context)
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
      var doc = this.RenderHtml(context);
      doc.Save(writer);
    }

    #endregion
  }
}

