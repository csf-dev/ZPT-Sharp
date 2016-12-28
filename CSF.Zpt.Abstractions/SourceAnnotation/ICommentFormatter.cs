using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.SourceAnnotation
{
  /// <summary>
  /// Service which creates formatted source annotation comments.
  /// </summary>
  public interface ICommentFormatter
  {
    /// <summary>
    /// Gets the formatted comment for the root element of a ZPT document.
    /// </summary>
    /// <returns>The formatted comment.</returns>
    /// <param name="source">The source information for the file.</param>
    /// <param name="lineNumber">The line number within the file.</param>
    string GetRootElementComment(string source, string lineNumber);

    /// <summary>
    /// Gets the formatted comment for a <c>define-macro</c> attribute.
    /// </summary>
    /// <returns>The formatted comment.</returns>
    /// <param name="source">The source information for the file.</param>
    /// <param name="lineNumber">The line number within the file.</param>
    string GetDefineMacroComment(string source, string lineNumber);

    /// <summary>
    /// Gets the formatted comment for a <c>define-slot</c> attribute.
    /// </summary>
    /// <returns>The formatted comment.</returns>
    /// <param name="source">The source information for the file.</param>
    /// <param name="lineNumber">The line number within the file.</param>
    string GetDefineSlotComment(string source, string lineNumber);

    /// <summary>
    /// Gets the formatted comment for an imported element.
    /// </summary>
    /// <returns>The formatted comment.</returns>
    /// <param name="source">The source information for the file.</param>
    /// <param name="lineNumber">The line number within the file.</param>
    string GetImportedElementComment(string source, string lineNumber);
  }
}

