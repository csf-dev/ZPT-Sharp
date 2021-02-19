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
        readonly IGetsAnnotationForNode annotationProvider;
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

            var node = context.CurrentNode;

            if (context.IsRootContext)
                AnnotateRootNode(node);
            else if (node.IsImported)
                AnnotateImportedNode(node);
            else if (HasDefineMacroAttribute(node))
                AnnotateDefineMacroNode(node);
            else if (HasDefineSlotAttribute(node))
                AnnotateDefineSlotNode(node);

            return Task.FromResult(ExpressionContextProcessingResult.Noop);
        }

        void AnnotateRootNode(INode node)
        {
            var annotation = annotationProvider.GetAnnotation(node, TagType.None);
            commenter.AddCommentBefore(node, annotation);
        }

        void AnnotateDefineMacroNode(INode node)
        {
            var annotation = annotationProvider.GetAnnotation(node);
            commenter.AddCommentBefore(node, annotation);
        }

        void AnnotateImportedNode(INode node)
        {
            var beforeAnnotation = annotationProvider.GetAnnotation(node);
            commenter.AddCommentBefore(node, beforeAnnotation);

            var afterAnnotation = annotationProvider.GetPreReplacementAnnotation(node, TagType.End);
            commenter.AddCommentAfter(node, afterAnnotation);
        }

        void AnnotateDefineSlotNode(INode node)
        {
            var annotation = annotationProvider.GetAnnotation(node);
            commenter.AddCommentAfter(node, annotation);
        }

        bool HasDefineMacroAttribute(INode node)
            => node.GetMatchingAttribute(metalSpecProvider.DefineMacro) != null;

        bool HasDefineSlotAttribute(INode node)
            => node.GetMatchingAttribute(metalSpecProvider.DefineSlot) != null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceAnnotationContextProcessor"/> class.
        /// </summary>
        /// <param name="metalSpecProvider">METAL spec provider.</param>
        /// <param name="annotationProvider">Annotation provider.</param>
        /// <param name="commenter">Commenter.</param>
        public SourceAnnotationContextProcessor(IGetsMetalAttributeSpecs metalSpecProvider,
                                                IGetsAnnotationForNode annotationProvider,
                                                IAddsComment commenter)
        {
            this.metalSpecProvider = metalSpecProvider ?? throw new ArgumentNullException(nameof(metalSpecProvider));
            this.annotationProvider = annotationProvider ?? throw new ArgumentNullException(nameof(annotationProvider));
            this.commenter = commenter ?? throw new ArgumentNullException(nameof(commenter));
        }
    }
}
