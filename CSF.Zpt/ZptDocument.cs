using System;
using System.Text;
using System.IO;
using CSF.Zpt.Rendering;
using System.Linq;
using CSF.Zpt.Tales;
using CSF.Zpt.Resources;

namespace CSF.Zpt
{
  /// <summary>
  /// Represents a ZPT document.
  /// </summary>
  public abstract class ZptDocument : IZptDocument
  {
    #region properties

    /// <summary>
    /// Gets the <see cref="RenderingMode"/> for which the current document type caters.
    /// </summary>
    /// <value>The rendering mode.</value>
    public abstract RenderingMode Mode { get; }

    #endregion

    #region methods

    /// <summary>
    /// Renders the document to a <c>System.String</c>.
    /// </summary>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="RenderingContext"/>, to configure it.</param>
    public virtual string Render(IRenderingOptions options = null,
                                 Action<RenderingContext> contextConfigurator = null)
    {
      return Render((object) null, options, contextConfigurator);
    }

    /// <summary>
    /// Renders the document to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="RenderingContext"/>, to configure it.</param>
    public virtual void Render(TextWriter writer,
                               IRenderingOptions options = null,
                               Action<RenderingContext> contextConfigurator = null)
    {
      Render(null, writer, options, contextConfigurator);
    }

    /// <summary>
    /// Renders the document to a <c>System.String</c>.
    /// </summary>
    /// <param name="model">An object for which the ZPT document is to be applied.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="RenderingContext"/>, to configure it.</param>
    public virtual string Render(object model,
                                 IRenderingOptions options = null,
                                 Action<RenderingContext> contextConfigurator = null)
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
        this.Render(model, writer, options, contextConfigurator);
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
    /// <param name="model">An object for which the ZPT document is to be applied.</param>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="RenderingContext"/>, to configure it.</param>
    public virtual void Render(object model,
                               TextWriter writer,
                               IRenderingOptions options = null,
                               Action<RenderingContext> contextConfigurator = null)
    {
      if(writer == null)
      {
        throw new ArgumentNullException(nameof(writer));
      }

      var opts = this.GetOptions(options);

      this.Render(writer, this.RenderElement(model, opts, contextConfigurator), opts);
    }

    /// <summary>
    /// Gets a collection of elements in the document which are defined as METAL macros.
    /// </summary>
    /// <returns>Elements representing the METAL macros.</returns>
    public abstract ITalesPathHandler GetMacros();

    /// <summary>
    /// Gets information about the source medium for the current instance
    /// </summary>
    /// <returns>The source info.</returns>
    public abstract ISourceInfo GetSourceInfo();

    /// <summary>
    /// Renders the current document, returning an <see cref="IZptElement"/> representing the rendered result.
    /// </summary>
    /// <returns>The result of the rendering process.</returns>
    /// <param name="model">An object to which the ZPT document is to be applied.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="RenderingContext"/>, to configure it.</param>
    protected virtual IZptElement RenderElement(object model,
                                               IRenderingOptions options,
                                               Action<RenderingContext> contextConfigurator)
    {
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      var output = this.GetRootElement();
      var context = options.CreateRootContext(output, model);

      ZptConstants.TraceSource.TraceInformation(Resources.LogMessageFormats.RenderingDocument,
                                                (output.SourceFile != null)? output.SourceFile.FullName : "<unknown>",
                                                nameof(ZptDocument),
                                                nameof(RenderElement));

      if(contextConfigurator != null)
      {
        contextConfigurator(context);
      }

      try
      {
        foreach(var visitor in options.ContextVisitors)
        {
          var contexts = visitor.VisitContext(context);

          if(contexts.Count() != 1)
          {
            string message = String.Format(Resources.ExceptionMessages.WrongCountOfReturnedContexts,
                                         typeof(IContextVisitor).Name,
                                         typeof(RenderingContext).Name);
            throw new RenderingException(message);
          }

          context = contexts.Single();
        }
      }
      catch(Exception ex)
      {
        ZptConstants.TraceSource.TraceEvent(System.Diagnostics.TraceEventType.Error,
                                            3,
                                            Resources.LogMessageFormats.UnexpectedRenderingException,
                                            nameof(ZptDocument),
                                            nameof(RenderElement),
                                            ex.ToString());
        throw;
      }

      return context.Element;
    }

    /// <summary>
    /// Gets a set of rendering options, or constructs a default set of options if a <c>null</c> reference is passed.
    /// </summary>
    /// <returns>The final non-null options.</returns>
    /// <param name="options">Options.</param>
    protected virtual IRenderingOptions GetOptions(IRenderingOptions options)
    {
      return options?? GetDefaultOptions();
    }

    /// <summary>
    /// Renders an element to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="element">The element to render.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    protected abstract void Render(TextWriter writer,
                                   IZptElement element,
                                   IRenderingOptions options);

    /// <summary>
    /// Creates a rendering model from the current instance.
    /// </summary>
    /// <returns>The rendering model.</returns>
    protected abstract IZptElement GetRootElement();

    /// <summary>
    /// Gets an instance of <see cref="IRenderingOptions"/> which represents the default options.
    /// </summary>
    /// <returns>The default options.</returns>
    protected abstract IRenderingOptions GetDefaultOptions();

    /// <summary>
    /// Converts the given <see cref="IZptElement"/> to an implementation-specific subclass, or raises an exception
    /// if the conversion is not valid.
    /// </summary>
    /// <returns>The converted element instance.</returns>
    /// <param name="element">The element for conversion.</param>
    /// <typeparam name="TElement">The desired element type.</typeparam>
    protected virtual TElement ConvertElement<TElement>(IZptElement element) where TElement : class,IZptElement
    {
      return ZptElement.ConvertElement<TElement>(element);
    }

    #endregion
  }
}

