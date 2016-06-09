using System;
using CSF.Zpt.Rendering;
using System.IO;
using CSF.IO;

namespace CSF.Zpt.Metal
{
  public class SourceAnnotator
  {
    #region constants

    private const int BORDER_CHARACTER_WIDTH = 78;
    private const char BORDER_CHARACTER = '=';

    private const string
      COMMENT_FORMAT = @"
{0}
{1}
{0}
",
      POSITION_FORMAT = "{0} (line {1})",
      PREVIOUS_ELEMENT = "(previous element)\n";

    #endregion

    /* Comments come:
     * 
     * * Before a define macro (with the location of the macro)
     * * Before a use macro (with the location of the macro)
     * 
     * * Before a fill slot (with the location of the slot filler)
     * * After a define slot (with the location of the original slot)
     */

    #region methods

    public void AddComment(RenderingContext context, bool skipLineNumber)
    {
      var body = CreateCommentBody(context, skipLineNumber);
      var comment = FormatComment(body);

      if(!context.Element.HasParent)
      {
        context.Element.AddCommentAfter(comment);
      }
      else
      {
        context.Element.AddCommentBefore(comment);
      }
    }

    private string FormatComment(string commentBody)
    {
      return String.Format(COMMENT_FORMAT,
                           new String(BORDER_CHARACTER, BORDER_CHARACTER_WIDTH),
                           commentBody);
    }

    private string CreateCommentBody(RenderingContext context, bool skipLineNumber)
    {
      string
        fullFilename = context.Element.SourceFile.GetFullName(),
        filename,
        filePosition = context.Element.GetFileLocation(),
        previousElement;

      if(!String.IsNullOrEmpty(context.SourceAnnotationRootPath)
         && Directory.Exists(context.SourceAnnotationRootPath)
         && File.Exists(fullFilename))
      {
        var root = new DirectoryInfo(context.SourceAnnotationRootPath);
        var file = new FileInfo(fullFilename);
        filename = file.IsChildOf(root)? file.GetRelative(root).Substring(1) : fullFilename;
      }
      else
      {
        filename = fullFilename;
      }

      if(context.Element.HasParent || context.Element.CanWriteCommentWithoutParent)
      {
        previousElement = String.Empty;
      }
      else
      {
        previousElement = PREVIOUS_ELEMENT;
      }

      var body = (skipLineNumber || String.IsNullOrEmpty(filePosition))? filename : String.Format(POSITION_FORMAT, filename, filePosition);
      return String.Concat(previousElement, body);
    }

    #endregion
  }
}

