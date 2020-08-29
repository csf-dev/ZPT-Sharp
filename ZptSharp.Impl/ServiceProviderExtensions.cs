using System;
namespace ZptSharp
{
    /// <summary>
    /// Extension methods for an <see cref="IServiceProvider"/>.
    /// </summary>
    internal static class ServiceProviderExtensions
    {
        /// <summary>
        /// Resolves a typed service by generic type parameter.
        /// </summary>
        /// <returns>The resolved service.</returns>
        /// <param name="provider">A service provider.</param>
        /// <typeparam name="T">The type of service to resolve.</typeparam>
        internal static T Resolve<T>(this IServiceProvider provider) where T : class
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            return (T) provider.GetService(typeof(T));
        }
    }
}
