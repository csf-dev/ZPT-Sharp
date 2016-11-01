using System;
using CSF.Zpt.Metal;
using NUnit.Framework;
using Moq;
using CSF.Zpt.Rendering;
using CSF.Zpt;

namespace Test.CSF.Zpt.Metal
{
  [TestFixture]
  public class TestMetalTidyUpVisitor
  {
    #region tests

    [Test]
    public void VisitContext_purges_all_elements_and_attributes_in_METAL_namespace()
    {
      // Arrange
      var context = new Mock<IRenderingContext>() { DefaultValue = DefaultValue.Mock };
      var element = Mock.Get(context.Object.Element);

      var sut = new MetalTidyUpVisitor();

      // Act
      sut.VisitContext(context.Object);

      // Assert
      element.Verify(x => x.PurgeElements(ZptConstants.Metal.Namespace), Times.Once());
      element.Verify(x => x.PurgeAttributes(ZptConstants.Metal.Namespace), Times.Once());
    }

    #endregion
  }
}

