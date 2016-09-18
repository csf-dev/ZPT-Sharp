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

      string errorOutput = string.Empty;

      using(var writer = SetupStdErrRedirection())
      {
        // Act
        sut.Begin(args);

        errorOutput = writer.ToString();
      }

      // Assert
      string expected = @"ERROR: The command-line parameters provided were not be understood.
Use 'ZptBuilder.exe --help' for help on providing correct parameters, or consult the manual.
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

      string errorOutput = string.Empty;
      using(var writer = SetupStdErrRedirection())
      {
        // Act
        sut.Begin(options);

        errorOutput = writer.ToString();
      }

      // Assert
      string expected = @"ERROR: The '--html' and '--xml' options are mutually exclusive and may not be used together.
Use 'ZptBuilder.exe --help' for help on providing correct parameters, or consult the manual.
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

    #endregion
  }
}

