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

    private IDictionary<string,IZptAttribute> _attributes;

    #endregion

    #region methods

    /// <summary>
    /// Gets a specific attribute by name (a TALES path fragment).
    /// </summary>
    /// <returns><c>true</c> if the path traversal was a success; <c>false</c> otherwise.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    /// <param name="result">Exposes the result if the traversal was a success</param>
    /// <param name="currentContext">Gets the current rendering context.</param>
    public bool HandleTalesPath(string pathFragment, out object result, RenderingContext currentContext)
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

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.OriginalAttributeValuesCollection"/> class.
    /// </summary>
    public OriginalAttributeValuesCollection()
    {
      _attributes = new Dictionary<string,IZptAttribute>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.OriginalAttributeValuesCollection"/> class.
    /// </summary>
    /// <param name="attributes">A collection of sttributes.</param>
    public OriginalAttributeValuesCollection(IEnumerable<IZptAttribute> attributes)
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

