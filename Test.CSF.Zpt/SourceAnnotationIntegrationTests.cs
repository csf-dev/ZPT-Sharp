using System;
using NUnit.Framework;
using CSF.Zpt;
using CSF.Zpt.Tales;
using System.IO;
using CSF.Configuration;
using CSF.Zpt.Rendering;
using System.Collections.Generic;
using System.Linq;
using CSF.IO;

namespace Test.CSF.Zpt
{
  [TestFixture]
  [Category("Integration")]
  public class SourceAnnotationIntegrationTests : IntegrationTestBase
  {
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

    [TestCase("test_sa3.xml")]
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
      return config.GetSourceDocumentPath(IntegrationTestType.SourceAnnotation);
    }

    protected override DirectoryInfo GetExpectedPath(IIntegrationTestConfiguration config)
    {
      return config.GetExpectedOutputPath(IntegrationTestType.SourceAnnotation);
    }

    protected override IRenderingContextFactory CreateTestEnvironment(DirectoryInfo rootPath)
    {
      var output = (TalesRenderingContextFactory) base.CreateTestEnvironment(rootPath);

      // The location of the other ZPT documents
      output.MetalLocalDefinitions.Add("documents", new TemplateDirectory(this.SourcePath));
      var tests = new NamedObjectWrapper();
      tests["input"] = new TemplateDirectory(this.SourcePath, true);
      output.MetalLocalDefinitions.Add("tests", tests);

      // The 'content' keyword option
      var content = new NamedObjectWrapper();
      content["args"] = "yes";
      output.TalKeywordOptions.Add("content", content);

      // The 'batch' keyword option
      var batch = new EnumerableObjectWrapperWithNamedItems();
      batch["previous_sequence"] = false;
      batch["previous_sequence_start_item"] = "yes";
      batch["next_sequence"] = true;
      batch["next_sequence_start_item"] = "six";
      batch["next_sequence_end_item"] = "ten";
      var items = Enumerable
        .Range(1, 5)
        .Select(x => {
        var item = new NamedObjectWrapper();
        item["num"] = x.ToString();
        return item;
      })
        .ToArray();
      items[0].SetStringRepresentation("one");
      items[1].SetStringRepresentation("two");
      items[2].SetStringRepresentation("three");
      items[3].SetStringRepresentation("four");
      items[4].SetStringRepresentation("five");
      foreach(var item in items)
      {
        batch.Items.Add(item);
      }
      output.TalKeywordOptions.Add("batch", batch);

      // The 'laf' keyword option

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

    protected override IRenderingOptions GetRenderingOptions(IRenderingContextFactory contextFactory)
    {
      return new RenderingOptions(contextFactory: contextFactory,
                                  addSourceFileAnnotation: true);
    }

    #endregion
  }
}

