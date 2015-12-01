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
    #region fields

    private ElementVisitor[] _visitors;

    #endregion

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
    /// Renders the current document, returning an <see cref="ZptElement"/> representing the rendered result.
    /// </summary>
    /// <returns>The result of the rendering process.</returns>
    /// <param name="context">The rendering context, containing the object model of data available to the document.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    protected virtual ZptElement RenderElement(RenderingContext context,
                                               RenderingOptions options)
    {
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      var output = this.GetRootElement().Clone();
      var opts = this.GetOptions(options);

      foreach(var visitor in _visitors)
      {
        visitor.VisitRecursively(output, context, opts);
      }

      return output;
    }

    /// <summary>
    /// Gets a set of rendering options, or constructs a default set of options if a <c>null</c> reference is passed.
    /// </summary>
    /// <returns>The final non-null options.</returns>
    /// <param name="options">Options.</param>
    protected virtual RenderingOptions GetOptions(RenderingOptions options)
    {
      return options?? RenderingOptions.Default;
    }

    /// <summary>
    /// Renders an element to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="element">The element to render.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    protected abstract void Render(TextWriter writer,
                                   ZptElement element,
                                   RenderingOptions options);

    /// <summary>
    /// Creates a rendering model from the current instance.
    /// </summary>
    /// <returns>The rendering model.</returns>
    protected abstract ZptElement GetRootElement();

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptDocument"/> class.
    /// </summary>
    /// <param name="visitors">A collection of the visitor types to use.</param>
    public ZptDocument(ElementVisitor[] visitors)
    {
      _visitors = visitors?? new ElementVisitor[] {
        new CSF.Zpt.Metal.MetalVisitor(),
        new CSF.Zpt.Metal.SourceAnnotationVisitor(),
        new CSF.Zpt.Tal.TalVisitor(),
      };
    }

    #endregion
  }
}

