using System;
using CSF.Zpt.Tales;
using System.Text;

namespace CSF.Zpt.Rendering
{
  public class DefaultRenderingOptions : IRenderingOptions
  {
    #region constants

    private static readonly IContextVisitor[] DEFAULT_VISITORS = new IContextVisitor[] {
      new CSF.Zpt.Metal.MetalVisitor(),
      new CSF.Zpt.Tal.TalVisitor(),
      new CSF.Zpt.Metal.MetalTidyUpVisitor(),
      new CSF.Zpt.Tal.TalTidyUpVisitor(),
    };

    /// <summary>
    /// Gets the default context visitors.
    /// </summary>
    /// <value>The default context visitors.</value>
    protected static IContextVisitor[] DefaultVisitors
    {
      get {
        var output = new IContextVisitor[DEFAULT_VISITORS.Length];
        Array.Copy(DEFAULT_VISITORS, output, DEFAULT_VISITORS.Length);
        return output;
      }
    }

    protected static readonly IRenderingContextFactory DefaultContextFactory = new TalesRenderingContextFactory();

    protected static readonly Encoding DefaultEncoding = Encoding.UTF8;

    protected internal const bool
      DefaultAddAnnotation = false,
      DefaultOmitXmlDeclaration = false,
      DefaultOutputIndentedXml = true;

    protected internal const string
      DefaultIndentCharacters = "  ";

    #endregion

    #region properties

    /// <summary>
    /// Gets the factory implementation with which to create <see cref="RenderingContext"/> instances.
    /// </summary>
    /// <value>The rendering context factory.</value>
    public IRenderingContextFactory ContextFactory
    {
      get;
      protected set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether source file annotations should be added to the rendered output.
    /// </summary>
    /// <value><c>true</c> if source file annotations are to be added; otherwise, <c>false</c>.</value>
    public bool AddSourceFileAnnotation
    {
      get;
      protected set;
    }

    /// <summary>
    /// Gets the context visitors to be used when processing ZPT documents.
    /// </summary>
    /// <value>The context visitors.</value>
    public IContextVisitor[] ContextVisitors
    {
      get;
      protected set;
    }

    /// <summary>
    /// Gets the encoding (eg: Unicode) for the rendered output.
    /// </summary>
    /// <value>The output encoding.</value>
    public Encoding OutputEncoding
    {
      get;
      protected set;
    }

    /// <summary>
    /// Gets a value indicating whether the XML declaration should be omitted (where applicable).
    /// </summary>
    /// <value><c>true</c> if the XML declaration is to be omitted; otherwise, <c>false</c>.</value>
    public bool OmitXmlDeclaration
    {
      get;
      protected set;
    }

    /// <summary>
    /// Gets a string used to indicate a single level of indentation to use when rendering an XML document.
    /// </summary>
    /// <value>The XML indentation characters.</value>
    public string XmlIndentationCharacters
    {
      get;
      protected set;
    }

    /// <summary>
    /// Gets a value indicating whether XML documents should be rendered with indentated formatting or not.
    /// </summary>
    /// <value><c>true</c> if the rendering process is to output indented XML; otherwise, <c>false</c>.</value>
    public bool OutputIndentedXml
    {
      get;
      protected set;
    }

    protected ITemplateFileFactory TemplateFileFactory
    {
      get;
      set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Creates a new root <see cref="RenderingContext"/> instance.
    /// </summary>
    /// <returns>The root rendering context.</returns>
    public virtual RenderingContext CreateRootContext(ZptElement element)
    {
      return this.CreateRootContext(element, null);
    }

    /// <summary>
    /// Creates a new root <see cref="RenderingContext"/> instance.
    /// </summary>
    /// <param name="element">The root ZPT element</param>
    /// <param name="model">The model to render</param>
    /// <returns>The root rendering context.</returns>
    public virtual RenderingContext CreateRootContext(ZptElement element, object model)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      return this.ContextFactory.Create(element, this, model);
    }

    /// <summary>
    /// Gets an instance of <see cref="ITemplateFileFactory"/> from the current instance.
    /// </summary>
    /// <returns>The template file factory</returns>
    public virtual ITemplateFileFactory GetTemplateFileFactory()
    {
      return this.TemplateFileFactory;
    }

    #endregion

    #region constructor

    protected DefaultRenderingOptions(ITemplateFileFactory documentFactory = null,
                                      IContextVisitor[] elementVisitors = null,
                                      IRenderingContextFactory contextFactory = null,
                                      bool addSourceFileAnnotation = DefaultAddAnnotation,
                                      Encoding outputEncoding = null,
                                      bool omitXmlDeclaration = DefaultOmitXmlDeclaration,
                                      string xmlIndentCharacters = null,
                                      bool outputIndentedXml = DefaultOutputIndentedXml)
    {
      this.TemplateFileFactory = documentFactory?? ZptDocumentFactory.DefaultTemplateFactory;
      this.AddSourceFileAnnotation = addSourceFileAnnotation;
      this.OmitXmlDeclaration = omitXmlDeclaration;
      this.OutputIndentedXml = outputIndentedXml;
      this.ContextVisitors = elementVisitors?? DefaultVisitors;
      this.ContextFactory = contextFactory?? DefaultContextFactory;
      this.OutputEncoding = outputEncoding?? DefaultEncoding;
      this.XmlIndentationCharacters = xmlIndentCharacters?? DefaultIndentCharacters;
    }

    public DefaultRenderingOptions() : this(null,
                                            null,
                                            null,
                                            DefaultAddAnnotation,
                                            null,
                                            DefaultOmitXmlDeclaration,
                                            null,
                                            DefaultOutputIndentedXml) {}

    #endregion
  }
}

