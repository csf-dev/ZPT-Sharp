using System;
using System.IO;

namespace CSF.Zpt.IntegrationTests
{
  public interface IIntegrationTestConfiguration
  {
    DirectoryInfo GetSourceDocumentPath(IntegrationTestType type = IntegrationTestType.Default);

    DirectoryInfo GetExpectedOutputPath(IntegrationTestType type = IntegrationTestType.Default);
  }
}

