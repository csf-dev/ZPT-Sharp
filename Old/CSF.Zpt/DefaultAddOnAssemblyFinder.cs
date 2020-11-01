using System;
using System.Reflection;
using System.IO;
using CSF.IO;

namespace CSF.Zpt
{
  /// <summary>
  /// Default implementation of <see cref="IAddOnAssemblyFinder"/> which uses the path of the <c>CSF.Zpt.dll</c>
  /// assembly as the basis for search.
  /// </summary>
  public class DefaultAddOnAssemblyFinder : IAddOnAssemblyFinder
  {
    #region methods

    /// <summary>
    /// Gets the full path to a given assembly, based on its name.
    /// </summary>
    /// <returns>The full assembly path.</returns>
    /// <param name="assemblyName">The name of the assembly.</param>
    public virtual string GetAssemblyPath(string assemblyName)
    {
      if(assemblyName == null)
      {
        throw new ArgumentNullException(nameof(assemblyName));
      }

      if(assemblyName.Contains(Path.DirectorySeparatorChar.ToString()))
      {
        return assemblyName;
      }

      var assembly = Assembly.ReflectionOnlyLoad(assemblyName);

      return assembly.Location;
    }

    #endregion
  }
}

