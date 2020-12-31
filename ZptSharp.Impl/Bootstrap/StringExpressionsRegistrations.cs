using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions.StringExpressions;

namespace ZptSharp.Bootstrap
{
    class StringExpressionsRegistrations
    {
        static internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<StringExpressionEvaluator>();
        }
    }
}
