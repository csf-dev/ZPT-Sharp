using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    [TestFixture]
    public class SlotFillerTests
    {
        [Test, AutoMoqData]
        public void FillSlots_does_not_fill_a_slot_which_has_no_filler(Slot defined,
                                                                       MacroExpansionContext context,
                                                                       [Frozen, MockLogger] ILogger<SlotFiller> logger,
                                                                       [Frozen] IReplacesNode replacer,
                                                                       SlotFiller sut)
        {
            context.SlotFillers.Clear();

            sut.FillSlots(context, new[] { defined });

            Mock.Get(replacer).Verify(x => x.Replace(It.IsAny<INode>(), It.IsAny<IList<INode>>()), Times.Never);
        }

        [Test, AutoMoqData]
        public void FillSlots_replaces_the_element_with_a_copy_of_its_filler(INode defineSlotElement,
                                                                             INode fillSlotElement,
                                                                             INode copiedFiller,
                                                                             string slotName,
                                                                             MacroExpansionContext context,
                                                                             [Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                             [Frozen, MockLogger] ILogger<SlotFiller> logger,
                                                                             [Frozen] IReplacesNode replacer,
                                                                             SlotFiller sut,
                                                                             AttributeSpec spec)
        {
            Mock.Get(fillSlotElement).Setup(x => x.GetCopy()).Returns(copiedFiller);
            context.SlotFillers.Clear();
            context.SlotFillers.Add(slotName, new Slot(slotName, fillSlotElement));
            Mock.Get(specProvider).SetupGet(x => x.FillSlot).Returns(spec);

            sut.FillSlots(context, new[] { new Slot(slotName, defineSlotElement) });

            Mock.Get(replacer).Verify(x => x.Replace(defineSlotElement, new[] { copiedFiller }), Times.Once);
        }

        [Test, AutoMoqData]
        public void FillSlots_removes_a_slot_filler_from_the_context_once_it_is_used(INode defineSlotElement,
                                                                                     INode fillSlotElement,
                                                                                     string slotName,
                                                                                     MacroExpansionContext context,
                                                                                     [Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                     [Frozen, MockLogger] ILogger<SlotFiller> logger,
                                                                                     [Frozen] IReplacesNode replacer,
                                                                                     SlotFiller sut,
                                                                                     AttributeSpec spec)
        {
            context.SlotFillers.Clear();
            context.SlotFillers.Add(slotName, new Slot(slotName, fillSlotElement));
            Mock.Get(specProvider).SetupGet(x => x.FillSlot).Returns(spec);

            sut.FillSlots(context, new[] { new Slot(slotName, defineSlotElement) });

            Assert.That(context.SlotFillers, Is.Empty);
        }

        [Test, AutoMoqData]
        public void FillSlots_copies_a_fillslot_attribute_from_the_defining_element_to_the_filler([StubDom] INode defineSlotElement,
                                                                                                  [StubDom] INode fillSlotElement,
                                                                                                  string slotName,
                                                                                                  MacroExpansionContext context,
                                                                                                  [Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                                  [Frozen, MockLogger] ILogger<SlotFiller> logger,
                                                                                                  [Frozen] IReplacesNode replacer,
                                                                                                  SlotFiller sut,
                                                                                                  [StubDom] IAttribute fillSlotAttribute,
                                                                                                  AttributeSpec fillSlotSpec)
        {
            context.SlotFillers.Clear();
            context.SlotFillers.Add(slotName, new Slot(slotName, fillSlotElement));
            defineSlotElement.Attributes.Add(fillSlotAttribute);
            Mock.Get(specProvider).SetupGet(x => x.FillSlot).Returns(fillSlotSpec);
            Mock.Get(fillSlotAttribute).Setup(x => x.Matches(fillSlotSpec)).Returns(true);

            sut.FillSlots(context, new[] { new Slot(slotName, defineSlotElement) });

            Assert.That(fillSlotElement.Attributes, Has.One.SameAs(fillSlotAttribute));
        }
    }
}
