using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions;
using ZptSharp.Expressions.CSharpExpressions;
using ZptSharp.Hosting;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods related to the use of ZPT Python expressions.
    /// </summary>
    public static class CSharpServiceCollectionExtensions
    {
        /// <summary>
        /// Adds service registrations to the <paramref name="serviceCollection"/> in order
        /// to enable the use of ZPT "csharp" expressions.
        /// </summary>
        /// <returns>The same service collection instance, with registrations added.</returns>
        /// <param name="serviceCollection">A service collection to which registrations will be added.</param>
        public static IServiceCollection AddZptCSharpExpressions(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddSingleton<ICachesCSharpExpressions, ExpressionCache>();
            serviceCollection.AddSingleton<CSharpExpressionEvaluator>();
            serviceCollection.AddSingleton<IGetsExpressionDescription, ExpressionDescriptionFactory>();
            serviceCollection.AddSingleton<ICreatesCSharpExpressions, ExpressionCompiler>();

            serviceCollection.AddTransient<AssemblyReferenceEvaluator>();
            serviceCollection.AddTransient<UsingNamespaceEvaluator>();
            serviceCollection.AddTransient<VariableTypeEvaluator>();

            return serviceCollection;
        }

        /// <summary>
        /// Adds both service registrations and a usage-callback so that ZPT "csharp" expressions may be used.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is actually a few expression types:
        /// </para>
        /// <list type="bullet">
        /// <item><c>csharp</c> expressions</item>
        /// <item><c>assemblyref</c> expressions</item>
        /// <item><c>using</c> expressions</item>
        /// <item><c>type</c> expressions</item>
        /// </list>
        /// </remarks>
        /// <param name="builder">The self-hosting builder.</param>
        /// <param name="configAction">An optional action permitting some global configuration of the C# expression evaluation.</param>
        /// <returns>The self-hosting builder instance, after setting it up.</returns>
        public static IBuildsSelfHostingEnvironment AddZptCSharpExpressions(this IBuildsSelfHostingEnvironment builder,
                                                                            Action<IConfiguresCSharpExpressionGlobals> configAction = null)
        {
            builder.ServiceRegistrations.Add(s => s.AddZptCSharpExpressions());
            builder.ServiceUsages.Add(p => p.UseZptCSharpExpressions(configAction));
            return builder;
        }

        /// <summary>
        /// Configures ZPT Sharp to read and handle TALES "csharp" expressions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is actually a few expression types:
        /// </para>
        /// <list type="bullet">
        /// <item><c>csharp</c> expressions</item>
        /// <item><c>assemblyref</c> expressions</item>
        /// <item><c>using</c> expressions</item>
        /// <item><c>type</c> expressions</item>
        /// </list>
        /// </remarks>
        /// <returns>The same service provider instance, after setting it up.</returns>
        /// <param name="provider">The service provider.</param>
        /// <param name="configAction">An optional action permitting some global configuration of the C# expression evaluation.</param>
        public static IServiceProvider UseZptCSharpExpressions(this IServiceProvider provider,
                                                               Action<IConfiguresCSharpExpressionGlobals> configAction = null)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            var registry = provider.GetRequiredService<IRegistersExpressionEvaluator>();
            registry.RegisterEvaluatorType(typeof(AssemblyReferenceEvaluator), AssemblyReferenceEvaluator.ExpressionPrefix);
            registry.RegisterEvaluatorType(typeof(UsingNamespaceEvaluator), UsingNamespaceEvaluator.ExpressionPrefix);
            registry.RegisterEvaluatorType(typeof(VariableTypeEvaluator), VariableTypeEvaluator.ExpressionPrefix);
            registry.RegisterEvaluatorType(typeof(CSharpExpressionEvaluator), CSharpExpressionEvaluator.ExpressionPrefix);

            configAction?.Invoke(provider.GetRequiredService<CSharpExpressionEvaluator>());

            return provider;
        }
    }
}
