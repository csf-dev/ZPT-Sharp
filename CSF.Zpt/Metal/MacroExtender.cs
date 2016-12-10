using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Default implementation of <see cref="IMacroExtender"/>.
  /// </summary>
  public class MacroExtender : IMacroExtender
  {
    /// <summary>
    /// Gets a <see cref="IRenderingContext"/> representing the result of extending the extended macro using the given
    /// macro.
    /// </summary>
    /// <param name="macro">The macro with which extends another 'parent' macro.</param>
    /// <param name="extendedMacro">The 'parent' macro to be extended.</param>
    public IRenderingContext Extend(IRenderingContext macro, IRenderingContext extendedMacro)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }
  }
}

