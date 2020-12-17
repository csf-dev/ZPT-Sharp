using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions;
using ZptSharp.Expressions.PythonExpressions;

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
