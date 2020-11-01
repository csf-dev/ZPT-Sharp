using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    public class IterativeDocumentModifier : IIterativelyModifiesDocument
    {
        readonly IGetsIterativeExpressionContextProcessor iteratorFactory;
        readonly IGetsRootExpressionContext rootContextProvider;

        public async Task ModifyDocumentAsync(IDocument document,
                                              RenderZptDocumentRequest request,
                                              IProcessesExpressionContext contextProcessor,
                                              CancellationToken token = default)
        {
            var rootContext = rootContextProvider.GetExpressionContext(document, request);
            var iterator = iteratorFactory.GetContextIterator(contextProcessor);

            await iterator.IterateContextAndChildrenAsync(rootContext).ConfigureAwait(false);
        }

        public IterativeDocumentModifier(IGetsIterativeExpressionContextProcessor iteratorFactory,
                                         IGetsRootExpressionContext rootContextProvider)
        {
            this.iteratorFactory = iteratorFactory ?? throw new ArgumentNullException(nameof(iteratorFactory));
            this.rootContextProvider = rootContextProvider ?? throw new ArgumentNullException(nameof(rootContextProvider));
        }
    }
}
