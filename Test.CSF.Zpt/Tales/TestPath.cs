using System;
using NUnit.Framework;
using CSF.Zpt.Tales;
using System.Linq;

namespace Test.CSF.Zpt.Tales
{
  [TestFixture]
  public class TestPath
  {
    #region tests

    [Test]
    [Description("Tests a Path with one component")]
    public void TestCreateScenarioOne()
    {
      // Arrange (nothing to do)

      // Act
      var sut = Path.Create("foo/bar");

      // Assert
      Assert.NotNull(sut,                                               "Result nullability");
      Assert.AreEqual(1,          sut.Components.Count,                 "Count of components");
      Assert.AreEqual(2,          sut.Components[0].Parts.Count,        "Count of parts (Component 1)");
      Assert.AreEqual("foo",      sut.Components[0].Parts[0].Value,     "Component 1, Part 1 value");
      Assert.AreEqual("bar",      sut.Components[0].Parts[1].Value,     "Component 1, Part 2 value");
      Assert.IsFalse(sut.Components[0].Parts.Any(x => x.IsInterpolated),"Component 1 has any parts which are interpolated");
    }

    [Test]
    [Description("Tests a Path with two components; no whitespace")]
    public void TestCreateScenarioTwo()
    {
      // Arrange (nothing to do)

      // Act
      var sut = Path.Create("foo/bar|wibble/wobble");

      // Assert
      Assert.NotNull(sut,                                               "Result nullability");
      Assert.AreEqual(2,          sut.Components.Count,                 "Count of components");
      Assert.AreEqual(2,          sut.Components[0].Parts.Count,        "Count of parts (Component 1)");
      Assert.AreEqual("foo",      sut.Components[0].Parts[0].Value,     "Component 1, Part 1 value");
      Assert.AreEqual("bar",      sut.Components[0].Parts[1].Value,     "Component 1, Part 2 value");
      Assert.AreEqual(2,          sut.Components[1].Parts.Count,        "Count of parts (Component 2)");
      Assert.AreEqual("wibble",   sut.Components[1].Parts[0].Value,     "Component 2, Part 1 value");
      Assert.AreEqual("wobble",   sut.Components[1].Parts[1].Value,     "Component 2, Part 2 value");
    }

    [Test]
    [Description("Tests a Path with two components; with whitespace between the components")]
    public void TestCreateScenarioThree()
    {
      // Arrange (nothing to do)

      // Act
      var sut = Path.Create(@"  foo/bar
                              | wibble/wobble
                              | spong/splurge");

      // Assert
      Assert.NotNull(sut,                                               "Result nullability");
      Assert.AreEqual(3,          sut.Components.Count,                 "Count of components");
      Assert.AreEqual(2,          sut.Components[0].Parts.Count,        "Count of parts (Component 1)");
      Assert.AreEqual("foo",      sut.Components[0].Parts[0].Value,     "Component 1, Part 1 value");
      Assert.AreEqual("bar",      sut.Components[0].Parts[1].Value,     "Component 1, Part 2 value");
      Assert.AreEqual(2,          sut.Components[1].Parts.Count,        "Count of parts (Component 2)");
      Assert.AreEqual("wibble",   sut.Components[1].Parts[0].Value,     "Component 2, Part 1 value");
      Assert.AreEqual("wobble",   sut.Components[1].Parts[1].Value,     "Component 2, Part 2 value");
      Assert.AreEqual(2,          sut.Components[2].Parts.Count,        "Count of parts (Component 3)");
      Assert.AreEqual("spong",    sut.Components[2].Parts[0].Value,     "Component 3, Part 1 value");
      Assert.AreEqual("splurge",  sut.Components[2].Parts[1].Value,     "Component 3, Part 2 value");
    }

    [Test]
    [Description("Tests a Path with one component and an interpolated item")]
    public void TestCreateScenarioFour()
    {
      // Arrange (nothing to do)

      // Act
      var sut = Path.Create("foo/?inter/baz");

      // Assert
      Assert.NotNull(sut,                                               "Result nullability");
      Assert.AreEqual(1,          sut.Components.Count,                 "Count of components");
      Assert.AreEqual(3,          sut.Components[0].Parts.Count,        "Count of parts (Component 1)");
      Assert.AreEqual("foo",      sut.Components[0].Parts[0].Value,     "Component 1, Part 1 value");
      Assert.AreEqual("inter",    sut.Components[0].Parts[1].Value,     "Component 1, Part 2 value");
      Assert.AreEqual("baz",      sut.Components[0].Parts[2].Value,     "Component 1, Part 3 value");
      Assert.IsTrue(sut.Components[0].Parts[1].IsInterpolated,          "Component 1, Part 2 interpolated");
    }

    [Test]
    [Description("Tests a Path with one and inner whitespace")]
    public void TestCreateScenarioFive()
    {
      // Arrange (nothing to do)

      // Act
      var sut = Path.Create("foo/w space/ more /space");

      // Assert
      Assert.NotNull(sut,                                               "Result nullability");
      Assert.AreEqual(1,          sut.Components.Count,                 "Count of components");
      Assert.AreEqual(4,          sut.Components[0].Parts.Count,        "Count of parts (Component 1)");
      Assert.AreEqual("foo",      sut.Components[0].Parts[0].Value,     "Component 1, Part 1 value");
      Assert.AreEqual("w space",  sut.Components[0].Parts[1].Value,     "Component 1, Part 2 value");
      Assert.AreEqual(" more ",   sut.Components[0].Parts[2].Value,     "Component 1, Part 3 value");
      Assert.AreEqual("space",    sut.Components[0].Parts[3].Value,     "Component 1, Part 4 value");
    }

    #endregion
  }
}

