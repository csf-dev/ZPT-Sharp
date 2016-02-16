using System;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Encapsulates the available options for rendering a <see cref="ZptDocument"/>.
  /// </summary>
  public class RenderingOptions
  {
    #region fields

    private static RenderingOptions _defaultOptions;

    #endregion

    #region properties

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
    /// Gets the element visitors to be used when processing ZPT documents.
    /// </summary>
    /// <value>The element visitors.</value>
    public ElementVisitor[] ElementVisitors
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the factory implementation with which to create <see cref="RenderingContext"/> instances.
    /// </summary>
    /// <value>The rendering context factory.</value>
    public IRenderingContextFactory ContextFactory
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Creates a new root <see cref="RenderingContext"/> instance.
    /// </summary>
    /// <returns>The root rendering context.</returns>
    public RenderingContext CreateRootContext()
    {
      return this.ContextFactory.Create();
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.RenderingOptions"/> class.
    /// </summary>
    /// <param name="addSourceFileAnnotation">Indicates whether or not source file annotation is to be added.</param>
    /// <param name="elementVisitors">The element visitors to use.</param>
    /// <param name="contextFactory">The rendering context factory.</param>
    public RenderingOptions(bool addSourceFileAnnotation = false,
                            ElementVisitor[] elementVisitors = null,
                            IRenderingContextFactory contextFactory = null)
    {
      this.AddSourceFileAnnotation = addSourceFileAnnotation;
      this.ElementVisitors = elementVisitors?? new ElementVisitor[] {
        new CSF.Zpt.Metal.MetalVisitor(),
        new CSF.Zpt.Metal.SourceAnnotationVisitor(),
        new CSF.Zpt.Tal.TalVisitor(),
      };
      this.ContextFactory = contextFactory?? new TalesRenderingContextFactory();
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.Rendering.RenderingOptions"/> class.
    /// </summary>
    static RenderingOptions()
    {
      _defaultOptions = new RenderingOptions();
    }

    #endregion

    #region static properties

    /// <summary>
    /// Gets a set of <see cref="RenderingOptions"/> representing the defaults.
    /// </summary>
    /// <value>The default rendering options.</value>
    public static RenderingOptions Default
    {
      get {
        return _defaultOptions;
      }
    }

    #endregion
  }
}

