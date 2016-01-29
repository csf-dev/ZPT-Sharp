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
  public class TestDefineAttributeHandler
  {
    #region fields

    private IFixture _autofixture;
    private Mock<ZptElement> _element;
    private DummyModel _model;

    private DefineAttributeHandler _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
      new DummyModelCustomisation().Customize(_autofixture);

      _model = _autofixture.Create<DummyModel>();
      _element = new Mock<ZptElement>() { CallBase = true };

      _sut = new DefineAttributeHandler();
    }

    #endregion

    #region tests

    [Test]
    public void TestHandleNoAttribute()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.DefineAttribute))
        .Returns((ZptAttribute) null);

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");
      Mock.Get(_model).Verify(x => x.AddLocal(It.IsAny<string>(), It.IsAny<object>()), Times.Never());
      Mock.Get(_model).Verify(x => x.AddGlobal(It.IsAny<string>(), It.IsAny<object>()), Times.Never());
    }

    //          Attribute value       Global?   Variable    Expression
    //          ---------------       -------   --------    ----------
    [TestCase(  "foo bar",            false,    "foo",      "bar")]
    [TestCase(  "local foo bar",      false,    "foo",      "bar")]
    [TestCase(  "global foo bar",     true,     "foo",      "bar")]
    [TestCase(  "foo bar/baz",        false,    "foo",      "bar/baz")]
    [TestCase(  "local foo bar/baz",  false,    "foo",      "bar/baz")]
    [TestCase(  "global foo bar/baz", true,     "foo",      "bar/baz")]
    public void TestHandleSingleDefinition(string attributeVal,
                                           bool expectGlobal,
                                           string variableName,
                                           string expression)
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.DefineAttribute))
        .Returns(Mock.Of<ZptAttribute>(x => x.Value == attributeVal));
      
      var obj = _autofixture.Create<object>();
      Mock.Get(_model)
        .Setup(x => x.Evaluate(expression, _element.Object))
        .Returns(new ExpressionResult(obj));

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");
      Mock.Get(_model).Verify(x => x.Evaluate(expression, _element.Object), Times.Once());
      Mock.Get(_model).Verify(x => x.AddLocal(variableName, obj),
                              expectGlobal? Times.Never() : Times.Once());
      Mock.Get(_model).Verify(x => x.AddGlobal(variableName, obj),
                              expectGlobal? Times.Once() : Times.Never());
    }

    [Test]
    public void TestHandleMultipleDefinition()
    {
      // Arrange
      string attributeVal = @"foo bar;
                              local wibble wobble/spong;
                              global someVal very/long/expression/string;
                              key string:This is a test;; it contains a semicolon!";
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.DefineAttribute))
        .Returns(Mock.Of<ZptAttribute>(x => x.Value == attributeVal));

      var expressionsAndResults = new [] {
        new { Key = "foo",      Expression = "bar",                                             Result = _autofixture.Create<object>(), Global = false },
        new { Key = "wibble",   Expression = "wobble/spong",                                    Result = _autofixture.Create<object>(), Global = false },
        new { Key = "someVal",  Expression = "very/long/expression/string",                     Result = _autofixture.Create<object>(), Global = true },
        new { Key = "key",      Expression = "string:This is a test; it contains a semicolon!", Result = _autofixture.Create<object>(), Global = false },
      };
      foreach(var item in expressionsAndResults)
      {
        Mock.Get(_model).Setup(x => x.Evaluate(item.Expression, _element.Object)).Returns(new ExpressionResult(item.Result));
      }

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");
      foreach(var item in expressionsAndResults)
      {
        Mock.Get(_model).Verify(x => x.Evaluate(item.Expression, _element.Object), Times.Once());
        Mock.Get(_model).Verify(x => x.AddLocal(item.Key, item.Result),
                                item.Global? Times.Never() : Times.Once());
        Mock.Get(_model).Verify(x => x.AddGlobal(item.Key, item.Result),
                                item.Global? Times.Once() : Times.Never());
      }
    }

    [Test]
    public void TestHandleMultipleDefinitionWithCancellation()
    {
      // Arrange
      string attributeVal = @"foo bar;
                              local wibble wobble/cancellation;
                              global someVal very/long/expression/string;
                              key string:This is a test;; it contains a semicolon!";
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.DefineAttribute))
        .Returns(Mock.Of<ZptAttribute>(x => x.Value == attributeVal));

      var expressionsAndResults = new [] {
        new { Key = "foo",      Expression = "bar",                                             Result = _autofixture.Create<object>(), Global = false },
        new { Key = "wibble",   Expression = "wobble/cancellation",                             Result = Model.CancelAction,            Global = false },
        new { Key = "someVal",  Expression = "very/long/expression/string",                     Result = _autofixture.Create<object>(), Global = true },
        new { Key = "key",      Expression = "string:This is a test; it contains a semicolon!", Result = _autofixture.Create<object>(), Global = false },
      };
      foreach(var item in expressionsAndResults)
      {
        Mock.Get(_model).Setup(x => x.Evaluate(item.Expression, _element.Object)).Returns(new ExpressionResult(item.Result));
      }

      // Act
      var result = _sut.Handle(_element.Object, _model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Elements.Length, "Count of results");
      Assert.AreSame(_element.Object, result.Elements[0], "Correct element returned");
      foreach(var item in expressionsAndResults)
      {
        Mock.Get(_model).Verify(x => x.Evaluate(item.Expression, _element.Object), Times.Once());

        if(item.Result != Model.CancelAction)
        {
          Mock.Get(_model).Verify(x => x.AddLocal(item.Key, item.Result),
                                  item.Global? Times.Never() : Times.Once());
          Mock.Get(_model).Verify(x => x.AddGlobal(item.Key, item.Result),
                                  item.Global? Times.Once() : Times.Never());
        }
        else
        {
          Mock.Get(_model).Verify(x => x.AddLocal(item.Key, item.Result), Times.Never());
          Mock.Get(_model).Verify(x => x.AddGlobal(item.Key, item.Result), Times.Never());
        }
      }
    }

    [TestCase("key ")]
    [TestCase(" value")]
    [TestCase("key1 value1;key2 ;key3 value3")]
    [ExpectedException(typeof(ParserException))]
    public void TestHandleBadDefinition(string attributeVal)
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.DefineAttribute))
        .Returns(Mock.Of<ZptAttribute>(x => x.Value == attributeVal));
      
      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _element.Object))
        .Returns(new ExpressionResult(_autofixture.Create<object>()));

      // Act
      _sut.Handle(_element.Object, _model);

      // Assert (by observing an exception)
    }

    #endregion
  }
}

