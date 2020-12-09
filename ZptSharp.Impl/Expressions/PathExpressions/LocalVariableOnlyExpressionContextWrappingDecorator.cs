using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsValueFromObject"/> which uses
    /// only local variables to resolve values from the expression context.
    /// Similar to <seealso cref="ExpressionContextWrappingDecorator"/>.
    /// </summary>
    public class LocalVariableOnlyExpressionContextWrappingDecorator : IGetsValueFromObject
    {
        readonly IGetsValueFromObject wrapped;

        /// <summary>
        /// Returns the result from the wrapped service.  If the <paramref name="object"/> parameter is an
        /// instance of <see cref="ExpressionContext"/> then it is substituted in the call to the wrapped
        /// service with an instance of <see cref="LocalVariablesOnlyTalesValueForExpressionContextAdapter"/> wrapping
        /// the original context.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The value name.</param>
        /// <param name="object">The object from which to retrieve the value.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, object @object, CancellationToken cancellationToken = default)
        {
            var obj = @object;

            if (obj is ExpressionContext expressionContext)
                obj = new LocalVariablesOnlyTalesValueForExpressionContextAdapter(expressionContext);

            return wrapped.TryGetValueAsync(name, obj, cancellationToken);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="LocalVariableOnlyExpressionContextWrappingDecorator"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        public LocalVariableOnlyExpressionContextWrappingDecorator(IGetsValueFromObject wrapped)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
