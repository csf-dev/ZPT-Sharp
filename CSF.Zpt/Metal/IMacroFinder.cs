using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// A type which finds METAL macros where they are referenced by a rendering context.
  /// </summary>
  public interface IMacroFinder
  {
    /// <summary>
    /// Examines the given <see cref="IRenderingContext"/> and - if it uses a METAL macro - gets that macro from the
    /// model contained within that context.
    /// </summary>
    /// <returns>Either an <see cref="IZptElement"/> instance representing the macro used, or a <c>null</c> reference.</returns>
    /// <param name="context">The rendering context.</param>
    IZptElement GetUsedMacro(IRenderingContext context);

    /// <summary>
    /// Examines the given <see cref="IRenderingContext"/> and - if it extends a METAL macro - gets that macro from the
    /// model contained within that context.
    /// </summary>
    /// <returns>Either an <see cref="IZptElement"/> instance representing the macro extended, or a <c>null</c> reference.</returns>
    /// <param name="context">The rendering context.</param>
    IZptElement GetExtendedMacro(IRenderingContext context);
  }
}

