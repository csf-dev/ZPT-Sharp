using System;
using Ploeh.AutoFixture;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt.Util.Autofixture
{
  public class RenderingOptionsCustomisation: ICustomization
  {
    public void Customize(IFixture fixture)
    {
      fixture.Customize<DefaultRenderingOptions>(x => x.FromFactory(() => new DefaultRenderingOptions()));
    }
  }
}

