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
      DefaultModelSourceDocumentPath = "../../../Common/ZptModelIntegrationTests/SourceDocuments",
      DefaultModelExpectedOutputPath = "../../../Common/ZptModelIntegrationTests/ExpectedOutputs",
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

    [ConfigurationProperty(@"ModelSourceDocumentPath", IsRequired = false, DefaultValue = "")]
    public virtual string ModelSourceDocumentPath
    {
      get {
        return (string) this["ModelSourceDocumentPath"];
      }
      set {
        this["ModelSourceDocumentPath"] = value;
      }
    }

    [ConfigurationProperty(@"ModelExpectedOutputPath", IsRequired = false, DefaultValue = "")]
    public virtual string ModelExpectedOutputPath
    {
      get {
        return (string) this["ModelExpectedOutputPath"];
      }
      set {
        this["ModelExpectedOutputPath"] = value;
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

    private DirectoryInfo GetPath(IntegrationTestType type, bool useExpectedOutputPath)
    {
      return GetPath(GetDefaultPathString(type, useExpectedOutputPath),
                     GetConfiguredPathString(type, useExpectedOutputPath));
    }

    private string GetConfiguredPathString(IntegrationTestType type, bool useExpectedOutputPath)
    {
      if(!type.IsDefinedValue())
      {
        throw new ArgumentException("Test type must be a defined enumeration constant", nameof(type));
      }

      string output;

      if(type == IntegrationTestType.Default && useExpectedOutputPath)
      {
        output = this.ExpectedOutputPath;
      }
      else if(type == IntegrationTestType.Default)
      {
        output = this.SourceDocumentPath;
      }
      else if(type == IntegrationTestType.SourceAnnotation && useExpectedOutputPath)
      {
        output = this.SourceAnnotationExpectedOutputPath;
      }
      else if(type == IntegrationTestType.SourceAnnotation)
      {
        output = this.SourceAnnotationSourceDocumentPath;
      }
      else if(type == IntegrationTestType.Model && useExpectedOutputPath)
      {
        output = this.ModelExpectedOutputPath;
      }
      else if(type == IntegrationTestType.Model)
      {
        output = this.ModelSourceDocumentPath;
      }
      else
      {
        throw new InvalidOperationException("Theoretically impossible scenario, broken test config implementation");
      }

      return output;
    }

    internal static DirectoryInfo GetDefaultPath(IntegrationTestType type, bool useExpectedOutputPath)
    {
      return GetPath(GetDefaultPathString(type, useExpectedOutputPath));
    }

    private static string GetDefaultPathString(IntegrationTestType type, bool useExpectedOutputPath)
    {
      if(!type.IsDefinedValue())
      {
        throw new ArgumentException("Test type must be a defined enumeration constant", nameof(type));
      }

      string output;

      if(type == IntegrationTestType.Default && useExpectedOutputPath)
      {
        output = DefaultExpectedOutputPath;
      }
      else if(type == IntegrationTestType.Default)
      {
        output = DefaultSourceDocumentPath;
      }
      else if(type == IntegrationTestType.SourceAnnotation && useExpectedOutputPath)
      {
        output = DefaultSourceAnnotationExpectedOutputPath;
      }
      else if(type == IntegrationTestType.SourceAnnotation)
      {
        output = DefaultSourceAnnotationSourceDocumentPath;
      }
      else if(type == IntegrationTestType.Model && useExpectedOutputPath)
      {
        output = DefaultModelExpectedOutputPath;
      }
      else if(type == IntegrationTestType.Model)
      {
        output = DefaultModelSourceDocumentPath;
      }
      else
      {
        throw new InvalidOperationException("Theoretically impossible scenario, broken test config implementation");
      }

      return output;
    }

    private static DirectoryInfo GetPath(string defaultPath, string configuredPath = null)
    {
      var output = configuredPath?? defaultPath;
      return new DirectoryInfo(output);
    }

    #endregion
  }
}

