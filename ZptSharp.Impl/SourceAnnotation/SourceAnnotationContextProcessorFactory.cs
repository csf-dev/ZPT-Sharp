using ZptSharp.Metal;
using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// Implementation of <see cref="IGetsSourceAnnotationContextProcessor"/> which returns
    /// a context processor suitable for adding source annotation to a rendered document.
    /// </summary>
    public class SourceAnnotationContextProcessorFactory : IGetsSourceAnnotationContextProcessor
    {
        readonly IGetsMetalAttributeSpecs metalSpecProvider;
        readonly IGetsAnnotationForElement annotationProvider;
        readonly IAddsComment commenter;

        /// <summary>
        /// Gets the source-annotation context processor.
        /// </summary>
        /// <returns>The source-annotation context processor.</returns>
        public IProcessesExpressionContext GetSourceAnnotationContextProcessor()
            => new SourceAnnotationContextProcessor(metalSpecProvider, annotationProvider, commenter);

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SourceAnnotationContextProcessorFactory"/> class.
        /// </summary>
        /// <param name="metalSpecProvider">METAL spec provider.</param>
        /// <param name="annotationProvider">Annotation provider.</param>
        /// <param name="commenter">Commenter.</param>
        public SourceAnnotationContextProcessorFactory(IGetsMetalAttributeSpecs metalSpecProvider,
                                                       IGetsAnnotationForElement annotationProvider,
                                                       IAddsComment commenter)
        {
            this.metalSpecProvider = metalSpecProvider ?? throw new System.ArgumentNullException(nameof(metalSpecProvider));
            this.annotationProvider = annotationProvider ?? throw new System.ArgumentNullException(nameof(annotationProvider));
            this.commenter = commenter ?? throw new System.ArgumentNullException(nameof(commenter));
        }
    }
}
