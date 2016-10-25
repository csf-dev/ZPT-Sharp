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

      return Path.IsPathRooted(path)? LoadAbsolute(path) : LoadRelative(path);
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

    /// <summary>
    /// Loads an assembly from a relative (non-rooted) path.
    /// </summary>
    /// <returns>The assembly.</returns>
    /// <param name="path">Path.</param>
    public virtual Assembly LoadRelative(string path)
    {
      var currentAssemblyDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).GetParent();
      var absolutePath = Path.Combine(currentAssemblyDirectory.FullName, path);

      return LoadAbsolute(absolutePath);
    }
  }
}

