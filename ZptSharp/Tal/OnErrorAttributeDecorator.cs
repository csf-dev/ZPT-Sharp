﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    /// <summary>
    /// <para>
    /// Decorator for <see cref="IHandlesProcessingError"/> which handles TAL 'on-error' attributes.
    /// </para>
    /// <para>
    /// This decorator is a little unusual (compared to the others) in that it really just puts a <c>try/catch</c>
    /// around whatever it wraps.  If there is an error and there is a TAL on-error attribute present upon the node
    /// then this decorator will handle the error and prevent the error from halting the whole rendering process.
    /// </para>
    /// </summary>
    public class OnErrorAttributeDecorator : IHandlesProcessingError
    {
        readonly IHandlesProcessingError wrapped;
        readonly IGetsTalAttributeSpecs specProvider;
        readonly IEvaluatesDomValueExpression evaluator;
        readonly ILogger logger;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public async Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            try
            {
                return await wrapped.ProcessContextAsync(context, token).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                var errorHandlingResult = await HandleErrorPrivateAsync(ex, context, true, token).ConfigureAwait(false);
                if (!errorHandlingResult.IsSuccess) throw;
                return errorHandlingResult.Result;
            }
        }

        /// <summary>
        /// Handles an error which was encountered whilst processing an expression context.
        /// </summary>
        /// <returns>A result object indicating the outcome of error handling.</returns>
        /// <param name="ex">The exception indicating the error.</param>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public Task<ErrorHandlingResult> HandleErrorAsync(Exception ex, ExpressionContext context, CancellationToken token = default)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return HandleErrorPrivateAsync(ex, context, false, token);
        }

        async Task<ErrorHandlingResult> HandleErrorPrivateAsync(Exception ex,
                                                                ExpressionContext context,
                                                                bool rethrow,
                                                                CancellationToken token)
        {
            var attribute = GetOnErrorAttribute(context);
            if (attribute == null)
                return ErrorHandlingResult.Failure;

            context.Error = new OnErrorExceptionAdapter(ex);

            try
            {
                var result = await ProcessErroredContextAsync(context, ex, attribute.Value, token)
                    .ConfigureAwait(false);

                return ErrorHandlingResult.Success(result);
            }
            catch(OnErrorHandlingException handlingException)
            {
                if(logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation(@"Whilst attempting to handle a ZPT rendering error, a further error was raised.
Original exception
------------------
{original_exception}

Exception raised whilst trying to handle that
---------------------------------------------
{new_exception}",
                                          ex, handlingException);
                }

                if(rethrow) throw;

                return ErrorHandlingResult.Failure;
            }
        }

        IAttribute GetOnErrorAttribute(ExpressionContext context)
            => context.CurrentNode.GetMatchingAttribute(specProvider.OnError);

        async Task<ExpressionContextProcessingResult> ProcessErroredContextAsync(ExpressionContext context,
                                                                                 Exception originalError,
                                                                                 string onErrorExpression,
                                                                                 CancellationToken token)
        {
            LogOriginalError(originalError, context.CurrentNode);

            DomValueExpressionResult onErrorExpressionResult;

            try
            {
                onErrorExpressionResult = await evaluator.EvaluateExpressionAsync(onErrorExpression, context, token)
                    .ConfigureAwait(false);
            }
            catch(Exception errorWhilstHandlingError)
            {
                LogErrorWhilstHandlingError(errorWhilstHandlingError, originalError, context.CurrentNode);

                var message = String.Format(Resources.ExceptionMessage.ErrorWhilstHandlingError, context.CurrentNode);
                throw new OnErrorHandlingException(message, errorWhilstHandlingError)
                {
                    OriginalException = originalError
                };
            }

            if(onErrorExpressionResult.AbortAction)
                return ExpressionContextProcessingResult.Noop;

            context.CurrentNode.ChildNodes.Clear();
            context.CurrentNode.AddChildren(onErrorExpressionResult.Nodes);

            return ExpressionContextProcessingResult.Noop;
        }

        void LogOriginalError(Exception originalError, INode node)
        {
            if (!logger.IsEnabled(LogLevel.Debug)) return;

            logger.LogDebug(@"A TAL on-error attribute is suppressing the following exception whilst processing {node}:
{exception}",
                            node,
                            originalError);
        }

        void LogErrorWhilstHandlingError(Exception errorWhilstHandlingError, Exception originalError, INode node)
        {
            if (!logger.IsEnabled(LogLevel.Error)) return;

            logger.LogError(@"A TAL on-error attribute encountered an exception whilst attempting to deal with a previous error.
           Node: {node}
Previous exception: {original_exception}
         Exception: {exception}",
                            node,
                            originalError,
                            errorWhilstHandlingError);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OnErrorAttributeDecorator"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="evaluator">Evaluator.</param>
        /// <param name="logger">Logger.</param>
        public OnErrorAttributeDecorator(IHandlesProcessingError wrapped,
                                         IGetsTalAttributeSpecs specProvider,
                                         IEvaluatesDomValueExpression evaluator,
                                         ILogger<OnErrorAttributeDecorator> logger)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
