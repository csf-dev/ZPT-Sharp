using System;
using ZptSharp.Config;
using ZptSharp.Metal;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsBuiltinContextsProvider"/> which uses dependency
    /// injection to backfill the other dependencies, as well as the context &amp; config.
    /// This may also return the contexts provider from
    /// <see cref="RenderingConfig.BuiltinContextsProvider"/> if it is not-null.
    /// </summary>
    public class BuiltinContextsProviderFactory : IGetsBuiltinContextsProvider
    {
        readonly IGetsMetalDocumentAdapter metalDocumentAdapterFactory;

        /// <summary>
        /// Gets the builtin contexts provider for the specified context &amp; config.
        /// </summary>
        /// <returns>The builtin contexts provider.</returns>
        /// <param name="context">Expression context.</param>
        /// <param name="config">Rendering configuration.</param>
        public IGetsNamedTalesValue GetBuiltinContextsProvider(ExpressionContext context, RenderingConfig config)
        {
            if (config.BuiltinContextsProvider != null)
                return config.BuiltinContextsProvider(context);

            return new BuiltinContextsProvider(context, config, metalDocumentAdapterFactory);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuiltinContextsProviderFactory"/> class.
        /// </summary>
        /// <param name="metalDocumentAdapterFactory">Metal document adapter factory.</param>
        public BuiltinContextsProviderFactory(IGetsMetalDocumentAdapter metalDocumentAdapterFactory)
        {
            this.metalDocumentAdapterFactory = metalDocumentAdapterFactory ?? throw new ArgumentNullException(nameof(metalDocumentAdapterFactory));
        }
    }
}
