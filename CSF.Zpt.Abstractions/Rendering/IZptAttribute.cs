using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Interface for a type which wraps a DOM attribute.
  /// </summary>
  public interface IZptAttribute
  {
    #region properties

    /// <summary>
    /// Gets the attribute value
    /// </summary>
    /// <value>The value.</value>
    string Value { get; }

    /// <summary>
    /// Gets the attribute name, including its prefix if applicable.
    /// </summary>
    /// <value>The name.</value>
    string Name { get; }

    #endregion

    #region methods

    /// <summary>
    /// Determines whether this instance matches the given namespace and attribute name or not.
    /// </summary>
    /// <returns><c>true</c> if this instance matches the specified namespace and name; otherwise, <c>false</c>.</returns>
    /// <param name="nspace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    bool IsMatch(ZptNamespace nspace, string name);

    #endregion
  }
}

