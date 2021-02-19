using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;

namespace ZptSharp.Dom
{
    [TestFixture, Parallelizable]
    public class NodeOmitterTests
    {
        [Test, AutoMoqData]
        public void Omit_replaces_the_node_with_its_children([Frozen] IReplacesNode replacer,
                                                                NodeOmitter sut,
                                                                [StubDom] INode node,
                                                                [StubDom] INode child1,
                                                                [StubDom] INode child2)
        {
            node.ChildNodes.Clear();
            node.ChildNodes.Add(child1);
            node.ChildNodes.Add(child2);

            sut.Omit(node);

            Mock.Get(replacer).Verify(x => x.Replace(node, new[] { child1, child2 }), Times.Once);
        }
    }
}
