using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Expressions;

namespace ZptSharp.Expressions.StringExpressions
{
    [TestFixture,Parallelizable]
    public class StringExpressionEvaluatorTests
    {
        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_can_get_simple_string([Frozen] IEvaluatesExpression evaluator,
                                                                        StringExpressionEvaluator sut,
                                                                        [StubDom] ExpressionContext context)
        {
            var result = await sut.EvaluateExpressionAsync("Hello there!", context);

            Assert.That(result, Is.EqualTo("Hello there!"));
        }

        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_can_get_string_with_two_placeholder_replacements([Frozen] IEvaluatesExpression evaluator,
                                                                                                   StringExpressionEvaluator sut,
                                                                                                   [StubDom] ExpressionContext context)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("path:time/oclock", context, CancellationToken.None))
                .Returns(Task.FromResult<object>("O'Clock"));
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("path:genre/rock", context, CancellationToken.None))
                .Returns(Task.FromResult<object>("Rock"));

            var result = await sut.EvaluateExpressionAsync("123 $time/oclock 4 O'Clock $genre/rock!", context);

            Assert.That(result, Is.EqualTo("123 O'Clock 4 O'Clock Rock!"));
        }

        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_can_get_string_with_a_complex_replacements_and_two_escaped_replacements([Frozen] IEvaluatesExpression evaluator,
                                                                                                                          StringExpressionEvaluator sut,
                                                                                                                          [StubDom] ExpressionContext context)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("path:value/cash | money", context, CancellationToken.None))
                .Returns(Task.FromResult<object>("money"));

            var result = await sut.EvaluateExpressionAsync("Money, $$money, ${value/cash | money}, in a $$rich man's world", context);

            Assert.That(result, Is.EqualTo("Money, $money, money, in a $rich man's world"));
        }

        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_does_not_throw_if_a_replacement_value_is_null([Frozen] IEvaluatesExpression evaluator,
                                                                                                StringExpressionEvaluator sut,
                                                                                                [StubDom] ExpressionContext context)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("path:greeting", context, CancellationToken.None))
                .Returns(Task.FromResult<object>(null));

            var result = await sut.EvaluateExpressionAsync("Why $greeting there!", context);

            Assert.That(result, Is.EqualTo("Why  there!"));
        }
    }
}
