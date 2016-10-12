using System;
using System.Text;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Encapsulates the available options for rendering a <see cref="ZptDocument"/>.
  /// </summary>
  public class RenderingSettings : IRenderingSettings
  {
    #region fields

    private static readonly IRenderingSettings DEFAULT;
    private IContextVisitor[] _contextVisitors;

    #endregion

    #region properties

    /// <summary>
    /// Gets the factory implementation with which to create <see cref="RenderingContext"/> instances.
    /// </summary>
    /// <value>The rendering context factory.</value>
    public IRenderingContextFactory ContextFactory
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether source file annotations should be added to the rendered output.
    /// </summary>
    /// <value><c>true</c> if source file annotations are to be added; otherwise, <c>false</c>.</value>
    public bool AddSourceFileAnnotation
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the context visitors to be used when processing ZPT documents.
    /// </summary>
    /// <value>The context visitors.</value>
    public IContextVisitor[] ContextVisitors
    {
      get {
        IContextVisitor[] output = new IContextVisitor[_contextVisitors.Length];
        Array.Copy(_contextVisitors, output, _contextVisitors.Length);
        return output;
      }
    }

    /// <summary>
    /// Gets the encoding (eg: Unicode) for the rendered output.
    /// </summary>
    /// <value>The output encoding.</value>
    public Encoding OutputEncoding
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a value indicating whether the XML declaration should be omitted (where applicable).
    /// </summary>
    /// <value><c>true</c> if the XML declaration is to be omitted; otherwise, <c>false</c>.</value>
    public bool OmitXmlDeclaration
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets or sets the implementation of <see cref="ITemplateFileFactory"/> to use.
    /// </summary>
    /// <value>The template file factory.</value>
    public ITemplateFileFactory TemplateFileFactory
    {
      get;
      private set;
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

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.IRenderingSettings"/> class.
    /// </summary>
    /// <param name="addSourceFileAnnotation">Indicates whether or not source file annotation is to be added.</param>
    /// <param name="elementVisitors">The element visitors to use.</param>
    /// <param name="contextFactory">The rendering context factory.</param>
    /// <param name="outputEncoding">The desired output encoding.</param>
    /// <param name="omitXmlDeclaration">Whether or not to omit the XML declaration.</param>
    /// <param name="documentFactory">An optional non-default implementation of <see cref="ITemplateFileFactory"/> to use.</param>
    public RenderingSettings(IContextVisitor[] elementVisitors,
                             IRenderingContextFactory contextFactory,
                             bool addSourceFileAnnotation,
                             System.Text.Encoding outputEncoding,
                             bool omitXmlDeclaration,
                             ITemplateFileFactory documentFactory)
    {
      if(elementVisitors == null)
      {
        throw new ArgumentNullException(nameof(elementVisitors));
      }
      if(contextFactory == null)
      {
        throw new ArgumentNullException(nameof(contextFactory));
      }
      if(outputEncoding == null)
      {
        throw new ArgumentNullException(nameof(outputEncoding));
      }
      if(documentFactory == null)
      {
        throw new ArgumentNullException(nameof(documentFactory));
      }

      _contextVisitors = elementVisitors;
      this.ContextFactory = contextFactory;
      this.AddSourceFileAnnotation = addSourceFileAnnotation;
      this.OutputEncoding = outputEncoding;
      this.OmitXmlDeclaration = omitXmlDeclaration;
      this.TemplateFileFactory = documentFactory;
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.Rendering.RenderingSettings"/> class.
    /// </summary>
    static RenderingSettings()
    {
      DEFAULT = RenderingSettingsFactory.GetDefaultSettings();
    }

    #endregion

    #region default instance

    /// <summary>
    /// Exposes an instance of <see cref="IRenderingSettings"/> which carries the default settings.
    /// </summary>
    /// <value>The default settings.</value>
    public static IRenderingSettings Default
    {
      get {
        return DEFAULT;
      }
    }

    #endregion
  }
}

