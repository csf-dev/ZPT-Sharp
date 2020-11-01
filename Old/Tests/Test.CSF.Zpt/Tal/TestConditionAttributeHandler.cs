using System;
using NUnit.Framework;
using Moq;
using Ploeh.AutoFixture;
using CSF.Zpt.Tal;
using CSF.Zpt.Rendering;
using CSF.Zpt.TestUtils;
using CSF.Zpt.TestUtils.Autofixture;
using CSF.Zpt;

namespace Test.CSF.Zpt.Tal
{
  [TestFixture]
  public class TestConditionAttributeHandler
  {
    #region fields

    private IFixture _autofixture;
    private Mock<ZptElement> _element;
    private DummyModel _model;
    private RenderingContext _context;

    private ConditionAttributeHandler _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
      new DummyModelCustomisation().Customize(_autofixture);

      _model = _autofixture.Create<DummyModel>();
      _element = new Mock<ZptElement>() { CallBase = true };
      _context = Mock.Of<RenderingContext>(x => x.Element == _element.Object && x.TalModel == _model);

      _element.Setup(x => x.Remove());

      _sut = new ConditionAttributeHandler();
    }

    #endregion

    #region tests

    [Test]
    public void Handle_makes_no_change_when_attribute_not_present()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ConditionAttribute))
        .Returns((ZptAttribute) null);

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Remove(), Times.Never());
    }

    //          Condition   Cancel    Expect element
    //          ---------   ------    --------------
    [TestCase(  true,       false,    true)]
    [TestCase(  false,      false,    false)]
    [TestCase(  true,       true,     true)]
    [TestCase(  false,      true,     true)]
    [Description("This test handles a number of different cases, with various condition values and" +
                 "action-cancellation flags")]
    public void Handle_performs_correct_action_when_attribute_is_present(bool conditionValue,
                                                                         bool cancelsAction,
                                                                         bool expectElement)
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ConditionAttribute))
        .Returns(Mock.Of<ZptAttribute>(x => x.Value == _autofixture.Create<string>()));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(cancelsAction? ZptConstants.CancellationToken : conditionValue));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      if(expectElement)
      {
        Assert.AreEqual(1, result.Contexts.Length, "Count of results");
        Assert.AreSame(_context, result.Contexts[0], "Correct element returned");
        _element.Verify(x => x.Remove(), Times.Never());
      }
      else
      {
        Assert.AreEqual(0, result.Contexts.Length, "Count of results");
        _element.Verify(x => x.Remove(), Times.Once());
      }
      Mock.Get(_model).Verify(x => x.Evaluate(It.IsAny<string>(), _context), Times.Once());
    }

    #endregion
  }
}

