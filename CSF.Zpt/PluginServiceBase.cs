using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CSF.Zpt
{
  /// <summary>
  /// Abstract base type for a service which exposes plugin instances.
  /// </summary>
  public abstract class PluginServiceBase
  {
    #region fields

    private static object _syncRoot;
    private static IEnumerable<Assembly> _cachedAssemblies;

    private readonly IPluginConfiguration _pluginConfig;
    private readonly IPluginAssemblyLoader _assemblyLoader;

    #endregion

    #region properties

    /// <summary>
    /// Gets the plugin configuration.
    /// </summary>
    /// <value>The plugin config.</value>
    protected IPluginConfiguration PluginConfig
    {
      get {
        return _pluginConfig;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets a collection of all of the available plugin assemblies.
    /// </summary>
    /// <returns>The plugin assemblies.</returns>
    protected internal IEnumerable<Assembly> GetAllPluginAssemblies()
    {
      lock(_syncRoot)
      {
        if(_cachedAssemblies == null)
        {
          _cachedAssemblies = LoadAllPluginAssemblies();
        }
      }

      return _cachedAssemblies;
    }

    /// <summary>
    /// Gets a collection of the types which implement a base type, from a given assembly.
    /// </summary>
    /// <returns>The collection of implementation types.</returns>
    /// <param name="sourceAssembly">The assembly to search.</param>
    /// <typeparam name="TBase">The base type to use for the implementation search.</typeparam>
    protected internal IEnumerable<Type> GetConcreteTypes<TBase>(Assembly sourceAssembly) where TBase : class
    {
      return (from type in sourceAssembly.GetExportedTypes()
              where
                type.IsClass
                && !type.IsAbstract
                && typeof(TBase).IsAssignableFrom(type)
                && type.GetConstructor(Type.EmptyTypes) != null
              select type);
    }

    private IEnumerable<Assembly> LoadAllPluginAssemblies()
    {
      var allPaths = _pluginConfig.GetAllPluginAssemblyPaths();
      return _assemblyLoader.Load(allPaths);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.PluginServiceBase"/> class.
    /// </summary>
    /// <param name="pluginConfig">Plugin config.</param>
    /// <param name="assemblyLoader">Assembly loader.</param>
    public PluginServiceBase(IPluginConfiguration pluginConfig = null,
                             IPluginAssemblyLoader assemblyLoader = null)
    {
      _pluginConfig = pluginConfig?? PluginConfigurationSection.GetDefault();
      _assemblyLoader = assemblyLoader?? new PluginAssemblyLoader();
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.PluginServiceBase"/> class.
    /// </summary>
    static PluginServiceBase()
    {
      _syncRoot = new object();
    }

    #endregion
  }
}

