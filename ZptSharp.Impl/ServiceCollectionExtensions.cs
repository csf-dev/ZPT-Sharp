using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Bootstrap;
using ZptSharp.Expressions;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> instances.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds service registrations to the <paramref name="serviceCollection"/> in order
        /// to enable the basic/core ZPT-Sharp functionality.
        /// </summary>
        /// <returns>The same service collection instance, with registrations added.</returns>
        /// <param name="serviceCollection">A service collection to which registrations will be added.</param>
        public static IServiceCollection AddZptSharp(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            new ConfigRegistrations().RegisterServices(serviceCollection);
            new DomServiceRegistrations().RegisterServices(serviceCollection);
            new ExpressionServiceRegistrations().RegisterServices(serviceCollection);
            new MetalServiceRegistrations().RegisterServices(serviceCollection);
            new NotExpressionsRegistrations().RegisterServices(serviceCollection);
            new PathExpressionsRegistrations().RegisterServices(serviceCollection);
            new RenderingRegistrations().RegisterServices(serviceCollection);
            new SourceAnnotationServiceRegistrations().RegisterServices(serviceCollection);
            new StringExpressionsRegistrations().RegisterServices(serviceCollection);
            new TalServiceRegistrations().RegisterServices(serviceCollection);

            return serviceCollection;
        }

        /// <summary>
        /// Configures ZPT Sharp to read and handle TALES "path" expressions.
        /// </summary>
        /// <returns>The same service provider instance, after setting it up.</returns>
        /// <param name="provider">The service provider.</param>
        public static IServiceProvider UseZptPathExpressions(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            provider
                .GetRequiredService<IRegistersExpressionEvaluator>()
                .RegisterEvaluatorType<Expressions.PathExpressions.PathExpressionEvaluator>(WellKnownExpressionPrefix.Path);

            return provider;
        }

        /// <summary>
        /// Configures ZPT Sharp to read and handle TALES "string" expressions.
        /// </summary>
        /// <returns>The same service provider instance, after setting it up.</returns>
        /// <param name="provider">The service provider.</param>
        public static IServiceProvider UseZptStringExpressions(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            provider
                .GetRequiredService<IRegistersExpressionEvaluator>()
                .RegisterEvaluatorType<Expressions.StringExpressions.StringExpressionEvaluator>(WellKnownExpressionPrefix.String);

            return provider;
        }

        /// <summary>
        /// Configures ZPT Sharp to read and handle TALES "not" expressions.
        /// </summary>
        /// <returns>The same service provider instance, after setting it up.</returns>
        /// <param name="provider">The service provider.</param>
        public static IServiceProvider UseZptNotExpressions(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            provider
                .GetRequiredService<IRegistersExpressionEvaluator>()
                .RegisterEvaluatorType<Expressions.NotExpressions.NotExpressionEvaluator>(WellKnownExpressionPrefix.Not);

            return provider;
        }
    }
}
