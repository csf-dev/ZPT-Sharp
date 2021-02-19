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
        public void GetSlotFillers_gets_nodes_with_fill_slot_attributes([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                           [Frozen] ISearchesForAttributes attributeFinder,
                                                                           SlotFinder sut,
                                                                           [StubDom] INode node,
                                                                           AttributeSpec spec,
                                                                           [StubDom] IAttribute attr1,
                                                                           [StubDom] INode attr1Node,
                                                                           [StubDom] IAttribute attr2,
                                                                           [StubDom] INode attr2Node)
        {
            Mock.Get(specProvider).SetupGet(x => x.FillSlot).Returns(spec);
            Mock.Get(attributeFinder).Setup(x => x.SearchForAttributes(node, spec)).Returns(new[] { attr1, attr2 });
            Mock.Get(attr1).SetupGet(x => x.Node).Returns(attr1Node);
            Mock.Get(attr2).SetupGet(x => x.Node).Returns(attr2Node);

            var result = sut.GetSlotFillers(node);

            Assert.That(result?.Select(x => x.Node), Is.EquivalentTo(new[] { attr1Node, attr2Node }));
        }

        [Test, AutoMoqData]
        public void GetDefinedSlots_gets_nodes_with_define_slot_attributes([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                              [Frozen] ISearchesForAttributes attributeFinder,
                                                                              SlotFinder sut,
                                                                              [StubDom] INode node,
                                                                              AttributeSpec spec,
                                                                              [StubDom] IAttribute attr1,
                                                                              [StubDom] INode attr1Node,
                                                                              [StubDom] IAttribute attr2,
                                                                              [StubDom] INode attr2Node)
        {
            Mock.Get(specProvider).SetupGet(x => x.DefineSlot).Returns(spec);
            Mock.Get(attributeFinder).Setup(x => x.SearchForAttributes(node, spec)).Returns(new[] { attr1, attr2 });
            Mock.Get(attr1).SetupGet(x => x.Node).Returns(attr1Node);
            Mock.Get(attr2).SetupGet(x => x.Node).Returns(attr2Node);

            var result = sut.GetDefinedSlots(node);

            Assert.That(result?.Select(x => x.Node), Is.EquivalentTo(new[] { attr1Node, attr2Node }));
        }
    }
}
