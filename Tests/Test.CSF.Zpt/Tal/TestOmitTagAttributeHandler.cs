using System;
using NUnit.Framework;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tal;
using Ploeh.AutoFixture;
using Moq;
using CSF.Zpt.TestUtils;
using CSF.Zpt.TestUtils.Autofixture;
using CSF.Zpt;

namespace Test.CSF.Zpt.Tal
{
  [TestFixture]
  public class TestOmitTagAttributeHandler
  {
    #region fields

    private IFixture _autofixture;
    private Mock<ZptElement> _element;
    private RenderingContext _context;
    private DummyModel _model;

    private OmitTagAttributeHandler _sut;

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

      _sut = new OmitTagAttributeHandler();
    }

    #endregion

    #region tests

    [Test]
    public void TestHandleNoAttribute()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OmitTagAttribute))
        .Returns((ZptAttribute) null);

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Omit(), Times.Never());
    }

    //          Condition   Cancel    Expect element
    //          ---------   ------    --------------
    [TestCase(  true,       false,    false)]
    [TestCase(  false,      false,    true)]
    [TestCase(  true,       true,     true)]
    [TestCase(  false,      true,     true)]
    public void TestHandleConditionPresent(bool conditionValue,
                                           bool cancelsAction,
                                           bool expectElement)
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OmitTagAttribute))
        .Returns(Mock.Of<ZptAttribute>(x => x.Value == _autofixture.Create<string>()));
      _element
        .Setup(x => x.Omit())
        .Returns(new[] { Mock.Of<ZptElement>(), Mock.Of<ZptElement>(), Mock.Of<ZptElement>() });

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
        _element.Verify(x => x.Omit(), Times.Never());
      }
      else
      {
        Assert.AreEqual(0, result.Contexts.Length, "Count of results");
        Assert.AreEqual(3, result.NewlyExposedContexts.Length, "Count of new elements exposed");
        _element.Verify(x => x.Omit(), Times.Once());
      }
      Mock.Get(_model).Verify(x => x.Evaluate(It.IsAny<string>(), _context), Times.Once());
    }

    #endregion
  }
}

