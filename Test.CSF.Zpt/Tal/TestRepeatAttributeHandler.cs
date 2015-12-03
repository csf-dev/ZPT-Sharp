using System;
using NUnit.Framework;
using Moq;
using Ploeh.AutoFixture;
using CSF.Zpt.Tal;
using CSF.Zpt.Rendering;
using Test.CSF.Zpt.Util;
using Test.CSF.Zpt.Util.Autofixture;
using CSF.Zpt;

namespace Test.CSF.Zpt.Tal
{
  [TestFixture]
  public class TestRepeatAttributeHandler
  {
    #region fields

    private IFixture _autofixture;
    private Mock<ZptElement> _element, _parent, _clone;
    private DummyModel _model;

    private RepeatAttributeHandler _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
      new DummyModelCustomisation().Customize(_autofixture);

      _model = _autofixture.Create<DummyModel>();
      _element = new Mock<ZptElement>() { CallBase = true };
      _parent = new Mock<ZptElement>() { CallBase = true };
      _clone = new Mock<ZptElement>() { CallBase = true };

      _element.Setup(x => x.Remove());
      _element.Setup(x => x.Clone()).Returns(_clone.Object);
      _element.Setup(x => x.GetParentElement()).Returns(_parent.Object);
      _parent.Setup(x => x.InsertAfter(It.IsAny<ZptElement>(), _clone.Object)).Returns(_clone.Object);

      _sut = new RepeatAttributeHandler();
    }

    #endregion

    #region tests

    [Test]
    public void TestHandleNoAttribute()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.DefaultPrefix,
                                   ZptConstants.Tal.RepeatAttribute))
        .Returns((ZptAttribute) null);

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Length, "Count of results");
      Assert.AreSame(_element.Object, result[0], "Correct element returned");

      _element.Verify(x => x.Remove(), Times.Never());
      _parent.Verify(x => x.InsertAfter(It.IsAny<ZptElement>(), It.IsAny<ZptElement>()), Times.Never());
    }

    [Test]
    public void TestHandleCancelsAction()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.DefaultPrefix,
                                   ZptConstants.Tal.RepeatAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == _autofixture.Create<string>()));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(Model.CancelAction));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Length, "Count of results");
      Assert.AreSame(_element.Object, result[0], "Correct element returned");

      _element.Verify(x => x.Remove(), Times.Never());
      _parent.Verify(x => x.InsertAfter(It.IsAny<ZptElement>(), It.IsAny<ZptElement>()), Times.Never());
    }

    [Test]
    public void TestHandleEmptySequence()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.DefaultPrefix,
                                   ZptConstants.Tal.RepeatAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == _autofixture.Create<string>()));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(new string[0]));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(0, result.Length, "Count of results");

      _element.Verify(x => x.Remove(), Times.Once());
      _parent.Verify(x => x.InsertAfter(It.IsAny<ZptElement>(), It.IsAny<ZptElement>()), Times.Never());
    }

    [Test]
    public void TestHandleSequence()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.DefaultPrefix,
                                   ZptConstants.Tal.RepeatAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == _autofixture.Create<string>()));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(new [] { "One", "Two", "Three", "Four" }));
      Mock.Get(_model).Setup(x => x.AddRepetitionInfo(It.IsAny<RepetitionInfo[]>()));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(4, result.Length, "Count of results");

      _element.Verify(x => x.Remove(), Times.Never());
      _parent.Verify(x => x.InsertAfter(_element.Object, _clone.Object), Times.Once());
      _parent.Verify(x => x.InsertAfter(_clone.Object, _clone.Object), Times.Exactly(2));
      Mock.Get(_model).Verify(x => x.AddRepetitionInfo(It.Is<RepetitionInfo[]>(r => r.Length == 4)), Times.Once());
    }

    #endregion
  }
}

