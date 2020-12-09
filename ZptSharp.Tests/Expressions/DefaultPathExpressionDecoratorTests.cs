using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Expressions
{
    [TestFixture, Parallelizable]
    public class DefaultPathExpressionDecoratorTests
    {
        [Test, AutoMoqData]
        public void GetExpressionType_returns_wrapped_result_if_it_is_not_null([Frozen] IGetsExpressionType wrapped,
                                                                               DefaultPathExpressionDecorator sut,
                                                                               string expression,
                                                                               string type)
        {
            Mock.Get(wrapped)
                .Setup(x => x.GetExpressionType(expression))
                .Returns(type);
            Assert.That(() => sut.GetExpressionType(expression), Is.EqualTo(type));
        }

        [Test, AutoMoqData]
        public void GetExpressionType_returns_path_if_wrapped_result_is_null([Frozen] IGetsExpressionType wrapped,
                                                                             DefaultPathExpressionDecorator sut,
                                                                             string expression)
        {
            Mock.Get(wrapped)
                .Setup(x => x.GetExpressionType(expression))
                .Returns(() => null);
            Assert.That(() => sut.GetExpressionType(expression), Is.EqualTo("path"));
        }
    }
}
