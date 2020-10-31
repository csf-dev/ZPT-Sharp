using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Rendering;

namespace ZptSharp.Metal
{
    public class MetalDocumentModifier : IModifiesDocument
    {
        readonly IGetsIterativeExpressionContextProcessor iteratorFactory;
        readonly IGetsRootExpressionContext rootContextProvider;

        public async Task ModifyDocumentAsync(IDocument document, RenderZptDocumentRequest request, CancellationToken token)
        {
            var rootContext = rootContextProvider.GetExpressionContext(document, request);
            throw new NotImplementedException();

            //var iterator = iteratorFactory.GetContextIterator(/* context iterator here */);
            //await iterator.IterateContextAndChildrenAsync(rootContext).ConfigureAwait(false);
        }

        public MetalDocumentModifier(IGetsIterativeExpressionContextProcessor iteratorFactory,
                                     IGetsRootExpressionContext rootContextProvider)
        {
            this.iteratorFactory = iteratorFactory ?? throw new ArgumentNullException(nameof(iteratorFactory));
            this.rootContextProvider = rootContextProvider ?? throw new ArgumentNullException(nameof(rootContextProvider));
        }
    }
}
