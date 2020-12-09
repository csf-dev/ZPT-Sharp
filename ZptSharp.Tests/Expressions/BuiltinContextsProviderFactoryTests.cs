using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class BuiltinContextsProviderFactoryTests
    {
        [Test, AutoMoqData]
        public void GetBuiltinContextsProvider_returns_context_from_config_it_exists_there([NoAutoProperties] ExpressionContext context,
                                                                                           [MockedConfig] RenderingConfig config,
                                                                                           IGetsDictionaryOfNamedTalesValues contextProvider,
                                                                                           BuiltinContextsProviderFactory sut)
        {
            Mock.Get(config).SetupGet(x => x.BuiltinContextsProvider).Returns(() => c => contextProvider);
            var result = sut.GetBuiltinContextsProvider(context, config);
            Assert.That(result, Is.SameAs(contextProvider));
        }

        [Test, AutoMoqData]
        public void GetBuiltinContextsProvider_creates_instance_if_not_available_on_config([NoAutoProperties] ExpressionContext context,
                                                                                           [MockedConfig] RenderingConfig config,
                                                                                           IGetsDictionaryOfNamedTalesValues contextProvider,
                                                                                           BuiltinContextsProviderFactory sut)
        {
            Assert.That(() => sut.GetBuiltinContextsProvider(context, config), Is.InstanceOf<BuiltinContextsProvider>());
        }
    }
}
