using System;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using HtmlAgilityPack;

namespace CSF.Zpt.DeploymentTasks
{
  public class SpliceHelloWorldCode : Task
  {
    #region constants

    private const string
      ModelElementId        = "model_code",
      ControllerElementId   = "controller_code",
      ViewElementId         = "view_code";

    #endregion

    #region properties

    [Required]
    public string ModelFile { get; set; }

    [Required]
    public string ControllerFile { get; set; }

    [Required]
    public string ViewFile { get; set; }

    [Required]
    public string OutputFile { get; set; }

    #endregion

    #region methods

    public override bool Execute()
    {
      var doc = LoadOutputFile(OutputFile);

      AddCode(ModelFile, ModelElementId, doc);
      AddCode(ControllerFile, ControllerElementId, doc);
      AddCode(ViewFile, ViewElementId, doc);

      SaveOutputFile(doc, OutputFile);

      return true;
    }

    private void AddCode(string sourceFile, string elementId, HtmlDocument doc)
    {
      var code = LoadInputFile(sourceFile);

      var node = doc.GetElementbyId(elementId);
      node.InnerHtml = HtmlEntity.Entitize(code, true, true);
    }

    private string LoadInputFile(string path)
    {
      return File.ReadAllText(path);
    }

    private HtmlDocument LoadOutputFile(string path)
    {
      var doc = new HtmlDocument();
      doc.Load(path);
      return doc;
    }

    private void SaveOutputFile(HtmlDocument doc, string path)
    {
      doc.Save(path);
    }



    #endregion
  }
}

