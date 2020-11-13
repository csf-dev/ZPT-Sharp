using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class AttributeSearcherTests
    {
        [Test, AutoMoqData]
        public void SearchForAttributes_returns_all_matching_attributes_including_from_grandchild_elements([StubDom, Frozen] IDocument doc,
                                                                                                           [StubDom] INode root,
                                                                                                           [StubDom] INode child1,
                                                                                                           [StubDom] INode child2,
                                                                                                           [StubDom] INode grandchild1,
                                                                                                           [StubDom] INode grandchild2,
                                                                                                           [StubDom] IAttribute matchingAttrib1,
                                                                                                           [StubDom] IAttribute matchingAttrib2,
                                                                                                           AttributeSpec spec,
                                                                                                           AttributeSearcher sut)
        {
            Mock.Get(doc).SetupGet(x => x.RootElement).Returns(root);
            root.ChildNodes.Add(child1);
            root.ChildNodes.Add(child2);
            child1.ChildNodes.Add(grandchild1);
            child2.ChildNodes.Add(grandchild2);
            root.Attributes.Add(matchingAttrib1);
            grandchild2.Attributes.Add(matchingAttrib2);
            Mock.Get(matchingAttrib1).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(matchingAttrib2).Setup(x => x.Matches(spec)).Returns(true);

            Assert.That(() => sut.SearchForAttributes(doc, spec), Is.EquivalentTo(new[] { matchingAttrib1, matchingAttrib2 }));
        }

        [Test, AutoMoqData]
        public void SearchForAttributes_does_not_return_non_matching_attributes([StubDom, Frozen] IDocument doc,
                                                                                [StubDom] INode root,
                                                                                [StubDom] IAttribute nonMatchingAttrib,
                                                                                AttributeSpec spec,
                                                                                AttributeSearcher sut)
        {
            Mock.Get(doc).SetupGet(x => x.RootElement).Returns(root);
            root.Attributes.Add(nonMatchingAttrib);

            Mock.Get(nonMatchingAttrib).Setup(x => x.Matches(spec)).Returns(false);

            Assert.That(() => sut.SearchForAttributes(doc, spec), Is.Empty);
        }

        [Test, AutoMoqData]
        public void SearchForAttributes_returns_an_attribute_from_the_root_element_if_used_as_the_basis_for_search([StubDom] INode root,
                                                                                                                   [StubDom] IAttribute matchingAttrib,
                                                                                                                   AttributeSpec spec,
                                                                                                                   AttributeSearcher sut)
        {
            root.Attributes.Add(matchingAttrib);

            Mock.Get(matchingAttrib).Setup(x => x.Matches(spec)).Returns(true);

            Assert.That(() => sut.SearchForAttributes(root, spec), Is.EquivalentTo(new[] { matchingAttrib }));
        }
    }
}
