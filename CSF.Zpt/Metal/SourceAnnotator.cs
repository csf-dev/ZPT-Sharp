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

    #region fields

    private log4net.ILog _logger;

    #endregion

    /* Comments come:
     * 
     * * Before a define macro (with the location of the macro)
     * * Before a use macro (with the location of the macro)
     * 
     * * Before a fill slot (with the location of the slot filler)
     * 
     * * After a define slot (with the location of the original slot)
     * * Before a root element (skipping the line number)
     */

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
        _logger.DebugFormat("Adding {1} annotation for element '{0}'", context.Element.Name, "root");
        AddAnnotation(context, skipLineNumber: true);
      }

      if((replacedContext == null
          && context.Element.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute) != null)
         || (replacedContext != null
             && context.Element.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute) != null
             && replacedContext.Element.GetMetalAttribute(ZptConstants.Metal.UseMacroAttribute) != null))
      {
        _logger.DebugFormat("Adding {1} annotation for element '{0}'", context.Element.Name, "define-macro");
        AddAnnotation(context);
      }

      if(replacedContext != null
         && replacedContext.Element.GetMetalAttribute(ZptConstants.Metal.UseMacroAttribute) != null)
      {
        _logger.DebugFormat("Adding {1} annotation for element '{0}'", context.Element.Name, "use-macro");
        AddAnnotation(context, beforeElement: false, replacedContext: replacedContext);
      }

      if((replacedContext != null
          && replacedContext.Element.GetMetalAttribute(ZptConstants.Metal.DefineSlotAttribute) != null)
         || (replacedContext == null
             && context.Element.GetMetalAttribute(ZptConstants.Metal.DefineSlotAttribute) != null))
      {
        _logger.DebugFormat("Adding {1} annotation for element '{0}'", context.Element.Name, "define-slot");
        AddAnnotation(context, beforeElement: false);
      }

      if(replacedContext != null
         && context.Element.GetMetalAttribute(ZptConstants.Metal.FillSlotAttribute) != null)
      {
        _logger.DebugFormat("Adding {1} annotation for element '{0}'", context.Element.Name, "fill-slot");
        AddAnnotation(context, replacedContext: replacedContext);
      }
    }

    private void AddAnnotation(RenderingContext context,
                               bool skipLineNumber = false,
                               bool beforeElement = true,
                               RenderingContext replacedContext = null)
    {
      var bodyContext = replacedContext?? context;
      var body = CreateCommentBody(bodyContext, skipLineNumber);

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

    #region constructor

    public SourceAnnotator()
    {
      _logger = log4net.LogManager.GetLogger(this.GetType());
    }

    #endregion
  }
}

