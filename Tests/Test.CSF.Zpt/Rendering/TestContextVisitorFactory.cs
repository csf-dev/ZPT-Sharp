using System;
using CSF.Zpt.Rendering;
using NUnit.Framework;
using Moq;
using Ploeh.AutoFixture;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  public class TestContextVisitorFactory
  {
    #region fields

    private IContextVisitorFactory _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _sut = new ContextVisitorFactory();
    }

    #endregion

    #region tests

    [Test]
    public void Create_creates_instance()
    {
      // Act
      var result = _sut.Create(typeof(StubContextVisitor).AssemblyQualifiedName);

      // Assert
      Assert.IsInstanceOf<StubContextVisitor>(result);
    }

    [Test]
    [ExpectedException(typeof(CouldNotCreateContextVisitorException))]
    public void Create_throws_exception_when_no_class_specified()
    {
      // Act
      _sut.Create(null);
    }

    [Test]
    [ExpectedException(typeof(CouldNotCreateContextVisitorException))]
    public void Create_throws_exception_for_nonexistent_type()
    {
      // Act
      _sut.Create("ZZNonExistentType");
    }

    [Test]
    public void CreateMany_creates_multiple_instances()
    {
      // Arrange
      var typeNames = String.Concat(typeof(StubContextVisitor).AssemblyQualifiedName,
                                    ";",
                                    typeof(AnotherStubContextVisitor).AssemblyQualifiedName);

      // Act
      var results = _sut.CreateMany(typeNames);

      // Assert
      Assert.AreEqual(2, results.Length, "Count of results");
      Assert.IsInstanceOf<StubContextVisitor>(results[0], "First result");
      Assert.IsInstanceOf<AnotherStubContextVisitor>(results[1], "Second result");
    }

    #endregion
  }
}

