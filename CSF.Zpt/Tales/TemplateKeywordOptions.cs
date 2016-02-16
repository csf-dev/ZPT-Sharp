using System;
using System.Collections.Generic;

namespace CSF.Zpt.Tales
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
    /// <returns>The result of the path traversal.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    public object HandleTalesPath(string pathFragment)
    {
      return this[pathFragment];
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.TemplateKeywordOptions"/> class.
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

