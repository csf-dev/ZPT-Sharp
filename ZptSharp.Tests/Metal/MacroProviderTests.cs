using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Metal
{
    [TestFixture,Parallelizable]
    public class MacroProviderTests
    {
        [Test, AutoMoqData]
        public async Task GetMacroAsync_returns_macro_from_expression_if_it_exists([Frozen] IEvaluatesExpression expressionEvaluator,
                                                                                   MacroProvider sut,
                                                                                   IElement element,
                                                                                   IAttribute attr,
                                                                                   string expression,
                                                                                   ExpressionContext context,
                                                                                   AttributeSpec attributeSpec,
                                                                                   MetalMacro macro)
        {
            Mock.Get(expressionEvaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(macro));
            Mock.Get(element).SetupGet(x => x.Attributes).Returns(new[] { attr });
            Mock.Get(attr).SetupGet(x => x.Value).Returns(expression);
            Mock.Get(attr).Setup(x => x.Matches(attributeSpec)).Returns(true);

            var result = await sut.GetMacroAsync(element, context, attributeSpec);

            Assert.That(result, Is.SameAs(macro));
        }

        [Test, AutoMoqData]
        public async Task GetMacroAsync_returns_null_if_no_attribute_matches_spec(MacroProvider sut,
                                                                                  IElement element,
                                                                                  IAttribute attr,
                                                                                  ExpressionContext context,
                                                                                  AttributeSpec attributeSpec)
        {
            Mock.Get(element).SetupGet(x => x.Attributes).Returns(new[] { attr });
            Mock.Get(attr).Setup(x => x.Matches(attributeSpec)).Returns(false);

            var result = await sut.GetMacroAsync(element, context, attributeSpec);

            Assert.That(result, Is.Null);
        }

        [Test, AutoMoqData]
        public void GetMacroAsync_throws_MacroNotFoundException_if_macro_is_null([Frozen] IEvaluatesExpression expressionEvaluator,
                                                                                  MacroProvider sut,
                                                                                  IElement element,
                                                                                  IAttribute attr,
                                                                                  string expression,
                                                                                  ExpressionContext context,
                                                                                  AttributeSpec attributeSpec)
        {
            Mock.Get(expressionEvaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(null));
            Mock.Get(element).SetupGet(x => x.Attributes).Returns(new[] { attr });
            Mock.Get(attr).SetupGet(x => x.Value).Returns(expression);
            Mock.Get(attr).Setup(x => x.Matches(attributeSpec)).Returns(true);

            Assert.That(() => sut.GetMacroAsync(element, context, attributeSpec).Result,
                        Throws.Exception.With.InnerException.InstanceOf<MacroNotFoundException>());
        }

        [Test, AutoMoqData]
        public void GetMacroAsync_throws_MacroNotFoundException_if_evaluator_throws([Frozen] IEvaluatesExpression expressionEvaluator,
                                                                                    MacroProvider sut,
                                                                                    IElement element,
                                                                                    IAttribute attr,
                                                                                    string expression,
                                                                                    ExpressionContext context,
                                                                                    AttributeSpec attributeSpec)
        {
            Mock.Get(expressionEvaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Throws(new EvaluationException());
            Mock.Get(element).SetupGet(x => x.Attributes).Returns(new[] { attr });
            Mock.Get(attr).SetupGet(x => x.Value).Returns(expression);
            Mock.Get(attr).Setup(x => x.Matches(attributeSpec)).Returns(true);

            Assert.That(() => sut.GetMacroAsync(element, context, attributeSpec).Result,
                        Throws.Exception.With.InnerException.InstanceOf<MacroNotFoundException>());
        }
    }
}
