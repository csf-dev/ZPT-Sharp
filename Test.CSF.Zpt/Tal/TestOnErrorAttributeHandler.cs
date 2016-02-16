﻿using System;
using NUnit.Framework;
using CSF.Zpt.Tal;
using CSF.Zpt.Rendering;
using Ploeh.AutoFixture;
using Moq;
using Test.CSF.Zpt.Util;
using Test.CSF.Zpt.Util.Autofixture;
using CSF.Zpt;

namespace Test.CSF.Zpt.Tal
{
  [TestFixture]
  public class TestOnErrorAttributeHandler
  {
    #region fields

    private IFixture _autofixture;
    private Mock<ZptElement> _element;
    private DummyModel _model;

    private OnErrorAttributeHandler _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
      new DummyModelCustomisation().Customize(_autofixture);

      _model = _autofixture.Create<DummyModel>();
      _element = new Mock<ZptElement>() { CallBase = true };

      _element.Setup(x => x.Remove());
      _element.Setup(x => x.RemoveAllChildren());
      _element.Setup(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>())).Returns(new ZptElement[0]);
      _element.Setup(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()));

      _sut = new OnErrorAttributeHandler();
    }

    #endregion

    #region no attribute

    [Test]
    public void TestHandleNoAttribute()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) null);

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    #endregion

    #region cancels action

    [Test]
    public void TestHandleCancelsAction()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(Model.CancelAction));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    [Test]
    public void TestHandleTextCancelsAction()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "text bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(Model.CancelAction));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    [Test]
    public void TestHandleStructureCancelsAction()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "structure bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(Model.CancelAction));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    #endregion

    #region attribute evaluates null

    [Test]
    public void TestHandleNull()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(null));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Once());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    [Test]
    public void TestHandleTextNull()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "text bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(null));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Once());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    [Test]
    public void TestHandleStructureNull()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "structure bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(null));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Once());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    #endregion

    #region attribute evaluates to something

    [Test]
    public void TestHandle()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "bar"));

      string value = _autofixture.Create<string>();

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(value));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(value, false),
                      Times.Once());
    }

    [Test]
    public void TestHandleText()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "text bar"));

      string value = _autofixture.Create<string>();

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(value));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(value, false),
                      Times.Once());
    }

    [Test]
    public void TestHandleStructure()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "structure bar"));

      string value = _autofixture.Create<string>();

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(value));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(value, true),
                      Times.Once());
    }

    #endregion
  }
}
