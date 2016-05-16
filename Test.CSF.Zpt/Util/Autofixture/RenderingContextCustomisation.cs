using System;
using Ploeh.AutoFixture;
using CSF.Zpt;
using CSF.Zpt.Rendering;
using Moq;

namespace Test.CSF.Zpt.Util.Autofixture
{
  public class RenderingContextCustomisation : ICustomization
  {
    #region fields

    private static int _nameIterator;

    #endregion

    #region methods

    public void Customize(IFixture fixture)
    {
      new DummyModelCustomisation().Customize(fixture);
      new ZptElementCustomisation().Customize(fixture);
      new RenderingOptionsCustomisation().Customize(fixture);

      fixture.Customize<RenderingContext>(x => x.FromFactory((DummyModel metal,
                                                              DummyModel tal,
                                                              ZptElement element,
                                                              RenderingOptions opts) => {
        return new Mock<RenderingContext>(metal, tal, element, opts) {
          CallBase = true,
          Name = String.Format("Context {0}", _nameIterator++)
        }.Object;
      }));
    }

    #endregion

    #region constructor

    static RenderingContextCustomisation ()
    {
      _nameIterator = 1;
    }

    #endregion
  }
}

