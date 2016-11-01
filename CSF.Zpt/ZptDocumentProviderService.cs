using System;
using System.Linq;

namespace CSF.Zpt
{
  /// <summary>
  /// Default implementation of <see cref="IZptDocumentProviderService"/>.
  /// </summary>
  public sealed class ZptDocumentProviderService : PluginServiceBase, IZptDocumentProviderService
  {
    #region fields

    private static readonly ZptDocumentProviderCache _cache;

    #endregion

    #region methods

    /// <summary>
    /// Gets a provider based upon its <c>System.Type</c>.
    /// </summary>
    /// <returns>The provider.</returns>
    /// <param name="providerType">Provider type.</param>
    public IZptDocumentProvider GetProvider(Type providerType)
    {
      PopulateCacheIfRequired();

      IZptDocumentProvider output;

      var success = _cache.TryGet(providerType, out output);

      return success? output : null;
    }

    /// <summary>
    /// Gets a provider based upon its <c>System.Type</c> assembly-qualified name.
    /// </summary>
    /// <returns>The provider.</returns>
    /// <param name="providerTypeName">Provider type name.</param>
    public IZptDocumentProvider GetProvider(string providerTypeName)
    {
      var providerType = Type.GetType(providerTypeName);

      return (providerType != null)? GetProvider(providerType) : null;
    }

    /// <summary>
    /// Gets the default provider for HTML documents.
    /// </summary>
    /// <returns>The default HTML provider.</returns>
    public IZptDocumentProvider GetDefaultHtmlProvider()
    {
      PopulateCacheIfRequired();
      return _cache.DefaultHtmlProvider;
    }

    /// <summary>
    /// Gets the default provider for XML documents.
    /// </summary>
    /// <returns>The default XML provider.</returns>
    public IZptDocumentProvider GetDefaultXmlProvider()
    {
      PopulateCacheIfRequired();
      return _cache.DefaultXmlProvider;
    }

    /// <summary>
    /// Populates the cache if it has not been done already.
    /// </summary>
    private void PopulateCacheIfRequired()
    {
      _cache.PopulateIfRequired(PopulateCache);
    }

    /// <summary>
    /// Populates the cache.
    /// </summary>
    /// <param name="cache">Cache.</param>
    private void PopulateCache(ZptDocumentProviderCache cache)
    {
      if(cache == null)
      {
        throw new ArgumentNullException(nameof(cache));
      }

      var allProviders = (from assembly in GetAllPluginAssemblies()
                          from type in base.GetConcreteTypes<IZptDocumentProvider>(assembly)
                          select new {  Type = type,
                                        Provider = (IZptDocumentProvider) Activator.CreateInstance(type) })
        .ToArray();

      foreach(var kvp in allProviders)
      {
        cache.Add(kvp.Type, kvp.Provider);
      }

      var defaultHtml = allProviders
        .SingleOrDefault(x => x.Type.FullName == PluginConfig.GetDefaultHtmlDocumentProviderTypeName());
      if(defaultHtml != null)
      {
        cache.DefaultHtmlProvider = defaultHtml.Provider;
      }

      var defaultXml = allProviders
        .SingleOrDefault(x => x.Type.FullName == PluginConfig.GetDefaultXmlDocumentProviderTypeName());
      if(defaultXml != null)
      {
        cache.DefaultXmlProvider = defaultXml.Provider;
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptDocumentProviderService"/> class.
    /// </summary>
    /// <param name="pluginConfig">Plugin config.</param>
    /// <param name="assemblyLoader">Plugin assembly loader.</param>
    public ZptDocumentProviderService(IPluginConfiguration pluginConfig = null,
                                      IPluginAssemblyLoader assemblyLoader = null) : base(pluginConfig, assemblyLoader)
    { }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.ZptDocumentProviderService"/> class.
    /// </summary>
    static ZptDocumentProviderService()
    {
      _cache = new ZptDocumentProviderCache();
    }

    #endregion
  }
}

