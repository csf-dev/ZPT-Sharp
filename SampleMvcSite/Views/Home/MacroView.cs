using System;
using CraigFowler.Web.ZPT;
using CraigFowler.Web.ZPT.Tales;

namespace CraigFowler.Samples.Mvc.Views.Home
{
  public class MacroView : ZptDocument
  {
    private static int AttemptCount = 0;
    
    protected override void AssignModelData (TalesContext talesContext)
    {
      talesContext.AddDefinition("foo", AttemptCount++);
    }
    
    public MacroView (ZptMetadata metadata) : base(metadata) {}
  }
}

