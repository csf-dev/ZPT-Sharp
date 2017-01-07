using System;
using NUnit.Framework;
using CSF.Zpt;
using Moq;
using System.Reflection;
using Ploeh.AutoFixture;
using System.IO;
using CSF.IO;

namespace Test.CSF.Zpt
{
  [TestFixture]
  public class TestPluginAssemblyLoader
  {
    #region fields

    private Mock<PluginAssemblyLoader> _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _sut = new Mock<PluginAssemblyLoader>() { CallBase = true };

      _sut.Setup(x => x.LoadAbsolute(It.IsAny<string>())).Returns(Assembly.GetExecutingAssembly());
      _sut.Setup(x => x.LoadRelative(It.IsAny<string>())).Returns(Assembly.GetExecutingAssembly());
    }

    #endregion

    #region tests

    [Test]
    public void Load_with_relative_path_uses_correct_method()
    {
      // Arrange
      var relPath = "foo";

      // Act
      _sut.Object.Load(relPath);

      // Assert
      _sut.Verify(x => x.LoadAbsolute(It.IsAny<string>()), Times.Never());
      _sut.Verify(x => x.LoadRelative(It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void Load_with_absolute_path_uses_correct_method()
    {
      // Arrange
      var thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
      var thisAssemblyFile = new FileInfo(thisAssemblyPath);
      var thisAssemblyDir = thisAssemblyFile.GetParentDirectory();
      var absPath = Path.Combine(thisAssemblyDir.FullName, "Foo.dll");

      // Act
      _sut.Object.Load(absPath);

      // Assert
      _sut.Verify(x => x.LoadAbsolute(It.IsAny<string>()), Times.Once());
      _sut.Verify(x => x.LoadRelative(It.IsAny<string>()), Times.Never());
    }

    #endregion
  }
}

