using System;
using NUnit.Framework;
using CSF.Zpt.Tales;
using Moq;
using Ploeh.AutoFixture;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt.Tales
{
  [TestFixture]
  public class TestTalesModel
  {
    #region fields

    private IFixture _autofixture;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
    }

    #endregion

    #region tests

    [Test]
    public void TestCreateChildModel()
    {
      // Arrange
      var sut = new TalesModel(null, null, SimpleEvaluatorRegistry.Default);

      // Act
      var child = sut.CreateChildModel();

      // Assert
      Assert.NotNull(child, "Result nullability");
      Assert.AreSame(sut, child.Parent, "Parent model");
    }

    [Test]
    public void TestEvaluate()
    {
      // Arrange
      var registry = new Mock<IEvaluatorRegistry>();
      var evaluator = new Mock<IExpressionEvaluator>();

      var sut = new TalesModel(null, null, registry.Object);

      var expressionResult = _autofixture.Create<ExpressionResult>();

      registry
        .Setup(x => x.GetEvaluator(It.IsAny<Expression>()))
        .Returns(evaluator.Object);
      evaluator
        .Setup(x => x.Evaluate(It.IsAny<Expression>(), It.IsAny<ZptElement>(), sut))
        .Returns(expressionResult);

      // Act
      var result = sut.Evaluate(_autofixture.Create<Expression>(), Mock.Of<ZptElement>());

      // Assert
      Assert.AreSame(expressionResult, result);
    }

    [Test]
    public void TestEvaluateString()
    {
      // Arrange
      var registry = new Mock<IEvaluatorRegistry>();
      var evaluator = new Mock<IExpressionEvaluator>();

      var sut = new TalesModel(null, null, registry.Object);

      var expressionResult = _autofixture.Create<ExpressionResult>();
      string expressionSource = _autofixture.Create<string>();

      registry
        .Setup(x => x.GetEvaluator(It.Is<Expression>(e => e.Source == expressionSource)))
        .Returns(evaluator.Object);
      evaluator
        .Setup(x => x.Evaluate(It.Is<Expression>(e => e.Source == expressionSource), It.IsAny<ZptElement>(), sut))
        .Returns(expressionResult);

      // Act
      var result = sut.Evaluate(expressionSource, Mock.Of<ZptElement>());

      // Assert
      Assert.AreSame(expressionResult, result);
    }

    #endregion
  }
}

