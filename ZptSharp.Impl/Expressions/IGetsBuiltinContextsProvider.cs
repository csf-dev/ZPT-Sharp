using System;
using ZptSharp.Config;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which gets an instance of <see cref="BuiltinContextsProvider"/> for a specified
    /// expression context and rendering configuration.
    /// </summary>
    public interface IGetsBuiltinContextsProvider
    {
        /// <summary>
        /// Gets the builtin contexts provider for the specified context &amp; config.
        /// </summary>
        /// <returns>The builtin contexts provider.</returns>
        /// <param name="context">Expression context.</param>
        /// <param name="config">Rendering configuration.</param>
        IGetsNamedTalesValue GetBuiltinContextsProvider(ExpressionContext context, RenderingConfig config);
    }
}
