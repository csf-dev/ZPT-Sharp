using System;
using NUnit.Framework;
using CSF.Zpt.Tales;
using System.IO;
using Moq;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt.Tales
{
  [TestFixture]
  public class TestFilesystemDirectory
  {
    [TestCase]
    public void HandleTalesPath_CanTraverseToParentDirectory()
    {
      // Arrange
      object pathResult;
      var originDir = new DirectoryInfo(".");
      var sut = new FilesystemDirectory(originDir);

      // Act
      var result = sut.HandleTalesPath("..", out pathResult, Mock.Of<RenderingContext>());

      // Assert
      Assert.IsTrue(result, "Correct result");
      Assert.IsInstanceOf<FilesystemDirectory>(pathResult, "Path result is correct type");
      var dir = (FilesystemDirectory) pathResult;
      Assert.AreEqual(originDir.Parent, dir.DirectoryInfo, "Correct DirectoryInfo");
    }
  }
}

