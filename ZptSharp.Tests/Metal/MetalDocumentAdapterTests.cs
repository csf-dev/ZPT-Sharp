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
    public class MetalDocumentAdapterTests
    {
        [Test, AutoMoqData]
        public void GetMacros_returns_results_from_attribute_searcher([Frozen] ISearchesForAttributes attributeSearcher,
                                                                      [Frozen] IDocument doc,
                                                                      [Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                      AttributeSpec spec,
                                                                      MetalDocumentAdapter sut,
                                                                      [StubDom] IAttribute attrib1,
                                                                      [StubDom] IAttribute attrib2)
        {
            Mock.Get(specProvider).SetupGet(x => x.DefineMacro).Returns(spec);
            var attribs = new[] { attrib1, attrib2 };
            Mock.Get(attributeSearcher).Setup(x => x.SearchForAttributes(doc, spec)).Returns(attribs);

            var result = sut.GetMacros();

            Assert.That(result, Has.Count.EqualTo(attribs.Length));
        }

        [Test, AutoMoqData]
        public void GetMacros_returns_results_with_correct_keys([Frozen] ISearchesForAttributes attributeSearcher,
                                                                [Frozen] IDocument doc,
                                                                [Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                AttributeSpec spec,
                                                                MetalDocumentAdapter sut,
                                                                [StubDom] IAttribute attrib1,
                                                                [StubDom] IAttribute attrib2)
        {
            Mock.Get(specProvider).SetupGet(x => x.DefineMacro).Returns(spec);
            var attribs = new[] { attrib1, attrib2 };
            Mock.Get(attributeSearcher).Setup(x => x.SearchForAttributes(doc, spec)).Returns(attribs);

            var result = sut.GetMacros();

            Assert.That(() => result.ContainsKey(attrib1.Value), Is.True, "Contains key 1");
            Assert.That(() => result.ContainsKey(attrib2.Value), Is.True, "Contains key 2");
        }

        [Test, AutoMoqData]
        public void GetMacros_returns_results_with_correct_values([Frozen] ISearchesForAttributes attributeSearcher,
                                                                  [Frozen] IDocument doc,
                                                                  [Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                  AttributeSpec spec,
                                                                  MetalDocumentAdapter sut,
                                                                  [StubDom] IAttribute attrib1,
                                                                  [StubDom] IAttribute attrib2)
        {
            Mock.Get(specProvider).SetupGet(x => x.DefineMacro).Returns(spec);
            var attribs = new[] { attrib1, attrib2 };
            Mock.Get(attributeSearcher).Setup(x => x.SearchForAttributes(doc, spec)).Returns(attribs);

            var result = sut.GetMacros();

            Assert.That(() => result[attrib1.Value].Element,
                        Is.SameAs(attrib1.Element),
                        $"Attribute 1 macro {nameof(IAttribute.Element)} is correct");
            Assert.That(() => result[attrib1.Value].Name,
                        Is.SameAs(attrib1.Value),
                        $"Attribute 1 macro {nameof(IAttribute.Name)} is correct");
            Assert.That(() => result[attrib2.Value].Element,
                        Is.SameAs(attrib2.Element),
                        $"Attribute 2 macro {nameof(IAttribute.Element)} is correct");
            Assert.That(() => result[attrib2.Value].Name,
                        Is.SameAs(attrib2.Value),
                        $"Attribute 2 macro {nameof(IAttribute.Name)} is correct");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_true_if_macros_requested([Frozen] ISearchesForAttributes attributeSearcher,
                                                                 MetalDocumentAdapter sut)
        {
            Mock.Get(attributeSearcher)
                .Setup(x => x.SearchForAttributes(It.IsAny<IHasElements>(), It.IsAny<AttributeSpec>()))
                .Returns(() => Enumerable.Empty<IAttribute>());
            Assert.That(() => sut.TryGetValue("macros", out object val), Is.True);
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_false_if_other_value_requested(MetalDocumentAdapter sut)
        {
            Assert.That(() => sut.TryGetValue("something else", out object val), Is.False);
        }
    }
}
