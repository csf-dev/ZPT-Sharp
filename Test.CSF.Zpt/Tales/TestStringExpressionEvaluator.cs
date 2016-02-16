﻿using System;
using NUnit.Framework;
using Moq;
using Ploeh.AutoFixture;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;
using Test.CSF.Zpt.Util.Autofixture;

namespace Test.CSF.Zpt.Tales
{
  [TestFixture]
  public class TestStringExpressionEvaluator
  {
    #region fields

    private IFixture _autofixture;
    private Mock<IExpressionEvaluator> _pathEvaluator;
    private StringExpressionEvaluator _sut;
    private TalesModel _model;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
      new TalesModelCustomisation().Customize(_autofixture);

      _pathEvaluator = new Mock<IExpressionEvaluator>();
      var registry = new Mock<IEvaluatorRegistry>();
      _autofixture.Inject(registry.Object);
      _model = _autofixture.Create<TalesModel>();

      registry.Setup(x => x.GetEvaluator<PathExpressionEvaluator>()).Returns(_pathEvaluator.Object);

      _sut = new StringExpressionEvaluator(registry.Object);
    }

    #endregion

    #region tests

    [TestCase("", "")]
    [TestCase("Foo bar", "Foo bar")]
    [TestCase("Foo bar\nbaz", "Foo bar\nbaz")]
    [TestCase("  Foo bar  ", "  Foo bar  ")]
    [TestCase("String $$ with escaped dollars", "String $ with escaped dollars")]
    [TestCase("I have $$megabucks!", "I have $megabucks!")]
    [TestCase("String $$$$ with doubled escaped dollars", "String $$ with doubled escaped dollars")]
    public void TestSimpleString(string supplied, string expected)
    {
      // Arrange
      var expression = Expression.Create("string", supplied);

      // Act
      var result = _sut.Evaluate(expression, Mock.Of<ZptElement>(), _model);

      // Assert
      Assert.AreEqual(expected, result.Value);
    }

    [Test]
    public void TestWithReplacements()
    {
      // Arrange
      string content = "Hello $name, how are you this fine ${current_state/time_of_day}?";
      var expression = Expression.Create("string", content);

      _pathEvaluator
        .Setup(x => x.Evaluate(It.Is<Expression>(exp => exp.Source == "name"),
                               It.IsAny<ZptElement>(),
                               It.IsAny<TalesModel>()))
        .Returns(new ExpressionResult("Fred Bloggs"));
      _pathEvaluator
        .Setup(x => x.Evaluate(It.Is<Expression>(exp => exp.Source == "current_state/time_of_day"),
                               It.IsAny<ZptElement>(),
                               It.IsAny<TalesModel>()))
        .Returns(new ExpressionResult("evening"));

      // Act
      var result = _sut.Evaluate(expression, Mock.Of<ZptElement>(), _model);

      // Assert
      Assert.AreEqual("Hello Fred Bloggs, how are you this fine evening?", result.Value);
    }

    [Test]
    public void TestWithEscapedDollarsAndReplacements()
    {
      // Arrange
      string content = "Hello $$$name, how are $$$$ you this fine $$${current_state/time_of_day}?";
      var expression = Expression.Create("string", content);

      _pathEvaluator
        .Setup(x => x.Evaluate(It.Is<Expression>(exp => exp.Source == "name"),
                               It.IsAny<ZptElement>(),
                               It.IsAny<TalesModel>()))
        .Returns(new ExpressionResult("Fred Bloggs"));
      _pathEvaluator
        .Setup(x => x.Evaluate(It.Is<Expression>(exp => exp.Source == "current_state/time_of_day"),
                               It.IsAny<ZptElement>(),
                               It.IsAny<TalesModel>()))
        .Returns(new ExpressionResult("evening"));

      // Act
      var result = _sut.Evaluate(expression, Mock.Of<ZptElement>(), _model);

      // Assert
      Assert.AreEqual("Hello $Fred Bloggs, how are $$ you this fine $evening?", result.Value);
    }

    #endregion
  }
}
