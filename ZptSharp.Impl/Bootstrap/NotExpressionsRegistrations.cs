using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Expressions.NotExpressions;

namespace ZptSharp.Bootstrap
{
    static class NotExpressionsRegistrations
    {
        static internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<NotExpressionEvaluator>();
            services.AddTransient<ICoercesValueToBoolean, BooleanValueConverter>();
        }
    }
}
