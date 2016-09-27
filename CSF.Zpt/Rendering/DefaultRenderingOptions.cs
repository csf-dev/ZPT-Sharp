using System;
using CSF.Zpt.Tales;
using System.Text;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents the default <see cref="IRenderingOptions"/>.
  /// </summary>
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

    /// <summary>
    /// Gets the refault <see cref="IRenderingContextFactory"/> implementation.
    /// </summary>
    protected static readonly IRenderingContextFactory DefaultContextFactory = new TalesRenderingContextFactory();

    /// <summary>
    /// Gets the default character encoding.
    /// </summary>
    protected static readonly Encoding DefaultEncoding = Encoding.UTF8;

    /// <summary>
    /// Gets the default value for <see cref="AddSourceFileAnnotation"/>.
    /// </summary>
    protected internal const bool DefaultAddAnnotation = false;

    /// <summary>
    /// Gets the default value for <see cref="OmitXmlDeclaration"/>.
    /// </summary>
    protected internal const bool DefaultOmitXmlDeclaration = false;

    #endregion

    #region fields

    private ITemplateFileFactory _templateFactory;

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
    /// Gets or sets the implementation of <see cref="ITemplateFileFactory"/> to use.
    /// </summary>
    /// <value>The template file factory.</value>
    protected ITemplateFileFactory TemplateFileFactory
    {
      get {
        return _templateFactory;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException(nameof(value));
        }

        _templateFactory = value;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Creates a new root <see cref="IRenderingContext"/> instance.
    /// </summary>
    /// <returns>The root rendering context.</returns>
    public virtual IRenderingContext CreateRootContext(IZptElement element)
    {
      return this.CreateRootContext(element, null);
    }

    /// <summary>
    /// Creates a new root <see cref="IRenderingContext"/> instance.
    /// </summary>
    /// <param name="element">The root ZPT element</param>
    /// <param name="model">The model to render</param>
    /// <returns>The root rendering context.</returns>
    public virtual IRenderingContext CreateRootContext(IZptElement element, object model)
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

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.DefaultRenderingOptions"/> class.
    /// </summary>
    /// <param name="documentFactory">Document factory.</param>
    /// <param name="elementVisitors">Element visitors.</param>
    /// <param name="contextFactory">Context factory.</param>
    /// <param name="addSourceFileAnnotation">If set to <c>true</c> add source file annotation.</param>
    /// <param name="outputEncoding">Output encoding.</param>
    /// <param name="omitXmlDeclaration">If set to <c>true</c> omit XML declaration.</param>
    protected DefaultRenderingOptions(ITemplateFileFactory documentFactory = null,
                                      IContextVisitor[] elementVisitors = null,
                                      IRenderingContextFactory contextFactory = null,
                                      bool addSourceFileAnnotation = DefaultAddAnnotation,
                                      Encoding outputEncoding = null,
                                      bool omitXmlDeclaration = DefaultOmitXmlDeclaration)
    {
      this.TemplateFileFactory = documentFactory?? new ZptDocumentFactory();
      this.AddSourceFileAnnotation = addSourceFileAnnotation;
      this.OmitXmlDeclaration = omitXmlDeclaration;
      this.ContextVisitors = elementVisitors?? DefaultVisitors;
      this.ContextFactory = contextFactory?? DefaultContextFactory;
      this.OutputEncoding = outputEncoding?? DefaultEncoding;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.DefaultRenderingOptions"/> class.
    /// </summary>
    public DefaultRenderingOptions() : this(null,
                                            null,
                                            null,
                                            DefaultAddAnnotation,
                                            null,
                                            DefaultOmitXmlDeclaration) {}

    #endregion
  }
}

