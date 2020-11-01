using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSF.Zpt
{
  /// <summary>
  /// Interface for a type which loads plugin assemblies.
  /// </summary>
  public interface IPluginAssemblyLoader
  {
    /// <summary>
    /// Load the plugin assemblies at the given paths.
    /// </summary>
    /// <param name="paths">Paths.</param>
    IEnumerable<Assembly> Load(IEnumerable<string> paths);

    /// <summary>
    /// Load the plugin assembly at the specified path.
    /// </summary>
    /// <param name="path">Path.</param>
    Assembly Load(string path);
  }
}

