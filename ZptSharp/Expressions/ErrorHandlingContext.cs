using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Rendering;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// A model/contaier object which represents a context and additionally a handler which may
    /// be able to handle errors encountered during context processing.
    /// </summary>
    public class ErrorHandlingContext
    {
        readonly ExpressionContext context;

        /// <summary>
        /// Gets the handler implementation which would handle exceptions.
        /// </summary>
        /// <value>The handler.</value>
        public IHandlesProcessingError Handler { get; }

        /// <summary>
        /// Attempts to handle the error.
        /// </summary>
        /// <returns>The error-handling result.</returns>
        /// <param name="exception">Exception.</param>
        /// <param name="token">Token.</param>
        public Task<ErrorHandlingResult> HandleErrorAsync(Exception exception, CancellationToken token = default)
            => Handler.HandleErrorAsync(exception, context, token);

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlingContext"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="handler">Handler.</param>
        public ErrorHandlingContext(ExpressionContext context, IHandlesProcessingError handler)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }
    }
}
