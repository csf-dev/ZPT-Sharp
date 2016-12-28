using System;
using CSF.Zpt.Rendering;
using System.IO;
using CSF.IO;

namespace CSF.Zpt.SourceAnnotation
{
  /// <summary>
  /// Performs source annotation tasks upon ZPT elements.
  /// </summary>
  public class SourceAnnotator : ISourceAnnotator
  {
    #region constants

    private const int BORDER_CHARACTER_WIDTH = 78;
    private const char BORDER_CHARACTER = '=';

    // TODO: Move all of these strings into a resources file
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

    private readonly ICommentFormatter _formatter;

    #endregion

    #region methods

    /// <summary>
    /// Processes source annotation and adds comments before/after the <paramref name="targetContext"/> if appropriate.
    /// </summary>
    /// <param name="targetContext">Target context.</param>
    public void WriteAnnotationIfAppropriate(IRenderingContext targetContext)
    {
      if(!targetContext.RenderingOptions.AddSourceFileAnnotation)
      {
        return;
      }

      var element = targetContext.Element;

      if(element.IsRoot)
      {
        AnnotateRootElement(targetContext);
      }
      else if(element.IsImported)
      {
        AnnotateImportedElement(targetContext);
      }
      else if(element.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute) != null)
      {
        AnnotateDefineMacro(targetContext);
      }
      else if(element.GetMetalAttribute(ZptConstants.Metal.DefineSlotAttribute) != null)
      {
        AnnotateDefineSlot(targetContext);
      }

    }

    /// <summary>
    /// Adds source annotation for the root element in a document.
    /// </summary>
    /// <param name="context">The rendering context to be annotated.</param>
    public void AnnotateRootElement(IRenderingContext context)
    {
      var source = GetSource(context);
      var fileLocation = context.Element.GetFileLocation();

      var comment = _formatter.GetRootElementComment(source, fileLocation);
      context.Element.AddCommentBefore(comment);
    }

    /// <summary>
    /// Adds source annotation for an element which contains a macro-definition attribute.
    /// </summary>
    /// <param name="context">The rendering context to be annotated.</param>
    public void AnnotateDefineMacro(IRenderingContext context)
    {
      var source = GetSource(context);
      var fileLocation = context.Element.GetFileLocation();

      var comment = _formatter.GetDefineMacroComment(source, fileLocation);
      context.Element.AddCommentBefore(comment);
    }

    /// <summary>
    /// Adds source annotation for an element which contains a slot-definition attribute.
    /// </summary>
    /// <param name="context">The rendering context to be annotated.</param>
    public void AnnotateDefineSlot(IRenderingContext context)
    {
      var source = GetSource(context);
      var fileLocation = context.Element.GetFileLocation();

      var comment = _formatter.GetDefineSlotComment(source, fileLocation);
      context.Element.AddCommentAfter(comment);
    }

    /// <summary>
    /// Adds source annotation for an element which contains a slot-definition attribute.
    /// </summary>
    /// <param name="context">The rendering context to be annotated.</param>
    public void AnnotateImportedElement(IRenderingContext context)
    {
      // Before
      var source = GetSource(context);
      var fileLocation = context.Element.GetFileLocation();

      var beforeComment = _formatter.GetImportedElementComment(source, fileLocation);
      context.Element.AddCommentBefore(beforeComment);

      // After
      var originalSource = GetOriginalSource(context);
      var originalFileLocation = context.Element.GetOriginalContextEndTagLocation();

      var afterComment = _formatter.GetAfterImportedElementComment(originalSource, originalFileLocation);
      context.Element.AddCommentAfter(afterComment);
    }

    private string GetSource(IRenderingContext context)
    {
      return context.Element.GetSourceInfo().GetRelativeName(context.SourceAnnotationRootPath);
    }

