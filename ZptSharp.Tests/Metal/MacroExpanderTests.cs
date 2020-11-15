using System;
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
    public class MacroExpanderTests
    {
        [Test, AutoMoqData]
        public async Task ExpandMacroAsync_fills_slots_and_does_nothing_more_for_a_macro_which_does_not_extend_another([Frozen] IGetsMacro macroProvider,
                                                                                                                       [Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                                                       [Frozen] IGetsSlots slotFinder,
                                                                                                                       [Frozen] IFillsSlots slotFiller,
                                                                                                                       [Frozen, MockLogger] ILogger<MacroExpander> logger,
                                                                                                                       MacroExpander sut,
                                                                                                                       MetalMacro macro,
                                                                                                                       ExpressionContext context,
                                                                                                                       Slot filler1,
                                                                                                                       Slot filler2,
                                                                                                                       Slot defined1,
                                                                                                                       Slot defined2)
        {
            Mock.Get(slotFinder)
                .Setup(x => x.GetSlotFillers(context.CurrentElement))
                .Returns(new[] { filler1, filler2 });
            Mock.Get(slotFinder)
                .Setup(x => x.GetDefinedSlots(macro.Element))
                .Returns(new[] { defined1, defined2 });
            Mock.Get(macroProvider)
                .Setup(x => x.GetMacroAsync(macro.Element, context, specProvider.ExtendMacro, CancellationToken.None))
                .Returns(Task.FromResult<MetalMacro>(null));

            var result = await sut.ExpandMacroAsync(macro, context);

            Assert.That(result, Is.SameAs(macro), "Returned object is expected macro");
            var comparer = new CSF.Collections.SetEqualityComparer<Slot>();
            Mock.Get(slotFiller)
                .Verify(x => x.FillSlots(It.Is<MacroExpansionContext>(ctx => ctx.Macro == macro && comparer.Equals(ctx.SlotFillers.Values, new[] { filler1, filler2 })),
                                         new[] { defined1, defined2 }),
                        Times.Once,
                        "Expected slots are filled with expected fillers");
        }

        [Test, AutoMoqData]
        public async Task ExpandMacroAsync_fills_slots_in_extended_macro_defined_in_extender([Frozen] IGetsMacro macroProvider,
                                                                                             [Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                             [Frozen] IGetsSlots slotFinder,
                                                                                             [Frozen] IFillsSlots slotFiller,
                                                                                             [Frozen, MockLogger] ILogger<MacroExpander> logger,
                                                                                             MacroExpander sut,
                                                                                             MetalMacro macro,
                                                                                             MetalMacro extended,
                                                                                             ExpressionContext context,
                                                                                             Slot filler1,
                                                                                             Slot filler2,
                                                                                             Slot defined1,
                                                                                             Slot defined2)
        {
            Mock.Get(slotFinder)
                .Setup(x => x.GetSlotFillers(context.CurrentElement))
                .Returns(Enumerable.Empty<Slot>());
            Mock.Get(slotFinder)
                .Setup(x => x.GetDefinedSlots(macro.Element))
                .Returns(Enumerable.Empty<Slot>());
            Mock.Get(macroProvider)
                .Setup(x => x.GetMacroAsync(macro.Element, context, specProvider.ExtendMacro, CancellationToken.None))
                .Returns(Task.FromResult(extended));
            Mock.Get(slotFinder)
                .Setup(x => x.GetSlotFillers(macro.Element))
                .Returns(new[] { filler1, filler2 });
            Mock.Get(slotFinder)
                .Setup(x => x.GetDefinedSlots(extended.Element))
                .Returns(new[] { defined1, defined2 });
            Mock.Get(macroProvider)
                .Setup(x => x.GetMacroAsync(extended.Element, context, specProvider.ExtendMacro, CancellationToken.None))
                .Returns(Task.FromResult<MetalMacro>(null));

            var result = await sut.ExpandMacroAsync(macro, context);

            Assert.That(result, Is.SameAs(extended), "Returned object is expected macro");
            var comparer = new CSF.Collections.SetEqualityComparer<Slot>();
            Mock.Get(slotFiller)
                .Verify(x => x.FillSlots(It.Is<MacroExpansionContext>(ctx => ctx.Macro == extended && comparer.Equals(ctx.SlotFillers.Values, new[] { filler1, filler2 })),
                                         new[] { defined1, defined2 }),
                        Times.Once,
                        "Expected slots are filled with expected fillers");
        }

        [Test, AutoMoqData]
        public async Task ExpandMacroAsync_fills_mixture_of_slots_in_extended_macro_defined_in_extender_and_used_macro([Frozen] IGetsMacro macroProvider,
                                                                                                                       [Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                                                       [Frozen] IGetsSlots slotFinder,
                                                                                                                       [Frozen] IFillsSlots slotFiller,
                                                                                                                       [Frozen, MockLogger] ILogger<MacroExpander> logger,
                                                                                                                       MacroExpander sut,
                                                                                                                       MetalMacro macro,
                                                                                                                       MetalMacro extended,
                                                                                                                       ExpressionContext context,
                                                                                                                       Slot filler1,
                                                                                                                       Slot filler2,
                                                                                                                       Slot defined1,
                                                                                                                       Slot defined2)
        {
            Mock.Get(slotFinder)
                .Setup(x => x.GetSlotFillers(context.CurrentElement))
                .Returns(new[] { filler1 });
            Mock.Get(slotFinder)
                .Setup(x => x.GetDefinedSlots(macro.Element))
                .Returns(Enumerable.Empty<Slot>());
            Mock.Get(macroProvider)
                .Setup(x => x.GetMacroAsync(macro.Element, context, specProvider.ExtendMacro, CancellationToken.None))
                .Returns(Task.FromResult(extended));
            Mock.Get(slotFinder)
                .Setup(x => x.GetSlotFillers(macro.Element))
                .Returns(new[] { filler2 });
            Mock.Get(slotFinder)
                .Setup(x => x.GetDefinedSlots(extended.Element))
                .Returns(new[] { defined1, defined2 });
            Mock.Get(macroProvider)
                .Setup(x => x.GetMacroAsync(extended.Element, context, specProvider.ExtendMacro, CancellationToken.None))
                .Returns(Task.FromResult<MetalMacro>(null));

            var result = await sut.ExpandMacroAsync(macro, context);

            Assert.That(result, Is.SameAs(extended), "Returned object is expected macro");
            var comparer = new CSF.Collections.SetEqualityComparer<Slot>();
            Mock.Get(slotFiller)
                .Verify(x => x.FillSlots(It.Is<MacroExpansionContext>(ctx => ctx.Macro == extended && comparer.Equals(ctx.SlotFillers.Values, new[] { filler1, filler2 })),
                                         new[] { defined1, defined2 }),
                        Times.Once,
                        "Expected slots are filled with expected fillers");
        }

        [Test, AutoMoqData]
        public async Task ExpandMacroAsync_uses_filler_defined_at_parent_level_over_filler_at_child_level_if_slot_not_redefined([Frozen] IGetsMacro macroProvider,
                                                                                                                                [Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                                                                [Frozen] IGetsSlots slotFinder,
                                                                                                                                [Frozen] IFillsSlots slotFiller,
                                                                                                                                [Frozen, MockLogger] ILogger<MacroExpander> logger,
                                                                                                                                MacroExpander sut,
                                                                                                                                MetalMacro macro,
                                                                                                                                MetalMacro extended,
                                                                                                                                ExpressionContext context,
                                                                                                                                string slotName,
                                                                                                                                [StubDom] INode element1,
                                                                                                                                [StubDom] INode element2,
                                                                                                                                [StubDom] INode element3)
        {
            var parentFiller = new Slot(slotName, element1);
            var childFiller = new Slot(slotName, element2);
            var defined = new Slot(slotName, element3);

            Mock.Get(slotFinder)
                .Setup(x => x.GetSlotFillers(context.CurrentElement))
                .Returns(new[] { childFiller });
            Mock.Get(slotFinder)
                .Setup(x => x.GetDefinedSlots(macro.Element))
                .Returns(Enumerable.Empty<Slot>());
            Mock.Get(macroProvider)
                .Setup(x => x.GetMacroAsync(macro.Element, context, specProvider.ExtendMacro, CancellationToken.None))
                .Returns(Task.FromResult(extended));
            Mock.Get(slotFinder)
                .Setup(x => x.GetSlotFillers(macro.Element))
                .Returns(new[] { parentFiller });
            Mock.Get(slotFinder)
                .Setup(x => x.GetDefinedSlots(extended.Element))
                .Returns(new[] { defined });
            Mock.Get(macroProvider)
                .Setup(x => x.GetMacroAsync(extended.Element, context, specProvider.ExtendMacro, CancellationToken.None))
                .Returns(Task.FromResult<MetalMacro>(null));

            var result = await sut.ExpandMacroAsync(macro, context);

            Assert.That(result, Is.SameAs(extended), "Returned object is expected macro");
            var comparer = new CSF.Collections.SetEqualityComparer<Slot>();
            Mock.Get(slotFiller)
                .Verify(x => x.FillSlots(It.Is<MacroExpansionContext>(ctx => ctx.Macro == extended && comparer.Equals(ctx.SlotFillers.Values, new[] { parentFiller })),
                                         new[] { defined }),
                        Times.Once,
                        "Expected slots are filled with expected fillers");
        }
    }
}
