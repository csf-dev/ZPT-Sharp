using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions.StructureExpressions;

namespace ZptSharp.Bootstrap
{
    static class StructureExpressionsRegistrations
    {
        static internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<StructureExpressionEvaluator>();
        }
    }
}
