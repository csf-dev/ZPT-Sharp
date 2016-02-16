using System;
using Ploeh.AutoFixture;
using Moq;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt.Util.Autofixture
{
  public class ZptElementCustomisation : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      fixture.Customize<ZptElement>(x => x.FromFactory(() => {
        return new Mock<ZptElement>() { CallBase = true }
          .Object;
      }));
    }
  }
}

