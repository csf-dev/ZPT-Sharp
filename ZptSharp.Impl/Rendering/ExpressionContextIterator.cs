using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// An implementation of <see cref="IIteratesExpressionContexts"/> which receives its
    /// <see cref="IProcessesExpressionContext"/> from the constructor.
    /// See also: <seealso cref="IGetsExpressionContextIterator"/> for a factory which may be
    /// used to create instances of this type.
    /// </summary>
    public class ExpressionContextIterator : IIteratesExpressionContexts
    {
        readonly IProcessesExpressionContext contextProcessor;
        readonly IGetsChildExpressionContexts childContextProvider;

        /// <summary>
        /// Iterate over the specified <paramref name="context"/>, as well as all of its children.
        /// </summary>
        /// <returns>A task indicating when processing is complete.</returns>
        /// <param name="context">The context over which to iterate.</param>
        public async Task IterateContextAndChildrenAsync(ExpressionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            List<ExpressionContext> openList;
            ExpressionContext currentContext;
            for (openList = new List<ExpressionContext> { context }, currentContext = openList.First();
                 (currentContext = openList.FirstOrDefault()) != null;
                 openList.Remove(currentContext))
            {
                var iterationResult = await contextProcessor.ProcessContextAsync(currentContext).ConfigureAwait(false);

                AddChildContextsToOpenList(openList, currentContext);
                AddAdditionalContextsToOpenList(openList, iterationResult);
            }
        }

        void AddChildContextsToOpenList(List<ExpressionContext> openList, ExpressionContext context)
        {
            var childContexts = childContextProvider.GetChildContexts(context) ?? Enumerable.Empty<ExpressionContext>();
            openList.AddRange(childContexts);
        }

        void AddAdditionalContextsToOpenList(List<ExpressionContext> openList, ExpressionContextProcessingResult result)
        {
            var additionalContexts = result?.AdditionalContexts ?? Enumerable.Empty<ExpressionContext>();
            openList.AddRange(additionalContexts);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionContextIterator"/> class.
        /// </summary>
        /// <param name="contextProcessor">Context processor.</param>
        /// <param name="childContextProvider">Child context provider.</param>
        public ExpressionContextIterator(IProcessesExpressionContext contextProcessor,
                                         IGetsChildExpressionContexts childContextProvider)
        {
            this.contextProcessor = contextProcessor ?? throw new ArgumentNullException(nameof(contextProcessor));
            this.childContextProvider = childContextProvider ?? throw new ArgumentNullException(nameof(childContextProvider));
        }
    }
}
