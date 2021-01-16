using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.SourceAnnotation;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// Implementation of <see cref="IModifiesDocument"/> which adds source annotation
    /// to the document DOM where appropriate.  Source annotation is a debugging and logging
    /// aid which helps developers see where each part of the output DOM originates.
    /// </summary>
    public class SourceAnnotationDocumentModifierDecorator : IModifiesDocument
    {
        readonly IGetsSourceAnnotationContextProcessor contextProcessorFactory;
        readonly IIterativelyModifiesDocument iterativeModifier;
        readonly RenderingConfig config;
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
            if (!config.IncludeSourceAnnotation)
                return;

            var contextProcessor = contextProcessorFactory.GetSourceAnnotationContextProcessor();
            var rootContext = rootContextProvider.GetExpressionContext(document, model);
            await iterativeModifier.ModifyDocumentAsync(rootContext, contextProcessor, token)
                .ConfigureAwait(false);
            await wrapped.ModifyDocumentAsync(document, model, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SourceAnnotationDocumentModifierDecorator"/> class.
        /// </summary>
        /// <param name="contextProcessorFactory">Context processor factory.</param>
        /// <param name="iterativeModifier">Iterative modifier.</param>
        /// <param name="config">Rendering config.</param>
        /// <param name="wrapped">Wrapped.</param>
        /// <param name="rootContextProvider">A provider for the root expression context.</param>
        public SourceAnnotationDocumentModifierDecorator(IGetsSourceAnnotationContextProcessor contextProcessorFactory,
                                                         IIterativelyModifiesDocument iterativeModifier,
                                                         RenderingConfig config,
                                                         IModifiesDocument wrapped,
                                                    IGetsRootExpressionContext rootContextProvider)
        {
            this.contextProcessorFactory = contextProcessorFactory ?? throw new ArgumentNullException(nameof(contextProcessorFactory));
            this.iterativeModifier = iterativeModifier ?? throw new ArgumentNullException(nameof(iterativeModifier));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.rootContextProvider = rootContextProvider ?? throw new ArgumentNullException(nameof(rootContextProvider));
        }
    }
}
