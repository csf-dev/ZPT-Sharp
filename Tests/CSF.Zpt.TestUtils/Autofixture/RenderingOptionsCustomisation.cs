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
      fixture.Customize<IRenderingOptions>(x => x.FromFactory(() => {
        return new RenderingOptions(documentFactory: Mock.Of<ITemplateFileFactory>());
      }));
    }
  }
}

