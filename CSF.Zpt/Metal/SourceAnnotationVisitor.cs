using System;
using System.Linq;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Implementation of <see cref="IContextVisitor"/> which add source-file annotation (such as for debugging purposes).
  /// </summary>
  public class SourceAnnotationVisitor : ContextVisitorBase
  {
    #region ElementVisitor implementation

    /// <summary>
    /// Visit the given context and return a collection of the resultant contexts.
    /// </summary>
    /// <returns>Zero or more <see cref="RenderingContext"/> instances, determined by the outcome of this visit.</returns>
    /// <param name="context">The rendering context to visit.</param>
    public override RenderingContext[] Visit(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
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
    /// Visits a rendering context and returns a collection of contexts which represent the result of that visit.
    /// </summary>
    /// <returns>The rendering contexts instances which are exposed after the visiting process is complete.</returns>
    /// <param name="context">The rendering context to visit.</param>
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

      if(element.IsRoot)
      {
        element.AddCommentAfter(commentText);
      }
      else
      {
        element.AddCommentBefore(commentText);
      }
    }

    #endregion
  }
}

