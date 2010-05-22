
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;
using Test.CraigFowler.Web.ZPT.Mocks;

namespace Test.CraigFowler.Web.ZPT.Tales
{
  [TestFixture]
  public class TestTalesContext
  {
    [Test]
    public void TestConstructor()
    {
      TalesContext parent, child;
      
      parent = new TalesContext();
      child = new TalesContext(parent);
      
      Assert.IsNotNull(child, "Child is not null");
      Assert.AreEqual(parent, child.ParentContext, "Parent context is as expected");
      Assert.IsNull(parent.ParentContext, "Parent context is null");
    }
    
    [Test]
    public void TestParsePathExpression()
    {
      MockObject mock = new MockObject(true);
      TalesContext context = new TalesContext();
      TalesPath path = new TalesPath("mock/InnerObject/IntegerValue");
      object testObj;
      int testInt;
      
      mock.InnerObject.IntegerValue = 2;
      
      context.ContextAliases.Add("mock", mock);
      
      testObj = context.EvaluateReference(path);
      
      Assert.IsInstanceOfType(typeof(Int32), testObj, "Test object is correct type");
      testInt = (int) testObj;
      Assert.AreEqual(2, testInt, "Test integer has correct value");
    }
  }
}
