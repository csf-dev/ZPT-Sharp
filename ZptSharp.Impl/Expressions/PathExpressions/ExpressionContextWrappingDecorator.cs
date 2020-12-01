using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using ZptSharp.Expressions;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// A decorator class for the <see cref="IGetsValueFromObject"/> service, which detects
    /// instances of <see cref="ExpressionContext"/> and wraps them with an instance of
    /// <see cref="NamedTalesValueForExpressionContextAdapter"/>, so that subsequent classes in
    /// the decorator/chain may treat them as <see cref="IGetsNamedTalesValue"/>.
    /// </summary>
    public class ExpressionContextWrappingDecorator : IGetsValueFromObject
    {
        readonly RenderingConfig config;
        readonly IGetsBuiltinContextsProvider builtinContextsProviderFactory;
        readonly IGetsValueFromObject wrapped;

        /// <summary>
        /// Returns the result from the wrapped service.  If the <paramref name="object"/> parameter is an
        /// instance of <see cref="ExpressionContext"/> then it is substituted in the call to the wrapped
        /// service with an instance of <see cref="NamedTalesValueForExpressionContextAdapter"/> wrapping
        /// the original context.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The value name.</param>
        /// <param name="object">The object from which to retrieve the value.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, object @object, CancellationToken cancellationToken = default)
        {
            var obj = @object;

            if(obj is ExpressionContext expressionContext)
            {
                obj = new NamedTalesValueForExpressionContextAdapter(expressionContext,
                                                                     config,
                                                                     builtinContextsProviderFactory);
            }

            return wrapped.TryGetValueAsync(name, obj, cancellationToken);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ExpressionContextWrappingDecorator"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="builtinContextsProviderFactory">Builtin contexts provider factory.</param>
        /// <param name="wrapped">Wrapped.</param>
        public ExpressionContextWrappingDecorator(RenderingConfig config,
                                                  IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                  IGetsValueFromObject wrapped)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.builtinContextsProviderFactory = builtinContextsProviderFactory ?? throw new ArgumentNullException(nameof(builtinContextsProviderFactory));
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
