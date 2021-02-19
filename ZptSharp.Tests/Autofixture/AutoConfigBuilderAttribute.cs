using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;
using ZptSharp.Config;

namespace ZptSharp.Autofixture
{
    public class AutoConfigBuilderAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new AutoConfigBuilderCustomization();
    }
}
