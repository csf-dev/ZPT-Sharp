using System;
using NUnit.Framework;
using CSF.Zpt.BatchRendering;
using Moq;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Test.CSF.Zpt.BatchRendering
{
  [TestFixture]
  public class TestBatchRenderingOptionsValidator
  {
    #region fields

    private Mock<IBatchRenderingOptions> _options;
    private IBatchRenderingOptionsValidator _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _options = new Mock<IBatchRenderingOptions>();
      _sut = new BatchRenderingOptionsValidator();
    }

    #endregion

    #region tests

    [Test]
    [ExpectedException(typeof(InvalidBatchRenderingOptionsException))]
    public void Validate_throws_exception_if_there_is_both_an_input_stream_and_paths()
    {
      // Arrange
      _options
        .SetupGet(x => x.InputPaths)
        .Returns(Enumerable.Range(0, 2).Select(x => new FileInfo(x.ToString())));
      _options
        .SetupGet(x => x.InputStream)
        .Returns(Mock.Of<Stream>());

      // Act & assert
      ExerciseSut(BatchRenderingFatalErrorType.InputCannotBeBothStreamAndPaths);
    }

    [Test]
    [ExpectedException(typeof(InvalidBatchRenderingOptionsException))]
    public void Validate_throws_exception_if_there_are_no_inputs_specified()
    {
      // Arrange
      _options
        .SetupGet(x => x.InputPaths)
        .Returns(Enumerable.Empty<FileSystemInfo>());
      _options
        .SetupGet(x => x.InputStream)
        .Returns((Stream) null);

      // Act & assert
      ExerciseSut(BatchRenderingFatalErrorType.NoInputsSpecified);
    }

    [Test]
    [ExpectedException(typeof(InvalidBatchRenderingOptionsException))]
    public void Validate_throws_exception_if_there_is_both_an_output_stream_and_paths()
    {
      // Arrange
      _options
        .SetupGet(x => x.InputPaths)
        .Returns(Enumerable.Empty<FileSystemInfo>());
      _options
        .SetupGet(x => x.InputStream)
        .Returns(Mock.Of<Stream>());
      _options
        .SetupGet(x => x.OutputPath)
        .Returns(new FileInfo("foo"));
      _options
        .SetupGet(x => x.OutputStream)
        .Returns(Mock.Of<Stream>());

      // Act & assert
      ExerciseSut(BatchRenderingFatalErrorType.OutputCannotBeBothStreamAndPaths);
    }

    [Test]
    [ExpectedException(typeof(InvalidBatchRenderingOptionsException))]
    public void Validate_throws_exception_if_there_are_no_outputs_specified()
    {
      // Arrange
      _options
        .SetupGet(x => x.InputPaths)
        .Returns(Enumerable.Empty<FileSystemInfo>());
      _options
        .SetupGet(x => x.InputStream)
        .Returns(Mock.Of<Stream>());
      _options
        .SetupGet(x => x.OutputPath)
        .Returns((FileInfo) null);
      _options
        .SetupGet(x => x.OutputStream)
        .Returns((Stream) null);

      // Act & assert
      ExerciseSut(BatchRenderingFatalErrorType.NoOutputsSpecified);
    }

    #endregion

    #region methods

    private void ExerciseSut(BatchRenderingFatalErrorType? expectedErrorType)
    {
      try
      {
        _sut.Validate(_options.Object);
      }
      catch(InvalidBatchRenderingOptionsException ex)
      {
        Assert.AreEqual(expectedErrorType, ex.FatalError, "Correct error type");
        throw;
      }
    }

    #endregion
  }
}