    private string GetOriginalSource(IRenderingContext context)
    {
      return context.Element.GetOriginalContextSourceInfo().GetRelativeName(context.SourceAnnotationRootPath);
    }

//    private string GetSource(IRenderingContext context)
//    {
//      var sourceInfo = context.Element.GetSourceInfo();
//
//      var sourceFile = sourceInfo as SourceFileInfo;
//      if(sourceFile != null)
//      {
//        return GetSource(sourceFile, context);
//      }
//      else
//      {
//        return sourceInfo.FullName;
//      }
//    }
//
//    private string GetSource(SourceFileInfo sourceInfo, IRenderingContext context)
//    {
//      if(context.SourceAnnotationRootPath == null
//         || !Directory.Exists(context.SourceAnnotationRootPath)
//         || !sourceInfo.FileInfo.Exists)
//      {
//        return sourceInfo.FullName;
//      }
//
//      var root = new DirectoryInfo(context.SourceAnnotationRootPath);
//      if(!sourceInfo.FileInfo.IsChildOf(root))
//      {
//        return sourceInfo.FullName;
//      }
//
//      var source = sourceInfo.FileInfo.GetRelativePath(root);
//    }



//    /// <summary>
//    /// Processes source annotation and adds comments before/after the <paramref name="targetContext"/> if appropriate.
//    /// </summary>
//    /// <param name="targetContext">Target context.</param>
//    /// <param name="originalContext">Original context.</param>
//    /// <param name="replacementContext">Replacement context.</param>
//    public void ProcessAnnotation(IRenderingContext targetContext,
//                                  IRenderingContext originalContext,
//                                  IRenderingContext replacementContext)
//    {
//      if(targetContext == null)
//      {
//        throw new ArgumentNullException(nameof(targetContext));
//      }
//
//
//      IZptAttribute attr;
//
//      if((replacementContext == null
//          && (attr = targetContext.Element.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute)) != null)
//         || (replacementContext != null
//             && (attr = targetContext.Element.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute)) != null
//             && originalContext.Element.GetMetalAttribute(ZptConstants.Metal.UseMacroAttribute) != null))
//      {
//        AddAnnotation(targetContext,
//                      extraText: String.Format(MACRO_DEFINITION_FORMAT, attr.Value),
//                      replacementContext: replacementContext);
//      }
//
//      if(originalContext != null
//         && (attr = originalContext.Element.GetMetalAttribute(ZptConstants.Metal.UseMacroAttribute)) != null)
//      {
//        AddAnnotation(targetContext,
//                      beforeElement: false,
//                      originalContext: originalContext,
//                      extraText: String.Format(USED_MACRO_FORMAT, attr.Value),
//                      useEndTagLocation: true);
//      }
//
//      if((replacementContext != null
//          && (attr = replacementContext.Element.GetMetalAttribute(ZptConstants.Metal.DefineSlotAttribute)) != null)
//         || (replacementContext == null
//             && (attr = targetContext.Element.GetMetalAttribute(ZptConstants.Metal.DefineSlotAttribute)) != null))
//      {
//        AddAnnotation(targetContext,
//                      beforeElement: false,
//                      extraText: String.Format(SLOT_DEFINITION_FORMAT, attr.Value));
//      }
//
//      if(replacementContext != null
//         && (attr = targetContext.Element.GetMetalAttribute(ZptConstants.Metal.FillSlotAttribute)) != null)
//      {
//        AddAnnotation(targetContext,
//                      replacementContext: replacementContext,
//                      extraText: String.Format(FILLED_SLOT_FORMAT, attr.Value));
//
//        if(!originalContext.Element.IsFromSameDocumentAs(replacementContext.Element))
//        {
//          AddAnnotation(targetContext,
//                        beforeElement: false,
//                        extraText: String.Format(SLOT_DEFINITION_FORMAT, attr.Value),
//                        originalContext: originalContext);
//        }
//      }
//    }

//    private void AddAnnotation(IRenderingContext targetContext,
//                               bool skipLineNumber = false,
//                               bool beforeElement = true,
//                               IRenderingContext originalContext = null,
//                               IRenderingContext replacementContext = null,
//                               string extraText = null,
//                               bool useEndTagLocation = false)
//    {
//      
//
//
////      var bodyContext = originalContext?? targetContext;
////      var body = CreateCommentBody(bodyContext, skipLineNumber, extraText, useEndTagLocation, replacementContext);
////
////      var comment = FormatComment(body);
////
////      if(beforeElement
////         && ((!targetContext.Element.IsRoot && targetContext.Element.HasParent)
////             || targetContext.Element.CanWriteCommentWithoutParent))
////      {
////        targetContext.Element.AddCommentBefore(comment);
////      }
////      else if(beforeElement)
////      {
////        targetContext.Element.AddCommentInside(comment);
////      }
////      else
////      {
////        targetContext.Element.AddCommentAfter(comment);
////      }
//    }

//    private string FormatComment(string commentBody)
//    {
//      return String.Format(COMMENT_FORMAT,
//                           new String(BORDER_CHARACTER, BORDER_CHARACTER_WIDTH),
//                           commentBody);
//    }

//    private string CreateCommentBody(IRenderingContext targetContext,
//                                     bool skipLineNumber,
//                                     string extraText,
//                                     bool useEndTagPosition,
//                                     IRenderingContext replacementContext)
//    {
//      IZptElement
//        targetElement = targetContext.Element,
//        sourceElement = replacementContext?.Element ?? targetContext.Element;
//      string
//        fullFilename = targetContext.Element.GetSourceInfo().FullName,
//        filename,
//        filePosition = useEndTagPosition? sourceElement.GetEndTagFileLocation() : sourceElement.GetFileLocation(),
//        previousElement;
//
//      if(!String.IsNullOrEmpty(targetContext.SourceAnnotationRootPath)
//         && Directory.Exists(targetContext.SourceAnnotationRootPath)
//         && File.Exists(fullFilename))
//      {
//        var root = new DirectoryInfo(targetContext.SourceAnnotationRootPath);
//        var file = new FileInfo(fullFilename);
//        filename = file.IsChildOf(root)? file.GetRelativePath(root).Substring(1) : fullFilename;
//      }
//      else
//      {
//        filename = fullFilename;
//      }
//
//      filename = filename.Replace(Path.DirectorySeparatorChar.ToString(), "/");
//
//      if((!targetElement.IsRoot && targetElement.HasParent) || targetElement.CanWriteCommentWithoutParent)
//      {
//        previousElement = String.Empty;
//      }
//      else
//      {
//        previousElement = PREVIOUS_ELEMENT;
//      }
//
//      var body = (skipLineNumber || String.IsNullOrEmpty(filePosition))? filename : String.Format(POSITION_FORMAT, filename, filePosition);
//      var extra = _renderExtraText? extraText?? String.Empty : String.Empty;
//      return String.Concat(previousElement, extra, body);
//    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.SourceAnnotation.SourceAnnotator"/> class.
    /// </summary>
    /// <param name="formatter">The source annotation comment formatter.</param>
    public SourceAnnotator(ICommentFormatter formatter = null)
    {
      _formatter = formatter?? new CommentFormatter();
    }

    #endregion
  }
}

