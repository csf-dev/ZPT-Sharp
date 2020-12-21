using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class ConfiguredDefaultExpressionTypeDecoratorTests
    {
        [Test, AutoMoqData]
        public void GetExpressionType_returns_result_from_wrapped_service_when_not_null([Frozen] IGetsExpressionType wrapped,
                                                                                        [MockedConfig,Frozen] RenderingConfig config,
                                                                                        ConfiguredDefaultExpressionTypeDecorator sut,
                                                                                        string wrappedResult,
                                                                                        string expression)
        {
            Mock.Get(wrapped).Setup(x => x.GetExpressionType(expression)).Returns(wrappedResult);
            Assert.That(() => sut.GetExpressionType(expression), Is.EqualTo(wrappedResult));
        }

        [Test, AutoMoqData]
        public void GetExpressionType_returns_configured_default_if_wrapped_service_returns_null([Frozen] IGetsExpressionType wrapped,
                                                                                                 [MockedConfig, Frozen] RenderingConfig config,
                                                                                                 ConfiguredDefaultExpressionTypeDecorator sut,
                                                                                                 string configuredType,
                                                                                                 string expression)
        {
            Mock.Get(wrapped).Setup(x => x.GetExpressionType(expression)).Returns(() => null);
            Mock.Get(config).SetupGet(x => x.DefaultExpressionType).Returns(configuredType);
            Assert.That(() => sut.GetExpressionType(expression), Is.EqualTo(configuredType));
        }
    }
}
