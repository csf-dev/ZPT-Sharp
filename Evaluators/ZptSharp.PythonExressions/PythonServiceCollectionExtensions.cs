using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions;
using ZptSharp.Expressions.PythonExpressions;
using ZptSharp.Hosting;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods related to the use of ZPT Python expressions.
    /// </summary>
    public static class PythonServiceCollectionExtensions
    {
        /// <summary>
        /// Adds service registrations to the <paramref name="serviceCollection"/> in order
        /// to enable the use of ZPT "python" expressions.
        /// </summary>
        /// <returns>The same service collection instance, with registrations added.</returns>
        /// <param name="serviceCollection">A service collection to which registrations will be added.</param>
        public static IServiceCollection AddZptPythonExpressions(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddSingleton(typeof(IGetsScriptEngine), s => new IronPythonScriptEngineContainer());
            serviceCollection.AddTransient<IEvaluatesPythonExpression, ScriptEngineEvaluator>();
            serviceCollection.AddTransient<PythonExpressionEvaluator>();
            serviceCollection.AddTransient<IGetsClassDefinitionScript, ClassDefinitionScriptFactory>();

            return serviceCollection;
        }

        /// <summary>
        /// Adds both service registrations and a usage-callback so that ZPT "python" expressions may be used.
        /// </summary>
        /// <param name="builder">The self-hosting builder.</param>
        /// <returns>The self-hosting builder instance, after setting it up.</returns>
        public static IBuildsSelfHostingEnvironment AddZptPythonExpressions(this IBuildsSelfHostingEnvironment builder)
        {
            builder.ServiceRegistrations.Add(s => s.AddZptPythonExpressions());
            builder.ServiceUsages.Add(p => p.UseZptPythonExpressions());
            return builder;
        }

        /// <summary>
        /// Configures ZPT Sharp to read and handle TALES "python" expressions.
        /// </summary>
        /// <returns>The same service provider instance, after setting it up.</returns>
        /// <param name="provider">The service provider.</param>
        public static IServiceProvider UseZptPythonExpressions(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            provider
                .GetRequiredService<IRegistersExpressionEvaluator>()
                .RegisterEvaluatorType(typeof(PythonExpressionEvaluator), PythonExpressionEvaluator.ExpressionPrefix);

            return provider;
        }
    }
}
