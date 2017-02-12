﻿using System;
using NUnit.Framework;
using System.IO;
using CSF.Zpt.Tales;
using System.Collections.Generic;
using CSF.Zpt;
using CSF.Zpt.Rendering;
using System.Linq;

namespace Test.CSF.Zpt.Integration
{
  [TestFixture]
  public class LoadPluginTests : IntegrationTestBase
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

    #endregion

    #region methods

    protected override DirectoryInfo GetSourcePath(IIntegrationTestConfiguration config)
    {
      return config.GetSourceDocumentPath(IntegrationTestType.LoadPlugin);
    }

    protected override DirectoryInfo GetExpectedPath(IIntegrationTestConfiguration config)
    {
      return config.GetExpectedOutputPath(IntegrationTestType.LoadPlugin);
    }

    protected override void PerformExtraSetup()
    {
      _model = new ModelClass(GetSourcePath(Config));

      base.PerformExtraSetup();
    }

    protected override string Render(IZptDocument document, IRenderingSettings options)
    {
      return document.Render(_model, options);
    }

    #endregion

    #region test class

    public class ModelClass
    {
      public TemplateDirectory Documents { get; private set; }

      public IEnumerable<SampleClass> Items { get; private set; }

      public ModelClass(DirectoryInfo sourceDir)
      {
        Documents = new TemplateDirectory(sourceDir);
        Items = new List<SampleClass>() {
          new SampleClass() { Name = "One",   DocumentName = "shared01" },
          new SampleClass() { Name = "Two",   DocumentName = "shared02" },
          new SampleClass() { Name = "Three", DocumentName = "shared03" },
        };
      }
    }

    public class SampleClass
    {
      public string Name { get; set; }
      public string DocumentName { get; set; }
    }

    #endregion
  }
}

