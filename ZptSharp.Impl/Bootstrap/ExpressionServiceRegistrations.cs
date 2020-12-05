using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the Expressions namespace.
    /// </summary>
    class ExpressionServiceRegistrations
    {
        internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IGetsBuiltinContextsProvider, BuiltinContextsProviderFactory>();
            services.AddTransient<IGetsExpressionTypeProvider, ExpressionTypeProviderFactory>();
            services.AddTransient(s => s.GetRequiredService<IGetsExpressionTypeProvider>().GetTypeProvider());
            services.AddSingleton<IRegistersExpressionEvaluator, EvaluatorTypeRegistry>();
            services.AddTransient<IGetsEvaluatorForExpressionType, EvaluatorTypeResolver>();
            services.AddTransient<IEvaluatesExpression, RegistryBasedExpressionEvaluator>();
            services.AddTransient<IRemovesPrefixFromExpression, PrefixExpressionTypeProvider>();
            services.AddTransient<IGetsAllVariablesFromContext, ContextAllVariablesAndValuesProvider>();
        }
    }
}
