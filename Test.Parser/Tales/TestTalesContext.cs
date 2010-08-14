
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Tales.Expressions;
using CraigFowler.Web.ZPT.Mocks;

namespace Test.CraigFowler.Web.ZPT.Tales
{
  [TestFixture]
  public class TestTalesContext
  {
    [Test]
    public void TestConstructor()
    {
      TalesContext context;
      
      context = new TalesContext();
      
      Assert.IsNotNull(context, "Context not null");
      Assert.IsNull(context.ParentContext, "Parent context is null");
    }
    
    [Test]
    public void TestCreateChildContext()
    {
      TalesContext parent, child;
      
      parent = new TalesContext();
      child = parent.CreateChildContext();
      
      Assert.IsNotNull(parent, "Context not null");
      Assert.IsNull(parent.ParentContext, "Parent context is null");
      
      Assert.IsNotNull(child, "Child is not null");
      Assert.AreEqual(parent, child.ParentContext, "Parent context is as expected");
    }
    
    [Test]
    public void TestCreateExpression()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression;
      
      expression = context.CreateExpression("foo");
      Assert.IsNotNull(expression, "Expression is not null");
      Assert.AreEqual(ExpressionType.Path, expression.ExpressionType, "Path expression type");
      
      expression = context.CreateExpression("not:foo");
      Assert.IsNotNull(expression, "Expression is not null");
      Assert.AreEqual(ExpressionType.InverseBoolean, expression.ExpressionType, "Inverse boolean expression type");
      
      expression = context.CreateExpression("string:foo bar baz");
      Assert.IsNotNull(expression, "Expression is not null");
      Assert.AreEqual(ExpressionType.String, expression.ExpressionType, "String expression type");
    }
    
    [Test]
    public void TestParsePathExpression()
    {
      MockObject mock = new MockObject(true);
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("mock/InnerObject/IntegerValue");
      object testObj;
      int testInt;
      
      mock.InnerObject.IntegerValue = 2;
      
      context.LocalDefinitions.Add("mock", mock);
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(Int32), testObj, "Test object is correct type");
      testInt = (int) testObj;
      Assert.AreEqual(2, testInt, "Test integer has correct value");
    }
  }
}
