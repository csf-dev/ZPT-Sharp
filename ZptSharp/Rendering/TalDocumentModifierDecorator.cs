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
        readonly IGetsRootExpressionContext rootContextProvider;

        /// <summary>
        /// Performs alterations for a specified document, using the specified rendering request.
        /// This method will manipulate the <paramref name="document"/>, according to the rules of ZPT.
        /// </summary>
        /// <returns>A task indicating when the process is complete.</returns>
        /// <param name="document">The document to render.</param>
        /// <param name="model">The model to render.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        public async Task ModifyDocumentAsync(IDocument document, object model, CancellationToken token = default)
        {
            var contextProcessor = contextProcessorFactory.GetTalContextProcessor();
            var rootContext = rootContextProvider.GetExpressionContext(document, model);
            await iterativeModifier.ModifyDocumentAsync(rootContext, contextProcessor, token)
                .ConfigureAwait(false);
            await wrapped.ModifyDocumentAsync(document, model, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TalDocumentModifierDecorator"/> class.
        /// </summary>
        /// <param name="contextProcessorFactory">Context processor factory.</param>
        /// <param name="iterativeModifier">Iterative modifier.</param>
        /// <param name="wrapped">Wrapped.</param>
        /// <param name="rootContextProvider">A provider for the root expression context.</param>
        public TalDocumentModifierDecorator(IGetsTalContextProcessor contextProcessorFactory,
                                            IIterativelyModifiesDocument iterativeModifier,
                                            IModifiesDocument wrapped,
                                            IGetsRootExpressionContext rootContextProvider)
        {
            this.contextProcessorFactory = contextProcessorFactory ?? throw new ArgumentNullException(nameof(contextProcessorFactory));
            this.iterativeModifier = iterativeModifier ?? throw new ArgumentNullException(nameof(iterativeModifier));
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.rootContextProvider = rootContextProvider ?? throw new ArgumentNullException(nameof(rootContextProvider));
        }
    }
}
