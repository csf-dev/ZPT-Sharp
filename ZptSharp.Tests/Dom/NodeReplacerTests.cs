using System;
using NUnit.Framework;
using ZptSharp.Autofixture;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class NodeReplacerTests
    {
        [Test, AutoMoqData]
        public void Repalce_replaces_node_with_the_specified_replacements([MockLogger] Microsoft.Extensions.Logging.ILogger<NodeReplacer> logger,
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
    }
}
