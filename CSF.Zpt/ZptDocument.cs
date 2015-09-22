using System;
using System.Text;
using System.IO;
using CSF.Zpt.Rendering;

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

    /// <summary>
    /// Gets the default attribute prefix for TAL attributes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For XML documents this is only a default prefix, since element matching is really handled via the namespace
    /// <see cref="TalNamespace"/>, and the prefix is only an alias defined using an <c>xmlns</c> directive.
    /// </para>
    /// <para>
    /// For HTML documents, which do not have or use namespaces, this prefix is absolute.
    /// </para>
    /// </remarks>
    public static readonly string TalAttributePrefix = "tal";

    /// <summary>
    /// Gets the default attribute prefix for METAL attributes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For XML documents this is only a default prefix, since element matching is really handled via the namespace
    /// <see cref="MetalNamespace"/>, and the prefix is only an alias defined using an <c>xmlns</c> directive.
    /// </para>
    /// <para>
    /// For HTML documents, which do not have or use namespaces, this prefix is absolute.
    /// </para>
    /// </remarks>
    public static readonly string MetalAttributePrefix = "metal";

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
    public virtual void Render(TextWriter writer, RenderingContext context)
    {
      if(writer == null)
      {
        throw new ArgumentNullException("writer");
      }
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      this.Render(writer, this.RenderElement(context));
    }

    /// <summary>
    /// Renders the current document, returning an <see cref="Element"/> representing the rendered result.
    /// </summary>
    /// <returns>The result of the rendering process.</returns>
    /// <param name="context">The rendering context, containing the object model of data available to the document.</param>
    protected virtual Element RenderElement(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      var output = this.GetRootElement();

      new MetalVisitor().VisitRecursively(output, context.MetalContext);
      new TalVisitor().VisitRecursively(output, context.TalContext);

      return output;
    }

    /// <summary>
    /// Renders an element to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="element">The element to render.</param>
    protected abstract void Render(TextWriter writer, Element element);

    /// <summary>
    /// Creates a rendering model from the current instance.
    /// </summary>
    /// <returns>The rendering model.</returns>
    protected abstract Element GetRootElement();

    #endregion
  }
}

