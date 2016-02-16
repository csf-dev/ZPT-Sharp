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
      var sut = new TalesModel(Mock.Of<IEvaluatorRegistry>());

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

      var sut = new TalesModel(registry.Object);

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

      var sut = new TalesModel(registry.Object);

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

    [Test]
    public void TestTryGetRootObject_Nothing()
    {
      // Arrange
      var registry = new Mock<IEvaluatorRegistry>();
      var sut = new TalesModel(registry.Object);

      // Act
      object output;
      var result = sut.TryGetRootObject("nothing",
                                        Mock.Of<ZptElement>(x => x.GetOriginalAttributes() == new OriginalAttributeValuesCollection()),
                                        out output);

      // Assert
      Assert.IsTrue(result, "Overall success");
      Assert.IsNull(output, "Output nullability");
    }

    [Test]
    public void TestTryGetRootObject_Default()
    {
      // Arrange
      var registry = new Mock<IEvaluatorRegistry>();
      var sut = new TalesModel(registry.Object);

      // Act
      object output;
      var result = sut.TryGetRootObject("default",
                                        Mock.Of<ZptElement>(x => x.GetOriginalAttributes() == new OriginalAttributeValuesCollection()),
                                        out output);

      // Assert
      Assert.IsTrue(result, "Overall success");
      Assert.NotNull(output, "Output nullability");
      Assert.AreSame(Model.CancelAction, output, "Expected output");
    }

    [Test]
    public void TestTryGetRootObject_Options()
    {
      // Arrange
      var registry = new Mock<IEvaluatorRegistry>();
      var sut = new TalesModel(registry.Object);

      // Act
      object output;
      var result = sut.TryGetRootObject("options",
                                        Mock.Of<ZptElement>(x => x.GetOriginalAttributes() == new OriginalAttributeValuesCollection()),
                                        out output);

      // Assert
      Assert.IsTrue(result, "Overall success");
      Assert.NotNull(output, "Output nullability");
      Assert.IsInstanceOf<TemplateKeywordOptions>(output, "Expected output type");
    }

    [Test]
    public void TestTryGetRootObject_Repeat()
    {
      // Arrange
      var registry = new Mock<IEvaluatorRegistry>();
      var sut = new TalesModel(registry.Object);

      // Act
      object output;
      var result = sut.TryGetRootObject("repeat",
                                        Mock.Of<ZptElement>(x => x.GetOriginalAttributes() == new OriginalAttributeValuesCollection()),
                                        out output);

      // Assert
      Assert.IsTrue(result, "Overall success");
      Assert.NotNull(output, "Output nullability");
      Assert.IsInstanceOf<ContextualisedRepetitionSummaryWrapper>(output, "Expected output type");
    }

    [Test]
    public void TestTryGetRootObject_Attrs()
    {
      // Arrange
      var registry = new Mock<IEvaluatorRegistry>();
      var sut = new TalesModel(registry.Object);

      // Act
      object output;
      var result = sut.TryGetRootObject("attrs",
                                        Mock.Of<ZptElement>(x => x.GetOriginalAttributes() == new OriginalAttributeValuesCollection()),
                                        out output);

      // Assert
      Assert.IsTrue(result, "Overall success");
      Assert.NotNull(output, "Output nullability");
      Assert.IsInstanceOf<OriginalAttributeValuesCollection>(output, "Expected output type");
    }

    [Test]
    public void TestTryGetRootObject_Contexts()
    {
      // Arrange
      var registry = new Mock<IEvaluatorRegistry>();
      var sut = new TalesModel(registry.Object);

      // Act
      object output;
      var result = sut.TryGetRootObject("CONTEXTS",
                                        Mock.Of<ZptElement>(x => x.GetOriginalAttributes() == new OriginalAttributeValuesCollection()),
                                        out output);

      // Assert
      Assert.IsTrue(result, "Overall success");
      Assert.NotNull(output, "Output nullability");
      Assert.IsInstanceOf<BuiltinContextsContainer>(output, "Expected output type");
    }

    #endregion
  }
}

