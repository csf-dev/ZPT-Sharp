using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.SourceAnnotation
{
  /// <summary>
  /// Implementation of <see cref="IContextVisitor"/> which adds source annotation comments to a document.
  /// </summary>
  public class SourceAnnotationVisitor : NoOpVisitor
  {
    #region fields

    private ISourceAnnotator _annotator;

    #endregion

    #region methods

    /// <summary>
    /// Visit the given context and return a collection of the resultant contexts.
    /// </summary>
    /// <param name="context">Context.</param>
    public override IRenderingContext[] Visit(IRenderingContext context)
    {
      _annotator.WriteAnnotationIfAppropriate(context);

      return base.Visit(context);
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.SourceAnnotation.SourceAnnotationVisitor"/> class.
    /// </summary>
    public SourceAnnotationVisitor() : this(null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.SourceAnnotation.SourceAnnotationVisitor"/> class.
    /// </summary>
    /// <param name="annotator">The annotator instance to use.</param>
    public SourceAnnotationVisitor(ISourceAnnotator annotator = null)
    {
      _annotator = annotator?? new SourceAnnotator();
    }

    #endregion
  }
}

