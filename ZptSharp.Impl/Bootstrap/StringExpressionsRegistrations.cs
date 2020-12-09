using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions.StringExpressions;

namespace ZptSharp.Bootstrap
{
    class StringExpressionsRegistrations
    {
        internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<StringExpressionEvaluator>();
        }
    }
}
