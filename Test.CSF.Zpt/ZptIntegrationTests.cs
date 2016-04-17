using System;
using NUnit.Framework;
using CSF.Zpt;
using System.IO;
using CSF.Configuration;
using System.Linq;
using System.Collections.Generic;
using CSF.Zpt.Tales;

namespace Test.CSF.Zpt
{
  [TestFixture]
  [Category("Integration")]
  public class ZptIntegrationTests
  {
    #region fields

    private IZptDocumentFactory _documentFactory;
    private IIntegrationTestConfiguration _config;
    private DirectoryInfo _sourcePath, _expectedPath;
    private log4net.ILog _logger;

    #endregion

    #region setup & teardown

    [TestFixtureSetUp]
    public void FixtureSetup()
    {
      _logger = log4net.LogManager.GetLogger(this.GetType());

      IIntegrationTestConfiguration configuredConfig = ConfigurationHelper.GetSection<IntegrationTestConfiguration>();

      _config = configuredConfig?? new FallbackIntegrationTestConfiguration();

      _sourcePath = _config.GetSourceDocumentPath();
      _expectedPath = _config.GetExpectedOutputPath();
    }

    [SetUp]
    public void Setup()
    {
      _documentFactory = new ZptDocumentFactory();
    }

    #endregion

    #region tests

    [Test]
    public void RunIntegrationTests()
    {
      // Arrange
      var filePairsToTest = (from expectedFile in _expectedPath.GetFiles()
                             let filename = expectedFile.Name
                             join sourceFile in _sourcePath.GetFiles() on
                              filename equals sourceFile.Name
                             select new { Source = sourceFile, Expected = expectedFile });

      var failedTests = new List<FileInfo>();

      // Act
      foreach(var pair in filePairsToTest)
      {
        if(!PerformTestRun(pair.Source, pair.Expected))
        {
          failedTests.Add(pair.Expected);
        }
      }

      _logger.InfoFormat("{0} integration test cases processed", filePairsToTest.Count());

      // Assert
      Assert.That(!failedTests.Any(),
                  "Out of {0} integration tests, {1} failed. See the log file for more info.",
                  filePairsToTest.Count(),
                  failedTests.Count());
    }

    #endregion

    #region methods

    private bool PerformTestRun(FileInfo sourceDocument,
                                FileInfo expectedResultDocument)
    {
      bool output;

      ZptDocument document = _documentFactory.Create(sourceDocument);
      string expectedRendering, actualRendering = null;
      bool exceptionCaught = false;

      using(var stream = expectedResultDocument.OpenRead())
      using(var reader = new StreamReader(stream))
      {
        expectedRendering = reader.ReadToEnd();
      }

      try
      {
        var options = new global::CSF.Zpt.Rendering.RenderingOptions();
        options.InitialModelState.MetalLocalDefinitions.Add("documents", new FilesystemDirectory(_sourcePath));

        actualRendering = document.Render(options);
        output = (actualRendering == expectedRendering);
      }
      catch(Exception ex)
      {
        _logger.ErrorFormat("Exception caught whilst processing output file:{0}{1}{2}",
                            expectedResultDocument.Name,
                            Environment.NewLine,
                            ex);
        output = false;
        exceptionCaught = true;
      }

      if(!output && !exceptionCaught)
      {
        _logger.ErrorFormat("Unexpected rendering whilst processing expected output:{0}{1}{2}",
                            expectedResultDocument.Name,
                            Environment.NewLine,
                            actualRendering);
      }

      return output;
    }

    #endregion
  }
}

