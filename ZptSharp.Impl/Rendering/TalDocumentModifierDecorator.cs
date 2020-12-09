using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Tal;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// Implementation of <see cref="IModifiesDocument"/> which handles TAL model-binding directives.
    /// </summary>
    public class TalDocumentModifierDecorator : IModifiesDocument
    {
        readonly IGetsTalContextProcessor contextProcessorFactory;
        readonly IIterativelyModifiesDocument iterativeModifier;
        readonly IModifiesDocument wrapped;

        /// <summary>
        /// Performs alterations for a specified document, using the specified rendering request.
        /// This method will manipulate the <paramref name="document"/>, according to the rules of ZPT.
        /// </summary>
        /// <returns>A task indicating when the process is complete.</returns>
        /// <param name="document">The document to render.</param>
        /// <param name="request">The rendering request.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        public async Task ModifyDocumentAsync(IDocument document, RenderZptDocumentRequest request, CancellationToken token = default)
        {
            var contextProcessor = contextProcessorFactory.GetTalContextProcessor();
            await iterativeModifier.ModifyDocumentAsync(document, request, contextProcessor, token)
                .ConfigureAwait(false);
            await wrapped.ModifyDocumentAsync(document, request, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TalDocumentModifierDecorator"/> class.
        /// </summary>
        /// <param name="contextProcessorFactory">Context processor factory.</param>
        /// <param name="iterativeModifier">Iterative modifier.</param>
        /// <param name="wrapped">Wrapped.</param>
        public TalDocumentModifierDecorator(IGetsTalContextProcessor contextProcessorFactory,
                                            IIterativelyModifiesDocument iterativeModifier,
                                            IModifiesDocument wrapped)
        {
            this.contextProcessorFactory = contextProcessorFactory ?? throw new ArgumentNullException(nameof(contextProcessorFactory));
            this.iterativeModifier = iterativeModifier ?? throw new ArgumentNullException(nameof(iterativeModifier));
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
