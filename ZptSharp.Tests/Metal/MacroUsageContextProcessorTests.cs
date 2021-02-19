using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Metal
{
    [TestFixture,Parallelizable]
    public class MacroUsageContextProcessorTests
    {
        [Test, AutoMoqData]
        public async Task ProcessContextAsync_does_not_replace_node_if_no_use_macro_attribute([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                                 [Frozen] IGetsMacro macroProvider,
                                                                                                 [Frozen, MockLogger] ILogger<MacroUsageContextProcessor> logger,
                                                                                                 MacroUsageContextProcessor sut,
                                                                                                 AttributeSpec spec1,
                                                                                                 AttributeSpec spec2,
                                                                                                 ExpressionContext context,
                                                                                                 INode node)
        {
            Mock.Get(specProvider).SetupGet(x => x.UseMacro).Returns(spec1);
            Mock.Get(specProvider).SetupGet(x => x.ExtendMacro).Returns(spec2);
            Mock.Get(macroProvider)
                .Setup(x => x.GetMacroAsync(node, context, new[] { spec1, spec2 }, CancellationToken.None))
                .Returns(Task.FromResult<MetalMacro>(null));
            context.CurrentNode = node;

            await sut.ProcessContextAsync(context);

            Assert.That(context.CurrentNode, Is.SameAs(node));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_replaces_node_in_context_with_expanded_macro([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                              [Frozen] IGetsMacro macroProvider,
                                                                                              [Frozen] IExpandsMacro macroExpander,
                                                                                              [Frozen, MockLogger] ILogger<MacroUsageContextProcessor> logger,
                                                                                              MacroUsageContextProcessor sut,
                                                                                              MetalMacro macro,
                                                                                              MetalMacro expandedMacro,
                                                                                              AttributeSpec spec1,
                                                                                              AttributeSpec spec2,
                                                                                              ExpressionContext context,
                                                                                              INode node,
                                                                                              IAttribute attribute,
                                                                                              string expression)
        {
            Mock.Get(specProvider).SetupGet(x => x.UseMacro).Returns(spec1);
            Mock.Get(specProvider).SetupGet(x => x.ExtendMacro).Returns(spec2);
            Mock.Get(node).SetupGet(x => x.Attributes).Returns(new List<IAttribute> { attribute });
            Mock.Get(attribute).Setup(x => x.Matches(spec1)).Returns(true);
            Mock.Get(attribute).SetupGet(x => x.Value).Returns(expression);
            context.CurrentNode = node;
            Mock.Get(macroProvider)
                .Setup(x => x.GetMacroAsync(node, context, new[] { spec1, spec2 }, CancellationToken.None))
                .Returns(Task.FromResult(macro));
            Mock.Get(macroExpander)
                .Setup(x => x.ExpandMacroAsync(macro, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expandedMacro));

            await sut.ProcessContextAsync(context);

            Assert.That(context.CurrentNode, Is.SameAs(expandedMacro.Node));
        }
    }
}
