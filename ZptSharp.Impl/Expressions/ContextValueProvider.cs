using System;
using ZptSharp.Config;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// A wrapper/adapter for an instance of <see cref="ExpressionContext"/> which allows it to be used as
    /// an instance of <see cref="IGetsNamedTalesValue"/>.
    /// </summary>
    public class ContextValueProvider : IGetsNamedTalesValue
    {
        /// <summary>
        /// A reserved identifier/alias which indicates that the built-in contexts should be returned.
        /// </summary>
        public static readonly string BuiltinContexts = "CONTEXTS";

        readonly ExpressionContext context;
        readonly RenderingConfig config;
        readonly IGetsBuiltinContextsProvider builtinContextsProviderFactory;

        /// <summary>
        /// Attempts to get a value for a named reference, relative to the current instance.
        /// </summary>
        /// <returns>A boolean indicating whether a value was successfully retrieved or not.</returns>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <param name="value">Exposes the retrieved value if this method returns success.</param>
        public bool TryGetValue(string name, out object value)
        {
            if (String.Equals(name, BuiltinContexts, StringComparison.InvariantCulture))
            {
                value = BuiltInContextsProvider;
                return true;
            }

            if(context.LocalDefinitions.ContainsKey(name))
            {
                value = context.LocalDefinitions[name];
                return true;
            }

            if (context.GlobalDefinitions.ContainsKey(name))
            {
                value = context.GlobalDefinitions[name];
                return true;
            }

            return BuiltInContextsProvider.TryGetValue(name, out value);
        }

        IGetsNamedTalesValue BuiltInContextsProvider
            => builtinContextsProviderFactory.GetBuiltinContextsProvider(context, config);

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextValueProvider"/> class.
        /// </summary>
        /// <param name="context">The expression context for which this provider will operate.</param>
        /// <param name="config">Rendering configuration.</param>
        /// <param name="builtinContextsProviderFactory">Builtin contexts provider factory.</param>
        public ContextValueProvider(ExpressionContext context, RenderingConfig config, IGetsBuiltinContextsProvider builtinContextsProviderFactory)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
            this.config = config ?? throw new System.ArgumentNullException(nameof(config));
            this.builtinContextsProviderFactory = builtinContextsProviderFactory ?? throw new ArgumentNullException(nameof(builtinContextsProviderFactory));
        }
    }
}
