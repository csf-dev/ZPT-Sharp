using System;
using NUnit.Framework;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tal;
using Ploeh.AutoFixture;
using Moq;
using Test.CSF.Zpt.Util;
using Test.CSF.Zpt.Util.Autofixture;
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

    [Test]
    [Description("Tests the special case where the element itself is in the 'tal' namespace")]
    public void TestHandleElementInTalNamespace()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OmitTagAttribute))
        .Returns((ZptAttribute) null);
      _element
        .Setup(x => x.IsInNamespace(ZptConstants.Tal.Namespace))
        .Returns(true);
      _element
        .Setup(x => x.Omit())
        .Returns(new[] { Mock.Of<ZptElement>(), Mock.Of<ZptElement>(), Mock.Of<ZptElement>() });

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.AreEqual(0, result.Contexts.Length, "Count of results");
      Assert.AreEqual(3, result.NewlyExposedContexts.Length, "Count of new elements exposed");
      _element.Verify(x => x.Omit(), Times.Once());
    }

    [Test]
    [Description("Tests the special case where the element itself is in the 'metal' namespace")]
    public void TestHandleElementInMetalNamespace()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OmitTagAttribute))
        .Returns((ZptAttribute) null);
      _element
        .Setup(x => x.IsInNamespace(ZptConstants.Metal.Namespace))
        .Returns(true);
      _element
        .Setup(x => x.Omit())
        .Returns(new[] { Mock.Of<ZptElement>(), Mock.Of<ZptElement>(), Mock.Of<ZptElement>() });

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.AreEqual(0, result.Contexts.Length, "Count of results");
      Assert.AreEqual(3, result.NewlyExposedContexts.Length, "Count of new elements exposed");
      _element.Verify(x => x.Omit(), Times.Once());
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
        .Returns(new ExpressionResult(cancelsAction? Model.CancelAction : conditionValue));

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

