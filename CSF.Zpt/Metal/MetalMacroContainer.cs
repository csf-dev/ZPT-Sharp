using System;
using CSF.Zpt.Tales;
using System.Collections.Generic;
using CSF.Zpt.Rendering;
using System.Linq;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Wrapper type for a collection of <see cref="MetalMacro"/>.
  /// </summary>
  public class MetalMacroContainer : IMetalMacroContainer
  {
    #region fields

    private Dictionary<string,IMetalMacro> _macros;

    #endregion

    #region methods

    /// <summary>
    /// Gets a single macro element by name.
    /// </summary>
    /// <returns>The macro element, or a <c>null</c> reference if no such macro exists.</returns>
    /// <param name="name">The macro name.</param>
    public IZptElement GetElement(string name)
    {
      return _macros.ContainsKey(name)? _macros[name].Element : null;
    }

    /// <summary>
    /// Hases the macro.
    /// </summary>
    /// <returns><c>true</c>, if macro was hased, <c>false</c> otherwise.</returns>
    /// <param name="name">Name.</param>
    public bool HasMacro (string name)
    {
      return _macros.ContainsKey(name);
    }

    /// <summary>
    /// Gets a collection of all of the macros in the container, by their names.
    /// </summary>
    /// <returns>The macros.</returns>
    public IEnumerable<IMetalMacro> GetAllMacros ()
    {
      return _macros.Values.ToArray();
    }

    bool ITalesPathHandler.HandleTalesPath(string pathFragment, out object result, IRenderingContext currentContext)
    {
      result = GetElement(pathFragment);
      return result != null;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Metal.MetalMacroContainer"/> class.
    /// </summary>
    /// <param name="macros">Macros.</param>
    public MetalMacroContainer(IEnumerable<IMetalMacro> macros)
    {
      if(macros == null)
      {
        throw new ArgumentNullException(nameof(macros));
      }

      _macros = macros.ToDictionary(k => k.Name, v => v);
    }

    #endregion
  }
}

