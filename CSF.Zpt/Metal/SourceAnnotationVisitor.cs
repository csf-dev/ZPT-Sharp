using System;
using System.Linq;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Element visitor which adds source file annotation.
  /// </summary>
  public class SourceAnnotationVisitor : ElementVisitor
  {
    #region ElementVisitor implementation

    /// <summary>
    /// Visit the given element and perform modifications as required.
    /// </summary>
    /// <param name="element">The element to visit.</param>
    /// <param name="context">The rendering context provided to the visitor.</param>
    /// <param name="options">The rendering options to use.</param>
    public override RenderingContext[] Visit(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      if(context.RenderingOptions.AddSourceFileAnnotation)
      {
        if(context.Element.IsRoot)
        {
          this.AddAnnotationComment(context.Element);
        }

        var childrenInDifferentFiles = context.Element
          .GetChildElements()
          .Where(x => x.IsImported)
          .ToArray();

        foreach(var child in childrenInDifferentFiles)
        {
          this.AddAnnotationComment(child);
        }
      }

      return new[] { context };
    }

    /// <summary>
    /// Visits the given element, and then recursively visits all of its child elements.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the rendering options do not indicate that source annotation is to be added, then this method becomes a no-op.
    /// </para>
    /// </remarks>
    /// <returns>A reference to the element which has been visited.  This might be the input <paramref name="element"/> or a replacement.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="context">The rendering context provided to the visitor.</param>
    /// <param name="options">The rendering options to use.</param>
    public override RenderingContext[] VisitRecursively(RenderingContext context)
    {
      return context.RenderingOptions.AddSourceFileAnnotation? base.VisitRecursively(context) : new [] { context };
    }

    #endregion

    #region methods

    /// <summary>
    /// Adds the source annotation comment to an element.
    /// </summary>
    /// <param name="element">The element to annotate.</param>
    private void AddAnnotationComment(ZptElement element)
    {
      string
        filename = element.SourceFile.GetFullName(),
        filePosition = element.GetFileLocation(),
        commentText;

      if(filePosition != null)
      {
        commentText = String.Format("{0}, {1}", filename, filePosition);
      }
      else
      {
        commentText = filename;
      }

      element.AddCommentBefore(commentText);
    }

    #endregion
  }
}

