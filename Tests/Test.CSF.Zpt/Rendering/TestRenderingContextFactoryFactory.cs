using System;
using CSF.Zpt.Rendering;
using NUnit.Framework;
using Moq;
using Ploeh.AutoFixture;
using CSF.Zpt.Tales;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  public class TestRenderingContextFactoryFactory
  {
    #region fields

    private IRenderingContextFactoryFactory _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _sut = new RenderingContextFactoryFactory();
    }

    #endregion

    #region tests

    [Test]
    public void Create_creates_default_factory()
    {
      // Act
      var result = _sut.Create(null);

      // Assert
      Assert.IsInstanceOf<TalesRenderingContextFactory>(result);
    }

    [Test]
    public void Create_creates_factory_by_type_name()
    {
      // Act
      var result = _sut.Create(typeof(StubRenderingContextFactory).AssemblyQualifiedName);

      // Assert
      Assert.IsInstanceOf<StubRenderingContextFactory>(result);
    }

    [Test]
    [ExpectedException(typeof(CouldNotCreateRenderingContextFactoryException))]
    public void Create_throws_exception_for_unknown_type_name()
    {
      // Act
      _sut.Create("ZZNonExistantType");
    }

    #endregion
  }
}

