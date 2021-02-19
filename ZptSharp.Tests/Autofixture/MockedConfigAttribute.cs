using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using ZptSharp.Config;

namespace ZptSharp.Autofixture
{
    public class MockedConfigAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new MockedConfigCustomization();

        public class MockedConfigCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<RenderingConfig>(x => x.FromFactory(() => {
                    return new Mock<RenderingConfig> { CallBase = true }.Object;
                }));
            }
        }
    }
}
