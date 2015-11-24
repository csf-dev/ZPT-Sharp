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
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    public virtual string Render(RenderingContext context,
                                 RenderingOptions options = null)
    {
      var output = new StringBuilder();

      using(var writer = new StringWriter(output))
      {
        this.Render(writer, context, this.GetOptions(options));
      }

      return output.ToString();
    }

    /// <summary>
    /// Renders the document to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="context">The rendering context, containing the object model of data available to the document.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    public virtual void Render(TextWriter writer,
                               RenderingContext context,
                               RenderingOptions options = null)
    {
      if(writer == null)
      {
        throw new ArgumentNullException("writer");
      }
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      var opts = this.GetOptions(options);
      this.Render(writer, this.RenderElement(context, opts), opts);
    }

    /// <summary>
    /// Renders the current document, returning an <see cref="Element"/> representing the rendered result.
    /// </summary>
    /// <returns>The result of the rendering process.</returns>
    /// <param name="context">The rendering context, containing the object model of data available to the document.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    protected virtual Element RenderElement(RenderingContext context,
                                            RenderingOptions options)
    {
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      var output = this.GetRootElement().Clone();
      var opts = this.GetOptions(options);

      new MetalVisitor(options: opts).VisitRecursively(output, context.MetalContext);
      new SourceAnnotationVisitor(options: opts).VisitRecursively(output, context.MetalContext);
      new TalVisitor(options: opts).VisitRecursively(output, context.TalContext);

      return output;
    }

    /// <summary>
    /// Gets a set of rendering options, or constructs a default set of options if a <c>null</c> reference is passed.
    /// </summary>
    /// <returns>The final non-null options.</returns>
    /// <param name="options">Options.</param>
    protected virtual RenderingOptions GetOptions(RenderingOptions options)
    {
      return options?? new RenderingOptions();
    }

    /// <summary>
    /// Renders an element to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="element">The element to render.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    protected abstract void Render(TextWriter writer,
                                   Element element,
                                   RenderingOptions options);

    /// <summary>
    /// Creates a rendering model from the current instance.
    /// </summary>
    /// <returns>The rendering model.</returns>
    protected abstract Element GetRootElement();

    #endregion
  }
}

