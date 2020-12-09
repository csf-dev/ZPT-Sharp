using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;

namespace ZptSharp.Expressions.NotExpressions
{
    [TestFixture,Parallelizable]
    public class NotExpressionEvaluatorTests
    {
        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_true_if_value_converts_to_false([Frozen] ICoercesValueToBoolean valueConverter,
                                                                                          [Frozen] IEvaluatesExpression evaluator,
                                                                                          NotExpressionEvaluator sut,
                                                                                          string expression,
                                                                                          [StubDom] ExpressionContext context,
                                                                                          object expressionResult)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(valueConverter)
                .Setup(x => x.CoerceToBoolean(expressionResult))
                .Returns(false);

            var result = await sut.EvaluateExpressionAsync(expression, context);

            Assert.That(result, Is.True);
        }

        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_false_if_value_converts_to_true([Frozen] ICoercesValueToBoolean valueConverter,
                                                                                          [Frozen] IEvaluatesExpression evaluator,
                                                                                          NotExpressionEvaluator sut,
                                                                                          string expression,
                                                                                          [StubDom] ExpressionContext context,
                                                                                          object expressionResult)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(valueConverter)
                .Setup(x => x.CoerceToBoolean(expressionResult))
                .Returns(true);

            var result = await sut.EvaluateExpressionAsync(expression, context);

            Assert.That(result, Is.False);
        }
    }
}
