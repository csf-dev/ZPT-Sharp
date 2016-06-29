using System;
using NUnit.Framework;
using CSF.Zpt.Tales;

namespace Test.CSF.Zpt.Tales
{
  [TestFixture]
  public class TestPathWalker
  {
    #region tests

    [TestCase("foo/bar",                1,  true)]
    [TestCase("foo/bar",                2,  false)]
    [TestCase("foo/bar",                3,  false)]
    [TestCase("foo/bar|wibble/wobble",  1,  true)]
    [TestCase("foo/bar|wibble/wobble",  2,  true)]
    [TestCase("foo/bar|wibble/wobble",  3,  false)]
    public void TestNextComponent(string pathString, int numberOfCalls, bool expectedResult)
    {
      // Arrange
      var path = Path.Create(pathString);
      var sut = new PathWalker(path);
      bool result = false;

      // Act
      for(int i = 0; i < numberOfCalls; i++)
      {
        result = sut.NextComponent();
      }

      // Assert
      Assert.AreEqual(expectedResult, result);
    }

    [TestCase("foo/bar",                1,  2)]
    [TestCase("foo/bar|wibble",         1,  2)]
    [TestCase("foo/bar|wibble",         2,  1)]
    public void TestCurrentComponent(string pathString, int component, int expectedPartCount)
    {
      // Arrange
      var path = Path.Create(pathString);
      var sut = new PathWalker(path);
      int result = 0;

      // Act
      for(int i = 0; i < component; i++)
      {
        sut.NextComponent();
        result = sut.CurrentComponent.Parts.Count;
      }

      // Assert
      Assert.AreEqual(expectedPartCount, result);
    }

    [TestCase("foo/bar",                1,  1,  true)]
    [TestCase("foo/bar",                1,  2,  true)]
    [TestCase("foo/bar",                1,  3,  false)]
    [TestCase("foo/bar|wibble",         1,  1,  true)]
    [TestCase("foo/bar|wibble",         1,  2,  true)]
    [TestCase("foo/bar|wibble",         1,  3,  false)]
    [TestCase("foo/bar|wibble",         2,  1,  true)]
    [TestCase("foo/bar|wibble",         2,  2,  false)]
    [TestCase("foo/bar|wibble",         2,  3,  false)]
    public void TestNextPart(string pathString, int component, int numberOfCalls, bool expectedResult)
    {
      // Arrange
      var path = Path.Create(pathString);
      var sut = new PathWalker(path);
      bool result = false;

      for(int i = 0; i < component; i++)
      {
        sut.NextComponent();
      }

      // Act
      for(int i = 0; i < numberOfCalls; i++)
      {
        result = sut.NextPart();
      }

      // Assert
      Assert.AreEqual(expectedResult, result);
    }

    [TestCase("foo/bar",                1,  1,  "foo")]
    [TestCase("foo/bar",                1,  2,  "bar")]
    [TestCase("foo/bar|wibble/wobble",  1,  1,  "foo")]
    [TestCase("foo/bar|wibble/wobble",  1,  2,  "bar")]
    [TestCase("foo/bar|wibble/wobble",  2,  1,  "wibble")]
    [TestCase("foo/bar|wibble/wobble",  2,  2,  "wobble")]
    public void TestCurrentPart(string pathString, int component, int part, string expectedValue)
    {
      // Arrange
      var path = Path.Create(pathString);
      var sut = new PathWalker(path);

      for(int i = 0; i < component; i++)
      {
        sut.NextComponent();
      }

      // Act
      for(int i = 0; i < part; i++)
      {
        sut.NextPart();
      }

      // Assert
      Assert.NotNull(sut.CurrentPart, "Result nullability");
      Assert.AreEqual(expectedValue, sut.CurrentPart.Value, "Result value");
    }

    [Test]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestNextPartException()
    {
      // Arrange
      var path = Path.Create("foo/bar");
      var sut = new PathWalker(path);

      // Act
      sut.NextPart();

      // Assert (by observing an exception)
    }

    [Test]
    public void TestReset()
    {
      // Arrange
      var path = Path.Create("foo/bar|wibble/wobble");
      var sut = new PathWalker(path);

      sut.NextComponent();
      sut.NextComponent();
      sut.NextPart();
      sut.NextPart();

      // Act
      sut.Reset();

      // Assert
      Assert.IsTrue(sut.NextComponent(),              "NextComponent result");
      Assert.IsTrue(sut.NextPart(),                   "NextPart result");
      Assert.AreEqual("foo",  sut.CurrentPart.Value,  "CurrentPart");
    }

    #endregion
  }
}

