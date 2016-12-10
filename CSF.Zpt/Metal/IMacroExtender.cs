using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Service which is respeonsible for extending METAL macros.
  /// </summary>
  public interface IMacroExtender
  {
    /// <summary>
    /// Gets a <see cref="IRenderingContext"/> representing the result of extending the extended macro using the given
    /// macro.
    /// </summary>
    /// <param name="macro">The macro with which extends another 'parent' macro.</param>
    /// <param name="extendedMacro">The 'parent' macro to be extended.</param>
    IRenderingContext Extend(IRenderingContext macro, IRenderingContext extendedMacro);
  }
}

