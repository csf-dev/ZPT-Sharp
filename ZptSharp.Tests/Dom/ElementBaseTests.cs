using System;
using NUnit.Framework;
using ZptSharp.Autofixture;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class ElementBaseTests
    {
        #region ReplaceChild

        [Test, AutoMoqData]
        public void ReplaceChild_can_replace_node_in_middle_of_collection(StubElement sut,
                                                                          [StubDom] INode node1,
                                                                          [StubDom] INode node2,
                                                                          [StubDom] INode node3,
                                                                          [StubDom] INode node4)
        {
            sut.ChildNodes.Clear();
            sut.ChildNodes.Add(node1);
            node1.ParentElement = sut;
            sut.ChildNodes.Add(node2);
            node2.ParentElement = sut;
            sut.ChildNodes.Add(node3);
            node3.ParentElement = sut;

            sut.ReplaceChild(node2, node4);

            Assert.That(sut.ChildNodes, Is.EqualTo(new[] { node1, node4, node3 }));
        }

        [Test, AutoMoqData]
        public void ReplaceChild_throws_if_node_to_replace_is_not_child_of_element(StubElement sut,
                                                                                   [StubDom] INode node1,
                                                                                   [StubDom] INode node2)
        {
            sut.ChildNodes.Clear();
            Assert.That(() => sut.ReplaceChild(node1, node2), Throws.ArgumentException);
        }

        [Test, AutoMoqData]
        public void ReplaceChild_throws_ANE_if_node_to_replace_is_null(StubElement sut, [StubDom] INode node)
        {
            sut.ChildNodes.Clear();
            Assert.That(() => sut.ReplaceChild(null, node), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void ReplaceChild_throws_ANE_if_replacement_is_null(StubElement sut, [StubDom] INode node)
        {
            sut.ChildNodes.Clear();
            sut.ChildNodes.Add(node);
            node.ParentElement = sut;

            Assert.That(() => sut.ReplaceChild(node, null), Throws.ArgumentNullException);
        }

        #endregion

        #region Omit

        [Test, AutoMoqData]
        public void Omit_throws_if_node_has_no_parent(StubElement sut)
        {
            sut.ParentElement = null;
            Assert.That(() => sut.Omit(), Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Omit_removes_node_from_parents_children(StubElement parent,
                                                            StubElement sut,
                                                            [StubDom] INode sibling1,
                                                            [StubDom] INode sibling2)
        {
            parent.ChildNodes.Clear();
            parent.ChildNodes.Add(sibling1);
            sibling1.ParentElement = parent;
            parent.ChildNodes.Add(sut);
            sut.ParentElement = parent;
            parent.ChildNodes.Add(sibling2);
            sibling2.ParentElement = parent;

            sut.Omit();

            Assert.That(parent.ChildNodes, Is.EqualTo(new[] { sibling1, sibling2 }));
        }

        [Test, AutoMoqData]
        public void Omit_replaces_node_on_parent_with_its_children(StubElement parent,
                                                            StubElement sut,
                                                            [StubDom] INode sibling1,
                                                            [StubDom] INode sibling2,
                                                            [StubDom] INode child1,
                                                            [StubDom] INode child2)
        {
            parent.ChildNodes.Clear();
            parent.ChildNodes.Add(sibling1);
            sibling1.ParentElement = parent;
            parent.ChildNodes.Add(sut);
            sut.ParentElement = parent;
            parent.ChildNodes.Add(sibling2);
            sibling2.ParentElement = parent;
            sut.ChildNodes.Clear();
            sut.ChildNodes.Add(child1);
            sut.ChildNodes.Add(child2);

            sut.Omit();

            Assert.That(parent.ChildNodes, Is.EqualTo(new[] { sibling1, child1, child2, sibling2 }));
        }

        #endregion
    }
}
