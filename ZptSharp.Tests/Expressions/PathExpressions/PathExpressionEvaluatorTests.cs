using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp.Expressions.PathExpressions
{
    [TestFixture,Parallelizable]
    public class PathExpressionEvaluatorTests
    {
        [Test, AutoMoqData]
        public void EvaluateExpressionAsync_returns_result_from_evaluating_first_alternate_expression_if_it_yields_result([Frozen] IParsesPathExpression expressionParser,
                                                                                                                          [Frozen] IWalksAndEvaluatesPathExpression pathWalker,
                                                                                                                          PathExpressionEvaluator sut,
                                                                                                                          PathExpression.AlternateExpression alt1,
                                                                                                                          PathExpression.AlternateExpression alt2,
                                                                                                                          string expression,
                                                                                                                          ExpressionContext context,
                                                                                                                          object expected)
        {
            var path = new PathExpression(new[] { alt1, alt2 });
            Mock.Get(expressionParser).Setup(x => x.Parse(expression)).Returns(path);
            Mock.Get(pathWalker)
                .Setup(x => x.WalkAndEvaluatePathExpressionAsync(alt1, It.Is<PathEvaluationContext>(c => c.ExpressionContext == context && c.IsRoot), CancellationToken.None))
                .Returns(Task.FromResult(expected));

            Assert.That(() => sut.EvaluateExpressionAsync(expression, context, CancellationToken.None).Result, Is.SameAs(expected));
        }

        [Test, AutoMoqData]
        public void EvaluateExpressionAsync_returns_result_from_evaluating_second_alternate_expression_if_first_throws([Frozen] IParsesPathExpression expressionParser,
                                                                                                                       [Frozen] IWalksAndEvaluatesPathExpression pathWalker,
                                                                                                                       PathExpressionEvaluator sut,
                                                                                                                       PathExpression.AlternateExpression alt1,
                                                                                                                       PathExpression.AlternateExpression alt2,
                                                                                                                       string expression,
                                                                                                                       ExpressionContext context,
                                                                                                                       object expected)
        {
            var path = new PathExpression(new[] { alt1, alt2 });
            Mock.Get(expressionParser).Setup(x => x.Parse(expression)).Returns(path);
            Mock.Get(pathWalker)
                .Setup(x => x.WalkAndEvaluatePathExpressionAsync(alt1, It.Is<PathEvaluationContext>(c => c.ExpressionContext == context && c.IsRoot), CancellationToken.None))
                .Throws<Exception>();
            Mock.Get(pathWalker)
                .Setup(x => x.WalkAndEvaluatePathExpressionAsync(alt2, It.Is<PathEvaluationContext>(c => c.ExpressionContext == context && c.IsRoot), CancellationToken.None))
                .Returns(Task.FromResult(expected));

            Assert.That(() => sut.EvaluateExpressionAsync(expression, context, CancellationToken.None).Result, Is.SameAs(expected));
        }

        [Test, AutoMoqData]
        public void EvaluateExpressionAsync_throws_AggregateException_if_evaluating_more_than_one_alternate_throws([Frozen] IParsesPathExpression expressionParser,
                                                                                                                   [Frozen] IWalksAndEvaluatesPathExpression pathWalker,
                                                                                                                   PathExpressionEvaluator sut,
                                                                                                                   PathExpression.AlternateExpression alt1,
                                                                                                                   PathExpression.AlternateExpression alt2,
                                                                                                                   string expression,
                                                                                                                   ExpressionContext context)
        {
            var path = new PathExpression(new[] { alt1, alt2 });
            Mock.Get(expressionParser).Setup(x => x.Parse(expression)).Returns(path);
            Mock.Get(pathWalker)
                .Setup(x => x.WalkAndEvaluatePathExpressionAsync(It.IsAny<PathExpression.AlternateExpression>(), It.IsAny<PathEvaluationContext>(), CancellationToken.None))
                .Throws<Exception>();

            Assert.That(() => sut.EvaluateExpressionAsync(expression, context, CancellationToken.None).Result,
                        Throws.InstanceOf<AggregateException>().With.Property(nameof(AggregateException.InnerExceptions)).Count.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void EvaluateExpressionAsync_throws_EvaluationException_if_evaluating_single_alternate_throws([Frozen] IParsesPathExpression expressionParser,
                                                                                                             [Frozen] IWalksAndEvaluatesPathExpression pathWalker,
                                                                                                             PathExpressionEvaluator sut,
                                                                                                             PathExpression.AlternateExpression alt,
                                                                                                             string expression,
                                                                                                             ExpressionContext context)
        {
            var path = new PathExpression(new[] { alt });
            Mock.Get(expressionParser).Setup(x => x.Parse(expression)).Returns(path);
            Mock.Get(pathWalker)
                .Setup(x => x.WalkAndEvaluatePathExpressionAsync(It.IsAny<PathExpression.AlternateExpression>(), It.IsAny<PathEvaluationContext>(), CancellationToken.None))
                .Throws<Exception>();

            Assert.That(() => sut.EvaluateExpressionAsync(expression, context, CancellationToken.None).Result,
                        Throws.InstanceOf<EvaluationException>());
        }
    }
}
