using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions.LoadExpressions;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the Pipe expressions namespace.
    /// </summary>
    static class LoadExpressionRegistrations
    {
        static internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<LoadExpressionEvaluator>();
            services.AddTransient<IRendersLoadedObject, LoadedObjectRenderer>();
        }
    }
}