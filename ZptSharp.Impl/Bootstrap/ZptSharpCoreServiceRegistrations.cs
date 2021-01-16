using System;
using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Contains all of the service registrations for the ZptSharp core.
    /// That's all of the other classes in this directory.
    /// </summary>
    static internal class ZptSharpCoreServiceRegistrations
    {
        static internal void RegisterServices(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            BulkRenderingRegistrations.RegisterServices(serviceCollection);
            ConfigRegistrations.RegisterServices(serviceCollection);
            DomServiceRegistrations.RegisterServices(serviceCollection);
            ExpressionServiceRegistrations.RegisterServices(serviceCollection);
            LoadExpressionRegistrations.RegisterServices(serviceCollection);
            MetalServiceRegistrations.RegisterServices(serviceCollection);
            NotExpressionsRegistrations.RegisterServices(serviceCollection);
            PathExpressionsRegistrations.RegisterServices(serviceCollection);
            PipeExpressionRegistrations.RegisterServices(serviceCollection);
            RenderingRegistrations.RegisterServices(serviceCollection);
            SourceAnnotationServiceRegistrations.RegisterServices(serviceCollection);
            StringExpressionsRegistrations.RegisterServices(serviceCollection);
            TalServiceRegistrations.RegisterServices(serviceCollection);
        }
    }
}