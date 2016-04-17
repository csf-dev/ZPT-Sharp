using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Implementation of <see cref="IRenderingContextFactory"/> which creates <see cref="TalesModel"/> instances.
  /// </summary>
  public class TalesRenderingContextFactory : IRenderingContextFactory
  {
    #region properties

    /// <summary>
    /// Gets the TALES evaluator registry implementation to use.
    /// </summary>
    /// <value>The evaluator registry.</value>
    public IEvaluatorRegistry EvaluatorRegistry
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the template keyword options, such as might be passed in via the command-line.
    /// </summary>
    /// <value>The keyword options.</value>
    public TemplateKeywordOptions KeywordOptions
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Create a context instance.
    /// </summary>
    public RenderingContext Create(ZptElement element, RenderingOptions options)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      Model
        talModel = new TalesModel(this.EvaluatorRegistry, this.KeywordOptions),
        metalModel = new TalesModel(this.EvaluatorRegistry, this.KeywordOptions);

      return new RenderingContext(metalModel, talModel, element, options);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TalesRenderingContextFactory"/> class.
    /// </summary>
    /// <param name="evaluatorRegistry">Evaluator registry.</param>
    /// <param name="keywordOptions">Keyword options.</param>
    public TalesRenderingContextFactory(IEvaluatorRegistry evaluatorRegistry = null,
                                        TemplateKeywordOptions keywordOptions = null)
    {
      this.EvaluatorRegistry = evaluatorRegistry?? SimpleEvaluatorRegistry.Default;
      this.KeywordOptions = keywordOptions?? new TemplateKeywordOptions();
    }

    #endregion
  }
}

