using System;
using NUnit.Framework;
using CSF.Zpt;
using System.IO;
using CSF.Configuration;
using System.Linq;
using System.Collections.Generic;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;

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

      ZptDocument document = _documentFactory.CreateHtml(sourceDocument);
      string expectedRendering, actualRendering = null;
      bool exceptionCaught = false;

      using(var stream = expectedResultDocument.OpenRead())
      using(var reader = new StreamReader(stream))
      {
        expectedRendering = reader.ReadToEnd();
      }

      try
      {
        var options = new RenderingOptions(initialState: this.CreateTestEnvironment());

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

    private InitialModelState CreateTestEnvironment()
    {
      var output = new InitialModelState();

      // The location of the other ZPT documents
      output.MetalLocalDefinitions.Add("documents", new FilesystemDirectory(_sourcePath));

      // The 'content' keyword option
      var content = new NamedObjectWrapper();
      content["args"] = "yes";
      output.TalKeywordOptions.Add("content", content);

      // The 'batch' keyword option
      var batch = new NamedObjectWrapper();
      batch["previous_sequence"] = false;
      batch["previous_sequence_start_item"] = "yes";
      output.TalKeywordOptions.Add("batch", batch);

      // The 'laf' keyword option
      var laf = new TemplateFile(new ZptDocumentFactory().CreateHtml(_sourcePath.GetFiles("teeshop1.html").Single()));
      output.MetalKeywordOptions.Add("laf", laf);

      // The 'getProducts' option
      var getProducts = new [] {
        new NamedObjectWrapper(),
        new NamedObjectWrapper(),
      };
      getProducts[0]["description"] = "This is the tee for those who LOVE Zope. Show your heart on your tee.";
      getProducts[0]["image"] = "smlatee.jpg";
      getProducts[0]["price"] = 12.99m;
      getProducts[1]["description"] = "This is the tee for Jim Fulton. He's the Zope Pope!";
      getProducts[1]["image"] = "smpztee.jpg";
      getProducts[1]["price"] = 11.99m;
      output.TalKeywordOptions.Add("getProducts", getProducts);

      return output;
    }

    #endregion
  }
}

