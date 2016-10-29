using System;
using NUnit.Framework;
using CSF.Zpt.Tal;
using Moq;
using CSF.Zpt.Rendering;
using CSF.Zpt;

namespace Test.CSF.Zpt.Tal
{
  [TestFixture]
  public class TestTalTidyUpVisitor
  {
    #region tests

    [Test]
    public void VisitContext_purges_all_elements_and_attributes_in_TAL_namespace()
    {
      // Arrange
      var context = new Mock<IRenderingContext>() { DefaultValue = DefaultValue.Mock };
      var element = Mock.Get(context.Object.Element);

      var sut = new TalTidyUpVisitor();

      // Act
      sut.VisitContext(context.Object);

      // Assert
      element.Verify(x => x.PurgeElements(ZptConstants.Tal.Namespace), Times.Once());
      element.Verify(x => x.PurgeAttributes(ZptConstants.Tal.Namespace), Times.Once());
    }

    #endregion
  }
}

