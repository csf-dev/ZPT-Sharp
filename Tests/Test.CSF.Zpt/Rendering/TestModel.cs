using System;
using NUnit.Framework;
using CSF.Zpt.Rendering;
using System.Linq;
using Ploeh.AutoFixture;
using Test.CSF.Zpt.Util.Autofixture;
using Test.CSF.Zpt.Util;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  public class TestModel
  {
    #region tests

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    public void TestEvaluateLocal(int levelsOfNesting)
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
    public void TestEvaluateGlobal(int levelsOfNesting)
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
    public void TestEvaluateLocalOverridden(int levelsOfNesting, int overrideLevel)
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

    #endregion
  }
}

