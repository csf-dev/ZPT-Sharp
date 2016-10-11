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
    #region methods

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

    #endregion
  }
}

