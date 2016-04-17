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

      TemplateKeywordOptions
        metalKeywordOptions = new TemplateKeywordOptions(options.InitialModelState.MetalKeywordOptions),
        talKeywordOptions = new TemplateKeywordOptions(options.InitialModelState.TalKeywordOptions);

      Model
        metalModel = new TalesModel(this.EvaluatorRegistry, metalKeywordOptions),
        talModel = new TalesModel(this.EvaluatorRegistry, talKeywordOptions);

      options.InitialModelState.PopulateMetalModel(metalModel);
      options.InitialModelState.PopulateTalModel(talModel);

      return new RenderingContext(metalModel, talModel, element, options);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TalesRenderingContextFactory"/> class.
    /// </summary>
    /// <param name="evaluatorRegistry">Evaluator registry.</param>
    public TalesRenderingContextFactory(IEvaluatorRegistry evaluatorRegistry = null)
    {
      this.EvaluatorRegistry = evaluatorRegistry?? SimpleEvaluatorRegistry.Default;
    }

    #endregion
  }
}

