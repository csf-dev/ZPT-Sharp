using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Expressions.PathExpressions
{
    public class PathExpressionRequestEvaluatorTests
    {

        [Test, AutoMoqData]
        public void EvaluateAsync_returns_result_from_evaluating_first_alternate_expression_if_it_yields_result([Frozen] IParsesPathExpression expressionParser,
                                                                                                                [Frozen] IGetsPathWalkingExpressionEvaluator walkingEvaluatorFactory,
                                                                                                                IWalksAndEvaluatesPathExpression pathWalker,
                                                                                                                PathExpressionRequestEvaluator sut,
                                                                                                                PathExpression.AlternateExpression alt1,
                                                                                                                PathExpression.AlternateExpression alt2,
                                                                                                                PathExpressionEvaluationRequest request,
                                                                                                                object expected)
        {
            var path = new PathExpression(new[] { alt1, alt2 });
            Mock.Get(expressionParser).Setup(x => x.Parse(request.Expression)).Returns(path);
            Mock.Get(walkingEvaluatorFactory)
                .Setup(x => x.GetEvaluator(request.ScopeLimitation))
                .Returns(pathWalker);
            Mock.Get(pathWalker)
                .Setup(x => x.WalkAndEvaluatePathExpressionAsync(alt1, It.Is<PathEvaluationContext>(c => c.ExpressionContext == request.ExpressionContext && c.IsRoot), CancellationToken.None))
                .Returns(Task.FromResult(expected));

            Assert.That(() => sut.EvaluateAsync(request, CancellationToken.None).Result, Is.SameAs(expected));
        }

        [Test, AutoMoqData]
        public void EvaluateAsync_returns_result_from_evaluating_second_alternate_expression_if_first_throws([Frozen] IParsesPathExpression expressionParser,
                                                                                                             [Frozen] IGetsPathWalkingExpressionEvaluator walkingEvaluatorFactory,
                                                                                                             IWalksAndEvaluatesPathExpression pathWalker,
                                                                                                             PathExpressionRequestEvaluator sut,
                                                                                                             PathExpression.AlternateExpression alt1,
                                                                                                             PathExpression.AlternateExpression alt2,
                                                                                                             PathExpressionEvaluationRequest request,
                                                                                                             object expected)
        {
            var path = new PathExpression(new[] { alt1, alt2 });
            Mock.Get(expressionParser).Setup(x => x.Parse(request.Expression)).Returns(path);
            Mock.Get(walkingEvaluatorFactory)
                .Setup(x => x.GetEvaluator(request.ScopeLimitation))
                .Returns(pathWalker);
            Mock.Get(pathWalker)
                .Setup(x => x.WalkAndEvaluatePathExpressionAsync(alt1, It.Is<PathEvaluationContext>(c => c.ExpressionContext == request.ExpressionContext && c.IsRoot), CancellationToken.None))
                .Throws<Exception>();
            Mock.Get(pathWalker)
                .Setup(x => x.WalkAndEvaluatePathExpressionAsync(alt2, It.Is<PathEvaluationContext>(c => c.ExpressionContext == request.ExpressionContext && c.IsRoot), CancellationToken.None))
                .Returns(Task.FromResult(expected));

            Assert.That(() => sut.EvaluateAsync(request, CancellationToken.None).Result, Is.SameAs(expected));
        }

        [Test, AutoMoqData]
        public void EvaluateAsync_throws_AggregateException_if_evaluating_all_alternates_throws([Frozen] IParsesPathExpression expressionParser,
                                                                                                [Frozen] IGetsPathWalkingExpressionEvaluator walkingEvaluatorFactory,
                                                                                                IWalksAndEvaluatesPathExpression pathWalker,
                                                                                                PathExpressionRequestEvaluator sut,
                                                                                                PathExpression.AlternateExpression alt1,
                                                                                                PathExpression.AlternateExpression alt2,
                                                                                                PathExpressionEvaluationRequest request)
        {
            var path = new PathExpression(new[] { alt1, alt2 });
            Mock.Get(expressionParser).Setup(x => x.Parse(request.Expression)).Returns(path);
            Mock.Get(walkingEvaluatorFactory)
                .Setup(x => x.GetEvaluator(request.ScopeLimitation))
                .Returns(pathWalker);
            Mock.Get(pathWalker)
                .Setup(x => x.WalkAndEvaluatePathExpressionAsync(It.IsAny<PathExpression.AlternateExpression>(), It.IsAny<PathEvaluationContext>(), CancellationToken.None))
                .Throws<Exception>();

            Assert.That(() => sut.EvaluateAsync(request, CancellationToken.None).Result,
                        Throws.InstanceOf<AggregateException>());
        }

        [Test, AutoMoqData]
        public void EvaluateAsync_throws_CannotParsePathException_if_path_is_invalid([Frozen] IParsesPathExpression expressionParser,
                                                                                     PathExpressionRequestEvaluator sut,
                                                                                     PathExpressionEvaluationRequest request,
                                                                                     CannotParsePathException inner)
        {
            Mock.Get(expressionParser).Setup(x => x.Parse(request.Expression)).Throws(inner);

            Assert.That(() => sut.EvaluateAsync(request, CancellationToken.None).Result,
                        Throws.InstanceOf<CannotParsePathException>().With.InnerException.SameAs(inner));
        }
    }
}
