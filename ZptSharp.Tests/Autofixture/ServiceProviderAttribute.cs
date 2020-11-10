using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ZptSharp.Autofixture
{
    public class ServiceProviderAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new ServiceProviderCustomization();

        public class ServiceProviderCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<IServiceProvider>(c => c.FromFactory((IServiceScope scope) => {
                    var provider = new Mock<IServiceProvider>();
                    var scopeFactory = provider.As<IServiceScopeFactory>();

                    provider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(() => scopeFactory.Object);
                    scopeFactory.Setup(x => x.CreateScope()).Returns(scope);
                    Mock.Get(scope).SetupGet(x => x.ServiceProvider).Returns(() => provider.Object);

                    return provider.Object;
                }));
            }
        }
    }
}
