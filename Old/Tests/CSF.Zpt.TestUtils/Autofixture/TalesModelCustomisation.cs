using System;
using Ploeh.AutoFixture;
using CSF.Zpt.Tales;

namespace CSF.Zpt.TestUtils.Autofixture
{
  public class TalesModelCustomisation : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      fixture.Customize<TalesModel>(x => x.FromFactory((IEvaluatorSelector reg) => new TalesModel(reg, null)));
    }
  }
}

