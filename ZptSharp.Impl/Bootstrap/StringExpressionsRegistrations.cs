using Microsoft.Extensions.DependencyInjection;
using ZptSharp.StringExpressions;

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
