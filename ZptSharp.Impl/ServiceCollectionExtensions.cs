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
        /// <para>
        /// Configures ZptSharp to use the standard/out-of-the-box expressions which are shipped with this package.
        /// This is equivalent to using all of the following methods:
        /// </para>
        /// <list type="bullet">
        /// <item><see cref="UseZptPathExpressions"/></item>
        /// <item><see cref="UseZptStringExpressions"/></item>
        /// <item><see cref="UseZptNotExpressions"/></item>
        /// </list>
        /// </summary>
        /// <returns>The same service provider instance, after setting it up.</returns>
        /// <param name="provider">The service provider.</param>
        public static IServiceProvider UseStandardZptExpressions(this IServiceProvider provider)
        {
            return provider
                .UseZptPathExpressions()
                .UseZptStringExpressions()
                .UseZptNotExpressions();
        }

        /// <summary>
        /// Configures ZptSharp to read and handle TALES "path" expressions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method registers all four of:
        /// </para>
        /// <list type="bullet">
        /// <item><see cref="Expressions.PathExpressions.PathExpressionEvaluator"/>, using the prefix <see cref="WellKnownExpressionPrefix.Path"/>.</item>
        /// <item><see cref="Expressions.PathExpressions.LocalVariablesOnlyPathExpressionEvaluator"/>, using the prefix <see cref="WellKnownExpressionPrefix.LocalVariablePath"/>.</item>
        /// <item><see cref="Expressions.PathExpressions.GlobalVariablesOnlyPathExpressionEvaluator"/>, using the prefix <see cref="WellKnownExpressionPrefix.GlobalVariablePath"/>.</item>
        /// <item><see cref="Expressions.PathExpressions.DefinedVariablesOnlyPathExpressionEvaluator"/>, using the prefix <see cref="WellKnownExpressionPrefix.DefinedVariablePath"/>.</item>
        /// </list>
        /// </remarks>
        /// <returns>The same service provider instance, after setting it up.</returns>
        /// <param name="provider">The service provider.</param>
        public static IServiceProvider UseZptPathExpressions(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            provider
                .GetRequiredService<IRegistersExpressionEvaluator>()
                .RegisterEvaluatorType<Expressions.PathExpressions.PathExpressionEvaluator>(WellKnownExpressionPrefix.Path);
            provider
                .GetRequiredService<IRegistersExpressionEvaluator>()
                .RegisterEvaluatorType<Expressions.PathExpressions.LocalVariablesOnlyPathExpressionEvaluator>(WellKnownExpressionPrefix.LocalVariablePath);
            provider
                .GetRequiredService<IRegistersExpressionEvaluator>()
                .RegisterEvaluatorType<Expressions.PathExpressions.GlobalVariablesOnlyPathExpressionEvaluator>(WellKnownExpressionPrefix.GlobalVariablePath);
            provider
                .GetRequiredService<IRegistersExpressionEvaluator>()
                .RegisterEvaluatorType<Expressions.PathExpressions.DefinedVariablesOnlyPathExpressionEvaluator>(WellKnownExpressionPrefix.DefinedVariablePath);

            return provider;
        }

        /// <summary>
        /// Configures ZptSharp to read and handle TALES "string" expressions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method registers the <see cref="Expressions.StringExpressions.StringExpressionEvaluator"/> under both of the
        /// prefixes <see cref="WellKnownExpressionPrefix.String"/> &amp; <see cref="WellKnownExpressionPrefix.ShortStringAlias"/>.
        /// </para>
        /// </remarks>
        /// <returns>The same service provider instance, after setting it up.</returns>
        /// <param name="provider">The service provider.</param>
        public static IServiceProvider UseZptStringExpressions(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            provider
                .GetRequiredService<IRegistersExpressionEvaluator>()
                .RegisterEvaluatorType<Expressions.StringExpressions.StringExpressionEvaluator>(WellKnownExpressionPrefix.String);
            provider
                .GetRequiredService<IRegistersExpressionEvaluator>()
                .RegisterEvaluatorType<Expressions.StringExpressions.StringExpressionEvaluator>(WellKnownExpressionPrefix.ShortStringAlias);

            return provider;
        }

        /// <summary>
        /// Configures ZptSharp to read and handle TALES "not" expressions.
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
