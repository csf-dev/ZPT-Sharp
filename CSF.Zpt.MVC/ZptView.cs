using System;
using System.Web.Mvc;
using System.IO;
using CSF.Zpt.Tales;

namespace CSF.Zpt.MVC
{
  public class ZptView : IView
  {
    #region fields

    private string _physicalPath;
    private static IZptDocumentFactory _documentFactory;

    #endregion

    #region methods

    public void Render(ViewContext viewContext, TextWriter writer)
    {
      var doc = _documentFactory.CreateDocument(new FileInfo(_physicalPath));
      doc.Render(writer, contextConfigurator: c => {
        var viewData = viewContext.ViewData;

        foreach(var key in viewData.Keys)
        {
          c.TalModel.AddGlobal(key, viewData[key]);
          c.MetalModel.AddGlobal(key, viewData[key]);
        }

        var viewsDirectoryPath = viewContext.HttpContext.Server.MapPath("~/Views/");
        var viewsDirectory = new TemplateDirectory(new DirectoryInfo(viewsDirectoryPath));

        c.TalModel.AddGlobal("views", viewsDirectory);
        c.MetalModel.AddGlobal("views", viewsDirectory);
      });
    }

    #endregion

    #region constructor

    public ZptView(string physicalPath)
    {
      _physicalPath = physicalPath;
    }

    static ZptView()
    {
      _documentFactory = new ZptDocumentFactory();
    }

    #endregion
  }
}

