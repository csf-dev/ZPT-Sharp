using System;
using System.Linq;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Default implementation of <see cref="IExpressionEvaluatorService"/>.
  /// </summary>
  public class ExpressionEvaluatorService : PluginServiceBase, IExpressionEvaluatorService
  {
    #region fields

    private static readonly Lazy<ExpressionEvaluatorCache> _evaluatorCache;
    private IPluginConfiguration _pluginConfig;

    #endregion

    #region methods

    /// <summary>
    /// Gets an evaluator by its <c>System.Type</c>.
    /// </summary>
    /// <returns>The evaluator.</returns>
    /// <param name="type">Type.</param>
    public IExpressionEvaluator GetEvaluator(Type type)
    {
      var cache = GetCache();

      return cache.TryGetByType(type);
    }

    /// <summary>
    /// Gets an evaluator by its expression prefix.
    /// </summary>
    /// <returns>The evaluator.</returns>
    /// <param name="prefix">Prefix.</param>
    public IExpressionEvaluator GetEvaluator(string prefix)
    {
      var cache = GetCache();
      IExpressionEvaluator output;

      var success = cache.TryGet(prefix, out output);

      return success? output : null;
    }

    /// <summary>
    /// Gets the default expression evaluator.
    /// </summary>
    /// <returns>The default evaluator.</returns>
    public IExpressionEvaluator GetDefaultEvaluator()
    {
      var cache = GetCache();
      return cache.Default;
    }

    /// <summary>
    /// Constructs the cache and populates it if it is applicable to do so.
    /// </summary>
    /// <returns>The cache.</returns>
    private ExpressionEvaluatorCache GetCache()
    {
      var output = _evaluatorCache.Value;
      output.PopulateIfRequired(PopulateCache);
      return output;
    }

    /// <summary>
    /// Populates the cache.
    /// </summary>
    /// <param name="cache">Cache.</param>
    private void PopulateCache(ExpressionEvaluatorCache cache)
    {
      if(cache == null)
      {
        throw new ArgumentNullException(nameof(cache));
      }

      var allEvaluators = (from assembly in _pluginConfig.GetAllPluginAssemblies()
                           from type in base.GetConcreteTypes<IExpressionEvaluator>(assembly)
                           let instance = (IExpressionEvaluator) Activator.CreateInstance(type)
                           select new {  Type = type,
                                         Prefix = instance.ExpressionPrefix,
                                         Evaluator = instance })
        .ToArray();

      foreach(var kvp in allEvaluators)
      {
        cache.Add(kvp.Prefix, kvp.Evaluator);
      }

      cache.Default = allEvaluators
        .Single(x => x.Type.FullName == _pluginConfig.GetDefaultExpressionEvaluatorTypeName())
        .Evaluator;
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.ExpressionEvaluatorService"/> class.
    /// </summary>
    /// <param name="pluginConfig">Plugin config.</param>
    public ExpressionEvaluatorService(IPluginConfiguration pluginConfig = null)
    {
      _pluginConfig = pluginConfig?? PluginConfigurationSection.GetDefault();
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.Tales.ExpressionEvaluatorService"/> class.
    /// </summary>
    static ExpressionEvaluatorService()
    {
      _evaluatorCache = new Lazy<ExpressionEvaluatorCache>();
    }

    #endregion
  }
}

