using System;
using System.Configuration;
using System.IO;
using CSF;
using System.Collections.Generic;

namespace Test.CSF.Zpt.Integration
{
  public class IntegrationTestConfiguration : ConfigurationSection, IIntegrationTestConfiguration
  {
    #region constants

    private const string
      TEST_DATA_ROOT_PATH           = "../../Test data/",
      INTEGRATION_TESTS_PATH        = "ZptIntegrationTests",
      MODEL_TESTS_PATH              = "ZptModelIntegrationTests",
      SOURCE_ANNOTATION_TESTS_PATH  = "SourceAnnotationIntegrationTests",
      LOAD_PLUGIN_PATH              = "LoadPluginTests",
      SOURCE_DOCS_DIRECTORY         = "SourceDocuments",
      EXPECTED_OUTPUTS_DIRECTORY    = "ExpectedOutputs";

    private static readonly
      Dictionary<IntegrationTestType,string> DefaultSubdirectores = new Dictionary<IntegrationTestType,string> {
        { IntegrationTestType.Default,            INTEGRATION_TESTS_PATH },
        { IntegrationTestType.Model,              MODEL_TESTS_PATH },
        { IntegrationTestType.SourceAnnotation,   SOURCE_ANNOTATION_TESTS_PATH },
        { IntegrationTestType.LoadPlugin,         LOAD_PLUGIN_PATH },
      };

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

    [ConfigurationProperty(@"LoadPluginSourceDocumentPath", IsRequired = false)]
    public virtual string LoadPluginSourceDocumentPath
    {
      get {
        return (string) this["LoadPluginSourceDocumentPath"];
      }
      set {
        this["LoadPluginSourceDocumentPath"] = value;
      }
    }

    [ConfigurationProperty(@"LoadPluginExpectedOutputPath", IsRequired = false)]
    public virtual string LoadPluginExpectedOutputPath
    {
      get {
        return (string) this["LoadPluginExpectedOutputPath"];
      }
      set {
        this["LoadPluginExpectedOutputPath"] = value;
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
      type.RequireDefinedValue(nameof(type));

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
      else if(type == IntegrationTestType.LoadPlugin && useExpectedOutputPath)
      {
        output = this.LoadPluginExpectedOutputPath;
      }
      else if(type == IntegrationTestType.LoadPlugin)
      {
        output = this.LoadPluginSourceDocumentPath;
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
      type.RequireDefinedValue(nameof(type));

      var testTypeDirectory = DefaultSubdirectores[type];
      var expectedSourceDirectory = useExpectedOutputPath? EXPECTED_OUTPUTS_DIRECTORY : SOURCE_DOCS_DIRECTORY;

      return Path.Combine(TEST_DATA_ROOT_PATH, testTypeDirectory, expectedSourceDirectory);
    }

    private static DirectoryInfo GetPath(string defaultPath, string configuredPath = null)
    {
      var output = configuredPath?? defaultPath;
      return new DirectoryInfo(output);
    }

    #endregion
  }
}

