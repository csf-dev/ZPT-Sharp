using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Expressions;
using System.Threading.Tasks;
using System.Threading;

namespace ZptSharp.Expressions.PathExpressions
{
    [TestFixture,Parallelizable]
    public class PathExpressionEvaluatorTests
    {
        [Test, AutoMoqData]
        public void EvaluateExpressionAsync_returns_result_from_wrapped_service_using_request([Frozen] IEvaluatesPathExpressionRequest requestEvaluator,
                                                                                              PathExpressionEvaluator sut,
                                                                                              string expression,
                                                                                              ExpressionContext context,
                                                                                              object expected)
        {
            Mock.Get(requestEvaluator)
                .Setup(x => x.EvaluateAsync(It.Is<PathExpressionEvaluationRequest>(r => r.Expression == expression
                                                                                        && r.ExpressionContext == context
                                                                                        && r.ScopeLimitation == RootScopeLimitation.None),
                                            CancellationToken.None))
                .Returns(() => Task.FromResult(expected));

            Assert.That(() => sut.EvaluateExpressionAsync(expression, context).Result, Is.SameAs(expected));
        }
    }
}
