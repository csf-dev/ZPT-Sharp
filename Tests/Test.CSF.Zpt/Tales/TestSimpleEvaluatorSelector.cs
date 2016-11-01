using System;
using NUnit.Framework;
using CSF.Zpt.Tales;
using Ploeh.AutoFixture;
//using CSF.Zpt.ExpressionEvaluators;
using Moq;

namespace Test.CSF.Zpt.Tales
{
  [TestFixture]
  public class TestSimpleEvaluatorSelector
  {
    #region fields

    private IFixture _autofixture;
    private Mock<IExpressionEvaluatorService> _registry;
    private IEvaluatorSelector _sut;
    private IExpressionEvaluator _evaluator;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();

      _registry = new Mock<IExpressionEvaluatorService>(MockBehavior.Strict);
      _sut = new SimpleEvaluatorSelector(_registry.Object);

      _evaluator = Mock.Of<IExpressionEvaluator>();
    }

    #endregion

    #region tests

    [Test]
    public void GetEvaluator_for_expression_uses_prefix()
    {
      // Arrange
      var expression = new Expression("foo", _autofixture.Create<string>());

      _registry.Setup(x => x.GetEvaluator("foo")).Returns(_evaluator);

      // Act
      var result = _sut.GetEvaluator(expression);

      // Assert
      Assert.AreSame(_evaluator, result, "Evaluator returned");
      _registry.Verify(x => x.GetEvaluator("foo"), Times.Once());
    }

    [Test]
    public void GetEvaluator_for_expression_without_prefix_returns_default()
    {
      // Arrange
      var expression = new Expression(null, _autofixture.Create<string>());

      _registry.Setup(x => x.GetDefaultEvaluator()).Returns(_evaluator);

      // Act
      var result = _sut.GetEvaluator(expression);

      // Assert
      Assert.AreSame(_evaluator, result, "Evaluator returned");
      _registry.Verify(x => x.GetDefaultEvaluator(), Times.Once());
    }

    [Test]
    public void GetEvaluator_for_type_uses_type()
    {
      // Arrange
      _registry.Setup(x => x.GetEvaluator(typeof(IExpressionEvaluator))).Returns(_evaluator);

      // Act
      var result = _sut.GetEvaluator<IExpressionEvaluator>();

      // Assert
      Assert.AreSame(_evaluator, result, "Evaluator returned");
      _registry.Verify(x => x.GetEvaluator(typeof(IExpressionEvaluator)), Times.Once());
    }

    #endregion
  }
}

