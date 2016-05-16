using System;
using NUnit.Framework;
using CSF.Zpt.Rendering;
using Test.CSF.Zpt.Rendering;
using Moq;
using System.Linq;
using CSF.Zpt.Metal;
using Ploeh.AutoFixture;
using Test.CSF.Zpt.Util.Autofixture;
using Test.CSF.Zpt.Util;

namespace Test.CSF.Zpt.Metal
{
  [TestFixture]
  public class TestSourceAnnotationVisitor
  {
    #region tests

    [Test]
    public void TestVisitRecursively()
    {
      // Arrange
      var fixture = new Fixture();
      new RenderingContextCustomisation().Customize(fixture);

      var repo = new MockRepository(MockBehavior.Strict);
      repo.CallBase = true;

      var elements = repo.Of<ZptElement>()
        .Take(5)
        .Select(x => Mock.Get(x))
        .ToArray();
      var sourceInfos = new [] { "One", "Two", "Three" }
        .Select(x => new SourceFileInfo(x))
        .ToArray();

      foreach(var ele in elements)
      {
        ele.SetupGet(x => x.IsRoot).Returns(false);
        ele.Setup(x => x.GetChildElements()).Returns(new ZptElement[0]);
        ele.SetupGet(x => x.SourceFile).Returns(sourceInfos[1]);
        ele.SetupGet(x => x.IsImported).Returns(true);
        ele
          .Setup(x => x.GetFileLocation())
          .Returns(() => String.Format("Element {0}", Array.IndexOf(elements, ele) + 1));
        ele.Setup(x => x.GetAttributes()).Returns(new ZptAttribute[0]);
      }
      elements[0].SetupGet(x => x.IsRoot).Returns(true);

      elements[0].Setup(x => x.GetChildElements()).Returns(new [] { elements[1].Object });
      elements[1].Setup(x => x.GetChildElements()).Returns(new [] { elements[2].Object, elements[3].Object, elements[4].Object });

      elements[0].SetupGet(x => x.SourceFile).Returns(sourceInfos[0]);
      elements[4].SetupGet(x => x.SourceFile).Returns(sourceInfos[2]);

      elements[0].SetupGet(x => x.IsImported).Returns(false);
      elements[2].SetupGet(x => x.IsImported).Returns(false);

      foreach(var ele in elements)
      {
        ele.Setup(x => x.AddCommentBefore(It.IsAny<string>()));
      }
      elements[0].Setup(x => x.AddCommentAfter(It.IsAny<string>()));

      var options = new RenderingOptions(addSourceFileAnnotation: true);
      var sut = new SourceAnnotationVisitor();
      var ctx = new RenderingContext(fixture.Create<DummyModel>(),
                                     fixture.Create<DummyModel>(),
                                     elements[0].Object,
                                     options);

      // Act
      sut.VisitRecursively(ctx);

      // Assert
      elements[0].Verify(x => x.AddCommentAfter("One, Element 1"), Times.Once());
      elements[1].Verify(x => x.AddCommentBefore("Two, Element 2"), Times.Once());
      elements[2].Verify(x => x.AddCommentBefore(It.IsAny<string>()), Times.Never());
      elements[3].Verify(x => x.AddCommentBefore("Two, Element 4"), Times.Once());
      elements[4].Verify(x => x.AddCommentBefore("Three, Element 5"), Times.Once());
    }

    #endregion
  }
}

