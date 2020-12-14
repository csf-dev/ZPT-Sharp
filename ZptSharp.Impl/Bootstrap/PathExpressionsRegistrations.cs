using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions.PathExpressions;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the Path expressions namespace.
    /// </summary>
    class PathExpressionsRegistrations
    {
        internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IGetsValueFromObject, DecoratorBasedObjectValueProvider>();
            services.AddTransient<PathExpressionEvaluator>();
            services.AddTransient<IParsesPathExpression, PathExpressionParser>();
            services.AddTransient<IWalksAndEvaluatesPathExpression, PathWalkingExpressionEvaluator>();
            services.AddTransient<LocalVariablesOnlyPathExpressionEvaluator>();
            services.AddTransient<GlobalVariablesOnlyPathExpressionEvaluator>();
            services.AddTransient<IEvaluatesPathExpressionRequest, PathExpressionRequestEvaluator>();
            services.AddTransient<IGetsPathWalkingExpressionEvaluator, PathWalkingExpressionEvaluatorFactory>();

            // Intentionally no registrations for anything in the ValueProviders sub-namespace.  The
            // DecoratorBasedObjectValueProvider (already registered above) creates new instances of
            // all of those classes manually.
        }
    }
}
