﻿using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Information about the source of an <see cref="IZptDocument"/> or <see cref="ZptElement"/>.
  /// </summary>
  public interface ISourceInfo : IEquatable<ISourceInfo>
  {
    #region properties

    /// <summary>
    /// Gets the full name of the source.
    /// </summary>
    /// <value>The full name.</value>
    string FullName { get; }

    #endregion

    #region methods

    /// <summary>
    /// Serves as a hash function for a <see cref="CSF.Zpt.Rendering.ISourceInfo"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures
    /// such as a hash table.</returns>
    int GetHashCode();

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ISourceInfo"/>.
    /// </summary>
    /// <param name="other">The <see cref="System.Object"/> to compare with the current
    /// <see cref="CSF.Zpt.Rendering.ISourceInfo"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ISourceInfo"/>; otherwise, <c>false</c>.</returns>
    bool Equals(object other);

    #endregion
  }
}
