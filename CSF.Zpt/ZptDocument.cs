﻿using System;
using System.Text;
using System.IO;
using CSF.Zpt.Rendering;
using System.Linq;
using CSF.Zpt.Tales;
using CSF.Zpt.Resources;
using CSF.Zpt.Metal;

namespace CSF.Zpt
{
  /// <summary>
  /// Represents a ZPT document.
  /// </summary>
  public abstract class ZptDocument : IZptDocument
  {
    #region fields

    private IElementRenderer _elementRenderer;

    #endregion

    #region properties

    /// <summary>
    /// Gets the <see cref="RenderingMode"/> for which the current document type caters.
    /// </summary>
    /// <value>The rendering mode.</value>
    public abstract RenderingMode Mode { get; }

    /// <summary>
    /// Gets the element renderer for the current instance.
    /// </summary>
    /// <value>The element renderer.</value>
    protected virtual IElementRenderer ElementRenderer { get { return _elementRenderer; } }

    #endregion

    #region methods

    /// <summary>
    /// Renders the document to a <c>System.String</c>.
    /// </summary>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="IModelValueContainer"/>, to configure it.</param>
    public virtual string Render(IRenderingSettings options = null,
                                 Action<IModelValueContainer> contextConfigurator = null)
    {
      return Render((object) null, options, contextConfigurator);
    }

    /// <summary>
    /// Renders the document to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="IModelValueContainer"/>, to configure it.</param>
    public virtual void Render(TextWriter writer,
                               IRenderingSettings options = null,
                               Action<IModelValueContainer> contextConfigurator = null)
    {
      Render(null, writer, options, contextConfigurator);
    }

    /// <summary>
    /// Renders the document to a <c>System.String</c>.
    /// </summary>
    /// <param name="model">An object for which the ZPT document is to be applied.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="IModelValueContainer"/>, to configure it.</param>
    public virtual string Render(object model,
                                 IRenderingSettings options = null,
                                 Action<IModelValueContainer> contextConfigurator = null)
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
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="IModelValueContainer"/>, to configure it.</param>
    public virtual void Render(object model,
                               TextWriter writer,
                               IRenderingSettings options = null,
                               Action<IModelValueContainer> contextConfigurator = null)
    {
      if(writer == null)
      {
        throw new ArgumentNullException(nameof(writer));
      }

      var opts = GetOptions(options);
      var root = GetRootElement();

      this.Render(writer, ElementRenderer.RenderElement(model, root, opts, contextConfigurator), opts);
    }

    /// <summary>
    /// Gets a collection of elements in the document which are defined as METAL macros.
    /// </summary>
    /// <returns>Elements representing the METAL macros.</returns>
    public abstract IMetalMacroContainer GetMacros();

    /// <summary>
    /// Gets information about the source medium for the current instance
    /// </summary>
    /// <returns>The source info.</returns>
    public abstract ISourceInfo GetSourceInfo();


    /// <summary>
    /// Gets a set of rendering options, or constructs a default set of options if a <c>null</c> reference is passed.
    /// </summary>
    /// <returns>The final non-null options.</returns>
    /// <param name="options">Options.</param>
    protected virtual IRenderingSettings GetOptions(IRenderingSettings options)
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
                                   IRenderingSettings options);

    /// <summary>
    /// Creates a rendering model from the current instance.
    /// </summary>
    /// <returns>The rendering model.</returns>
    protected abstract IZptElement GetRootElement();

    /// <summary>
    /// Gets an instance of <see cref="IRenderingSettings"/> which represents the default options.
    /// </summary>
    /// <returns>The default options.</returns>
    protected abstract IRenderingSettings GetDefaultOptions();

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

    /// <summary>
    /// Coordinates logic to the <see cref="IElementRenderer"/>.
    /// </summary>
    /// <returns>The rendered element.</returns>
    /// <param name="model">Model.</param>
    /// <param name="options">Options.</param>
    /// <param name="contextConfigurator">Context configurator.</param>
    protected virtual IZptElement RenderElement(object model,
                                                IRenderingSettings options,
                                                Action<IModelValueContainer> contextConfigurator)
    {
      var opts = this.GetOptions(options);
      var root = this.GetRootElement();

      return ElementRenderer.RenderElement(model, root, opts, contextConfigurator);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptDocument"/> class.
    /// </summary>
    /// <param name="elementRenderer">The element renderer.</param>
    public ZptDocument(IElementRenderer elementRenderer = null)
    {
      _elementRenderer = elementRenderer?? new ElementRenderer();
    }

    #endregion
  }
}

