using System;
using NUnit.Framework;
using CSF.Zpt.Cli;
using Moq;
using Ploeh.AutoFixture;
using System.IO;
using CSF.Zpt.BatchRendering;
using System.Linq;
using CSF.Zpt;

namespace Test.CSF.Zpt.Cli
{
  [TestFixture]
  public class TestBatchRenderingOptionsFactory
  {
    #region constants

    private const string DIRECTORY_NAME1 = "test-directory", DIRECTORY_NAME2 = "test-dir-2";

    #endregion

    #region fields

    private IBatchRenderingOptionsFactory _sut;
    private IFixture _autofixture;
    private DirectoryInfo _baseDirectory;

    private CommandLineOptions _options;
    private IBatchRenderingOptions _result;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
      _baseDirectory = new DirectoryInfo(System.Environment.CurrentDirectory);
      _sut = new BatchRenderingOptionsFactory(_baseDirectory);

      _options = new CommandLineOptions();
    }

    [TearDown]
    public void Teardown()
    {
      if(_result != null)
      {
        _result.Dispose();
      }

      if(Directory.Exists(DIRECTORY_NAME1))
      {
        Directory.Delete(DIRECTORY_NAME1);
      }
      if(Directory.Exists(DIRECTORY_NAME2))
      {
        Directory.Delete(DIRECTORY_NAME2);
      }
    }

    #endregion

    #region tests

    [Test]
    public void GetBatchOptions_uses_STDIN_with_applicable_input()
    {
      // Arrange
      _options.InputPaths.Add("-");

      // Act
      ExerciseSut();

      // Assert
      Assert.NotNull(_result.InputStream);
    }

    [Test]
    public void GetBatchOptions_does_not_use_STDIN_with_missing_input()
    {
      // Arrange

      // Act
      ExerciseSut();

      // Assert
      Assert.IsNull(_result.InputStream);
    }

    [Test]
    public void GetBatchOptions_does_not_use_STDIN_with_incorrect_input()
    {
      // Arrange
      _options.InputPaths.Add("foo");

      // Act
      ExerciseSut();

      // Assert
      Assert.IsNull(_result.InputStream);
    }

    [Test]
    public void GetBatchOptions_does_not_use_STDIN_with_multiple_inputs()
    {
      // Arrange
      _options.InputPaths.Add("-");
      _options.InputPaths.Add("-");

      // Act
      ExerciseSut();

      // Assert
      Assert.IsNull(_result.InputStream);
    }

    [Test]
    public void GetBatchOptions_uses_STDOUT_with_standard_input()
    {
      // Arrange
      _options.InputPaths.Add("-");

      // Act
      ExerciseSut();

      // Assert
      Assert.NotNull(_result.OutputStream);
    }

    [Test]
    public void GetBatchOptions_does_not_use_STDOUT_when_input_is_file_and_output_path_specified()
    {
      // Arrange
      _options.InputPaths.Add("a-file");
      _options.OutputPath = "foo";

      // Act
      ExerciseSut();

      // Assert
      Assert.IsNull(_result.OutputStream);
    }

    [Test]
    public void GetBatchOptions_does_not_use_STDOUT_when_input_is_STDIN_and_output_path_specified()
    {
      // Arrange
      _options.InputPaths.Add("-");
      _options.OutputPath = "foo";

      // Act
      ExerciseSut();

      // Assert
      Assert.IsNull(_result.OutputStream);
    }

    [Test]
    public void GetBatchOptions_uses_STDOUT_with_single_input()
    {
      // Arrange
      _options.InputPaths.Add("a-file");

      // Act
      ExerciseSut();

      // Assert
      Assert.NotNull(_result.OutputStream);
    }

    [Test]
    public void GetBatchOptions_does_not_use_STDOUT_with_missing_input()
    {
      // Arrange

      // Act
      ExerciseSut();

      // Assert
      Assert.IsNull(_result.OutputStream);
    }

    [Test]
    public void GetBatchOptions_does_not_use_STDOUT_with_multiple_inputs()
    {
      // Arrange
      _options.InputPaths.Add("-");
      _options.InputPaths.Add("-");

      // Act
      ExerciseSut();

      // Assert
      Assert.IsNull(_result.OutputStream);
    }

    [Test]
    public void GetBatchOptions_does_not_use_STDOUT_with_directory_inputs()
    {
      // Arrange
      Directory.CreateDirectory(DIRECTORY_NAME1);
      _options.InputPaths.Add(DIRECTORY_NAME1);

      // Act
      ExerciseSut();

      // Assert
      Assert.IsNull(_result.OutputStream);
    }

