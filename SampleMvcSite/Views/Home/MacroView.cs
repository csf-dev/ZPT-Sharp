using System;
using CraigFowler.Web.ZPT;

namespace CraigFowler.Samples.Mvc.Views.Home
{
  public class MacroView : ZptDocument
  {
    public override ITemplateDocument GetTemplateDocument ()
    {
      ITemplateDocument output = base.GetTemplateDocument ();
      
      output.TalesContext.AddDefinition("foo", "bar");
      
      return output;
    }
    
    public MacroView (ZptMetadata metadata) : base(metadata) {}
  }
}

