using System;
using NUnit.Framework;
using CSF.Zpt.Cli;
using System.IO;
using Moq;
using Ploeh.AutoFixture;
using CSF.Zpt.Cli.Exceptions;
using CSF.Zpt.BatchRendering;
using CSF.Zpt.Rendering;
using CSF.Zpt;

namespace Test.CSF.Zpt.Cli
{
  [TestFixture]
  public class TestApplication
  {
    #region fields

    private IFixture _autofixture;
    private Mock<IApplicationTerminator> _terminator;
    private Mock<IBatchRenderer> _renderer;

    #endregion

    #region setup/teardown

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();

      _terminator = new Mock<IApplicationTerminator>();
      _terminator.Setup(x => x.Terminate());
      _terminator.Setup(x => x.Terminate(It.IsAny<int>()));

      _renderer = new Mock<IBatchRenderer>();
      _renderer
        .Setup(x => x.Render(It.IsAny<IRenderingOptions>(),
                             It.IsAny<IBatchRenderingOptions>(),
                             It.IsAny<RenderingMode?>()))
        .Returns(Mock.Of<IBatchRenderingResponse>());
    }

    [TearDown]
    public void Teardown()
    {
      RestoreConsoleToUsualStreams();
    }

    #endregion

    #region tests

    [Test]
    public void WritesErrorToStdErrAndTerminates_WhenCommandLineOptionsAreInvalid()
    {
      // Arrange
      var commandlineOptionsFactory = new Mock<ICommandLineOptionsFactory>();
      var sut = new Application(commandlineOptionsFactory: commandlineOptionsFactory.Object,
                                renderer: _renderer.Object,
                                terminator: _terminator.Object);
      var args = _autofixture.Create<string[]>();

      commandlineOptionsFactory
        .Setup(x => x.GetOptions(args))
        .Throws<OptionsParsingException>();

      // Act
      var errorOutput = ExerciseSutWithStdErrRedirection(sut, args);

      // Assert
      string expected = @"ERROR: The command-line arguments provided were not be understood.
Use 'ZptBuilder.exe --help' for help on providing correct arguments, or consult the manual.
";
      Assert.AreEqual(expected, errorOutput, "Correct message written");
      _terminator.Verify(x => x.Terminate(ApplicationTerminator.ExpectedErrorExitCode),
                         Times.Once(),
                         "Application should be terminated after writing message");
    }

    [Test]
    public void WritesErrorToStdErrAndTerminates_WhenBothHtmlAndXmlModesSpecified()
    {
      // Arrange
      var sut = new Application(renderer: _renderer.Object,
                                terminator: _terminator.Object);
      var options = new CommandLineOptions() {
        ForceHtmlMode = true,
        ForceXmlMode = true,
      };

      // Act
      var errorOutput = ExerciseSutWithStdErrRedirection(sut, options);

      // Assert
      string expected = @"ERROR: The '--html' and '--xml' arguments are mutually exclusive and may not be used together.
Use 'ZptBuilder.exe --help' for help on providing correct arguments, or consult the manual.
";
      Assert.AreEqual(expected, errorOutput, "Correct message written");
      _terminator.Verify(x => x.Terminate(ApplicationTerminator.ExpectedErrorExitCode),
                         Times.Once(),
                         "Application should be terminated after writing message");
    }

    [Test]
    public void WritesErrorToStdErrAndTerminates_WhenInvalidInputFilesAreSpecified()
    {
      // Arrange
      var filename = "FOO BAR";

      var batchFactory = new Mock<IBatchRenderingOptionsFactory>();
      batchFactory
        .Setup(x => x.GetBatchOptions(It.IsAny<CommandLineOptions>()))
        .Throws(new InvalidInputPathException() { Path = filename });

      var sut = new Application(renderer: _renderer.Object,
                                batchOptionsFactory: batchFactory.Object,
                                terminator: _terminator.Object);
      var options = new CommandLineOptions();

      // Act
      var errorOutput = ExerciseSutWithStdErrRedirection(sut, options);

      // Assert
      string expected = @"ERROR: A specified input path is invalid, perhaps it does not exist or you have insufficient permissions to access it?
The problematic path is:FOO BAR
";
      Assert.AreEqual(expected, errorOutput, "Correct message written");
      _terminator.Verify(x => x.Terminate(ApplicationTerminator.ExpectedErrorExitCode),
                         Times.Once(),
                         "Application should be terminated after writing message");
    }

