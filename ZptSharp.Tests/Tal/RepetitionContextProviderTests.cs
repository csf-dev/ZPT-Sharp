using System;
using System.Collections;
using System.Linq;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Tal
{
    [TestFixture,Parallelizable]
    public class RepetitionContextProviderTests
    {
        [Test, AutoMoqData]
        public void GetRepetitionContexts_throws_evaluation_exception_if_sequence_is_not_enumerable(RepetitionContextProvider sut,
                                                                                                    object notAnEnumerable,
                                                                                                    [StubDom] ExpressionContext sourceContext,
                                                                                                    string repeatVariableName)
        {
            Assert.That(() => sut.GetRepetitionContexts(notAnEnumerable, sourceContext, repeatVariableName), Throws.InstanceOf<EvaluationException>());
        }

        [Test, AutoMoqData]
        public void GetRepetitionContexts_throws_evaluation_exception_if_enumerating_the_sequence_throws(RepetitionContextProvider sut,
                                                                                                         ThrowingEnumerable throwingEnumerable,
                                                                                                         [StubDom] ExpressionContext sourceContext,
                                                                                                         string repeatVariableName)
        {
            Assert.That(() => sut.GetRepetitionContexts(throwingEnumerable, sourceContext, repeatVariableName), Throws.InstanceOf<EvaluationException>());
        }

        [Test, AutoMoqData]
        public void GetRepetitionContexts_returns_collection_of_repetitions_separated_by_whitespace(RepetitionContextProvider sut,
                                                                                                    [StubDom] ExpressionContext sourceContext,
                                                                                                    string repeatVariableName,
                                                                                                    string value1,
                                                                                                    string value2,
                                                                                                    string value3,
                                                                                                    [StubDom] INode node1,
                                                                                                    [StubDom] INode node2,
                                                                                                    [StubDom] INode node3,
                                                                                                    [StubDom] INode parent,
                                                                                                    INode newline)
        {
            Mock.Get(sourceContext.CurrentNode)
                .SetupSequence(x => x.GetCopy())
                .Returns(node1)
                .Returns(node2)
                .Returns(node3);
            sourceContext.CurrentNode.ParentNode = parent;
            parent.ChildNodes.Add(sourceContext.CurrentNode);
            var enumerable = new[] { value1, value2, value3 };
            Mock.Get(sourceContext.CurrentNode)
                .Setup(x => x.CreateTextNode(Environment.NewLine))
                .Returns(newline);

            var result = sut.GetRepetitionContexts(enumerable, sourceContext, repeatVariableName);

            Assert.That(result?.Select(x => x.LocalDefinitions[repeatVariableName]).ToList(),
                        Is.EqualTo(new[] { value1, It.IsAny<object>(), value2, It.IsAny<object>(), value3 }),
                        "Repeat variable values are correct");
            Assert.That(result?.Select(x => x.CurrentNode).ToList(),
                        Is.EqualTo(new[] { node1, newline, node2, newline, node3 }),
                        "Nodes are correct");
        }

        #region contained stub type

        public class ThrowingEnumerable : IEnumerable
        {
            public IEnumerator GetEnumerator() => new ThrowingEnumerator();
        }

        public class ThrowingEnumerator : IEnumerator
        {
            public object Current { get; } = new object();

            public bool MoveNext() => throw new NotImplementedException();

            public void Reset() { }
        }

        #endregion
    }
}
