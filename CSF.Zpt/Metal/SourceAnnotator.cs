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
      PREVIOUS_ELEMENT = "(previous element)\n",
      MACRO_DEFINITION_FORMAT = "Macro definition '{0}' from:\n",
      SLOT_DEFINITION_FORMAT = "Slot definition '{0}' from:\n",
      USED_MACRO_FORMAT = "Completed using macro '{0}'; resuming at:\n",
      FILLED_SLOT_FORMAT = "Slot '{0}' filled from:\n";

    #endregion

    #region fields

    private log4net.ILog _logger;

    #endregion

    #region methods

    public void ProcessAnnotation(RenderingContext context,
                                  RenderingContext replacedContext = null)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      if(!context.RenderingOptions.AddSourceFileAnnotation)
      {
        return;
      }

      if(context.Element.IsRoot)
      {
        AddAnnotation(context, skipLineNumber: true);
      }

      ZptAttribute name;

      if((replacedContext == null
          && (name = context.Element.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute)) != null)
         || (replacedContext != null
             && (name = context.Element.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute)) != null
             && replacedContext.Element.GetMetalAttribute(ZptConstants.Metal.UseMacroAttribute) != null))
      {
        AddAnnotation(context, extraText: String.Format(MACRO_DEFINITION_FORMAT, name.Value));
      }

      if(replacedContext != null
         && (name = replacedContext.Element.GetMetalAttribute(ZptConstants.Metal.UseMacroAttribute)) != null)
      {
        AddAnnotation(context,
                      beforeElement: false,
                      replacedContext: replacedContext,
                      extraText: String.Format(USED_MACRO_FORMAT, name.Value),
                      useEndTagLocation: true);
      }

      if((replacedContext != null
          && (name = replacedContext.Element.GetMetalAttribute(ZptConstants.Metal.DefineSlotAttribute)) != null)
         || (replacedContext == null
             && (name = context.Element.GetMetalAttribute(ZptConstants.Metal.DefineSlotAttribute)) != null))
      {
        AddAnnotation(context,
                      beforeElement: false,
                      extraText: String.Format(SLOT_DEFINITION_FORMAT, name.Value));
      }

      if(replacedContext != null
         && (name = context.Element.GetMetalAttribute(ZptConstants.Metal.FillSlotAttribute)) != null)
      {
        AddAnnotation(context,
                      replacedContext: replacedContext,
                      extraText: String.Format(FILLED_SLOT_FORMAT, name.Value));
      }
    }

    private void AddAnnotation(RenderingContext context,
                               bool skipLineNumber = false,
                               bool beforeElement = true,
                               RenderingContext replacedContext = null,
                               string extraText = null,
                               bool useEndTagLocation = false)
    {
      var bodyContext = replacedContext?? context;
      var body = CreateCommentBody(bodyContext, skipLineNumber, extraText, useEndTagLocation);

      var comment = FormatComment(body);

      if(beforeElement
         && (context.Element.HasParent
             || context.Element.CanWriteCommentWithoutParent))
      {
        context.Element.AddCommentBefore(comment);
      }
      else if(beforeElement)
      {
        context.Element.AddCommentInside(comment);
      }
      else
      {
        context.Element.AddCommentAfter(comment);
      }
    }

    private string FormatComment(string commentBody)
    {
      return String.Format(COMMENT_FORMAT,
                           new String(BORDER_CHARACTER, BORDER_CHARACTER_WIDTH),
                           commentBody);
    }

    private string CreateCommentBody(RenderingContext context,
                                     bool skipLineNumber,
                                     string extraText,
                                     bool useEndTagPosition)
    {
      var element = context.Element;
      string
        fullFilename = context.Element.SourceFile.GetFullName(),
        filename,
        filePosition = useEndTagPosition? element.GetEndTagFileLocation() : element.GetFileLocation(),
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

      if(element.HasParent || element.CanWriteCommentWithoutParent)
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

    #region constructor

    public SourceAnnotator()
    {
      _logger = log4net.LogManager.GetLogger(this.GetType());
    }

    #endregion
  }
}

