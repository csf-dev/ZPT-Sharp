using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class ZptCleanupContextProcessorTests
    {
        [Test, AutoMoqData]
        public async Task ProcessContextAsync_removes_entire_element_if_it_is_in_metal_namespace([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                                                 [MockLogger, Frozen] ILogger<ZptCleanupContextProcessor> logger,
                                                                                                 [Frozen] IOmitsNode omitter,
                                                                                                 ZptCleanupContextProcessor sut,
                                                                                                 ExpressionContext context,
                                                                                                 INode node,
                                                                                                 Namespace metal)
        {
            context.CurrentElement = node;
            Mock.Get(namespaceProvider).Setup(x => x.MetalNamespace).Returns(metal);
            Mock.Get(node).Setup(x => x.IsInNamespace(metal)).Returns(true);

            await sut.ProcessContextAsync(context);

            Mock.Get(omitter).Verify(x => x.Omit(node), Times.Once);
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_removes_entire_element_if_it_is_in_tal_namespace([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                                               [MockLogger, Frozen] ILogger<ZptCleanupContextProcessor> logger,
                                                                                               [Frozen] IOmitsNode omitter,
                                                                                               ZptCleanupContextProcessor sut,
                                                                                               ExpressionContext context,
                                                                                               INode node,
                                                                                               Namespace tal)
        {
            context.CurrentElement = node;
            Mock.Get(namespaceProvider).Setup(x => x.TalNamespace).Returns(tal);
            Mock.Get(node).Setup(x => x.IsInNamespace(tal)).Returns(true);

            await sut.ProcessContextAsync(context);

            Mock.Get(omitter).Verify(x => x.Omit(node), Times.Once);
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_does_not_remove_element_if_it_is_not_in_tal_or_metal_namespaces([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                                                              [MockLogger, Frozen] ILogger<ZptCleanupContextProcessor> logger,
                                                                                                              [Frozen] IOmitsNode omitter,
                                                                                                              ZptCleanupContextProcessor sut,
                                                                                                              ExpressionContext context,
                                                                                                              INode node,
                                                                                                              Namespace metal,
                                                                                                              Namespace tal)
        {
            context.CurrentElement = node;
            Mock.Get(namespaceProvider).Setup(x => x.MetalNamespace).Returns(metal);
            Mock.Get(namespaceProvider).Setup(x => x.TalNamespace).Returns(tal);
            Mock.Get(node).Setup(x => x.IsInNamespace(metal)).Returns(false);
            Mock.Get(node).Setup(x => x.IsInNamespace(tal)).Returns(false);

            await sut.ProcessContextAsync(context);

            Mock.Get(omitter).Verify(x => x.Omit(node), Times.Never);
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_removes_an_attribute_if_it_is_in_the_metal_namespace([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                                                   [MockLogger, Frozen] ILogger<ZptCleanupContextProcessor> logger,
                                                                                                   ZptCleanupContextProcessor sut,
                                                                                                   ExpressionContext context,
                                                                                                   [StubDom] INode node,
                                                                                                   [StubDom] IAttribute attrib,
                                                                                                   Namespace metal,
                                                                                                   Namespace tal)
        {
            context.CurrentElement = node;
            Mock.Get(namespaceProvider).Setup(x => x.MetalNamespace).Returns(metal);
            Mock.Get(namespaceProvider).Setup(x => x.TalNamespace).Returns(tal);
            Mock.Get(node).Setup(x => x.IsInNamespace(It.IsAny<Namespace>())).Returns(false);
            node.Attributes.Clear();
            node.Attributes.Add(attrib);
            Mock.Get(attrib).Setup(x => x.IsInNamespace(metal)).Returns(true);

            await sut.ProcessContextAsync(context);

            Assert.That(node.Attributes, Is.Empty);
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_removes_an_attribute_if_it_is_in_the_tal_namespace([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                                                 [MockLogger, Frozen] ILogger<ZptCleanupContextProcessor> logger,
                                                                                                 ZptCleanupContextProcessor sut,
                                                                                                 ExpressionContext context,
                                                                                                 [StubDom] INode node,
                                                                                                 [StubDom] IAttribute attrib,
                                                                                                 Namespace metal,
                                                                                                 Namespace tal)
        {
            context.CurrentElement = node;
            Mock.Get(namespaceProvider).Setup(x => x.MetalNamespace).Returns(metal);
            Mock.Get(namespaceProvider).Setup(x => x.TalNamespace).Returns(tal);
            Mock.Get(node).Setup(x => x.IsInNamespace(It.IsAny<Namespace>())).Returns(false);
            node.Attributes.Clear();
            node.Attributes.Add(attrib);
            Mock.Get(attrib).Setup(x => x.IsInNamespace(tal)).Returns(true);

            await sut.ProcessContextAsync(context);

            Assert.That(node.Attributes, Is.Empty);
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_removes_an_attribute_if_it_is_not_in_either_tal_or_metal_namespaces([Frozen] IGetsWellKnownNamespace namespaceProvider,
                                                                                                                  [MockLogger, Frozen] ILogger<ZptCleanupContextProcessor> logger,
                                                                                                                  ZptCleanupContextProcessor sut,
                                                                                                                  ExpressionContext context,
                                                                                                                  [StubDom] INode node,
                                                                                                                  [StubDom] IAttribute attrib,
                                                                                                                  Namespace metal,
                                                                                                                  Namespace tal)
        {
            context.CurrentElement = node;
            Mock.Get(namespaceProvider).Setup(x => x.MetalNamespace).Returns(metal);
            Mock.Get(namespaceProvider).Setup(x => x.TalNamespace).Returns(tal);
            Mock.Get(node).Setup(x => x.IsInNamespace(It.IsAny<Namespace>())).Returns(false);
            node.Attributes.Clear();
            node.Attributes.Add(attrib);
            Mock.Get(attrib).Setup(x => x.IsInNamespace(tal)).Returns(false);
            Mock.Get(attrib).Setup(x => x.IsInNamespace(metal)).Returns(false);

            await sut.ProcessContextAsync(context);

            Assert.That(node.Attributes, Is.EqualTo(new[] { attrib }));
        }
    }
}
