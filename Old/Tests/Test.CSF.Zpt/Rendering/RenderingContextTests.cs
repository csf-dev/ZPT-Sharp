using System;
using System.Collections.Generic;
using CSF.Zpt.Rendering;
using Moq;
using NUnit.Framework;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  public class RenderingContextTests
  {
    [Test]
    public void CopyTo_calls_CopyTo_on_TAL_model()
    {
      // Arrange
      var talModel = Mock.Of<IModel>();
      var destinationTalModel = new Mock<IModel>();
      destinationTalModel.As<IModelValueStore>();
      var destination = Mock.Of<IModelValueContainer>(x => x.TalModel == destinationTalModel.Object);

      var sut = new RenderingContext(Mock.Of<IModel>(),
                                     talModel,
                                     Mock.Of<IZptElement>(),
                                     Mock.Of<IRenderingSettings>());

      // Act
      sut.CopyTo(destination);

      // Assert
      Mock.Get(talModel)
          .Verify(x => x.CopyTo(destinationTalModel.Object), Times.Once);
    }

    [Test]
    public void CopyTo_calls_CopyTo_on_METAL_model()
    {
      // Arrange
      var metalModel = Mock.Of<IModel>();
      var destinationMetalModel = new Mock<IModel>();
      destinationMetalModel.As<IModelValueStore>();
      var destination = Mock.Of<IModelValueContainer>(x => x.MetalModel == destinationMetalModel.Object);

      var sut = new RenderingContext(metalModel,
                                     Mock.Of<IModel>(),
                                     Mock.Of<IZptElement>(),
                                     Mock.Of<IRenderingSettings>());

      // Act
      sut.CopyTo(destination);

      // Assert
      Mock.Get(metalModel)
          .Verify(x => x.CopyTo(destinationMetalModel.Object), Times.Once);
    }
  }
}
