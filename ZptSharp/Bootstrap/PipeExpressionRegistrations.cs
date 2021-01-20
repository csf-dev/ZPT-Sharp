using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions.PipeExpressions;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the Pipe expressions namespace.
    /// </summary>
    static class PipeExpressionRegistrations
    {
        static internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<PipeExpressionEvaluator>();
            services.AddTransient<IEvaluatesPipeDelegate, PipeDelegateExecutor>();
        }
    }
}