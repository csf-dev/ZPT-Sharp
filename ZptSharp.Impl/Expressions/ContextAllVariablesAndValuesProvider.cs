using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// <para>
    /// Implementation of <see cref="IGetsAllVariablesFromContext"/> which prefers definitions in the order:
    /// </para>
    /// <list type="number">
    /// <item>Local definitions: <see cref="ExpressionContext.LocalDefinitions"/></item>
    /// <item>Global definitions: <see cref="ExpressionContext.GlobalDefinitions"/></item>
    /// <item>Built-in definitions: <see cref="IGetsBuiltinContextsProvider"/></item>
    /// </list>
    /// </summary>
    public class ContextAllVariablesAndValuesProvider : IGetsAllVariablesFromContext
    {
        readonly IGetsBuiltinContextsProvider builtinContextsProviderFactory;
        readonly Config.RenderingConfig config;

        /// <summary>
        /// Gets all of the variables &amp; corresponding values from the specified context.
        /// </summary>
        /// <returns>A dictionary of variables and values.</returns>
        /// <param name="context">An expression context.</param>
        public Task<IDictionary<string, object>> GetAllVariablesAsync(ExpressionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return GetAllVariablesPrivateAsync(context);
        }

        async Task<IDictionary<string, object>> GetAllVariablesPrivateAsync(ExpressionContext context)
        {
            var builtInContexts = builtinContextsProviderFactory.GetBuiltinContextsProvider(context, config);
            var builtInDefinitions = await builtInContexts.GetAllNamedValues();

            return MergeDictionariesWithoutOverwriting(context.LocalDefinitions,
                                                       context.GlobalDefinitions,
                                                       builtInDefinitions);
        }

        static IDictionary<string, object> MergeDictionariesWithoutOverwriting(params IDictionary<string,object>[] dictionaries)
        {
            var output = new Dictionary<string, object>();

            foreach (var dictionary in dictionaries)
                output = output.Union(dictionary.Where(x => !output.ContainsKey(x.Key))).ToDictionary(k => k.Key, v => v.Value);

            return output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextAllVariablesAndValuesProvider"/> class.
        /// </summary>
        /// <param name="builtinContextsProviderFactory">Builtin contexts provider factory.</param>
        /// <param name="config">Config.</param>
        public ContextAllVariablesAndValuesProvider(IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                    Config.RenderingConfig config)
        {
            this.builtinContextsProviderFactory = builtinContextsProviderFactory ?? throw new ArgumentNullException(nameof(builtinContextsProviderFactory));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }
    }
}
