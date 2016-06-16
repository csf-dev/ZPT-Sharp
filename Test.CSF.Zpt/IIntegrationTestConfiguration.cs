using System;
using System.IO;

namespace Test.CSF.Zpt
{
  public interface IIntegrationTestConfiguration
  {
    DirectoryInfo GetSourceDocumentPath(IntegrationTestType type = IntegrationTestType.Default);

    DirectoryInfo GetExpectedOutputPath(IntegrationTestType type = IntegrationTestType.Default);
  }
}

