using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Expressions.LoadExpressions
{
    [TestFixture,Parallelizable]
    public class LoadExpressionEvaluatorTests
    {
        [Test,AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_result_from_renderer([Frozen] IEvaluatesExpression evaluator,
                                                                               [Frozen] IRendersLoadedObject objectRenderer,
                                                                               LoadExpressionEvaluator sut,
                                                                               string expression,
                                                                               ExpressionContext context,
                                                                               object expressionResult,
                                                                               string renderingResult)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(objectRenderer)
                .Setup(x => x.RenderObjectAsync(expressionResult, context, CancellationToken.None))
                .Returns(Task.FromResult(renderingResult));

            var result = await sut.EvaluateExpressionAsync(expression, context);

            Assert.That(result, Is.EqualTo(renderingResult));
        }
    }
}