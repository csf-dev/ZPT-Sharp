using System;
using System.IO;

namespace Test.CSF.Zpt
{
  public class FallbackIntegrationTestConfiguration : IIntegrationTestConfiguration
  {
    #region methods

    public DirectoryInfo GetSourceDocumentPath(IntegrationTestType type)
    {
      return IntegrationTestConfiguration.GetDefaultPath(type, false);
    }

    public DirectoryInfo GetExpectedOutputPath(IntegrationTestType type)
    {
      return IntegrationTestConfiguration.GetDefaultPath(type, true);
    }

    #endregion
  }
}

