using System;
using AutoFixture;
using ZptSharp.Config;

namespace ZptSharp.Autofixture
{
    public class AutoConfigBuilderCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<RenderingConfig.Builder>(x => x.FromFactory(() => RenderingConfig.CreateBuilder()));
        }
    }
}
