using NUnit.Framework;
using System;
using CSF.Zpt.Metal;
using Moq;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt.Metal
{
  [TestFixture]
  public class MetalMacroContainerTests
  {
    [Test]
    public void Constructor_should_raise_duplicate_macro_exception_if_there_are_duplicate_macros()
    {
      // Arrange
      var macros = new [] {
        CreateMacro("one"),
        CreateMacro("two"),
        CreateMacro("one"),
      };

      // Act & assert
      Assert.Throws<DuplicateMacroException>(() => new MetalMacroContainer(macros));
    }

    [Test]
    public void Constructor_does_not_raise_exception_if_there_are_no_duplicate_macros()
    {
      // Arrange
      var macros = new [] {
        CreateMacro("one"),
        CreateMacro("two"),
        CreateMacro("three"),
      };

      // Act & assert
      Assert.DoesNotThrow(() => new MetalMacroContainer(macros));
    }

    IMetalMacro CreateMacro(string name)
    {
      var element = Mock.Of<IZptElement>();
      return Mock.Of<IMetalMacro>(x => x.Name == name && x.Element == element);
    }
  }
}
