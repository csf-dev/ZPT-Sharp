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
  public class MetalMacroCollection : ITalesPathHandler
  {
    #region fields

    private Dictionary<string,MetalMacro> _macros;

    #endregion

    #region properties

    /// <summary>
    /// Gets the <see cref="CSF.Zpt.Rendering.ZptElement"/> with the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    public ZptElement this[string key]
    {
      get {
        return _macros.ContainsKey(key)? _macros[key].Element : null;
      }
    }

    #endregion

    #region methods

    bool ITalesPathHandler.HandleTalesPath(string pathFragment, out object result, IRenderingContext currentContext)
    {
      result = this[pathFragment];
      return result != null;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Metal.MetalMacroCollection"/> class.
    /// </summary>
    /// <param name="macros">Macros.</param>
    public MetalMacroCollection(IEnumerable<MetalMacro> macros)
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

