using System;
using System.Collections.Generic;
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
    public class MetalMacroExpanderTests
    {
        [Test, AutoMoqData]
        public async Task ExpandMacroAsync_extends_macro_fills_slots_and_returns_expanded_macro([Frozen] IExtendsMacro macroExtender,
                                                                                                [Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                                [Frozen] ISearchesForAttributes attributeFinder,
                                                                                                [Frozen] IFillsSlots slotFiller,
                                                                                                MetalMacroExpander sut,
                                                                                                MetalMacro macro,
                                                                                                MetalMacro extendedMacro,
                                                                                                ExpressionContext context,
                                                                                                AttributeSpec spec,
                                                                                                StubAttribute attr1,
                                                                                                StubAttribute attr2)
        {
            Mock.Get(specProvider).SetupGet(x => x.FillSlot).Returns(spec);
            Mock.Get(attributeFinder)
                .Setup(x => x.SearchForAttributes(macro.Element, spec))
                .Returns(new[] { attr1, attr2 });

            Mock.Get(macroExtender)
                .Setup(x => x.ExtendMacroAsync(macro,
                                               context,
                                               It.Is<IDictionary<string, SlotFiller>>(s => s.ContainsKey(attr1.Value) && s.ContainsKey(attr2.Value)),
                                               CancellationToken.None))
               .Returns(Task.FromResult(extendedMacro));

            var result = await sut.ExpandMacroAsync(macro, context);

            Assert.That(result, Is.SameAs(extendedMacro), "Service returns the extended macro");
            Mock.Get(slotFiller)
                .Verify(x => x.FillSlots(extendedMacro, It.Is<IDictionary<string, SlotFiller>>(s => s.ContainsKey(attr1.Value) && s.ContainsKey(attr2.Value))),
                        Times.Once,
                        "The slot filler was used to fill slots in the extended macro");
        }
    }
}
