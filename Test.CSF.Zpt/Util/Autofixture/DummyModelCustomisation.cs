using System;
using Ploeh.AutoFixture;
using CSF.Zpt.Rendering;
using Moq;

namespace Test.CSF.Zpt.Util.Autofixture
{
  public class DummyModelCustomisation : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      fixture.Customize<DummyModel>(x => x.FromFactory(() => {
        return new Mock<DummyModel>() { CallBase = true }
          .Object;
      }));
    }
  }
}

