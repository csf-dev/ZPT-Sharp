using System;
using Ploeh.AutoFixture;
using CSF.Zpt;
using CSF.Zpt.Rendering;
using Moq;

namespace CSF.Zpt.TestUtils.Autofixture
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

      fixture.Customize<RenderingContext>(x => x.FromFactory(GetContextFactory<RenderingContext>()));
      fixture.Customize<IRenderingContext>(x => x.FromFactory(GetContextFactory<IRenderingContext>()));
    }

    private Func<DummyModel,DummyModel,ZptElement,IRenderingOptions,TContext> GetContextFactory<TContext>()
      where TContext : IRenderingContext
    {
      return CreateContext<TContext>;
    }

    private TContext CreateContext<TContext>(DummyModel metal,
                                             DummyModel tal,
                                             ZptElement element,
                                             IRenderingOptions opts)
      where TContext : IRenderingContext
    {
      return (TContext) new Mock<RenderingContext>(metal, tal, element, opts, (string) null) {
        CallBase = true,
        Name = String.Format("Context {0}", _nameIterator++)
      }
        .As<IRenderingContext>()
        .Object;
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

