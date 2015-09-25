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

