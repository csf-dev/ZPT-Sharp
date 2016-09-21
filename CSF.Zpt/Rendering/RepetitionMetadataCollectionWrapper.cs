using System;
using CSF.Zpt.Tales;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Wrapper type for a collection of named <see cref="IRepetitionInfo"/> instances.
  /// </summary>
  public class RepetitionMetadataCollectionWrapper : ITalesPathHandler
  {
    #region fields

    private Dictionary<string,ITalesPathHandler> _repetitions;

    #endregion

    #region properties

    /// <summary>
    /// Gets the <see cref="CSF.Zpt.Tales.ITalesPathHandler"/> (representing a repetition) with the specified name.
    /// </summary>
    /// <param name="key">The repetition name.</param>
    public ITalesPathHandler this [string key]
    {
      get {
        return _repetitions.ContainsKey(key)? _repetitions[key] : null;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets a <see cref="IRepetitionInfo"/> based upon a TALES path fragment.
    /// </summary>
    /// <returns><c>true</c> if the path traversal was a success; <c>false</c> otherwise.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    /// <param name="result">Exposes the result if the traversal was a success</param>
    /// <param name="currentContext">Gets the current rendering context.</param>
    public bool HandleTalesPath(string pathFragment, out object result, RenderingContext currentContext)
    {
      bool output = _repetitions.ContainsKey(pathFragment);
      result = output? _repetitions[pathFragment] : null;
      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.RepetitionMetadataCollectionWrapper"/> class.
    /// </summary>
    /// <param name="repetitions">Repetitions.</param>
    public RepetitionMetadataCollectionWrapper(IEnumerable<IRepetitionInfo> repetitions)
    {
      if(repetitions == null)
      {
        throw new ArgumentNullException(nameof(repetitions));
      }

      _repetitions = repetitions.ToDictionary(k => k.Name, v => (ITalesPathHandler) v);
    }

    #endregion
  }
}

