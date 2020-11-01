using System;
using System.Collections.Generic;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Represents a collection of METAL macros.
  /// </summary>
  public interface IMetalMacroContainer : ITalesPathHandler
  {
    #region methods

    /// <summary>
    /// Gets a single macro element by name.
    /// </summary>
    /// <returns>The macro element, or a <c>null</c> reference if no such macro exists.</returns>
    /// <param name="name">The macro name.</param>
    IZptElement GetElement(string name);

    /// <summary>
    /// Gets a value indicating whether the current container contains a macro of the given name.
    /// </summary>
    /// <returns><c>true</c>, if a macro exists of the given name, <c>false</c> otherwise.</returns>
    /// <param name="name">The macro name.</param>
    bool HasMacro(string name);

    /// <summary>
    /// Gets a collection of all of the macros in the container.
    /// </summary>
    /// <returns>The macros.</returns>
    IEnumerable<IMetalMacro> GetAllMacros();

    #endregion
  }
}
