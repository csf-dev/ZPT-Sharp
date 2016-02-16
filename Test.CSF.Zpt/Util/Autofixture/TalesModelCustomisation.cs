using System;
using Ploeh.AutoFixture;
using CSF.Zpt.Tales;

namespace Test.CSF.Zpt.Util.Autofixture
{
  public class TalesModelCustomisation : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      fixture.Customize<TalesModel>(x => x.FromFactory((IEvaluatorRegistry reg) => new TalesModel(reg, null)));
    }
  }
}

