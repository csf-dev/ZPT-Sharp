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

    private int _nameIterator;

    #endregion

    #region methods

    public void Customize(IFixture fixture)
    {
      new DummyModelCustomisation().Customize(fixture);
      fixture.Customize<RenderingContext>(x => x.FromFactory((DummyModel metal,
                                                              DummyModel tal) => {
        return new Mock<RenderingContext>(metal,
                                          tal,
                                          Mock.Of<ZptElement>(),
                                          RenderingOptions.Default) {
          CallBase = true,
          Name = String.Format("Context {0}", _nameIterator++)
        }.Object;
      }));
    }

    #endregion

    #region constructor

    public RenderingContextCustomisation ()
    {
      _nameIterator = 1;
    }

    #endregion
  }
}

