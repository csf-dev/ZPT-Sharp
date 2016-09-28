using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;
using CSF.Zpt;

namespace CSF.Zpt.IntegrationTests
{
  [TestFixture]
  public class ModelIntegrationTests : IntegrationTestBase
  {
    #region fields

    private ModelClass _model;

    #endregion

    #region tests

    [Test]
    public void RunIntegrationTests()
    {
      // Arrange
      var filePairsToTest = this.GetFilePairsToTest();
      var failedTests = new List<FileInfo>();

      // Act
      foreach(var pair in filePairsToTest)
      {
        if(!PerformTestRun(pair.Item1, pair.Item2))
        {
          failedTests.Add(pair.Item2);
        }
      }

      this.Logger.InfoFormat("{0} integration test cases processed", filePairsToTest.Count());

      // Assert
      Assert.That(!failedTests.Any(),
                  "Out of {0} integration tests, {1} failed. See the log file for more info.",
                  filePairsToTest.Count(),
                  failedTests.Count());
    }

    [TestCase("")]
    [Explicit("This test is covered by RunIntegrationTests - this method is for running them one at a time though.")]
    public void TestSingleIntegrationTest(string inputFileName)
    {
      // Arrange
      var allFilePairs = this.GetFilePairsToTest();
      var filePairToTest = allFilePairs.SingleOrDefault(x => x.Item2.Name == inputFileName);

      if(filePairToTest == null)
      {
        Assert.Fail("The input parameter must identify a valid expected output document by filename.");
      }

      // Act
      var result = this.PerformTestRun(filePairToTest.Item1, filePairToTest.Item2);

      // Assert
      Assert.IsTrue(result, "Test must result in a successful rendering");
    }

    #endregion

    #region methods

    protected override DirectoryInfo GetSourcePath(IIntegrationTestConfiguration config)
    {
      return config.GetSourceDocumentPath(IntegrationTestType.Model);
    }

    protected override DirectoryInfo GetExpectedPath(IIntegrationTestConfiguration config)
    {
      return config.GetExpectedOutputPath(IntegrationTestType.Model);
    }

    protected override void PerformExtraSetup()
    {
      _model = new ModelClass();

      _model.Sequence.Add(new SampleClass() { Name = "Craig", Description = "silly" });
      _model.Sequence.Add(new SampleClass() { Name = "The sky", Description = "blue" });
      _model.Sequence.Add(new SampleClass() { Name = "ZPT-Sharp model rendering", Description = "working" });

      base.PerformExtraSetup();
    }

    protected override string Render(IZptDocument document, IRenderingOptions options)
    {
      return document.Render(_model, options);
    }

    #endregion

    #region test class

    public class ModelClass
    {
      public IList<SampleClass> Sequence { get; private set; }

      public ModelClass()
      {
        this.Sequence = new List<SampleClass>();
      }
    }

    public class SampleClass
    {
      public string Name
      {
        get;
        set;
      }

      public string Description
      {
        get;
        set;
      }
    }

    #endregion
  }
}

