using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;
using ZptSharp.Config;

namespace ZptSharp.Autofixture
{
    public class DefaultConfigAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new DefaultConfigCustomization();

        public class DefaultConfigCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<RenderingConfig>(x => x.FromFactory(new RenderingConfig.Builder().GetConfig));
            }
        }
    }
}
