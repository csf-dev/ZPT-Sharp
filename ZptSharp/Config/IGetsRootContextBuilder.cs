using System;
namespace ZptSharp.Config
{
    /// <summary>
    /// An object which gets a 'configuration callback' of sorts, which may configure/set-up
    /// the root rendering context.  See also: <seealso cref="RenderingConfig.ContextBuilder"/>.
    /// </summary>
    public interface IGetsRootContextBuilder
    {
        /// <summary>
        /// Gets the root context builder callback.
        /// </summary>
        /// <returns>The root context builder.</returns>
        Action<IConfiguresRootContext, IServiceProvider> GetRootContextBuilder();
    }
}
