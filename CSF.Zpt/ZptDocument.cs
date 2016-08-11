using System;
using System.Text;
using System.IO;
using CSF.Zpt.Rendering;
using System.Linq;
using CSF.Zpt.Tales;

namespace CSF.Zpt
{
  /// <summary>
  /// Represents a ZPT document.
  /// </summary>
  public abstract class ZptDocument : IZptDocument
  {
    #region fields

    private static log4net.ILog _logger;

    #endregion

    #region methods

    /// <summary>
    /// Renders the document to a <c>System.String</c>.
    /// </summary>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="RenderingContext"/>, to configure it.</param>
    public virtual string Render(RenderingOptions options = null,
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
        this.Render(writer, options, contextConfigurator);
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
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="RenderingContext"/>, to configure it.</param>
    public virtual void Render(TextWriter writer,
                               RenderingOptions options = null,
                               Action<RenderingContext> contextConfigurator = null)
    {
      if(writer == null)
      {
        throw new ArgumentNullException(nameof(writer));
      }

      var opts = this.GetOptions(options);

      this.Render(writer, this.RenderElement(opts, contextConfigurator), opts);
    }

    /// <summary>
    /// Gets a collection of elements in the document which are defined as METAL macros.
    /// </summary>
    /// <returns>Elements representing the METAL macros.</returns>
    public abstract ITalesPathHandler GetMacros();

    /// <summary>
    /// Gets information about the source file for the current instance.
    /// </summary>
    /// <returns>The file info.</returns>
    public abstract SourceFileInfo GetSourceFileInfo();

    /// <summary>
    /// Renders the current document, returning an <see cref="ZptElement"/> representing the rendered result.
    /// </summary>
    /// <returns>The result of the rendering process.</returns>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="RenderingContext"/>, to configure it.</param>
    protected virtual ZptElement RenderElement(RenderingOptions options,
                                               Action<RenderingContext> contextConfigurator)
    {
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      var output = this.GetRootElement();
      var context = options.CreateRootContext(output);

      _logger.InfoFormat(Resources.LogMessageFormats.RenderingDocument,
                         (output.SourceFile != null)? output.SourceFile.GetFullName() : "<unknown>");

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
        _logger.Error(Resources.LogMessageFormats.UnexpectedRenderingException);
        _logger.Error(ex);
        throw;
      }

      return context.Element;
    }

    /// <summary>
    /// Gets a set of rendering options, or constructs a default set of options if a <c>null</c> reference is passed.
    /// </summary>
    /// <returns>The final non-null options.</returns>
    /// <param name="options">Options.</param>
    protected virtual RenderingOptions GetOptions(RenderingOptions options)
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
                                   ZptElement element,
                                   RenderingOptions options);

    /// <summary>
    /// Creates a rendering model from the current instance.
    /// </summary>
    /// <returns>The rendering model.</returns>
    protected abstract ZptElement GetRootElement();

    /// <summary>
    /// Gets an instance of <see cref="RenderingOptions"/> which represents the default options.
    /// </summary>
    /// <returns>The default options.</returns>
    protected abstract RenderingOptions GetDefaultOptions();

    #endregion

    #region constructor

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.ZptDocument"/> class.
    /// </summary>
    static ZptDocument()
    {
      _logger = log4net.LogManager.GetLogger(typeof(ZptDocument));
    }

    #endregion
  }
}

