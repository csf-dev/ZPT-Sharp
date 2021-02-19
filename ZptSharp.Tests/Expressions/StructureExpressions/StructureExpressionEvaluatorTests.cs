using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Expressions.StructureExpressions
{
    [TestFixture,Parallelizable]
    public class StructureExpressionEvaluatorTests
    {
        public async Task EvaluateExpressionAsync_gets_an_IGetsStructuredMarkup_containing_the_wrapped_result([Frozen] IEvaluatesExpression wrappedEvaluator,
                                                                                                              StructureExpressionEvaluator sut,
                                                                                                              int wrappedResult,
                                                                                                              string expression,
                                                                                                              ExpressionContext context)
        {
            Mock.Get(wrappedEvaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(wrappedResult));

            var result = await sut.EvaluateExpressionAsync(expression, context);
            Assert.That(result, Is.InstanceOf<IGetsStructuredMarkup>(), "Result is of expected type");

            var markup = await ((IGetsStructuredMarkup)result).GetMarkupAsync();
            Assert.That(markup, Is.EqualTo(wrappedResult.ToString()), "Returned markup has correct value");
        }
    }
}