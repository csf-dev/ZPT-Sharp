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
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    public virtual string Render(RenderingOptions options = null)
    {
      string output;
      var opts = this.GetOptions(options);

      Stream stream = null;
      StreamWriter writer = null;
      StreamReader reader = null;

      try
      {
        stream = new MemoryStream();

        writer = new StreamWriter(stream, opts.OutputEncoding);
        this.Render(writer, options);
        writer.Flush();

        stream.Position = 0;
        reader = new StreamReader(stream);
        output = reader.ReadToEnd();
      }
      finally
      {
        if(stream != null)
        {
          stream.Dispose();
        }
      }

      return output.ToString();
    }

    /// <summary>
    /// Renders the document to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    public virtual void Render(TextWriter writer,
                               RenderingOptions options = null)
    {
      if(writer == null)
      {
        throw new ArgumentNullException(nameof(writer));
      }

      var opts = this.GetOptions(options);

      this.Render(writer, this.RenderElement(opts), opts);
    }

    /// <summary>
    /// Gets a collection of elements in the document which are defined as METAL macros.
    /// </summary>
    /// <returns>Elements representing the METAL macros.</returns>
    internal abstract CSF.Zpt.Metal.MetalMacro[] GetMacros();

    /// <summary>
    /// Renders the current document, returning an <see cref="ZptElement"/> representing the rendered result.
    /// </summary>
    /// <returns>The result of the rendering process.</returns>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    protected virtual ZptElement RenderElement(RenderingOptions options)
    {
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      var output = this.GetRootElement().Clone();
      var context = options.CreateRootContext(output);

      foreach(var visitor in options.ContextVisitors)
      {
        visitor.VisitContext(context);
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
  }
}

