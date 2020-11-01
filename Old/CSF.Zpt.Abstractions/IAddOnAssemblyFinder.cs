using System;

namespace CSF.Zpt
{
  /// <summary>
  /// A service which gets the full paths for add-on assemblies, given a partial name for the assembly.
  /// </summary>
  public interface IAddOnAssemblyFinder
  {
    /// <summary>
    /// Gets the full path to a given assembly, based on its name.
    /// </summary>
    /// <returns>The full assembly path.</returns>
    /// <param name="assemblyName">The name of the assembly.</param>
    string GetAssemblyPath(string assemblyName);
  }
}

