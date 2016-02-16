using System;
using NUnit.Framework;
using CSF.Zpt.Tales;
using Moq;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using Test.CSF.Zpt.Util;

namespace Test.CSF.Zpt.Tales
{
  [TestFixture]
  public class TestObjectTraverser
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
    public void TestTraverseCustomHandler()
    {
      // Arrange
      var path = _autofixture.Create<string>();

      var source = new Mock<ITalesPathHandler>();
      var output = _autofixture.Create<object>();
      source.Setup(x => x.HandleTalesPath(path, out output)).Returns(true);

      // Act
      object exposedOuput;
      bool result = ObjectTraverser.Default.Traverse(source.Object, path, out exposedOuput);

      // Assert
      Assert.IsTrue(result, "Result");
      Assert.AreSame(output, exposedOuput, "Exposed output");
    }

    [Test]
    public void TestTraverseStringIndexer()
    {
      // Arrange
      var path = _autofixture.Create<string>();

      var source = new Dictionary<string,object>();
      var output = _autofixture.Create<object>();
      source.Add(path, output);

      // Act
      object exposedOuput;
      bool result = ObjectTraverser.Default.Traverse(source, path, out exposedOuput);

      // Assert
      Assert.IsTrue(result, "Result");
      Assert.AreSame(output, exposedOuput, "Exposed output");
    }

    [Test]
    public void TestTraverseIntegerIndexer()
    {
      // Arrange
      var path = "0";

      var source = new List<object>();
      var output = _autofixture.Create<object>();
      source.Add(output);

      // Act
      object exposedOuput;
      bool result = ObjectTraverser.Default.Traverse(source, path, out exposedOuput);

      // Assert
      Assert.IsTrue(result, "Result");
      Assert.AreSame(output, exposedOuput, "Exposed output");
    }

    [Test]
    public void TestTraverseProperty()
    {
      // Arrange
      var path = "Length";

      var source = "Thirteen";
      var output = 8;

      // Act
      object exposedOuput;
      bool result = ObjectTraverser.Default.Traverse(source, path, out exposedOuput);

      // Assert
      Assert.IsTrue(result, "Result");
      Assert.AreEqual(output, exposedOuput, "Exposed output");
    }

    [Test]
    public void TestTraversePropertyRefType()
    {
      // Arrange
      var path = "Day";

      var source = new DateTime(2000,01,01);
      var output = 1;

      // Act
      object exposedOuput;
      bool result = ObjectTraverser.Default.Traverse(source, path, out exposedOuput);

      // Assert
      Assert.IsTrue(result, "Result");
      Assert.AreEqual(output, exposedOuput, "Exposed output");
    }

    [Test]
    public void TestTraverseMethod()
    {
      // Arrange
      var path = "Trim";

      var source = " Foo bar baz ";
      var output = "Foo bar baz";

      // Act
      object exposedOuput;
      bool result = ObjectTraverser.Default.Traverse(source, path, out exposedOuput);

      // Assert
      Assert.IsTrue(result, "Result");
      Assert.AreEqual(output, exposedOuput, "Exposed output");
    }

    [Test]
    public void TestTraverseMethodRefType()
    {
      // Arrange
      var path = "ToString";

      var source = 13;
      var output = "13";

      // Act
      object exposedOuput;
      bool result = ObjectTraverser.Default.Traverse(source, path, out exposedOuput);

      // Assert
      Assert.IsTrue(result, "Result");
      Assert.AreEqual(output, exposedOuput, "Exposed output");
    }

    [Test]
    public void TestTraverseField()
    {
      // Arrange
      var path = "SomeField";

      var source = new DummyObjectWithField();
      var output = _autofixture.Create<object>();
      source.SomeField = output;

      // Act
      object exposedOuput;
      bool result = ObjectTraverser.Default.Traverse(source, path, out exposedOuput);

      // Assert
      Assert.IsTrue(result, "Result");
      Assert.AreEqual(output, exposedOuput, "Exposed output");
    }

    #endregion
  }
}

