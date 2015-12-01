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

      var sut = new Mock<ElementVisitor>() { CallBase = true };
      sut
        .Setup(x => x.Visit(It.IsAny<ZptElement>(), It.IsAny<RenderingContext>(), It.IsAny<RenderingOptions>()))
        .Returns((ZptElement ele, RenderingContext con, RenderingOptions opts) => new [] { ele });

      var contexts = Enumerable.Range(0,2)
        .Select(x => fixture.Create<RenderingContext>())
        .ToArray();
      var topContext = contexts[0];
      var secondLevelContext = contexts[1];
      Mock.Get(topContext).Setup(x => x.CreateChildContext()).Returns(secondLevelContext);

      var elements = Enumerable.Range(0,3)
        .Select(x => new Mock<ZptElement>())
        .ToArray();
      var topElement = elements.First();
      var secondLevelElements = elements
        .Skip(1)
        .Take(2)
        .ToArray();
      topElement
        .Setup(x => x.GetChildElements())
        .Returns(secondLevelElements.Select(x => x.Object).ToArray());
      foreach(var item in secondLevelElements)
      {
        item.Setup(x => x.GetChildElements()).Returns(new ZptElement[0]);
      }

      // Act
      sut.Object.VisitRecursively(topElement.Object, topContext, fixture.Create<RenderingOptions>());

      // Assert
      sut.Verify(x => x.Visit(topElement.Object, topContext, It.IsAny<RenderingOptions>()),
                 Times.Once());
      sut.Verify(x => x.Visit(secondLevelElements[0].Object, secondLevelContext, It.IsAny<RenderingOptions>()),
                 Times.Once());
      sut.Verify(x => x.Visit(secondLevelElements[1].Object, secondLevelContext, It.IsAny<RenderingOptions>()),
                 Times.Once());
    }

    #endregion
  }
}

