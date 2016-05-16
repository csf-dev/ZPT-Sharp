using System;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents a collection of <see cref="ZptAttribute"/>, exposing the original attributes present upon an element.
  /// </summary>
  public class OriginalAttributeValuesCollection : ITalesPathHandler
  {
    #region properties

    private IDictionary<string,ZptAttribute> _attributes;

    #endregion

    #region methods

    /// <summary>
    /// Gets a specific attribute by name (a TALES path fragment).
    /// </summary>
    /// <returns><c>true</c> if the path traversal was a success; <c>false</c> otherwise.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    /// <param name="result">Exposes the result if the traversal was a success</param>
    public bool HandleTalesPath(string pathFragment, out object result)
    {
      bool output;

      if(_attributes.ContainsKey(pathFragment))
      {
        result = _attributes[pathFragment].Value;
        output = true;
      }
      else
      {
        result = null;
        output = false;
      }

      return output;
    }

    #endregion

    #region constructor

    public OriginalAttributeValuesCollection()
    {
      _attributes = new Dictionary<string,ZptAttribute>();
    }

    public OriginalAttributeValuesCollection(IEnumerable<ZptAttribute> attributes)
    {
      if(attributes == null)
      {
        throw new ArgumentNullException(nameof(attributes));
      }

      _attributes = attributes.ToDictionary(k => k.Name, v => v);
    }

    #endregion
  }
}

