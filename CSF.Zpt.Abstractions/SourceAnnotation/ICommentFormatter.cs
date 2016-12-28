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
  }
}

