using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Metal;
using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// An implementation of <see cref="IProcessesExpressionContext"/> which adds
    /// source annotation to the DOM where it is applicable to do so.
    /// </summary>
    public class SourceAnnotationContextProcessor : IProcessesExpressionContext
    {
        readonly IGetsMetalAttributeSpecs metalSpecProvider;
        readonly IGetsAnnotationForElement annotationProvider;
        readonly IAddsComment commenter;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var element = context.CurrentElement;

            // Just an optimisation; if this isn't an element node then there's nothing to do
            if(!element.IsElement)
                return Task.FromResult(ExpressionContextProcessingResult.Noop);

            if (IsRootElement(element))
                AnnotateRootElement(element);
            else if (element.IsImported)
                AnnotateImportedElement(element);
            else if (HasDefineMacroAttribute(element))
                AnnotateDefineMacroElement(element);
            else if (HasDefineSlotAttribute(element))
                AnnotateDefineSlotElement(element);

            return Task.FromResult(ExpressionContextProcessingResult.Noop);
        }

        void AnnotateRootElement(INode element)
        {
            // Annotate before the element
            var annotation = annotationProvider.GetAnnotation(element);
            commenter.AddCommentBefore(element, annotation);
        }

        void AnnotateImportedElement(INode element)
        {
            // Annotate before the start tag
            var beforeAnnotation = annotationProvider.GetAnnotation(element);
            commenter.AddCommentBefore(element, beforeAnnotation);

            // Annotate after the end tag
            var afterAnnotation = annotationProvider.GetAnnotation(element, false);
            commenter.AddCommentAfter(element, afterAnnotation);
        }

        void AnnotateDefineMacroElement(INode element)
        {
            // Annotate before the element
            var annotation = annotationProvider.GetAnnotation(element);
            commenter.AddCommentBefore(element, annotation);
        }

        void AnnotateDefineSlotElement(INode element)
        {
            // Annotate after the element
            var annotation = annotationProvider.GetAnnotation(element);
            commenter.AddCommentAfter(element, annotation);
        }

        bool IsRootElement(INode element) => element.ParentElement == null;

        bool HasDefineMacroAttribute(INode element)
            => element.GetMatchingAttribute(metalSpecProvider.DefineMacro) != null;

        bool HasDefineSlotAttribute(INode element)
            => element.GetMatchingAttribute(metalSpecProvider.DefineSlot) != null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceAnnotationContextProcessor"/> class.
        /// </summary>
        /// <param name="metalSpecProvider">METAL spec provider.</param>
        /// <param name="annotationProvider">Annotation provider.</param>
        /// <param name="commenter">Commenter.</param>
        public SourceAnnotationContextProcessor(IGetsMetalAttributeSpecs metalSpecProvider,
                                                IGetsAnnotationForElement annotationProvider,
                                                IAddsComment commenter)
        {
            this.metalSpecProvider = metalSpecProvider ?? throw new ArgumentNullException(nameof(metalSpecProvider));
            this.annotationProvider = annotationProvider ?? throw new ArgumentNullException(nameof(annotationProvider));
            this.commenter = commenter ?? throw new ArgumentNullException(nameof(commenter));
        }
    }
}
