using System;
using System.IO;

namespace Test.CSF.Zpt
{
  public class FallbackIntegrationTestConfiguration : IIntegrationTestConfiguration
  {
    #region methods

    public DirectoryInfo GetSourceDocumentPath()
    {
      return new DirectoryInfo(IntegrationTestConfiguration.DefaultSourceDocumentPath);
    }

    public DirectoryInfo GetExpectedOutputPath()
    {
      return new DirectoryInfo(IntegrationTestConfiguration.DefaultExpectedOutputPath);
    }

    #endregion
  }
}

