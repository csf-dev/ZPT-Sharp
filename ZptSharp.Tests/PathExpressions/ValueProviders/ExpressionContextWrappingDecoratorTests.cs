using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions.ValueProviders
{
    [TestFixture,Parallelizable]
    public class ExpressionContextWrappingDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_value_from_wrapped_service_using_context_adaper_when_input_object_is_expression_context([MockedConfig,Frozen] RenderingConfig config,
                                                                                                                                           [Frozen] IGetsValueFromObject wrapped,
                                                                                                                                           ExpressionContextWrappingDecorator sut,
                                                                                                                                           ExpressionContext context,
                                                                                                                                           GetValueResult expected,
                                                                                                                                           string name)
        {
            Mock.Get(wrapped)
                .Setup(x => x.TryGetValueAsync(name, It.Is<NamedTalesValueForExpressionContextAdapter>(a => a.Context == context), CancellationToken.None))
                .Returns(Task.FromResult(expected));

            var result = await sut.TryGetValueAsync(name, context);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_value_from_wrapped_service_using_original_input_when_input_is_not_expression_context([MockedConfig, Frozen] RenderingConfig config, 
                                                                                                                                        [Frozen] IGetsValueFromObject wrapped,
                                                                                                                                        ExpressionContextWrappingDecorator sut,
                                                                                                                                        object obj,
                                                                                                                                        GetValueResult expected,
                                                                                                                                        string name)
        {
            Mock.Get(wrapped)
                .Setup(x => x.TryGetValueAsync(name, obj, CancellationToken.None))
                .Returns(Task.FromResult(expected));

            var result = await sut.TryGetValueAsync(name, obj);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
