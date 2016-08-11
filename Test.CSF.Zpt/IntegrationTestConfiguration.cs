using System;
using System.Configuration;
using System.IO;
using CSF;

namespace Test.CSF.Zpt
{
  public class IntegrationTestConfiguration : ConfigurationSection, IIntegrationTestConfiguration
  {
    #region constants

    internal const string
      DefaultSourceDocumentPath = "../../../Common/ZptIntegrationTests/SourceDocuments",
      DefaultExpectedOutputPath = "../../../Common/ZptIntegrationTests/ExpectedOutputs",
      DefaultSourceAnnotationSourceDocumentPath = "../../../Common/SourceAnnotationIntegrationTests/SourceDocuments",
      DefaultSourceAnnotationExpectedOutputPath = "../../../Common/SourceAnnotationIntegrationTests/ExpectedOutputs";

    #endregion

    #region properties

    [ConfigurationProperty(@"SourceDocumentPath", IsRequired = false)]
    public virtual string SourceDocumentPath
    {
      get {
        return (string) this["SourceDocumentPath"];
      }
      set {
        this["SourceDocumentPath"] = value;
      }
    }

    [ConfigurationProperty(@"ExpectedOutputPath", IsRequired = false)]
    public virtual string ExpectedOutputPath
    {
      get {
        return (string) this["ExpectedOutputPath"];
      }
      set {
        this["ExpectedOutputPath"] = value;
      }
    }

    [ConfigurationProperty(@"SourceAnnotationSourceDocumentPath", IsRequired = false)]
    public virtual string SourceAnnotationSourceDocumentPath
    {
      get {
        return (string) this["SourceAnnotationSourceDocumentPath"];
      }
      set {
        this["SourceAnnotationSourceDocumentPath"] = value;
      }
    }

    [ConfigurationProperty(@"SourceAnnotationExpectedOutputPath", IsRequired = false)]
    public virtual string SourceAnnotationExpectedOutputPath
    {
      get {
        return (string) this["SourceAnnotationExpectedOutputPath"];
      }
      set {
        this["SourceAnnotationExpectedOutputPath"] = value;
      }
    }

    #endregion

    #region methods

    public DirectoryInfo GetSourceDocumentPath(IntegrationTestType type)
    {
      return GetPath(type, false);
    }

    public DirectoryInfo GetExpectedOutputPath(IntegrationTestType type)
    {
      return GetPath(type, true);
    }

    private DirectoryInfo GetPath(IntegrationTestType type, bool getExpectedPath)
    {
      if(!type.IsDefinedValue())
      {
        throw new ArgumentException("Test type must be a defined enumeration constant", nameof(type));
      }

      DirectoryInfo output;

      if(type == IntegrationTestType.Default && getExpectedPath)
      {
        output = GetPath(this.ExpectedOutputPath, DefaultExpectedOutputPath);
      }
      else if(type == IntegrationTestType.Default)
      {
        output = GetPath(this.SourceDocumentPath, DefaultSourceDocumentPath);
      }
      else if(type == IntegrationTestType.SourceAnnotation && getExpectedPath)
      {
        output = GetPath(this.SourceAnnotationExpectedOutputPath, DefaultSourceAnnotationExpectedOutputPath);
      }
      else if(type == IntegrationTestType.SourceAnnotation)
      {
        output = GetPath(this.SourceAnnotationSourceDocumentPath, DefaultSourceAnnotationSourceDocumentPath);
      }
      else
      {
        throw new InvalidOperationException("Theoretically impossible scenario, broken test config implementation");
      }

      return output;
    }

    private DirectoryInfo GetPath(string configuredPath, string defaultPath)
    {
      var output = configuredPath?? defaultPath;
      return new DirectoryInfo(output);
    }

    #endregion
  }
}

