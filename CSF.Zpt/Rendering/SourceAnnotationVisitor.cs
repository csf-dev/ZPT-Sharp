using System;
using System.Linq;

namespace CSF.Zpt.Rendering
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
    /// <param name="element">Element.</param>
    /// <param name="model">Model.</param>
    public override Element Visit(Element element, Model model)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      if(this.RenderingOptions.AddSourceFileAnnotation)
      {
        if(element.IsRoot)
        {
          this.AddAnnotationComment(element);
        }

        var childrenInDifferentFiles = element
          .GetChildElements()
          .Where(x => x.IsImported)
          .ToArray();

        foreach(var child in childrenInDifferentFiles)
        {
          this.AddAnnotationComment(child);
        }
      }

      return element;
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
    /// <param name="model">The object model provided as context to the visitor.</param>
    public override Element VisitRecursively(Element element, Model model)
    {
      return this.RenderingOptions.AddSourceFileAnnotation? base.VisitRecursively(element, model) : element;
    }

    #endregion

    #region methods

    /// <summary>
    /// Adds the source annotation comment to an element.
    /// </summary>
    /// <param name="element">The element to annotate.</param>
    private void AddAnnotationComment(Element element)
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

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.SourceAnnotationVisitor"/> class.
    /// </summary>
    /// <param name="options">Options.</param>
    public SourceAnnotationVisitor(RenderingOptions options = null) : base(options) {}

    #endregion
  }
}

