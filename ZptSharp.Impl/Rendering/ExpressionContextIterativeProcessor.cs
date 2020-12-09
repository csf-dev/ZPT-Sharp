using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public Task IterateContextAndChildrenAsync(ExpressionContext context, CancellationToken token = default)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return IterateContextAndChildrenPrivateAsync(context, token);
        }

        async Task IterateContextAndChildrenPrivateAsync(ExpressionContext context, CancellationToken token)
        {
            ExpressionContext currentContext;
            for (var openList = new List<ExpressionContext> { context };
                 (currentContext = openList.FirstOrDefault()) != null;
                 openList.Remove(currentContext))
            {
                var iterationResult = await GetIterationResultAsync(currentContext, openList, token)
                    .ConfigureAwait(false);

                var processNext = GetFurtherContextsToProcess(currentContext, iterationResult);
                openList.InsertRange(0, processNext);
            }
        }

        /// <summary>
        /// Gets the result for a single iteration, but also attempts to handle an exception, if raised
        /// during processing.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The way this works is that firstly the current context processor is used to try and get a result.
        /// If that works, then that is the result which is returned.  No further work is done.
        /// </para>
        /// <para>
        /// If the current processor raises an exception, then the current context is searched for
        /// implementations of <see cref="ErrorHandlingContext"/> in its <see cref="ExpressionContext.ErrorHandlers"/>
        /// property.  Each of these which is present is used (in turn) in an attempt to handle the error.
        /// </para>
        /// <para>
        /// If the error cannot be handled in this way (no error-handling context in the stack can handle the error)
        /// then the caught exception is rethrown and will exit this class.
        /// </para>
        /// <para>
        /// If the error is handled by a context handler in this way then the result from that error-handler is
        /// returned as the final result of handling.  Additionally, all other <paramref name="contextsToBeProcessed"/>
        /// which include the handler which did in fact handle the error are removed.  This is because - when a context
        /// handles an error, all child contexts are treated as being in an errored-state, thus they are not processed.
        /// </para>
        /// </remarks>
        /// <returns>The iteration result async.</returns>
        /// <param name="currentContext">Current context.</param>
        /// <param name="contextsToBeProcessed">Contexts to be processed.</param>
        /// <param name="token">Token.</param>
        async Task<ExpressionContextProcessingResult> GetIterationResultAsync(ExpressionContext currentContext,
                                                                              List<ExpressionContext> contextsToBeProcessed,
                                                                              CancellationToken token)
        {
            try
            {
                return await contextProcessor.ProcessContextAsync(currentContext)
                    .ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                foreach(var handler in currentContext.ErrorHandlers)
                {
                    var handlerResult = await handler.HandleErrorAsync(ex, token)
                        .ConfigureAwait(false);

                    if (handlerResult.IsSuccess)
                    {
                        contextsToBeProcessed.RemoveAll(x => x.ErrorHandlers.Contains(handler));
                        return handlerResult.Result;
                    }
                }

                throw;
            }
        }

        /// <summary>
        /// Gets extra contexts (child and/or additional contexts) which should be processed next.
        /// </summary>
        /// <param name="context">The context which was just processed.</param>
        /// <param name="processingResult">The processing result.</param>
        List<ExpressionContext> GetFurtherContextsToProcess(ExpressionContext context,
                                                            ExpressionContextProcessingResult processingResult)
        {
            if (processingResult == null)
                throw new ArgumentNullException(nameof(processingResult));

            var output = new List<ExpressionContext>();

            if (!processingResult.DoNotProcessChildren)
            {
                var childContexts = childContextProvider.GetChildContexts(context) ?? Enumerable.Empty<ExpressionContext>();
                PermitCurrentHandlerToHandleChildErrorsWhereApplicable(context, childContexts);
                output.AddRange(childContexts);
            }

            var additionalContexts = processingResult.AdditionalContexts ?? Enumerable.Empty<ExpressionContext>();
            output.AddRange(additionalContexts);

            return output;
        }

        /// <summary>
        /// Configures the collection of <paramref name="childContexts"/> such that the current
        /// context processor may handle errors if they are encountered whilst processing those
        /// child contexts.
        /// </summary>
        /// <param name="currentContext">Current context.</param>
        /// <param name="childContexts">Child contexts.</param>
        void PermitCurrentHandlerToHandleChildErrorsWhereApplicable(ExpressionContext currentContext,
                                                                    IEnumerable<ExpressionContext> childContexts)
        {
            // If the current processor is not an error handler then this is irrelevant
            if (!(contextProcessor is IHandlesProcessingError errorHandler)) return;

            var handlerContext = new ErrorHandlingContext(currentContext, errorHandler);

            foreach (var child in childContexts)
                child.ErrorHandlers.Push(handlerContext);
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
