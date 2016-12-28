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

      return String.Format(Resources.SourceAnnotationFormats.BaseCommentFormat, source);
    }

    /// <summary>
    /// Gets the formatted comment for a <c>define-macro</c> attribute.
    /// </summary>
    /// <returns>The formatted comment.</returns>
    /// <param name="source">The source information for the file.</param>
    /// <param name="lineNumber">The line number within the file.</param>
    public string GetDefineMacroComment(string source, string lineNumber)
    {
      return GetStandardComment(source, lineNumber);
    }

    /// <summary>
    /// Gets the formatted comment for a <c>define-slot</c> attribute.
    /// </summary>
    /// <returns>The formatted comment.</returns>
    /// <param name="source">The source information for the file.</param>
    /// <param name="lineNumber">The line number within the file.</param>
    public string GetDefineSlotComment(string source, string lineNumber)
    {
      return GetStandardComment(source, lineNumber);
    }

    /// <summary>
    /// Gets the formatted comment for an imported element.
    /// </summary>
    /// <returns>The formatted comment.</returns>
    /// <param name="source">The source information for the file.</param>
    /// <param name="lineNumber">The line number within the file.</param>
    public string GetImportedElementComment(string source, string lineNumber)
    {
      return GetStandardComment(source, lineNumber);
    }

    /// <summary>
    /// Gets the formatted comment for displaying after an imported element.
    /// </summary>
    /// <returns>The formatted comment.</returns>
    /// <param name="source">The source information for the file.</param>
    /// <param name="lineNumber">The line number within the file.</param>
    public string GetAfterImportedElementComment(string source, string lineNumber)
    {
      return GetStandardComment(source, lineNumber);
    }

    private string GetStandardComment(string source, string lineNumber)
    {
      if(source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }

      return String.Format(Resources.SourceAnnotationFormats.BaseCommentFormat,
                           GetSourceWithLineNumber(source, lineNumber));
    }

    private string GetSourceWithLineNumber(string source, string lineNumber)
    {
      if(String.IsNullOrEmpty(lineNumber))
      {
        return source;
      }

      return String.Format(Resources.SourceAnnotationFormats.SourceWithLineNumberFormat, source, lineNumber);
    }

    #endregion
  }
}

