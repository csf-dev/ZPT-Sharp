using System;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using CSF.Zpt.Rendering;
using CSF.Zpt.Resources;
using CSF.Zpt.Tales;
using CSF.Zpt;
using CSF.Zpt.Metal;

namespace CSF.Zpt.DocumentProviders
{
  /// <summary>
  /// Implementation of <see cref="ZptDocument"/> based on an <c>HtmlAgilityPack.HtmlDocument</c>.
  /// </summary>
  public class ZptHtmlDocument : ZptDocument
  {
    #region fields

    private HtmlDocument _document;
    private ISourceInfo _sourceFile;

    #endregion

    #region properties

    /// <summary>
    /// Gets the original HTML document.
    /// </summary>
    /// <value>The original HTML document.</value>
    public virtual HtmlDocument Document
    {
      get {
        return _document;
      }
    }

    /// <summary>
    /// Gets information about the document's source file.
    /// </summary>
    /// <value>The source file.</value>
    public virtual ISourceInfo SourceFile
    {
      get {
        return _sourceFile;
      }
    }

    /// <summary>
    /// Gets the <see cref="RenderingMode"/> for which the current document type caters.
    /// </summary>
    /// <value>The rendering mode.</value>
    public override RenderingMode Mode
    {
      get {
        return RenderingMode.Html;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Renders the document to an <c>HtmlAgilityPack.HtmlDocument</c> instance.
    /// </summary>
    /// <returns>The rendered HTML document.</returns>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="IModelValueContainer"/>, to configure it.</param>
    public HtmlDocument RenderHtml(IRenderingSettings options = null,
                                   Action<IModelValueContainer> contextConfigurator = null)
    {
      return RenderHtml(null, options, contextConfigurator);
    }

    /// <summary>
    /// Renders the document to an <c>HtmlAgilityPack.HtmlDocument</c> instance.
    /// </summary>
    /// <returns>The rendered HTML document.</returns>
    /// <param name="model">An object for which the ZPT document is to be applied.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    /// <param name="contextConfigurator">An optional action to perform upon the root <see cref="IModelValueContainer"/>, to configure it.</param>
    public HtmlDocument RenderHtml(object model,
                                   IRenderingSettings options = null,
                                   Action<IModelValueContainer> contextConfigurator = null)
    {
      var element = RenderElement(model, options, contextConfigurator);

      var output = new HtmlDocument();
      output.LoadHtml(element.ToString());

      return output;
    }

    /// <summary>
    /// Gets a collection of elements in the document which are defined as METAL macros.
    /// </summary>
    /// <returns>Elements representing the METAL macros.</returns>
    public override IMetalMacroContainer GetMacros()
    {
      var output = this.Document
        .DocumentNode
        .DescendantsAndSelf()
        .Where(ele => ele.Attributes.Any(attr => attr.Name == String.Format("{0}:{1}",
                                                                            ZptConstants.Metal.Namespace.Prefix,
                                                                            ZptConstants.Metal.DefineMacroAttribute)))
        .Select(x => {
          var element = new ZptHtmlElement(x, this.SourceFile, this, isImported: true);
          var context = new RenderingContext(Model.Empty, Model.Empty, element, GetDefaultOptions());
          return new Metal.MetalMacro(context.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute).Value, element);
        })
        .ToArray();

      return new CSF.Zpt.Metal.MetalMacroContainer(output);
    }

    /// <summary>
    /// Gets information about the source medium for the current instance
    /// </summary>
    /// <returns>The source info.</returns>
    public override ISourceInfo GetSourceInfo()
    {
      return this.SourceFile;
    }

    /// <summary>
    /// Renders an element to the given <c>System.IO.TextWriter</c>.
    /// </summary>
    /// <param name="writer">The text writer to render to.</param>
    /// <param name="element">The element to render.</param>
    /// <param name="options">The rendering options to use.  If <c>null</c> then default options are used.</param>
    protected override void Render(TextWriter writer,
                                   IZptElement element,
                                   IRenderingSettings options)
    {
      if(writer == null)
      {
        throw new ArgumentNullException(nameof(writer));
      }
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      var htmlElement = ConvertElement<ZptHtmlElement>(element);
      htmlElement.Node.WriteTo(writer);
    }

    /// <summary>
    /// Creates a rendering model from the current instance.
    /// </summary>
    /// <returns>The rendering model.</returns>
    protected override IZptElement GetRootElement()
    {
      return new ZptHtmlElement(this.Document.DocumentNode, this.SourceFile, this, isRoot: true);
    }

    /// <summary>
    /// Gets an instance of <see cref="IRenderingSettings"/> which represents the default options.
    /// </summary>
    /// <returns>The default options.</returns>
    protected override IRenderingSettings GetDefaultOptions()
    {
      return new DefaultRenderingSettings();
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.DocumentProviders.ZptHtmlDocument"/> class.
    /// </summary>
    /// <param name="document">An HTML document from which to create the current instance.</param>
    /// <param name="sourceFile">Information about the document's source file.</param>
    /// <param name="elementRenderer">The element renderer.</param>
    public ZptHtmlDocument(HtmlDocument document,
                           ISourceInfo sourceFile,
                           IElementRenderer elementRenderer = null) : base(elementRenderer)
    {
      if(document == null)
      {
        throw new ArgumentNullException(nameof(document));
      }
      if(sourceFile == null)
      {
        throw new ArgumentNullException(nameof(sourceFile));
      }

      _document = document;
      _sourceFile = sourceFile;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Zpt.DocumentProviders.ZptHtmlDocument"/> class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Beware that this method constructs a new instance of <see cref="HtmlAgilityPackConfigurator"/> and executes its
    /// <see cref="HtmlAgilityPackConfigurator.Configure"/> method, thus influencing the standard operation of the HTML
    /// agility pack.
    /// </para>
    /// </remarks>
    static ZptHtmlDocument()
    {
      new HtmlAgilityPackConfigurator().Configure();
    }

    #endregion
  }
}

