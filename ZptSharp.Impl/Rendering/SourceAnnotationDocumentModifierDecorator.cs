using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.SourceAnnotation;

namespace ZptSharp.Rendering
{
    public class SourceAnnotationDocumentModifierDecorator : IModifiesDocument
    {
        readonly IGetsSourceAnnotationContextProcessor contextProcessorFactory;
        readonly IIterativelyModifiesDocument iterativeModifier;
        readonly IModifiesDocument wrapped;

        public async Task ModifyDocumentAsync(IDocument document, RenderZptDocumentRequest request, CancellationToken token = default)
        {
            var contextProcessor = contextProcessorFactory.GetSourceAnnotationContextProcessor();
            await iterativeModifier.ModifyDocumentAsync(document, request, contextProcessor, token);
            await wrapped.ModifyDocumentAsync(document, request, token);
        }

        public SourceAnnotationDocumentModifierDecorator(IGetsSourceAnnotationContextProcessor contextProcessorFactory,
                                                         IIterativelyModifiesDocument iterativeModifier,
                                                         IModifiesDocument wrapped)
        {
            this.contextProcessorFactory = contextProcessorFactory ?? throw new ArgumentNullException(nameof(contextProcessorFactory));
            this.iterativeModifier = iterativeModifier ?? throw new ArgumentNullException(nameof(iterativeModifier));
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
