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
        public void Omit_replaces_the_element_with_its_children([Frozen] IReplacesNode replacer,
                                                                NodeOmitter sut,
                                                                [StubDom] INode element,
                                                                [StubDom] INode child1,
                                                                [StubDom] INode child2)
        {
            element.ChildNodes.Clear();
            element.ChildNodes.Add(child1);
            element.ChildNodes.Add(child2);

            sut.Omit(element);

            Mock.Get(replacer).Verify(x => x.Replace(element, new[] { child1, child2 }), Times.Once);
        }
    }
}
