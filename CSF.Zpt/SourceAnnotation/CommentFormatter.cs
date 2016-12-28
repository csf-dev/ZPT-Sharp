using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.SourceAnnotation
{
  /// <summary>
  /// Default implementation of <see cref="ICommentFormatter"/>.
  /// </summary>
  public class CommentFormatter : ICommentFormatter
  {
    #region methods

    /// <summary>
    /// Gets the formatted comment for the root element of a ZPT document.
    /// </summary>
    /// <returns>The formatted comment.</returns>
    /// <param name="source">The source information for the file.</param>
    /// <param name="lineNumber">The line number within the file.</param>
    public string GetRootElementComment(string source, string lineNumber)
    {
      if(source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }

      return String.Format(Resources.SourceAnnotationFormats.RootElementComment, source);
    }

    #endregion
  }
}

