using System;
using NUnit.Framework;
using CSF.Zpt.Rendering;
using System.Linq;
using Ploeh.AutoFixture;

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

      var models = new Model[levelsOfNesting + 1];
      for(int i = 0; i < models.Length; i++)
      {
        models[i] = (i == 0)? new DummyModel(null, null) : models[i - 1].CreateChildModel();
      }

      var obj = fixture.Create<object>();
      var key = fixture.Create<string>();
      models[0].AddLocal(key, obj);

      // Act
      var result = models[levelsOfNesting].Evaluate(key);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.IsTrue(result.EvaluationSuccess, "Evaluation success");
      Assert.AreSame(obj, result.GetResult(), "Result value");
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    public void TestEvaluateGlobal(int levelsOfNesting)
    {
      // Arrange
      var fixture = new Fixture();

      var models = new Model[levelsOfNesting + 1];
      for(int i = 0; i < models.Length; i++)
      {
        models[i] = (i == 0)? new DummyModel(null, null) : models[i - 1].CreateChildModel();
      }

      var obj = fixture.Create<object>();
      var key = fixture.Create<string>();
      models[0].AddGlobal(key, obj);

      // Act
      var result = models[levelsOfNesting].Evaluate(key);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.IsTrue(result.EvaluationSuccess, "Evaluation success");
      Assert.AreSame(obj, result.GetResult(), "Result value");
    }

    [TestCase(0, 0)]
    [TestCase(1, 1)]
    [TestCase(5, 2)]
    [TestCase(5, 5)]
    public void TestEvaluateLocalOverridden(int levelsOfNesting, int overrideLevel)
    {
      // Arrange
      var fixture = new Fixture();

      var models = new Model[levelsOfNesting + 1];
      for(int i = 0; i < models.Length; i++)
      {
        models[i] = (i == 0)? new DummyModel(null, null) : models[i - 1].CreateChildModel();
      }

      var obj = fixture.Create<object>();
      var obj2 = fixture.Create<object>();
      var key = fixture.Create<string>();
      models[0].AddLocal(key, obj);
      models[overrideLevel].AddLocal(key, obj2);

      // Act
      var result = models[levelsOfNesting].Evaluate(key);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.IsTrue(result.EvaluationSuccess, "Evaluation success");
      Assert.AreSame(obj2, result.GetResult(), "Result value");
    }

    #endregion
  }
}

