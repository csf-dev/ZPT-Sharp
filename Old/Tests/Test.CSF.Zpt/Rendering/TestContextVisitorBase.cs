using System;
using NUnit.Framework;
using Moq;
using CSF.Zpt.Rendering;
using System.Linq;
using CSF.Zpt.TestUtils;
using Ploeh.AutoFixture;
using CSF.Zpt.TestUtils.Autofixture;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  public class TestContextVisitorBase
  {
    #region tests

    [Test]
    [Description("This test ensures that all rendering contexts in a tree are visited.")]
    public void VisitRecursively_visits_all_child_contexts()
    {
      // Arrange
      var fixture = new Fixture();
      new RenderingContextCustomisation().Customize(fixture);

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

