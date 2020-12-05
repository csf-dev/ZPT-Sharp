using System;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class NodeReplacerTests
    {
        [Test, AutoMoqData]
        public void Replace_replaces_node_with_the_specified_replacements([MockLogger] Microsoft.Extensions.Logging.ILogger<NodeReplacer> logger,
                                                                          NodeReplacer sut,
                                                                          [StubDom] INode parent,
                                                                          [StubDom] INode toReplace,
                                                                          [StubDom] INode replacement1,
                                                                          [StubDom] INode replacement2,
                                                                          [StubDom] INode sibling1,
                                                                          [StubDom] INode sibling2)
        {
            parent.ChildNodes.Clear();
            parent.ChildNodes.Add(sibling1);
            parent.ChildNodes.Add(toReplace);
            parent.ChildNodes.Add(sibling2);
            toReplace.ParentElement = parent;

            sut.Replace(toReplace, new[] { replacement1, replacement2 });

            Assert.That(parent.ChildNodes, Is.EqualTo(new[] { sibling1, replacement1, replacement2, sibling2 }));
        }

        [Test, AutoMoqData]
        public void Replace_uses_root_replacer_if_parent_is_empty(NodeReplacer sut,
                                                                  [StubDom] INode toReplace,
                                                                  [StubDom] INode replacement)
        {
            var doc = new Mock<IDocument>();
            doc.As<ICanReplaceRootElement>();
            Mock.Get(toReplace).SetupGet(x => x.Document).Returns(doc.Object);
            Mock.Get(toReplace).SetupGet(x => x.ParentElement).Returns(() => null);

            sut.Replace(toReplace, new[] { replacement });

            doc.As<ICanReplaceRootElement>()
                .Verify(x => x.ReplaceRootElement(replacement), Times.Once);
        }

        [Test, AutoMoqData]
        public void Replace_throws_ArgEx_if_parent_is_empty_and_document_is_not_root_replacer(NodeReplacer sut,
                                                                                              [StubDom] INode toReplace,
                                                                                              [StubDom] INode replacement)
        {
            var doc = new Mock<IDocument>();
            Mock.Get(toReplace).SetupGet(x => x.Document).Returns(doc.Object);
            Mock.Get(toReplace).SetupGet(x => x.ParentElement).Returns(() => null);

            Assert.That(() => sut.Replace(toReplace, new[] { replacement }), Throws.ArgumentException);
        }
    }
}
