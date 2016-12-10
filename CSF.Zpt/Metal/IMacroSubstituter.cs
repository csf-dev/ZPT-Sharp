using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Serivce which is responsible for making substitutions of content in METAL macros.
  /// </summary>
  public interface IMacroSubstituter
  {
    /// <summary>
    /// Makes the substitutions from the macro into the given source.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These substitutions include replacing the source context with the content of the macro, but also filling
    /// slots which have been defined by the macro and for which filler content is found within the source context.
    /// </para>
    /// </remarks>
    /// <returns>A rendering context representing the result of the substitutions.</returns>
    /// <param name="sourceContext">The source context, from the original document (to be replaced).</param>
    /// <param name="macroContext">The macro context from which to draw replacements.</param>
    IRenderingContext MakeSubstitutions(IRenderingContext sourceContext, IRenderingContext macroContext);
  }
}

