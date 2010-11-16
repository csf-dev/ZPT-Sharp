
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Mocks;

namespace Test.CraigFowler.Web.ZPT.Tales
{
  [TestFixture]
  [Category("Benchmark")]
  [Category("Information")]
  [Explicit]
  public class TalesBenchmarks
  {
    #region tests
    
    [Test]
    public void TestBenchmarkPathExpressions()
    {
      int startTicks, endTicks, iterations = 10000;
      double totalTime;
      
      startTicks = System.Environment.TickCount;
      
      for(int i = 0; i < iterations; i++)
      {
        ParsePathExpression();
      }
      
      endTicks = System.Environment.TickCount;
      
      totalTime = (endTicks - startTicks) / 1000d;
      
      Console.WriteLine ("Performed {2} parsings in {0} seconds.\n" +
                         "That would be {1} milliseconds per operation",
                         totalTime,
                         totalTime / 10d, iterations);
    }
    
    #endregion
    
    #region private methods
    
    private void ParsePathExpression()
    {
      TalesContext context;
      TalesExpression expression;
      MockObject mock = new MockObject(true);
      object testObj;
      int testInt;
      
      mock.InnerObject.IntegerValue = 2;
      
      context = new TalesContext();
      context.AddDefinition("mock", mock);
      
      expression = context.CreateExpression("mock/InnerObject/IntegerValue");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(Int32), testObj, "Test object is correct type");
      testInt = (int) testObj;
      Assert.AreEqual(2, testInt, "Test integer has correct value");
    }
    
    #endregion
  }
}
