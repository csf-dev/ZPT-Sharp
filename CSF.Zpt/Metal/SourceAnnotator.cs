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
    private const bool RENDER_EXTRA_TEXT = false;

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

    public void ProcessAnnotation(RenderingContext targetContext,
                                  RenderingContext originalContext = null,
                                  RenderingContext replacementContext = null)
    {
      if(targetContext == null)
      {
        throw new ArgumentNullException(nameof(targetContext));
      }

      if(!targetContext.RenderingOptions.AddSourceFileAnnotation)
      {
        return;
      }

      if(targetContext.Element.IsRoot)
      {
        AddAnnotation(targetContext, skipLineNumber: true);
      }

      ZptAttribute attr;

      if((replacementContext == null
          && (attr = targetContext.Element.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute)) != null)
         || (replacementContext != null
             && (attr = targetContext.Element.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute)) != null
             && originalContext.Element.GetMetalAttribute(ZptConstants.Metal.UseMacroAttribute) != null))
      {
        AddAnnotation(targetContext,
                      extraText: String.Format(MACRO_DEFINITION_FORMAT, attr.Value),
                      replacementContext: replacementContext);
      }

      if(originalContext != null
         && (attr = originalContext.Element.GetMetalAttribute(ZptConstants.Metal.UseMacroAttribute)) != null)
      {
        AddAnnotation(targetContext,
                      beforeElement: false,
                      originalContext: originalContext,
                      extraText: String.Format(USED_MACRO_FORMAT, attr.Value),
                      useEndTagLocation: true);
      }

      if((replacementContext != null
          && (attr = replacementContext.Element.GetMetalAttribute(ZptConstants.Metal.DefineSlotAttribute)) != null)
         || (replacementContext == null
             && (attr = targetContext.Element.GetMetalAttribute(ZptConstants.Metal.DefineSlotAttribute)) != null))
      {
        AddAnnotation(targetContext,
                      beforeElement: false,
                      extraText: String.Format(SLOT_DEFINITION_FORMAT, attr.Value));
      }

      if(replacementContext != null
         && (attr = targetContext.Element.GetMetalAttribute(ZptConstants.Metal.FillSlotAttribute)) != null)
      {
        AddAnnotation(targetContext,
                      replacementContext: replacementContext,
                      extraText: String.Format(FILLED_SLOT_FORMAT, attr.Value));

        if(!originalContext.Element.IsFromSameDocumentAs(replacementContext.Element))
        {
          AddAnnotation(targetContext,
                        beforeElement: false,
                        extraText: String.Format(SLOT_DEFINITION_FORMAT, attr.Value),
                        originalContext: originalContext);
        }
      }
    }

    private void AddAnnotation(RenderingContext targetContext,
                               bool skipLineNumber = false,
                               bool beforeElement = true,
                               RenderingContext originalContext = null,
                               RenderingContext replacementContext = null,
                               string extraText = null,
                               bool useEndTagLocation = false)
    {
      var bodyContext = originalContext?? targetContext;
      var body = CreateCommentBody(bodyContext, skipLineNumber, extraText, useEndTagLocation, replacementContext);

      var comment = FormatComment(body);

      if(beforeElement
         && ((!targetContext.Element.IsRoot && targetContext.Element.HasParent)
             || targetContext.Element.CanWriteCommentWithoutParent))
      {
        targetContext.Element.AddCommentBefore(comment);
      }
      else if(beforeElement)
      {
        targetContext.Element.AddCommentInside(comment);
      }
      else
      {
        targetContext.Element.AddCommentAfter(comment);
      }
    }

    private string FormatComment(string commentBody)
    {
      return String.Format(COMMENT_FORMAT,
                           new String(BORDER_CHARACTER, BORDER_CHARACTER_WIDTH),
                           commentBody);
    }

    private string CreateCommentBody(RenderingContext targetContext,
                                     bool skipLineNumber,
                                     string extraText,
                                     bool useEndTagPosition,
                                     RenderingContext replacementContext)
    {
      ZptElement
        targetElement = targetContext.Element,
        sourceElement = replacementContext?.Element ?? targetContext.Element;
      string
        fullFilename = targetContext.Element.SourceFile.GetFullName(),
        filename,
        filePosition = useEndTagPosition? sourceElement.GetEndTagFileLocation() : sourceElement.GetFileLocation(),
        previousElement;

      if(!String.IsNullOrEmpty(targetContext.SourceAnnotationRootPath)
         && Directory.Exists(targetContext.SourceAnnotationRootPath)
         && File.Exists(fullFilename))
      {
        var root = new DirectoryInfo(targetContext.SourceAnnotationRootPath);
        var file = new FileInfo(fullFilename);
        filename = file.IsChildOf(root)? file.GetRelative(root).Substring(1) : fullFilename;
      }
      else
      {
        filename = fullFilename;
      }

      if((!targetElement.IsRoot && targetElement.HasParent) || targetElement.CanWriteCommentWithoutParent)
      {
        previousElement = String.Empty;
      }
      else
      {
        previousElement = PREVIOUS_ELEMENT;
      }

      var body = (skipLineNumber || String.IsNullOrEmpty(filePosition))? filename : String.Format(POSITION_FORMAT, filename, filePosition);
      var extra = RENDER_EXTRA_TEXT? extraText?? String.Empty : String.Empty;
      return String.Concat(previousElement, extra, body);
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

