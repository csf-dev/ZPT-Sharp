using System;
using CSF.Zpt.Rendering;
using System.Collections.Generic;

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
    /// Gets the local definitions to be pre-loaded into the TAL model.
    /// </summary>
    /// <value>The TAL local definitions.</value>
    public IDictionary<string,object> TalLocalDefinitions
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the global definitions to be pre-loaded into the TAL model.
    /// </summary>
    /// <value>The TAL global definitions.</value>
    public IDictionary<string,object> TalGlobalDefinitions
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the keyword options to be pre-loaded into the TAL model.
    /// </summary>
    /// <value>The TAL keyword options.</value>
    public IDictionary<string,object> TalKeywordOptions
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the local definitions to be pre-loaded into the METAL model.
    /// </summary>
    /// <value>The METAL local definitions.</value>
    public IDictionary<string,object> MetalLocalDefinitions
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the global definitions to be pre-loaded into the METAL model.
    /// </summary>
    /// <value>The METAL global definitions.</value>
    public IDictionary<string,object> MetalGlobalDefinitions
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the keyword options to be pre-loaded into the METAL model.
    /// </summary>
    /// <value>The METAL keyword options.</value>
    public IDictionary<string,object> MetalKeywordOptions
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Create a context instance.
    /// </summary>
    public virtual RenderingContext Create(ZptElement element, RenderingOptions options)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      NamedObjectWrapper
        metalKeywordOptions = new NamedObjectWrapper(MetalKeywordOptions),
        talKeywordOptions = new NamedObjectWrapper(TalKeywordOptions);

      IModel
        metalModel = new TalesModel(this.EvaluatorRegistry, metalKeywordOptions),
        talModel = new TalesModel(this.EvaluatorRegistry, talKeywordOptions);

      PopulateMetalModel(metalModel);
      PopulateTalModel(talModel);

      return new RenderingContext(metalModel, talModel, element, options);
    }

    /// <summary>
    /// Adds a keyword option to contexts created by the current instance.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    public virtual void AddKeywordOption(string key, string value)
    {
      TalKeywordOptions[key] = value;
      MetalKeywordOptions[key] = value;
    }

    /// <summary>
    /// Populates a METAL model from the state of the current instance.
    /// </summary>
    /// <param name="model">Model.</param>
    protected virtual void PopulateMetalModel(IModel model)
    {
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      foreach(string key in this.MetalLocalDefinitions.Keys)
      {
        model.AddLocal(key, this.MetalLocalDefinitions[key]);
      }
      foreach(string key in this.MetalGlobalDefinitions.Keys)
      {
        model.AddGlobal(key, this.MetalGlobalDefinitions[key]);
      }
    }

    /// <summary>
    /// Populates a TAL model from the state of the current instance.
    /// </summary>
    /// <param name="model">Model.</param>
    protected virtual void PopulateTalModel(IModel model)
    {
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      foreach(string key in this.TalLocalDefinitions.Keys)
      {
        model.AddLocal(key, this.TalLocalDefinitions[key]);
      }
      foreach(string key in this.TalGlobalDefinitions.Keys)
      {
        model.AddGlobal(key, this.TalGlobalDefinitions[key]);
      }
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

      this.TalLocalDefinitions = new Dictionary<string, object>();
      this.TalGlobalDefinitions = new Dictionary<string, object>();
      this.TalKeywordOptions = new Dictionary<string, object>();
      this.MetalLocalDefinitions = new Dictionary<string, object>();
      this.MetalGlobalDefinitions = new Dictionary<string, object>();
      this.MetalKeywordOptions = new Dictionary<string, object>();
    }

    #endregion
  }
}

