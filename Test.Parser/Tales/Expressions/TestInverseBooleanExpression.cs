
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Mocks;

namespace Test.CraigFowler.Web.ZPT.Tales.Expressions
{
  [TestFixture]
  public class TestInverseBooleanExpression
  {
    [Test]
    public void TestBooleanTrue()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("not: mock/BooleanValue");
      MockObject mock  = new MockObject(true);
      object testObj;
      bool testBool;
      
      context.AddDefinition("mock", mock);
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(bool), testObj, "Test object is correct type");
      testBool = (bool) testObj;
      
      Assert.IsFalse(testBool, "Value is correct");
    }
    
    [Test]
    public void TestBooleanFalse()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("not: mock/BooleanValue");
      MockObject mock  = new MockObject(true);
      object testObj;
      bool testBool;
      
      mock.BooleanValue = false;
      
      context.AddDefinition("mock", mock);
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(bool), testObj, "Test object is correct type");
      testBool = (bool) testObj;
      
      Assert.IsTrue(testBool, "Value is correct");
    }
    
    [Test]
    public void TestPositiveInteger()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("not: mock/IntegerValue");
      MockObject mock  = new MockObject(true);
      object testObj;
      bool testBool;
      
      mock.IntegerValue = 5;
      
      context.AddDefinition("mock", mock);
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(bool), testObj, "Test object is correct type");
      testBool = (bool) testObj;
      
      Assert.IsFalse(testBool, "Value is correct");
    }
    
    [Test]
    public void TestZeroInteger()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("not: mock/IntegerValue");
      MockObject mock  = new MockObject(true);
      object testObj;
      bool testBool;
      
      mock.IntegerValue = 0;
      
      context.AddDefinition("mock", mock);
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(bool), testObj, "Test object is correct type");
      testBool = (bool) testObj;
      
      Assert.IsTrue(testBool, "Value is correct");
    }
    
    [Test]
    public void TestNegativeInteger()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("not: mock/IntegerValue");
      MockObject mock  = new MockObject(true);
      object testObj;
      bool testBool;
      
      mock.IntegerValue = -3;
      
      context.AddDefinition("mock", mock);
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(bool), testObj, "Test object is correct type");
      testBool = (bool) testObj;
      
      Assert.IsFalse(testBool, "Value is correct");
    }
    
    [Test]
    public void TestEmptyString()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("not: mock/someString");
      MockObject mock  = new MockObject(true);
      object testObj;
      bool testBool;
      
      mock["someString"] = String.Empty;
      
      context.AddDefinition("mock", mock);
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(bool), testObj, "Test object is correct type");
      testBool = (bool) testObj;
      
      Assert.IsTrue(testBool, "Value is correct");
    }
    
    [Test]
    public void TestString()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("not: mock/someString");
      MockObject mock  = new MockObject(true);
      object testObj;
      bool testBool;
      
      mock["someString"] = " ";
      
      context.AddDefinition("mock", mock);
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(bool), testObj, "Test object is correct type");
      testBool = (bool) testObj;
      
      Assert.IsFalse(testBool, "Value is correct");
    }
    
    [Test]
    public void TestNull()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("not: mock/someString");
      MockObject mock  = new MockObject(true);
      object testObj;
      bool testBool;
      
      context.AddDefinition("mock", mock);
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(bool), testObj, "Test object is correct type");
      testBool = (bool) testObj;
      
      Assert.IsTrue(testBool, "Value is correct");
    }
    
    [Test]
    public void TestNonConvertible()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("not: mock/inner");
      MockObject mock  = new MockObject(true);
      object testObj;
      bool testBool;
      
      context.AddDefinition("mock", mock);
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(bool), testObj, "Test object is correct type");
      testBool = (bool) testObj;
      
      Assert.IsTrue(testBool, "Value is correct");
    }
  }
}
