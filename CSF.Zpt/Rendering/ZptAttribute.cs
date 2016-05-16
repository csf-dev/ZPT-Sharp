using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents an <see cref="ZptElement"/> attribute.
  /// </summary>
  public abstract class ZptAttribute
  {
    #region properties

    /// <summary>
    /// Gets the attribute value
    /// </summary>
    /// <value>The value.</value>
    public abstract string Value { get; }

    /// <summary>
    /// Gets the attribute name, including its prefix if applicable.
    /// </summary>
    /// <value>The name.</value>
    public abstract string Name { get; }

    #endregion

    #region methods

    public abstract bool IsMatch(ZptNamespace nspace, string name);

    #endregion
  }
}

