using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Tal;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the TAL namespace.
    /// </summary>
    class TalServiceRegistrations
    {
        internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IGetsTalContextProcessor, TalContextProcessorFactory>();
            services.AddTransient<IGetsTalAttributeSpecs, TalAttributeSpecProvider>();
            services.AddTransient<IGetsAttributeDefinitions, AttributeDefinitionsProvider>();
            services.AddTransient<IEvaluatesDomValueExpression, DomExpressionEvaluator> ();
            services.AddTransient<IGetsRepetitionContexts, RepetitionContextProvider>();
            services.AddTransient<IInterpretsExpressionResult, TalExpressionResultInterpreter>();
            services.AddTransient<IGetsVariableDefinitionsFromAttributeValue, VariableDefinitionProvider>();
        }
    }
}
