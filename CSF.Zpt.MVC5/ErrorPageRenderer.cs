using System;
using System.IO;
using System.Reflection;

namespace CSF.Zpt.MVC
{
  public class ErrorPageRenderer
  {
    const string ErrorPageResourceName = "ZptErrorPage.pt";
    readonly IZptDocumentFactory factory;

    public void Render(TextWriter writer, Exception ex)
    {
      var template = GetTemplate();
      var model = GetModel(ex);

      template.Render(model, writer);
    }

    ErrorModel GetModel(Exception ex)
    {
      return new ErrorModel
      {
        Exception = ex,
        ZptVersion = Assembly.GetAssembly(typeof(ZptDocument)).GetName().Version.ToSemanticVersion(),
        ZptMvcVersion = Assembly.GetExecutingAssembly().GetName().Version.ToSemanticVersion(),
      };
    }

    IZptDocument GetTemplate()
    {
      var thisAssembly = Assembly.GetExecutingAssembly();
      using(var page = thisAssembly.GetManifestResourceStream(ErrorPageResourceName))
      {
        return factory.CreateDocument(page, RenderingMode.Html);
      }
    }

    internal class ErrorModel
    {
      public Exception Exception { get; set; }

      public string ZptVersion { get; set; }

      public string ZptMvcVersion { get; set; }
    }

    public ErrorPageRenderer()
    {
      factory = new ZptDocumentFactory();
    }
  }
}
