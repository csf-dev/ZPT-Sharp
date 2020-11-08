using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// A wrapper/adapter for an instance of <see cref="ExpressionContext"/> which allows it to be used as
    /// an instance of <see cref="IGetsNamedTalesValue"/>.
    /// </summary>
    public class NamedTalesValueForExpressionContextAdapter : IGetsNamedTalesValue
    {
        /// <summary>
        /// A reserved identifier/alias which indicates that the built-in contexts should be returned.
        /// </summary>
        public static readonly string ContextsName = "CONTEXTS";

        readonly ExpressionContext context;
        readonly RenderingConfig config;
        readonly IGetsBuiltinContextsProvider builtinContextsProviderFactory;

        /// <summary>
        /// Attempts to get a value for a named reference, relative to the current instance.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, CancellationToken cancellationToken = default)
        {
            if (String.Equals(name, ContextsName, StringComparison.InvariantCulture))
                return Task.FromResult(GetValueResult.For(BuiltinContexts));

            if(context.LocalDefinitions.ContainsKey(name))
                return Task.FromResult(GetValueResult.For(context.LocalDefinitions[name]));

            if (context.GlobalDefinitions.ContainsKey(name))
                return Task.FromResult(GetValueResult.For(context.GlobalDefinitions[name]));

            return BuiltinContexts.TryGetValueAsync(name);
        }

        IGetsNamedTalesValue BuiltinContexts
            => builtinContextsProviderFactory.GetBuiltinContextsProvider(context, config);

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedTalesValueForExpressionContextAdapter"/> class.
        /// </summary>
        /// <param name="context">The expression context for which this provider will operate.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="builtinContextsProviderFactory">Builtin contexts provider factory.</param>
        public NamedTalesValueForExpressionContextAdapter(ExpressionContext context, RenderingConfig config, IGetsBuiltinContextsProvider builtinContextsProviderFactory)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
            this.config = config ?? throw new System.ArgumentNullException(nameof(config));
            this.builtinContextsProviderFactory = builtinContextsProviderFactory ?? throw new ArgumentNullException(nameof(builtinContextsProviderFactory));
        }
    }
}
