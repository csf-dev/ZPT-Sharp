using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsNamedTalesValue"/> which exposes global variables from the
    /// wrapped <see cref="ExpressionContext"/>.
    /// </summary>
    public class GlobalVariablesOnlyTalesValueForExpressionContextAdapter : IGetsNamedTalesValue
    {
        /// <summary>
        /// Gets the expression context wrapped by the current instance.
        /// </summary>
        /// <value>The expression context.</value>
        public ExpressionContext Context { get; }

        /// <summary>
        /// Attempts to get a value for a named reference, relative to the current instance.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, CancellationToken cancellationToken = default)
        {
            if (Context.GlobalDefinitions.ContainsKey(name))
                return Task.FromResult(GetValueResult.For(Context.GlobalDefinitions[name]));

            return Task.FromResult(GetValueResult.Failure);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GlobalVariablesOnlyTalesValueForExpressionContextAdapter"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public GlobalVariablesOnlyTalesValueForExpressionContextAdapter(ExpressionContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
