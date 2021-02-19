using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Expressions;
using ZptSharp.Expressions.PathExpressions;

namespace ZptSharp.Tests.Expressions.PathExpressions
{
    [TestFixture, Parallelizable]
    public class GlobalVariableOnlyExpressionContextWrappingDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task TryGetValueAsync_calls_wrapped_using_adapter([Frozen] IGetsValueFromObject wrapped,
                                                                       GlobalVariableOnlyExpressionContextWrappingDecorator sut,
                                                                       ExpressionContext context,
                                                                       string name)
        {
            var result = await sut.TryGetValueAsync(name, context);

            Mock.Get(wrapped)
                .Verify(x => x.TryGetValueAsync(name, It.Is<GlobalVariablesOnlyTalesValueForExpressionContextAdapter>(a => a.Context == context), CancellationToken.None),
                        Times.Once);
        }
    }
}