    [Test]
    public void GetBatchOptions_skips_STDIN_in_input_files()
    {
      // Arrange
      _options.InputPaths.Add("-");
      _options.InputPaths.Add("foo");

      // Act
      ExerciseSut();

      // Assert
      Assert.AreEqual(1, _result.InputPaths.Count());
    }

    [Test]
    public void GetBatchOptions_accepts_all_input_paths()
    {
      // Arrange
      _options.InputPaths.Add("foo");
      _options.InputPaths.Add("bar");
      _options.InputPaths.Add("baz");

      // Act
      ExerciseSut();

      // Assert
      Assert.AreEqual(3, _result.InputPaths.Count());
    }

    [Test]
    public void GetBatchOptions_assumes_inputs_are_files()
    {
      // Arrange
      _options.InputPaths.Add("foo");
      _options.InputPaths.Add("bar");
      _options.InputPaths.Add("baz");

      // Act
      ExerciseSut();

      // Assert
      Assert.That(_result.InputPaths.All(x => x is FileInfo));
    }

    [Test]
    public void GetBatchOptions_finds_directories()
    {
      // Arrange
      Directory.CreateDirectory(DIRECTORY_NAME1);
      _options.InputPaths.Add(DIRECTORY_NAME1);

      // Act
      ExerciseSut();

      // Assert
      Assert.That(_result.InputPaths.All(x => x is DirectoryInfo));
    }

    [Test]
    public void GetBatchOptions_finds_output_directory()
    {
      // Arrange
      Directory.CreateDirectory(DIRECTORY_NAME1);
      _options.OutputPath = DIRECTORY_NAME1;

      // Act
      ExerciseSut();

      // Assert
      Assert.IsInstanceOf<DirectoryInfo>(_result.OutputPath);
    }

    [Test]
    public void GetBatchOptions_creates_output_file()
    {
      // Arrange
      var path = "a-file";
      _options.OutputPath = path;

      // Act
      ExerciseSut();

      // Assert
      Assert.IsInstanceOf<FileInfo>(_result.OutputPath);
    }

    [Test]
    public void GetBatchOptions_uses_input_search_pattern()
    {
      // Arrange
      var pattern = _autofixture.Create<string>();
      _options.InputFilenamePattern = pattern;

      // Act
      ExerciseSut();

      // Assert
      Assert.AreEqual(pattern, _result.InputSearchPattern);
    }

    [Test]
    public void GetBatchOptions_uses_extension_override()
    {
      // Arrange
      var pattern = _autofixture.Create<string>();
      _options.OutputFilenameExtension = pattern;

      // Act
      ExerciseSut();

      // Assert
      Assert.AreEqual(pattern, _result.OutputExtensionOverride);
    }

    [Test]
    public void GetBatchOptions_uses_ignored_paths()
    {
      // Arrange
      var dirs = new [] {
        new DirectoryInfo(DIRECTORY_NAME1),
        new DirectoryInfo(DIRECTORY_NAME2),
      };

      foreach(var dir in dirs)
      {
        Directory.CreateDirectory(dir.Name);
      }
      _options.IgnoredPaths = String.Join(";", dirs.Select(x => x.Name));

      // Act
      ExerciseSut();

      // Assert
      CollectionAssert.AreEquivalent(dirs, _result.IgnoredPaths);
    }

    [Test]
    public void GetBatchOptions_uses_forced_xml_mode()
    {
      // Arrange
      _options.ForceXmlMode = true;

      // Act
      ExerciseSut();

      // Assert
      Assert.AreEqual(RenderingMode.Xml, _result.RenderingMode);
    }

    [Test]
    public void GetBatchOptions_uses_forced_html_mode()
    {
      // Arrange
      _options.ForceHtmlMode = true;

      // Act
      ExerciseSut();

      // Assert
      Assert.AreEqual(RenderingMode.Html, _result.RenderingMode);
    }

    [Test]
    public void GetBatchOptions_uses_no_forced_mode()
    {
      // Arrange

      // Act
      ExerciseSut();

      // Assert
      Assert.IsNull(_result.RenderingMode);
    }

    #endregion

    #region methods

    private void ExerciseSut()
    {
      _result = _sut.GetBatchOptions(_options);
    }

    #endregion
  }
}