    [Test]
    public void WritesErrorToStdErrAndTerminates_WhenInvalidContextVisitorSpecified()
    {
      // Arrange
      var classname = "FOO BAR";
      var optionsFactory = new Mock<IRenderingOptionsFactory>();
      optionsFactory
        .Setup(x => x.GetOptions(It.IsAny<CommandLineOptions>()))
        .Throws(new CouldNotCreateContextVisitorException() { InvalidClassname = classname });

      var sut = new Application(renderer: _renderer.Object,
                                renderingOptionsFactory: optionsFactory.Object,
                                terminator: _terminator.Object);
      var options = new CommandLineOptions();

      // Act
      var errorOutput = ExerciseSutWithStdErrRedirection(sut, options);

      // Assert
      string expected = @"ERROR: A type named using the --context-visitors argument could not be instantiated.
Where specified, these types must exist and have parameterless constructors.
The invalid type is:FOO BAR
";
      Assert.AreEqual(expected, errorOutput, "Correct message written");
      _terminator.Verify(x => x.Terminate(ApplicationTerminator.ExpectedErrorExitCode),
                         Times.Once(),
                         "Application should be terminated after writing message");
    }

    [Test]
    public void WritesErrorToStdErrAndTerminates_WhenInvalidRenderingContextFactorySpecified()
    {
      // Arrange
      var optionsFactory = new Mock<IRenderingOptionsFactory>();
      optionsFactory
        .Setup(x => x.GetOptions(It.IsAny<CommandLineOptions>()))
        .Throws(new CouldNotCreateRenderingContextFactoryException() { InvalidClassname = _autofixture.Create<string>() });

      var sut = new Application(renderer: _renderer.Object,
                                renderingOptionsFactory: optionsFactory.Object,
                                terminator: _terminator.Object);
      var options = new CommandLineOptions();

      // Act
      var errorOutput = ExerciseSutWithStdErrRedirection(sut, options);

      // Assert
      string expected = @"ERROR: The type named using the --rendering-context-factory argument could not be instantiated.
Where specified, this type must exist and have a parameterless constructor.
";
      Assert.AreEqual(expected, errorOutput, "Correct message written");
      _terminator.Verify(x => x.Terminate(ApplicationTerminator.ExpectedErrorExitCode),
                         Times.Once(),
                         "Application should be terminated after writing message");
    }

    #endregion

    #region methods

    private StringWriter SetupStdOutRedirection()
    {
      return SetupRedirection(Console.SetOut);
    }

    private StringWriter SetupStdErrRedirection()
    {
      return SetupRedirection(Console.SetError);
    }

    private StringWriter SetupRedirection(Action<TextWriter> redirectAction)
    {
      var output = new StringWriter();
      redirectAction(output);
      return output;
    }

    private void RestoreConsoleToUsualStreams()
    {
      if(Console.IsOutputRedirected)
      {
        var outputWriter = new StreamWriter(Console.OpenStandardOutput());
        outputWriter.AutoFlush = true;
        Console.SetOut(outputWriter);
      }

      if(Console.IsErrorRedirected)
      {
        var errorWriter = new StreamWriter(Console.OpenStandardError());
        errorWriter.AutoFlush = true;
        Console.SetError(errorWriter);
      }
    }

    private string ExerciseSutWithStdErrRedirection(Application sut,
                                                    CommandLineOptions options)
    {
      string errorOutput;

      using(var writer = SetupStdErrRedirection())
      {
        sut.Begin(options);

        errorOutput = writer.ToString();
      }

      return errorOutput;
    }

    private string ExerciseSutWithStdErrRedirection(Application sut,
                                                    string[] args)
    {
      string errorOutput;

      using(var writer = SetupStdErrRedirection())
      {
        sut.Begin(args);

        errorOutput = writer.ToString();
      }

      return errorOutput;
    }

    #endregion
  }
}

