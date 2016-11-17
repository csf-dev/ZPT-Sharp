using System;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using CSF.IniDocuments;
using System.IO;
using System.Text.RegularExpressions;

namespace CSF.Zpt.DeploymentTasks
{
  public class ReadVersionNumber : Task
  {
    #region constants

    internal const string
      SEMANTIC_VERSION        = "semantic_version",
      BUILD_NUMBER            = "build_number",
      INFORMATIONAL_VERSION   = "informational_version";

    private const string
      MAJOR_VERSION_PATTERN = @"^(\d+)\.\d+\.\d+$",
      MAJOR_VERSION_FORMAT = "{0}.0.0";

    private static readonly Regex MajorVersionParser = new Regex(MAJOR_VERSION_PATTERN, RegexOptions.Compiled);

    #endregion

    #region properties

    [Required]
    public string VersionFile { get; set; }

    [Output]
    public string FullVersionNumber { get; set; }

    [Output]
    public string SemanticVersionNumber { get; set; }

    [Output]
    public string NextBreakingVersionNumber { get; set; }

    [Output]
    public string BuildNumber { get; set; }

    [Output]
    public string InformationalVersion { get; set; }

    #endregion

    #region methods

    public override bool Execute()
    {
      var doc = IniDocument.Read(new FileInfo(VersionFile));

      string semanticVersion, buildNumber, informationalVersion;
      var semanticSuccess = doc.TryGetValue(SEMANTIC_VERSION, out semanticVersion);
      var buildSuccess = doc.TryGetValue(BUILD_NUMBER, out buildNumber);
      var informationalSuccess = doc.TryGetValue(INFORMATIONAL_VERSION, out informationalVersion);

      if(semanticSuccess && buildSuccess)
      {
        this.SemanticVersionNumber = semanticVersion;

        var majorVersion = Int32.Parse(MajorVersionParser.Match(semanticVersion).Groups[1].Value);
        this.NextBreakingVersionNumber = String.Format(MAJOR_VERSION_FORMAT, majorVersion + 1);

        if(buildSuccess)
        {
          this.BuildNumber = buildNumber;
          this.FullVersionNumber = String.Concat(SemanticVersionNumber, ".", BuildNumber);
        }
      }

      if(informationalSuccess)
      {
        this.InformationalVersion = informationalVersion;
      }

      return semanticSuccess && buildSuccess && informationalSuccess;
    }

    #endregion
  }
}

