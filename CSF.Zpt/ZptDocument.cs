using System;
using System.Text;
using System.IO;

namespace CSF.Zpt
{
  /// <summary>
  /// Represents a ZPT document.
  /// </summary>
  public abstract class ZptDocument
  {
    #region constants

    /// <summary>
    /// Constant value indicates the XML namespace for TAL: Template Attribute Language.
    /// </summary>
    public static readonly string TalNamespace = "http://xml.zope.org/namespaces/tal";

    /// <summary>
    /// Constant value indicates the XML namespace for METAL: Macro Expansion Template Attribute Language.
    /// </summary>
    public static readonly string MetalNamespace = "http://xml.zope.org/namespaces/metal";

    #endregion

    #region methods

    /// <summary>
    /// Renders the document to a <c>System.String</c>.
    /// </summary>
    /// <param name="context">The rendering context, containing the object model of data available to the document.</param>
    public virtual string Render(RenderingContext context)
    {
      var output = new StringBuilder();

      using(var writer = new StringWriter(output))
      {
        this.Render(writer, context);
      }

      return output.ToString();
    }

    /// <summary>
    /// Renders the document to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="context">The rendering context, containing the object model of data available to the document.</param>
    public abstract void Render(TextWriter writer, RenderingContext context);

    #endregion
  }
}

