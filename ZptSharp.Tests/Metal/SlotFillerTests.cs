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
                                                                       INode parent,
                                                                       MacroExpansionContext context,
                                                                       SlotFiller sut)
        {
            Mock.Get(defined.Element).SetupProperty(x => x.ParentElement, parent);
            context.SlotFillers.Clear();

            sut.FillSlots(context, new[] { defined });

            Mock.Get(parent).Verify(x => x.ReplaceChild(It.IsAny<INode>(), It.IsAny<INode>()), Times.Never);
        }

        [Test, AutoMoqData]
        public void FillSlots_replaces_the_element_with_a_copy_of_its_filler(INode defineSlotElement,
                                                                             INode fillSlotElement,
                                                                             INode copiedFiller,
                                                                             INode parent,
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
        public void FillSlots_removes_a_slot_filler_from_the_context_once_it_is_used(INode defineSlotElement,
                                                                                     INode fillSlotElement,
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
