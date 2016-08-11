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

    /// <summary>
    /// Determines whether this instance matches the given namespace and attribute name or not.
    /// </summary>
    /// <returns><c>true</c> if this instance matches the specified namespace and name; otherwise, <c>false</c>.</returns>
    /// <param name="nspace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public abstract bool IsMatch(ZptNamespace nspace, string name);

    #endregion
  }
}

