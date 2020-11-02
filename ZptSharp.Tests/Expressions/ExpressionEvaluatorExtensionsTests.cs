using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class ExpressionEvaluatorExtensionsTests
    {
        [Test, AutoMoqData]
        public void EvaluateExpressionAsync_throws_ANE_if_evaluator_is_null(string expression,
                                                                            ExpressionContext context)
        {
            IEvaluatesExpression evaluator = null;
            Assert.That(() => evaluator.EvaluateExpressionAsync<string>(expression, context), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_uses_evaluator_to_resolve_expression(IEvaluatesExpression evaluator,
                                                                                       string expression,
                                                                                       ExpressionContext context,
                                                                                       string expected)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(expected));

            var result = await evaluator.EvaluateExpressionAsync<string>(expression, context);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void EvaluateExpressionAsync_throws_EvaluationException_if_result_is_wrong_type(IEvaluatesExpression evaluator,
                                                                                               string expression,
                                                                                               ExpressionContext context,
                                                                                               int result)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(result));

            Assert.That(() => evaluator.EvaluateExpressionAsync<string>(expression, context).Result,
                        Throws.InstanceOf<AggregateException>().And.InnerException.InstanceOf<EvaluationException>());
        }
    }
}
