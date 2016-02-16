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
    /// Gets the TALES evaluator registry implementation.
    /// </summary>
    /// <value>The TALES evaluator registry.</value>
    public IEvaluatorRegistry TalesEvaluatorRegistry
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
    /// Gets the keyword options passed to the template mechanism, typically via the command-line.
    /// </summary>
    /// <value>The keyword options.</value>
    public TemplateKeywordOptions KeywordOptions
    {
      get;
      private set;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.RenderingOptions"/> class.
    /// </summary>
    /// <param name="addSourceFileAnnotation">Indicates whether or not source file annotation is to be added.</param>
    public RenderingOptions(bool addSourceFileAnnotation = false,
                            IEvaluatorRegistry talesEvaluatorRegistry = null,
                            ElementVisitor[] elementVisitors = null,
                            TemplateKeywordOptions keywordOptions = null)
    {
      this.AddSourceFileAnnotation = addSourceFileAnnotation;
      this.TalesEvaluatorRegistry = talesEvaluatorRegistry?? SimpleEvaluatorRegistry.Default;
      this.ElementVisitors = elementVisitors?? new ElementVisitor[] {
        new CSF.Zpt.Metal.MetalVisitor(),
        new CSF.Zpt.Metal.SourceAnnotationVisitor(),
        new CSF.Zpt.Tal.TalVisitor(),
      };
      this.KeywordOptions = keywordOptions?? new TemplateKeywordOptions();
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

