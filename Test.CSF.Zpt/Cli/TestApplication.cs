using System;
using NUnit.Framework;
using CSF.Zpt.Cli;
using System.IO;
using Moq;
using Ploeh.AutoFixture;

namespace Test.CSF.Zpt.Cli
{
  [TestFixture]
  public class TestApplication
  {
    #region fields

    private IFixture _autofixture;

    #endregion

    #region setup/teardown

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
    }

    [TearDown]
    public void Teardown()
    {
      RestoreConsoleToUsualStreams();
    }

    #endregion

    #region tests

    [Test]
    public void WritesErrorToStdErrWhenCommandLineOptionsAreInvalid()
    {
      // Arrange
      var commandlineOptionsFactory = new Mock<ICommandLineOptionsFactory>();
      var sut = new Application(commandlineOptionsFactory: commandlineOptionsFactory.Object);
      var args = _autofixture.Create<string[]>();

      commandlineOptionsFactory
        .Setup(x => x.GetOptions(args))
        .Throws<OptionsParsingException>();

      string errorOutput;

      using(var writer = SetupStdErrRedirection())
      {
        // Act
        sut.Begin(args);

        errorOutput = writer.ToString();
      }

      // Assert
      Assert.AreEqual("The command-line arguments must be in a valid format.", errorOutput);
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
      var outputWriter = new StreamWriter(Console.OpenStandardOutput());
      outputWriter.AutoFlush = true;
      Console.SetOut(outputWriter);

      var errorWriter = new StreamWriter(Console.OpenStandardError());
      errorWriter.AutoFlush = true;
      Console.SetError(errorWriter);
    }

    #endregion
  }
}

