using System;
using System.Collections.Generic;
using CSF.Zpt.Rendering;
using Moq;
using NUnit.Framework;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  public class RenderingContextTests
  {
    [Test]
    public void CopyTo_copies_both_TAL_and_METAL_contexts()
    {
      // Arrange
      var talDefinitions = new Dictionary<string,object> {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
      };
      var metalDefinitions = new Dictionary<string,object> {
        { "metal-one", 1 },
        { "metal-two", 2 },
        { "metal-three", 3 },
      };
      var sut = GetSut(metalDefinitions, talDefinitions);
      var container = GetContainer();

      // Act
      sut.CopyTo(container);

      // Assert
      foreach(var def in talDefinitions)
      {
        Mock.Get(container.TalModel).Verify(x => x.AddLocal(def.Key, def.Value),
                                            Times.Once(),
                                            $"Definition {def.Key} has been added to the receiving TAL model");
      }

      foreach(var def in metalDefinitions)
      {
        Mock.Get(container.MetalModel).Verify(x => x.AddLocal(def.Key, def.Value),
                                              Times.Once(),
                                              $"Definition {def.Key} has been added to the receiving METAL model");
      }
    }

    IModel GetModel(IDictionary<string,object> defs)
    {
      var model = new Mock<IModel>();
      model
        .Setup(x => x.GetAllDefinitions())
        .Returns(defs);
      return model.Object;
    }

    IRenderingContext GetSut(IDictionary<string,object> metalDefs,
                             IDictionary<string,object> talDefs)
    {
      var talModel = GetModel(talDefs);
      var metalModel = GetModel(metalDefs);
      return new RenderingContext(metalModel, talModel, Mock.Of<IZptElement>(), Mock.Of<IRenderingSettings>());
    }

    IModelValueContainer GetContainer()
    {
      var talReceiver = Mock.Of<IModelValueStore>();
      var metalReceiver = Mock.Of<IModelValueStore>();
      return Mock.Of<IModelValueContainer>(x => x.TalModel == talReceiver && x.MetalModel == metalReceiver);
    }
  }
}
