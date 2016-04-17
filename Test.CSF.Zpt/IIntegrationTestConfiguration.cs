using System;
using System.IO;

namespace Test.CSF.Zpt
{
  public interface IIntegrationTestConfiguration
  {
    DirectoryInfo GetSourceDocumentPath();

    DirectoryInfo GetExpectedOutputPath();
  }
}

