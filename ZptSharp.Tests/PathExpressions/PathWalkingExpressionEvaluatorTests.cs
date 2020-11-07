using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions
{
    [TestFixture,Parallelizable]
    public class PathWalkingExpressionEvaluatorTests
    {
        [Test, AutoMoqData]
        public async Task WalkAndEvaluatePathExpressionAsync_returns_value_from_expression_context_for_root_path_part([Frozen] IGetsValueFromObject objectValueProvider,
                                                                                                                      PathWalkingExpressionEvaluator sut,
                                                                                                                      ExpressionContext expressionContext,
                                                                                                                      string name,
                                                                                                                      object expected)
        {
            var pathPart = new PathExpression.PathPart(name);
            var alt = new PathExpression.AlternateExpression(pathPart);

            Mock.Get(objectValueProvider)
                .Setup(x => x.TryGetValueAsync(name, expressionContext, CancellationToken.None))
                .Returns(Task.FromResult(GetValueResult.For(expected)));

            var result = await sut.WalkAndEvaluatePathExpressionAsync(alt, PathEvaluationContext.CreateRoot(expressionContext));

            Assert.That(result, Is.SameAs(expected));
        }

        [Test, AutoMoqData]
        public async Task WalkAndEvaluatePathExpressionAsync_returns_value_from_root_object_for_2nd_level_path_part([Frozen] IGetsValueFromObject objectValueProvider,
                                                                                                                    PathWalkingExpressionEvaluator sut,
                                                                                                                    ExpressionContext expressionContext,
                                                                                                                    string rootName,
                                                                                                                    string secondName,
                                                                                                                    object rootValue,
                                                                                                                    object secondValue)
        {
            var rootPath = new PathExpression.PathPart(rootName);
            var secondPath = new PathExpression.PathPart(secondName);
            var alt = new PathExpression.AlternateExpression(rootPath, secondPath);

            Mock.Get(objectValueProvider)
                .Setup(x => x.TryGetValueAsync(rootName, expressionContext, CancellationToken.None))
                .Returns(Task.FromResult(GetValueResult.For(rootValue)));
            Mock.Get(objectValueProvider)
                .Setup(x => x.TryGetValueAsync(secondName, rootValue, CancellationToken.None))
                .Returns(Task.FromResult(GetValueResult.For(secondValue)));

            var result = await sut.WalkAndEvaluatePathExpressionAsync(alt, PathEvaluationContext.CreateRoot(expressionContext));

            Assert.That(result, Is.SameAs(secondValue));
        }

        [Test, AutoMoqData]
        public async Task WalkAndEvaluatePathExpressionAsync_returns_value_from_expression_context_for_2nd_level_interpolated_path_part([Frozen] IGetsValueFromObject objectValueProvider,
                                                                                                                                        PathWalkingExpressionEvaluator sut,
                                                                                                                                        ExpressionContext expressionContext,
                                                                                                                                        string rootName,
                                                                                                                                        string secondName,
                                                                                                                                        object rootValue,
                                                                                                                                        object secondValue,
                                                                                                                                        string interpolatedSecondName)
        {
            var rootPath = new PathExpression.PathPart(rootName);
            var secondPath = new PathExpression.PathPart(secondName, true);
            var alt = new PathExpression.AlternateExpression(rootPath, secondPath);

            Mock.Get(objectValueProvider)
                .Setup(x => x.TryGetValueAsync(rootName, expressionContext, CancellationToken.None))
                .Returns(Task.FromResult(GetValueResult.For(rootValue)));
            Mock.Get(objectValueProvider)
                .Setup(x => x.TryGetValueAsync(secondName, expressionContext, CancellationToken.None))
                .Returns(Task.FromResult(GetValueResult.For(interpolatedSecondName)));
            Mock.Get(objectValueProvider)
                .Setup(x => x.TryGetValueAsync(interpolatedSecondName, rootValue, CancellationToken.None))
                .Returns(Task.FromResult(GetValueResult.For(secondValue)));

            var result = await sut.WalkAndEvaluatePathExpressionAsync(alt, PathEvaluationContext.CreateRoot(expressionContext));

            Assert.That(result, Is.SameAs(secondValue));
        }

        [Test, AutoMoqData]
        public void WalkAndEvaluatePathExpressionAsync_throws_EvaluationException_if_a_value_cannot_be_retrieved([Frozen] IGetsValueFromObject objectValueProvider,
                                                                                                                 PathWalkingExpressionEvaluator sut,
                                                                                                                 ExpressionContext expressionContext,
                                                                                                                 string rootName,
                                                                                                                 string secondName,
                                                                                                                 object rootValue)
        {
            var rootPath = new PathExpression.PathPart(rootName);
            var secondPath = new PathExpression.PathPart(secondName);
            var alt = new PathExpression.AlternateExpression(rootPath, secondPath);

            Mock.Get(objectValueProvider)
                .Setup(x => x.TryGetValueAsync(rootName, expressionContext, CancellationToken.None))
                .Returns(Task.FromResult(GetValueResult.For(rootValue)));
            Mock.Get(objectValueProvider)
                .Setup(x => x.TryGetValueAsync(secondName, rootValue, CancellationToken.None))
                .Returns(Task.FromResult(GetValueResult.Failure));

            Assert.That(() => sut.WalkAndEvaluatePathExpressionAsync(alt, PathEvaluationContext.CreateRoot(expressionContext)).Result,
                        Throws.InstanceOf<AggregateException>().With.InnerException.InstanceOf<EvaluationException>());
        }

        [Test, AutoMoqData]
        public void WalkAndEvaluatePathExpressionAsync_throws_EvaluationException_if_an_interpolated_name_cannot_be_retrieved([Frozen] IGetsValueFromObject objectValueProvider,
                                                                                                                              PathWalkingExpressionEvaluator sut,
                                                                                                                              ExpressionContext expressionContext,
                                                                                                                              string rootName,
                                                                                                                              string secondName,
                                                                                                                              object rootValue)
        {
            var rootPath = new PathExpression.PathPart(rootName);
            var secondPath = new PathExpression.PathPart(secondName, true);
            var alt = new PathExpression.AlternateExpression(rootPath, secondPath);

            Mock.Get(objectValueProvider)
                .Setup(x => x.TryGetValueAsync(rootName, expressionContext, CancellationToken.None))
                .Returns(Task.FromResult(GetValueResult.For(rootValue)));
            Mock.Get(objectValueProvider)
                .Setup(x => x.TryGetValueAsync(secondName, expressionContext, CancellationToken.None))
                .Returns(Task.FromResult(GetValueResult.Failure));

            Assert.That(() => sut.WalkAndEvaluatePathExpressionAsync(alt, PathEvaluationContext.CreateRoot(expressionContext)).Result,
                        Throws.InstanceOf<AggregateException>().With.InnerException.InstanceOf<EvaluationException>());
        }

    }
}
