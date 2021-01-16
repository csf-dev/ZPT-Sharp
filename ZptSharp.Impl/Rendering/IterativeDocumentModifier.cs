using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A service which which makes use of an <see cref="IIterativelyProcessesExpressionContexts"/>
    /// in order to apply a <see cref="IProcessesExpressionContext"/> to every node exposed by a document.
    /// </summary>
    public class IterativeDocumentModifier : IIterativelyModifiesDocument
    {
        readonly IGetsIterativeExpressionContextProcessor iteratorFactory;

        /// <summary>
        /// Modifies the document using the specified context processor.
        /// </summary>
        /// <param name="rootContext">The root expression context.</param>
        /// <param name="contextProcessor">The processor to use when processing each expression context.</param>
        /// <param name="token">A cancellation token.</param>
        public async Task ModifyDocumentAsync(ExpressionContext rootContext,
                                              IProcessesExpressionContext contextProcessor,
                                              CancellationToken token = default)
        {
            var iterator = iteratorFactory.GetContextIterator(contextProcessor);

            await iterator.IterateContextAndChildrenAsync(rootContext, token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IterativeDocumentModifier"/> class.
        /// </summary>
        /// <param name="iteratorFactory">Iterator factory.</param>
        public IterativeDocumentModifier(IGetsIterativeExpressionContextProcessor iteratorFactory)
        {
            this.iteratorFactory = iteratorFactory ?? throw new ArgumentNullException(nameof(iteratorFactory));
        }
    }
}
