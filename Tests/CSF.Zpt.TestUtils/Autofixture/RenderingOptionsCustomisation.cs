using System;
using Ploeh.AutoFixture;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.TestUtils.Autofixture
{
  public class RenderingOptionsCustomisation: ICustomization
  {
    public void Customize(IFixture fixture)
    {
      fixture.Customize<IRenderingOptions>(x => x.FromFactory(() => new RenderingOptions()));
    }
  }
}

