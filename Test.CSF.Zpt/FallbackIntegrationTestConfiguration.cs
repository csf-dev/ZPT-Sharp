using System;
using System.IO;

namespace Test.CSF.Zpt
{
  public class FallbackIntegrationTestConfiguration : IIntegrationTestConfiguration
  {
    #region methods

    public DirectoryInfo GetSourceDocumentPath(IntegrationTestType type)
    {
      if(type == IntegrationTestType.Default)
      {
        return new DirectoryInfo(IntegrationTestConfiguration.DefaultSourceDocumentPath);
      }
      else if(type == IntegrationTestType.SourceAnnotation)
      {
        return new DirectoryInfo(IntegrationTestConfiguration.DefaultSourceAnnotationSourceDocumentPath);
      }
      else
      {
        throw new ArgumentException("Test type must be a supported value", nameof(type));
      }
    }

    public DirectoryInfo GetExpectedOutputPath(IntegrationTestType type)
    {
      if(type == IntegrationTestType.Default)
      {
        return new DirectoryInfo(IntegrationTestConfiguration.DefaultExpectedOutputPath);
      }
      else if(type == IntegrationTestType.SourceAnnotation)
      {
        return new DirectoryInfo(IntegrationTestConfiguration.DefaultSourceAnnotationExpectedOutputPath);
      }
      else
      {
        throw new ArgumentException("Test type must be a supported value", nameof(type));
      }
    }

    #endregion
  }
}

