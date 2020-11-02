using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MacroUsageContextProcessorTests
    {
        [Test, AutoMoqData]
        public async Task ProcessContextAsync_does_not_replace_element_if_no_use_macro_attribute([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                                 MacroUsageContextProcessor sut,
                                                                                                 AttributeSpec spec,
                                                                                                 ExpressionContext context,
                                                                                                 IElement element)
        {
            Mock.Get(specProvider).SetupGet(x => x.UseMacro).Returns(spec);
            Mock.Get(element).SetupGet(x => x.Attributes).Returns(new List<IAttribute>());
            context.CurrentElement = element;

            await sut.ProcessContextAsync(context);

            Assert.That(context.CurrentElement, Is.SameAs(element));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_replaces_element_in_context_with_expanded_macro([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                              [Frozen] IEvaluatesExpression expressionEvaluator,
                                                                                              [Frozen] IExpandsMacro macroExpander,
                                                                                              MacroUsageContextProcessor sut,
                                                                                              MetalMacro macro,
                                                                                              MetalMacro expandedMacro,
                                                                                              AttributeSpec spec,
                                                                                              ExpressionContext context,
                                                                                              IElement element,
                                                                                              IAttribute attribute,
                                                                                              string expression)
        {
            Mock.Get(specProvider).SetupGet(x => x.UseMacro).Returns(spec);
            Mock.Get(element).SetupGet(x => x.Attributes).Returns(new List<IAttribute> { attribute });
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(attribute).SetupGet(x => x.Value).Returns(expression);
            context.CurrentElement = element;
            Mock.Get(expressionEvaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(macro));
            Mock.Get(macroExpander)
                .Setup(x => x.ExpandMacroAsync(macro, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expandedMacro));

            await sut.ProcessContextAsync(context);

            Assert.That(context.CurrentElement, Is.SameAs(expandedMacro.Element));
        }

        [Test, AutoMoqData]
        public void ProcessContextAsync_throws_MacroNotFoundException_if_macro_not_found([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                         [Frozen] IEvaluatesExpression expressionEvaluator,
                                                                                         MacroUsageContextProcessor sut,
                                                                                         AttributeSpec spec,
                                                                                         ExpressionContext context,
                                                                                         IElement element,
                                                                                         IAttribute attribute,
                                                                                         string expression)
        {
            Mock.Get(specProvider).SetupGet(x => x.UseMacro).Returns(spec);
            Mock.Get(element).SetupGet(x => x.Attributes).Returns(new List<IAttribute> { attribute });
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(attribute).SetupGet(x => x.Value).Returns(expression);
            context.CurrentElement = element;
            Mock.Get(expressionEvaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(null));

            Assert.That(() => sut.ProcessContextAsync(context).Result,
                        Throws.InstanceOf<AggregateException>().And.InnerException.InstanceOf<MacroNotFoundException>());
        }
    }
}
