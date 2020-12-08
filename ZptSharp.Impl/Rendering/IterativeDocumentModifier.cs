using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A service which which makes use of an <see cref="IIterativelyProcessesExpressionContexts"/>
    /// in order to apply a <see cref="IProcessesExpressionContext"/> to every element exposed by a document.
    /// </summary>
    public class IterativeDocumentModifier : IIterativelyModifiesDocument
    {
        readonly IGetsIterativeExpressionContextProcessor iteratorFactory;
        readonly IGetsRootExpressionContext rootContextProvider;

        /// <summary>
        /// Modifies the document using the specified context processor.
        /// </summary>
        /// <param name="document">The document to modify.</param>
        /// <param name="request">The rendering request.</param>
        /// <param name="contextProcessor">The processor to use when processing each expression context.</param>
        /// <param name="token">A cancellation token.</param>
        public async Task ModifyDocumentAsync(IDocument document,
                                              RenderZptDocumentRequest request,
                                              IProcessesExpressionContext contextProcessor,
                                              CancellationToken token = default)
        {
            var rootContext = rootContextProvider.GetExpressionContext(document, request);
            var iterator = iteratorFactory.GetContextIterator(contextProcessor);

            await iterator.IterateContextAndChildrenAsync(rootContext, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IterativeDocumentModifier"/> class.
        /// </summary>
        /// <param name="iteratorFactory">Iterator factory.</param>
        /// <param name="rootContextProvider">Root context provider.</param>
        public IterativeDocumentModifier(IGetsIterativeExpressionContextProcessor iteratorFactory,
                                         IGetsRootExpressionContext rootContextProvider)
        {
            this.iteratorFactory = iteratorFactory ?? throw new ArgumentNullException(nameof(iteratorFactory));
            this.rootContextProvider = rootContextProvider ?? throw new ArgumentNullException(nameof(rootContextProvider));
        }
    }
}
