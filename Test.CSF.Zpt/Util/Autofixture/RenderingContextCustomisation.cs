using System;
using Ploeh.AutoFixture;
using CSF.Zpt;
using CSF.Zpt.Rendering;
using Moq;

namespace Test.CSF.Zpt.Util.Autofixture
{
  public class RenderingContextCustomisation : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      new DummyModelCustomisation().Customize(fixture);
      fixture.Customize<RenderingContext>(x => x.FromFactory((DummyModel metal, DummyModel tal) => {
        return new Mock<RenderingContext>(metal, tal) { CallBase = true }.Object;
      }));
    }
  }
}

