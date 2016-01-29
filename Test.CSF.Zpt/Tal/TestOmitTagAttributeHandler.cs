using System;
using NUnit.Framework;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tal;
using Ploeh.AutoFixture;
using Moq;
using Test.CSF.Zpt.Util;
using Test.CSF.Zpt.Util.Autofixture;
using CSF.Zpt;

namespace Test.CSF.Zpt.Tal
{
  [TestFixture]
  public class TestOmitTagAttributeHandler
  {
    #region fields

    private IFixture _autofixture;
    private Mock<ZptElement> _element;
    private DummyModel _model;

    private OmitTagAttributeHandler _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
      new DummyModelCustomisation().Customize(_autofixture);

      _model = _autofixture.Create<DummyModel>();
      _element = new Mock<ZptElement>() { CallBase = true };

      _sut = new OmitTagAttributeHandler();
    }

    #endregion

    #region tests

    [Test]
    public void TestHandleNoAttribute()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OmitTagAttribute))
        .Returns((ZptAttribute) null);

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");

      _element.Verify(x => x.Omit(), Times.Never());
    }

    //          Condition   Cancel    Expect element
    //          ---------   ------    --------------
    [TestCase(  true,       false,    false)]
    [TestCase(  false,      false,    true)]
    [TestCase(  true,       true,     true)]
    [TestCase(  false,      true,     true)]
    public void TestHandleConditionPresent(bool conditionValue,
                                           bool cancelsAction,
                                           bool expectElement)
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OmitTagAttribute))
        .Returns(Mock.Of<ZptAttribute>(x => x.Value == _autofixture.Create<string>()));
      _element
        .Setup(x => x.Omit())
        .Returns(new[] { Mock.Of<ZptElement>(), Mock.Of<ZptElement>(), Mock.Of<ZptElement>() });

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(cancelsAction? Model.CancelAction : conditionValue));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      if(expectElement)
      {
        Assert.AreEqual(1, result.Elements.Length, "Count of results");
        Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");
        _element.Verify(x => x.Omit(), Times.Never());
      }
      else
      {
        Assert.AreEqual(0, result.Elements.Length, "Count of results");
        Assert.AreEqual(3, result.NewlyExposedElements.Length, "Count of new elements exposed");
        _element.Verify(x => x.Omit(), Times.Once());
      }
      Mock.Get(_model).Verify(x => x.Evaluate(It.IsAny<string>(), _element.Object), Times.Once());
    }

    #endregion
  }
}

