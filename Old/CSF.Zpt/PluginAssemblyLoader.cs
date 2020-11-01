using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using CSF.IO;

namespace CSF.Zpt
{
  /// <summary>
  /// Default implementation of <see cref="IPluginAssemblyLoader"/>.
  /// </summary>
  public class PluginAssemblyLoader : IPluginAssemblyLoader
  {
    #region fields

    private readonly IAddOnAssemblyFinder _assemblyFinder;

    #endregion

    #region methods

    /// <summary>
    /// Load the plugin assemblies at the given paths.
    /// </summary>
    /// <param name="paths">Paths.</param>
    public IEnumerable<Assembly> Load(IEnumerable<string> paths)
    {
      if(paths == null)
      {
        throw new ArgumentNullException(nameof(paths));
      }

      return paths.Select(x => Load(x));
    }

    /// <summary>
    /// Load the plugin assembly at the specified path.
    /// </summary>
    /// <param name="path">Path.</param>
    public Assembly Load(string path)
    {
      if(path == null)
      {
        throw new ArgumentNullException(nameof(path));
      }

      var fullPath = _assemblyFinder.GetAssemblyPath(path);
      return LoadAbsolute(fullPath);
    }

    /// <summary>
    /// Loads an assembly from an absolute (rooted) path.
    /// </summary>
    /// <returns>The assembly.</returns>
    /// <param name="path">Path.</param>
    public virtual Assembly LoadAbsolute(string path)
    {
      try
      {
        return Assembly.LoadFrom(path);
      }
      catch(Exception ex)
      {
        // TODO: Wrap this in a better exception and move message to resource file
        throw new InvalidOperationException("The plugin assembly could not be loaded.", ex);
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Framework constructor for internal purposes only - do not use.
    /// </summary>
    protected PluginAssemblyLoader() : this(null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.PluginAssemblyLoader"/> class.
    /// </summary>
    /// <param name="assemblyFinder">Assembly finder.</param>
    public PluginAssemblyLoader(IAddOnAssemblyFinder assemblyFinder = null)
    {
      _assemblyFinder = assemblyFinder?? new DefaultAddOnAssemblyFinder();
    }

    #endregion
  }
}

