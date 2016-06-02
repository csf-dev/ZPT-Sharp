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
        var output = new Mock<DummyModel>() { CallBase = true };
        output.As<IModel>();
        return output.Object;
      }));
    }
  }
}

