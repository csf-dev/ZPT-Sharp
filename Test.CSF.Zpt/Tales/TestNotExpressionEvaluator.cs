using System;
using NUnit.Framework;
using Moq;
using CSF.Zpt.Tales;
using Ploeh.AutoFixture;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt.Tales
{
  [TestFixture]
  public class TestNotExpressionEvaluator
  {
    #region tests

    [TestCase(0,      true)]
    [TestCase(-1,     false)]
    [TestCase(1,      false)]
    [TestCase("",     true)]
    [TestCase(null,   true)]
    [TestCase("a",    false)]
    [TestCase(false,  true)]
    [TestCase(true,   false)]
    public void TestEvaluate(object expressionResult, bool expectedResult)
    {
      // Arrange
      var aFixture = new Fixture();

      var expressionContent = aFixture.Create<string>();
      var expression = new Expression("not", expressionContent);

      var evaluator = new Mock<IExpressionEvaluator>();
      var registry = Mock.Of<IEvaluatorRegistry>(x => x.GetEvaluator(It.IsAny<Expression>()) == evaluator.Object);
      evaluator
        .Setup(x => x.Evaluate(It.Is<Expression>(e => e.ToString() == expressionContent),
                               It.IsAny<RenderingContext>(),
                               It.IsAny<TalesModel>()))
        .Returns(new ExpressionResult(expressionResult));

      var model = new TalesModel(registry);

      var sut = new NotExpressionEvaluator(registry);

      // Act
      var result = sut.Evaluate(expression, Mock.Of<RenderingContext>(), model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(expectedResult, result.Value, "Correct result");
    }

    [Test]
    public void TestEvaluateCancel()
    {
      // Arrange
      var aFixture = new Fixture();

      var expressionContent = aFixture.Create<string>();
      var expression = new Expression("not", expressionContent);

      var evaluator = new Mock<IExpressionEvaluator>();
      var registry = Mock.Of<IEvaluatorRegistry>(x => x.GetEvaluator(It.IsAny<Expression>()) == evaluator.Object);
      evaluator
        .Setup(x => x.Evaluate(It.Is<Expression>(e => e.ToString() == expressionContent),
                               It.IsAny<RenderingContext>(),
                               It.IsAny<TalesModel>()))
        .Returns(new ExpressionResult(Model.CancelAction));

      var model = new TalesModel(registry);

      var sut = new NotExpressionEvaluator(registry);

      // Act
      var result = sut.Evaluate(expression, Mock.Of<RenderingContext>(), model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(true, result.Value, "Correct result");
    }

    [TestCase(true, false)]
    [TestCase(false, true)]
    public void TestEvaluateTalesConvertible(bool conversionValue, bool expectedResult)
    {
      // Arrange
      var aFixture = new Fixture();

      var expressionContent = aFixture.Create<string>();
      var expression = new Expression("not", expressionContent);

      var evaluator = new Mock<IExpressionEvaluator>();
      var registry = Mock.Of<IEvaluatorRegistry>(x => x.GetEvaluator(It.IsAny<Expression>()) == evaluator.Object);
      var convertible = Mock.Of<ITalesConvertible>(x => x.AsBoolean() == conversionValue);
      evaluator
        .Setup(x => x.Evaluate(It.Is<Expression>(e => e.ToString() == expressionContent),
                               It.IsAny<RenderingContext>(),
                               It.IsAny<TalesModel>()))
        .Returns(new ExpressionResult(convertible));

      var model = new TalesModel(registry);

      var sut = new NotExpressionEvaluator(registry);

      // Act
      var result = sut.Evaluate(expression, Mock.Of<RenderingContext>(), model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(expectedResult, result.Value, "Correct result");
    }

    #endregion
  }
}

