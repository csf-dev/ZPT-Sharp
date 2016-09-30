using System;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Text.RegularExpressions;

namespace CSF.Zpt.DeploymentTasks
{
  public class FixDocTag : Task
  {
    #region constants

    private const string
      OPEN_FORMAT   = @"<div metal:define-macro=""{0}"" tal:omit-tag=""string:True"">",
      CLOSE_FORMAT  = @"</div>";

    #endregion

    #region properties

    [Required]
    public string InputFile { get; set; }

    [Required]
    public string MacroName { get; set; }

    public string FirstMatch { get; set; }

    public string SecondMatch { get; set; }

    #endregion

    #region methods

    public override bool Execute()
    {
      var input = GetInputFile();

      var output = PerformReplacements(input);

      SaveOutput(output);

      return true;
    }

    private string GetInputFile()
    {
      return File.ReadAllText(InputFile);
    }

    private string PerformReplacements(string fileContent)
    {
      var firstMatch = new Regex(FirstMatch);
      var secondMatch = new Regex(SecondMatch);

      var output = fileContent;

      output = firstMatch.Replace(output, match => String.Format(OPEN_FORMAT, MacroName), 1);
      output = secondMatch.Replace(output, match => String.Format(CLOSE_FORMAT, MacroName), 1);

      return output;
    }

    private void SaveOutput(string output)
    {
      File.WriteAllText(InputFile, output);
    }

    #endregion

    #region constructor

    public FixDocTag()
    {
      FirstMatch  = "<hr>";
      SecondMatch = "<hr>";
    }

    #endregion
  }
}

