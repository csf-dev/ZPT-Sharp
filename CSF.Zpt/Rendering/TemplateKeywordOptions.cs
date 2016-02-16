using System;
using System.Collections.Generic;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents a collection of keyword-options passed to a template.
  /// </summary>
  public class TemplateKeywordOptions : ITalesPathHandler
  {
    #region fields

    private IDictionary<string,object> _options;
    private object _syncRoot;

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets the option with the specified key.
    /// </summary>
    /// <param name="key">Key.</param>
    public object this[string key]
    {
      get {
        object output;

        lock(_syncRoot)
        {
          output = _options.ContainsKey(key)? _options[key] : null;
        }

        return output;
      }
      set {
        lock(_syncRoot)
        {
          _options[key] = value;
        }
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets the keyword-option present at the given key, identified as a TALES path fragment.
    /// </summary>
    /// <returns><c>true</c> if the path traversal was a success; <c>false</c> otherwise.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    /// <param name="result">Exposes the result if the traversal was a success</param>
    public bool HandleTalesPath(string pathFragment, out object result)
    {
      bool output = _options.ContainsKey(pathFragment);
      result = output? _options[pathFragment] : null;
      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.TemplateKeywordOptions"/> class.
    /// </summary>
    /// <param name="options">Options.</param>
    public TemplateKeywordOptions(IDictionary<string,object> options = null)
    {
      _syncRoot = new object();
      _options = options?? new Dictionary<string, object>();
    }

    #endregion
  }
}

