using System;
using NUnit.Framework;
using Moq;
using CSF.Zpt.Rendering;
using System.Linq;
using Test.CSF.Zpt.Util;
using Ploeh.AutoFixture;
using Test.CSF.Zpt.Util.Autofixture;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  public class TestElementVisitor
  {
    #region tests

    [Test]
    [Description("This test ensures that all elements in a tree are visited and that each time a recursive visit" +
                 "occurs, a child model is created.")]
    public void TestVisitRecursively()
    {
      // Arrange
      var fixture = new Fixture();
      new RenderingContextCustomisation().Customize(fixture);
      new RenderingOptionsCustomisation().Customize(fixture);

      var sut = new Mock<ContextVisitorBase>() { CallBase = true };
      sut
        .Setup(x => x.Visit(It.IsAny<RenderingContext>()))
        .Returns((RenderingContext ctx) => new [] { ctx });

      var contexts = Enumerable.Range(0,3)
        .Select(x => fixture.Create<RenderingContext>())
        .ToArray();
      var topContext = contexts[0];
      var secondLevelContexts = contexts.Skip(1).ToArray();
      Mock.Get(topContext).Setup(x => x.GetChildContexts()).Returns(secondLevelContexts);

      // Act
      sut.Object.VisitRecursively(topContext);

      // Assert
      sut.Verify(x => x.Visit(topContext), Times.Once());
      sut.Verify(x => x.Visit(contexts[1]), Times.Once());
      sut.Verify(x => x.Visit(contexts[2]), Times.Once());
    }

    #endregion
  }
}

