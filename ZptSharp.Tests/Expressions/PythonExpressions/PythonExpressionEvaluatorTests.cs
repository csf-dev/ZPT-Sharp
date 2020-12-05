using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Expressions.PythonExpressions
{
    [TestFixture, Parallelizable]
    public class PythonExpressionEvaluatorTests
    {
        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_result_from_inner_evaluator_using_variables([Frozen] IGetsAllVariablesFromContext variableProvider,
                                                                                                      [Frozen] IEvaluatesPythonExpression pythonEvaluator,
                                                                                                      PythonExpressionEvaluator sut,
                                                                                                      ExpressionContext context,
                                                                                                      IDictionary<string,object> variablesDictionary,
                                                                                                      string expression,
                                                                                                      object evaluatorResult)
        {
            Mock.Get(variableProvider)
                .Setup(x => x.GetAllVariablesAsync(context))
                .Returns(() => Task.FromResult(variablesDictionary));
            var variables = variablesDictionary.Select(x => new Variable(x.Key, x.Value)).ToList();
            Mock.Get(pythonEvaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, variables, CancellationToken.None))
                .Returns(() => Task.FromResult(evaluatorResult));

            var result = await sut.EvaluateExpressionAsync(expression, context);

            Assert.That(result, Is.SameAs(evaluatorResult));
        }
    }
}
