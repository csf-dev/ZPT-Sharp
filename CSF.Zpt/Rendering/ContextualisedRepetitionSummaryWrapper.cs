using System;
using CSF.Zpt.Tales;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Wrapper type for a contextualised (in the context of a chain of <see cref="ZptElement"/>) set of available
  /// <see cref="RepetitionSummary"/>.
  /// </summary>
  public class ContextualisedRepetitionSummaryWrapper : ITalesPathHandler
  {
    #region fields

    private Dictionary<string,RepetitionSummary> _repetitions;

    #endregion

    #region properties

    /// <summary>
    /// Gets the <see cref="CSF.Zpt.Rendering.ContextualisedRepetitionSummaryWrapper"/> with the specified key.
    /// </summary>
    /// <param name="key">Key.</param>
    public RepetitionSummary this [string key]
    {
      get {
        return _repetitions.ContainsKey(key)? _repetitions[key] : null;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets a <see cref="RepetitionSummary"/> based upon a TALES path fragment.
    /// </summary>
    /// <returns><c>true</c> if the path traversal was a success; <c>false</c> otherwise.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    /// <param name="result">Exposes the result if the traversal was a success</param>
    public bool HandleTalesPath(string pathFragment, out object result)
    {
      bool output = _repetitions.ContainsKey(pathFragment);
      result = output? _repetitions[pathFragment] : null;
      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.ContextualisedRepetitionSummaryWrapper"/> class.
    /// </summary>
    /// <param name="repetitions">Repetitions.</param>
    public ContextualisedRepetitionSummaryWrapper(IEnumerable<RepetitionInfo> repetitions)
    {
      if(repetitions == null)
      {
        throw new ArgumentNullException("repetitions");
      }

      _repetitions = repetitions.ToDictionary(k => k.Name, v => v.ToSummary());
    }

    #endregion
  }
}

