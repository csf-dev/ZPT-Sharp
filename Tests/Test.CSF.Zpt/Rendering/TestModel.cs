using System;
using NUnit.Framework;
using CSF.Zpt.Rendering;
using System.Linq;
using Ploeh.AutoFixture;
using CSF.Zpt.TestUtils.Autofixture;
using CSF.Zpt.TestUtils;
using System.Collections.Generic;
using Moq;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  public class TestModel
  {
    #region tests

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    public void Evaluate_gets_local_item_through_nested_models(int levelsOfNesting)
    {
      // Arrange
      var fixture = new Fixture();
      new RenderingContextCustomisation().Customize(fixture);

      var models = new IModel[levelsOfNesting + 1];
      for(int i = 0; i < models.Length; i++)
      {
        models[i] = (i == 0)? fixture.Create<DummyModel>() : models[i - 1].CreateChildModel();
      }

      var obj = fixture.Create<object>();
      var key = fixture.Create<string>();
      models[0].AddLocal(key, obj);

      // Act
      var result = models[levelsOfNesting].Evaluate(key, fixture.Create<RenderingContext>());

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreSame(obj, result.Value, "Result value");
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    public void Evaluate_gets_global_item_through_nested_models(int levelsOfNesting)
    {
      // Arrange
      var fixture = new Fixture();
      new RenderingContextCustomisation().Customize(fixture);

      var models = new IModel[levelsOfNesting + 1];
      for(int i = 0; i < models.Length; i++)
      {
        models[i] = (i == 0)? fixture.Create<DummyModel>() : models[i - 1].CreateChildModel();
      }

      var obj = fixture.Create<object>();
      var key = fixture.Create<string>();
      models[0].AddGlobal(key, obj);

      // Act
      var result = models[levelsOfNesting].Evaluate(key, fixture.Create<RenderingContext>());

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreSame(obj, result.Value, "Result value");
    }

    [TestCase(0, 0)]
    [TestCase(1, 1)]
    [TestCase(5, 2)]
    [TestCase(5, 5)]
    public void Evaluate_gets_overridden_local_item_through_nested_models(int levelsOfNesting, int overrideLevel)
    {
      // Arrange
      var fixture = new Fixture();
      new RenderingContextCustomisation().Customize(fixture);

      var models = new IModel[levelsOfNesting + 1];
      for(int i = 0; i < models.Length; i++)
      {
        models[i] = (i == 0)? fixture.Create<DummyModel>() : models[i - 1].CreateChildModel();
      }

      var obj = fixture.Create<object>();
      var obj2 = fixture.Create<object>();
      var key = fixture.Create<string>();
      models[0].AddLocal(key, obj);
      models[overrideLevel].AddLocal(key, obj2);

      // Act
      var result = models[levelsOfNesting].Evaluate(key, fixture.Create<RenderingContext>());

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreSame(obj2, result.Value, "Result value");
    }

    [Test]
    public void CopyTo_copies_all_local_definitions()
    {
      var definitions = new Dictionary<string,object> {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
      };
      var sut = new DummyModel(null);

      foreach(var key in definitions.Keys)
        sut.AddLocal(key, definitions[key]);

      var destination = Mock.Of<IModelValueStore>();

      // Act
      sut.CopyTo(destination);

      // Assert
      Mock.Get(destination).Verify(x => x.AddLocal("one", 1), Times.Once);
      Mock.Get(destination).Verify(x => x.AddLocal("two", 2), Times.Once);
      Mock.Get(destination).Verify(x => x.AddLocal("three", 3), Times.Once);
    }

    [Test]
    public void CopyTo_copies_model_object()
    {
      var modelObject = new object();
      var sut = new DummyModel(null, modelObject);

      var destination = new DummyModel();

      // Act
      sut.CopyTo(destination);

      // Assert
      Assert.That(destination.ModelObject, Is.SameAs(modelObject));
    }

    #endregion
  }
}

