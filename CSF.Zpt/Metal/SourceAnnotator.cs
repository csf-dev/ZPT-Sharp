using System;
using CSF.Zpt.Rendering;

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
      POSITION_FORMAT = "{0} (line {1})";

    #endregion

    #region methods

    public string CreateComment(RenderingContext context)
    {
      var body = CreateCommentBody(context);
      return FormatComment(body);
    }

    private string FormatComment(string commentBody)
    {
      return String.Format(COMMENT_FORMAT,
                           new String(BORDER_CHARACTER, BORDER_CHARACTER_WIDTH),
                           commentBody);
    }

    private string CreateCommentBody(RenderingContext context)
    {
      string
        filename = context.Element.SourceFile.GetFullName(),
        filePosition = context.Element.GetFileLocation();

      return String.IsNullOrEmpty(filePosition)? filename : String.Format(POSITION_FORMAT, filename, filePosition);
    }

    #endregion
  }
}

