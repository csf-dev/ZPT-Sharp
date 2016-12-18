using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Factory type for <see cref="ISourceInfo"/>.
  /// </summary>
  public interface ISourceInfoFactory
  {
    /// <summary>
    /// Creates and returns a <see cref="ISourceInfo"/> instance based on the given type name and information.
    /// </summary>
    /// <returns>A source info instance.</returns>
    /// <param name="typeAQN">The Assembly-qualified name for the <see cref="ISourceInfo"/> type.</param>
    /// <param name="info">The string representation of the source info.</param>
    ISourceInfo CreateSourceInfo(string typeAQN, string info);
  }
}

