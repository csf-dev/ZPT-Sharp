using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Interface for a service which applies METAL macro expansion to an element within a <see cref="IRenderingContext"/>.
  /// </summary>
  public interface IMacroExpander
  {
    /// <summary>
    /// Expands macros in the given context and returns an appropriate context.
    /// </summary>
    /// <returns>The rendering context exposed by the current operation.</returns>
    /// <param name="context">A rendering context.</param>
    IRenderingContext ExpandMacros(IRenderingContext context);
  }
}

