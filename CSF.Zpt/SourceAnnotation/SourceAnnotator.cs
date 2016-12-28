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

