using System;
using Ploeh.AutoFixture;
using CSF.Zpt.Rendering;
using Moq;
using CSF.Zpt.Tales;

namespace CSF.Zpt.TestUtils.Autofixture
{
  public class RenderingOptionsCustomisation: ICustomization
  {
    public void Customize(IFixture fixture)
    {
      fixture.Customize<IRenderingSettings>(x => x.FromFactory(() => RenderingSettings.Default));
    }
  }
}
