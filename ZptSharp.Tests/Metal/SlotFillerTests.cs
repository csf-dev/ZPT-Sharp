using System;
using Moq;
using NUnit.Framework;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    [TestFixture]
    public class SlotFillerTests
    {
        [Test, AutoMoqData]
        public void FillSlots_does_not_fill_a_slot_which_has_no_filler(Slot defined,
                                                                       IElement parent,
                                                                       MacroExpansionContext context,
                                                                       SlotFiller sut)
        {
            Mock.Get(defined.Element).SetupProperty(x => x.ParentElement, parent);
            context.SlotFillers.Clear();

            sut.FillSlots(context, new[] { defined });

            Mock.Get(parent).Verify(x => x.ReplaceChild(It.IsAny<IElement>(), It.IsAny<IElement>()), Times.Never);
        }

        [Test, AutoMoqData]
        public void FillSlots_replaces_the_element_with_a_copy_of_its_filler(IElement defineSlotElement,
                                                                             IElement fillSlotElement,
                                                                             IElement copiedFiller,
                                                                             IElement parent,
                                                                             string slotName,
                                                                             MacroExpansionContext context,
                                                                             SlotFiller sut)
        {
            Mock.Get(defineSlotElement).SetupProperty(x => x.ParentElement, parent);
            Mock.Get(fillSlotElement).Setup(x => x.GetCopy()).Returns(copiedFiller);
            context.SlotFillers.Clear();
            context.SlotFillers.Add(slotName, new Slot(slotName, fillSlotElement));

            sut.FillSlots(context, new[] { new Slot(slotName, defineSlotElement) });

            Mock.Get(parent).Verify(x => x.ReplaceChild(defineSlotElement, copiedFiller), Times.Once);
        }

        [Test, AutoMoqData]
        public void FillSlots_removes_a_slot_filler_from_the_context_once_it_is_used(IElement defineSlotElement,
                                                                                     IElement fillSlotElement,
                                                                                     string slotName,
                                                                                     MacroExpansionContext context,
                                                                                     SlotFiller sut)
        {
            context.SlotFillers.Clear();
            context.SlotFillers.Add(slotName, new Slot(slotName, fillSlotElement));

            sut.FillSlots(context, new[] { new Slot(slotName, defineSlotElement) });

            Assert.That(context.SlotFillers, Is.Empty);
        }
    }
}
