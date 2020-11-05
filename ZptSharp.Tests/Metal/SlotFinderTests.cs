using System;
using System.Linq;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    [TestFixture,Parallelizable]
    public class SlotFinderTests
    {
        [Test, AutoMoqData]
        public void GetSlotFillers_gets_elements_with_fill_slot_attributes([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                           [Frozen] ISearchesForAttributes attributeFinder,
                                                                           SlotFinder sut,
                                                                           [StubDom] IElement element,
                                                                           AttributeSpec spec,
                                                                           [StubDom] IAttribute attr1,
                                                                           [StubDom] IElement attr1Element,
                                                                           [StubDom] IAttribute attr2,
                                                                           [StubDom] IElement attr2Element)
        {
            Mock.Get(specProvider).SetupGet(x => x.FillSlot).Returns(spec);
            Mock.Get(attributeFinder).Setup(x => x.SearchForAttributes(element, spec)).Returns(new[] { attr1, attr2 });
            Mock.Get(attr1).SetupGet(x => x.Element).Returns(attr1Element);
            Mock.Get(attr2).SetupGet(x => x.Element).Returns(attr2Element);

            var result = sut.GetSlotFillers(element);

            Assert.That(result?.Select(x => x.Element), Is.EquivalentTo(new[] { attr1Element, attr2Element }));
        }

        [Test, AutoMoqData]
        public void GetDefinedSlots_gets_elements_with_define_slot_attributes([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                              [Frozen] ISearchesForAttributes attributeFinder,
                                                                              SlotFinder sut,
                                                                              [StubDom] IElement element,
                                                                              AttributeSpec spec,
                                                                              [StubDom] IAttribute attr1,
                                                                              [StubDom] IElement attr1Element,
                                                                              [StubDom] IAttribute attr2,
                                                                              [StubDom] IElement attr2Element)
        {
            Mock.Get(specProvider).SetupGet(x => x.DefineSlot).Returns(spec);
            Mock.Get(attributeFinder).Setup(x => x.SearchForAttributes(element, spec)).Returns(new[] { attr1, attr2 });
            Mock.Get(attr1).SetupGet(x => x.Element).Returns(attr1Element);
            Mock.Get(attr2).SetupGet(x => x.Element).Returns(attr2Element);

            var result = sut.GetDefinedSlots(element);

            Assert.That(result?.Select(x => x.Element), Is.EquivalentTo(new[] { attr1Element, attr2Element }));
        }
    }
}
