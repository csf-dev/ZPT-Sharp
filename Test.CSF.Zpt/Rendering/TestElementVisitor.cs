using System;
using NUnit.Framework;
using Moq;
using CSF.Zpt.Rendering;
using System.Linq;

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
      var sut = new Mock<ElementVisitor>((object) null) { CallBase = true };
      sut
        .Setup(x => x.Visit(It.IsAny<ZptElement>(), It.IsAny<Model>()))
        .Returns((ZptElement ele, Model mod) => new [] { ele });

      var models = Enumerable.Range(0,2)
        .Select(x => new Mock<DummyModel>() { CallBase = true })
        .ToArray();
      var topModel = models[0];
      var secondLevelModel = models[1];
      topModel.Setup(x => x.CreateChildModel()).Returns(secondLevelModel.Object);

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
      sut.Object.VisitRecursively(topElement.Object, topModel.Object);

      // Assert
      sut.Verify(x => x.Visit(topElement.Object, topModel.Object), Times.Once());
      sut.Verify(x => x.Visit(secondLevelElements[0].Object, secondLevelModel.Object), Times.Once());
      sut.Verify(x => x.Visit(secondLevelElements[1].Object, secondLevelModel.Object), Times.Once());
    }

    #endregion
  }
}

