using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using ZptSharp.Dom;

namespace ZptSharp.SourceAnnotation
{
    [TestFixture,Parallelizable]
    public class CommenterTests
    {
        [Test, AutoMoqData]
        public void AddCommentBefore_adds_to_beginning_of_document_if_element_has_no_parent(Commenter sut,
                                                                                            INode element,
                                                                                            IDocument document,
                                                                                            string commentText)
        {
            Mock.Get(element).SetupGet(x => x.ParentElement).Returns(() => null);
            Mock.Get(element).SetupGet(x => x.Document).Returns(document);

            sut.AddCommentBefore(element, commentText);

            Mock.Get(document)
                .Verify(x => x.AddCommentToBeginningOfDocument(commentText), Times.Once);
        }

        [Test, AutoMoqData]
        public void AddCommentBefore_adds_comment_as_previous_sibling(Commenter sut,
                                                                      INode element,
                                                                      INode parent,
                                                                      INode sibling1,
                                                                      INode sibling2,
                                                                      INode comment,
                                                                      string commentText)
        {
            Mock.Get(element).SetupGet(x => x.ParentElement).Returns(parent);
            Mock.Get(parent).SetupGet(x => x.ChildNodes).Returns(new List<INode> { sibling1, element, sibling2 });
            Mock.Get(element).Setup(x => x.CreateComment(commentText)).Returns(comment);

            sut.AddCommentBefore(element, commentText);

            Assert.That(parent.ChildNodes, Is.EqualTo(new[] { sibling1, comment, element, sibling2 }));
        }

        [Test, AutoMoqData]
        public void AddCommentAfter_adds_comment_as_next_sibling(Commenter sut,
                                                                      INode element,
                                                                      INode parent,
                                                                      INode sibling1,
                                                                      INode sibling2,
                                                                      INode comment,
                                                                      string commentText)
        {
            Mock.Get(element).SetupGet(x => x.ParentElement).Returns(parent);
            Mock.Get(parent).SetupGet(x => x.ChildNodes).Returns(new List<INode> { sibling1, element, sibling2 });
            Mock.Get(element).Setup(x => x.CreateComment(commentText)).Returns(comment);

            sut.AddCommentAfter(element, commentText);

            Assert.That(parent.ChildNodes, Is.EqualTo(new[] { sibling1, element, comment, sibling2 }));
        }
    }
}
