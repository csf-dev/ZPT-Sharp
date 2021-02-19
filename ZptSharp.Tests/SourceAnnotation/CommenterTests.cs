using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;

namespace ZptSharp.SourceAnnotation
{
    [TestFixture,Parallelizable]
    public class CommenterTests
    {
        [Test, AutoMoqData]
        public void AddCommentBefore_adds_to_beginning_of_document_if_node_has_no_parent([MockLogger, Frozen] ILogger<Commenter> logger,
                                                                                            Commenter sut,
                                                                                            INode node,
                                                                                            IDocument document,
                                                                                            string commentText)
        {
            Mock.Get(node).SetupGet(x => x.ParentNode).Returns(() => null);
            Mock.Get(node).SetupGet(x => x.Document).Returns(document);

            sut.AddCommentBefore(node, commentText);

            Mock.Get(document)
                .Verify(x => x.AddCommentToBeginningOfDocument(commentText), Times.Once);
        }

        [Test, AutoMoqData]
        public void AddCommentBefore_adds_comment_as_previous_sibling([MockLogger, Frozen] ILogger<Commenter> logger,
                                                                      Commenter sut,
                                                                      INode node,
                                                                      INode parent,
                                                                      INode sibling1,
                                                                      INode sibling2,
                                                                      INode comment,
                                                                      string commentText)
        {
            Mock.Get(node).SetupGet(x => x.ParentNode).Returns(parent);
            Mock.Get(parent).SetupGet(x => x.ChildNodes).Returns(new List<INode> { sibling1, node, sibling2 });
            Mock.Get(node).Setup(x => x.CreateComment(commentText)).Returns(comment);

            sut.AddCommentBefore(node, commentText);

            Assert.That(parent.ChildNodes, Is.EqualTo(new[] { sibling1, comment, node, sibling2 }));
        }

        [Test, AutoMoqData]
        public void AddCommentAfter_adds_comment_as_next_sibling([MockLogger, Frozen] ILogger<Commenter> logger,
                                                                 Commenter sut,
                                                                 INode node,
                                                                 INode parent,
                                                                 INode sibling1,
                                                                 INode sibling2,
                                                                 INode comment,
                                                                 string commentText)
        {
            Mock.Get(node).SetupGet(x => x.ParentNode).Returns(parent);
            Mock.Get(parent).SetupGet(x => x.ChildNodes).Returns(new List<INode> { sibling1, node, sibling2 });
            Mock.Get(node).Setup(x => x.CreateComment(commentText)).Returns(comment);

            sut.AddCommentAfter(node, commentText);

            Assert.That(parent.ChildNodes, Is.EqualTo(new[] { sibling1, node, comment, sibling2 }));
        }
    }
}
