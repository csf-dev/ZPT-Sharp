using System;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;
using CSF.Zpt.MVC.Tales;
using System.IO;

namespace CSF.Zpt.MVC.Rendering
{
  public class MvcRenderingContextFactory : TalesRenderingContextFactory
  {
    #region methods

    public override RenderingContext Create(ZptElement element, RenderingOptions options)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      NamedObjectWrapper
        metalKeywordOptions = new NamedObjectWrapper(MetalKeywordOptions),
        talKeywordOptions = new NamedObjectWrapper(TalKeywordOptions);

      IModel
        metalModel = new MvcTalesModel(this.EvaluatorRegistry, metalKeywordOptions),
        talModel = new MvcTalesModel(this.EvaluatorRegistry, talKeywordOptions);

      var viewsPath = System.Web.HttpContext.Current.Server.MapPath("~/Views/");
      var viewsDirectory = new DirectoryInfo(viewsPath);

      metalModel.AddGlobal("Views", new TemplateDirectory(viewsDirectory));

      PopulateMetalModel(metalModel);
      PopulateTalModel(talModel);

      return new RenderingContext(metalModel, talModel, element, options);
    }

    #endregion
  }
}

