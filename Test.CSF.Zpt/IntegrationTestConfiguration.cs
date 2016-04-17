using System;
using System.Configuration;
using System.IO;

namespace Test.CSF.Zpt
{
  public class IntegrationTestConfiguration : ConfigurationSection, IIntegrationTestConfiguration
  {
    #region constants

    internal const string
      DefaultSourceDocumentPath = "../../../Common/ZptIntegrationTests/SourceDocuments",
      DefaultExpectedOutputPath = "../../../Common/ZptIntegrationTests/ExpectedOutputs";

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

    #endregion

    #region methods

    public DirectoryInfo GetSourceDocumentPath()
    {
      var output = this.SourceDocumentPath?? DefaultSourceDocumentPath;
      return new DirectoryInfo(output);
    }

    public DirectoryInfo GetExpectedOutputPath()
    {
      var output = this.ExpectedOutputPath?? DefaultExpectedOutputPath;
      return new DirectoryInfo(output);
    }

    #endregion
  }
}

