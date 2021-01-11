using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Dom;
using ZptSharp.Hosting;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods for <see cref="IBuildsHostingEnvironment"/> instances.
    /// </summary>
    public static class AngleSharpHostingBuilderExtensions
    {
        /// <summary>
        /// Adds service registrations to the <paramref name="builder"/> in order
        /// to enable reading &amp; writing of AngleSharp documents.
        /// </summary>
        /// <param name="builder">The self-hosting builder.</param>
        /// <returns>The self-hosting builder instance, after setting it up.</returns>
        public static IBuildsHostingEnvironment AddAngleSharpZptDocuments(this IBuildsHostingEnvironment builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.ServiceCollection.AddTransient<AngleSharpDocumentProvider>();
            builder.ServiceRegistry.DocumentProviderTypes.Add(typeof(AngleSharpDocumentProvider));

            return builder;
        }
    }
}
