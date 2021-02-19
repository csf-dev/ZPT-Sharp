using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Expressions.PipeExpressions
{
    [TestFixture, Parallelizable]
    public class PipeExpressionEvaluatorTests
    {
        [Test,AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_result_from_delegate_executor_if_everything_is_valid([Frozen] IEvaluatesExpression evaluator,
                                                                                                               [Frozen] IEvaluatesPipeDelegate delegateEvaluator,
                                                                                                               PipeExpressionEvaluator sut,
                                                                                                               object source,
                                                                                                               object pipeDelegate,
                                                                                                               object expected,
                                                                                                               ExpressionContext context)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("path:source", context, CancellationToken.None))
                .Returns(() => Task.FromResult(source));
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("delegate", context, CancellationToken.None))
                .Returns(() => Task.FromResult(pipeDelegate));
            Mock.Get(delegateEvaluator)
                .Setup(x => x.EvaluateDelegate(source, pipeDelegate))
                .Returns(expected);
            
            var result = await sut.EvaluateExpressionAsync("source delegate", context);

            Assert.That(result, Is.SameAs(expected));
        }

        [Test,AutoMoqData]
        public void EvaluateExpressionAsync_throws_CannotParsePipeExpressionException_if_expression_is_invalid(PipeExpressionEvaluator sut, ExpressionContext context)
        {
            Assert.That(async () => await sut.EvaluateExpressionAsync("nope", context, CancellationToken.None),
                        Throws.InstanceOf<CannotParsePipeExpressionException>()
                            .And.Message.StartsWith("A 'pipe' expression must use the correct syntax."));
        }

        [Test,AutoMoqData]
        public void EvaluateExpressionAsync_throws_PipeExpressionException_if_getting_source_object_throws([Frozen] IEvaluatesExpression evaluator,
                                                                                                           PipeExpressionEvaluator sut,
                                                                                                           object pipeDelegate,
                                                                                                           ExpressionContext context)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("path:source", context, CancellationToken.None))
                .Throws<Exception>();
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("delegate", context, CancellationToken.None))
                .Returns(() => Task.FromResult(pipeDelegate));
            
            Assert.That(async () => await sut.EvaluateExpressionAsync("source delegate", context),
                        Throws.InstanceOf<PipeExpressionException>()
                            .And.Message.StartWith("An exception occurred whilst getting the source object"));
        }

        [Test,AutoMoqData]
        public void EvaluateExpressionAsync_throws_PipeExpressionException_if_getting_delegate_object_throws([Frozen] IEvaluatesExpression evaluator,
                                                                                                             PipeExpressionEvaluator sut,
                                                                                                             object source,
                                                                                                             ExpressionContext context)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("path:source", context, CancellationToken.None))
                .Returns(() => Task.FromResult(source));
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("delegate", context, CancellationToken.None))
                .Throws<Exception>();
            
            Assert.That(async () => await sut.EvaluateExpressionAsync("source delegate", context),
                        Throws.InstanceOf<PipeExpressionException>()
                            .And.Message.StartWith("An exception occurred whilst getting the function delegate"));
        }

        [Test,AutoMoqData]
        public void EvaluateExpressionAsync_throws_PipeExpressionException_if_evaluating_delegate_object_throws([Frozen] IEvaluatesExpression evaluator,
                                                                                                                [Frozen] IEvaluatesPipeDelegate delegateEvaluator,
                                                                                                                PipeExpressionEvaluator sut,
                                                                                                                object source,
                                                                                                                object pipeDelegate,
                                                                                                                ExpressionContext context)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("path:source", context, CancellationToken.None))
                .Returns(() => Task.FromResult(source));
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("delegate", context, CancellationToken.None))
                .Returns(() => Task.FromResult(pipeDelegate));
            Mock.Get(delegateEvaluator)
                .Setup(x => x.EvaluateDelegate(source, pipeDelegate))
                .Throws<Exception>();
            
            Assert.That(async () => await sut.EvaluateExpressionAsync("source delegate", context),
                        Throws.InstanceOf<PipeExpressionException>()
                            .And.Message.StartWith("An exception occurred whilst executing the function delegate"));
        }
    }
}