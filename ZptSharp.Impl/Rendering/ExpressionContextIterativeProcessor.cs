using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// An implementation of <see cref="IIterativelyProcessesExpressionContexts"/> which receives its
    /// <see cref="IProcessesExpressionContext"/> from the constructor.
    /// See also: <seealso cref="IGetsIterativeExpressionContextProcessor"/> for a factory which may be
    /// used to create instances of this type.
    /// </summary>
    public class ExpressionContextIterativeProcessor : IIterativelyProcessesExpressionContexts
    {
        readonly IProcessesExpressionContext contextProcessor;
        readonly IGetsChildExpressionContexts childContextProvider;

        /// <summary>
        /// Iterate over the specified <paramref name="context"/>, as well as all of its children.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method uses an "open list" to store the contexts which are due to be processed; essentially a
        /// list of the contexts which must still be processed.  After processing each context, further contexts
        /// may be added to the open list. There are two ways in which we find further contexts:
        /// </para>
        /// <list type="bullet">
        /// <item>Child contexts of the currently-processed context</item>
        /// <item>Additional contexts which have been created by the processing</item>
        /// </list>
        /// <para>
        /// Conceptually, as an example, child contexts refer to child DOM elements.  Additional contexts are
        /// those which result from a processing operation.  For example a 'repetition' operation might create
        /// several copies of a context, each one containing an iteration of a collection item from the model.
        /// </para>
        /// </remarks>
        /// <returns>A task indicating when processing is complete.</returns>
        /// <param name="context">The context over which to iterate.</param>
        public Task IterateContextAndChildrenAsync(ExpressionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return IterateContextAndChildrenPrivateAsync(context);
        }

        async Task IterateContextAndChildrenPrivateAsync(ExpressionContext context)
        {
            ExpressionContext currentContext;
            for (var openList = new List<ExpressionContext> { context };
                 (currentContext = openList.FirstOrDefault()) != null;
                 openList.Remove(currentContext))
            {
                var iterationResult = await contextProcessor.ProcessContextAsync(currentContext)
                    .ConfigureAwait(false);

                AddExtraContextsToBeginningOfOpenList(openList, currentContext, iterationResult);
            }
        }

        /// <summary>
        /// Adds extra contexts (child and/or additional contexts) to the beginning of the open list.
        /// The processing of these extra contexts should always occur before any contexts which were
        /// already scheduled in the open list.  Of those, child contexts always come first, with
        /// additional contexts (essentially, further generated siblings) coming next.
        /// </summary>
        /// <param name="openList">The open list.</param>
        /// <param name="context">The context which was just processed.</param>
        /// <param name="processingResult">The processing result.</param>
        void AddExtraContextsToBeginningOfOpenList(List<ExpressionContext> openList,
                                                   ExpressionContext context,
                                                   ExpressionContextProcessingResult processingResult)
        {
            var childContexts = childContextProvider.GetChildContexts(context) ?? Enumerable.Empty<ExpressionContext>();
            var additionalContexts = processingResult?.AdditionalContexts ?? Enumerable.Empty<ExpressionContext>();

            openList.InsertRange(0, additionalContexts);

            if(!processingResult.DoNotProcessChildren)
                openList.InsertRange(0, childContexts);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionContextIterativeProcessor"/> class.
        /// </summary>
        /// <param name="contextProcessor">Context processor.</param>
        /// <param name="childContextProvider">Child context provider.</param>
        public ExpressionContextIterativeProcessor(IProcessesExpressionContext contextProcessor,
                                         IGetsChildExpressionContexts childContextProvider)
        {
            this.contextProcessor = contextProcessor ?? throw new ArgumentNullException(nameof(contextProcessor));
            this.childContextProvider = childContextProvider ?? throw new ArgumentNullException(nameof(childContextProvider));
        }
    }
}
