using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions.StringExpressions;

namespace ZptSharp.Bootstrap
{
    static class StringExpressionsRegistrations
    {
        static internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<StringExpressionEvaluator>();
        }
    }
}
