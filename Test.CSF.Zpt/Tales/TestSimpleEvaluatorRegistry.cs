using System;
using NUnit.Framework;
using CSF.Zpt.Tales;
using Ploeh.AutoFixture;

namespace Test.CSF.Zpt.Tales
{
  [TestFixture]
  public class TestSimpleEvaluatorRegistry
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
    [Description("Just tests that the static constructor creates a valid instance and that the Default property is accessible.")]
    public void TestStaticConstructor()
    {
      // Arrange (nothing to do)

      // Act
      var defaultInstance = new SimpleEvaluatorSelector();

      // Assert
      Assert.NotNull(defaultInstance);
    }

    [TestCase("path", typeof(PathExpressionEvaluator))]
    [TestCase("string", typeof(StringExpressionEvaluator))]
    [TestCase("not", typeof(NotExpressionEvaluator))]
    public void TestGetEvaluator(string prefix, Type expectedType)
    {
      // Arrange
      var sut = new SimpleEvaluatorSelector();
      var expression = new Expression(prefix, _autofixture.Create<string>());

      // Act
      var result = sut.GetEvaluator(expression);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.IsInstanceOf(expectedType, result, "Result type");
    }

    [Test]
    [ExpectedException(typeof(InvalidExpressionException))]
    public void TestGetEvaluatorNotRecognised()
    {
      // Arrange
      var sut = new SimpleEvaluatorSelector();
      var expression = new Expression("zzinvalid", _autofixture.Create<string>());

      // Act
      sut.GetEvaluator(expression);

      // Assert (by observing an exception)
    }

    [Test]
    public void TestGetEvaluatorDefault()
    {
      // Arrange
      var sut = new SimpleEvaluatorSelector();
      var expression = new Expression(null, _autofixture.Create<string>());

      // Act
      var result = sut.GetEvaluator(expression);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.IsInstanceOf<PathExpressionEvaluator>(result, "Result type");
    }

    #endregion
  }
}

