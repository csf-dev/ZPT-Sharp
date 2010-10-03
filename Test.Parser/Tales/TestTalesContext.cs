
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
      TalesContext context = new TalesContext();
      Assert.IsNotNull(context, "Context not null");
    }
    
    [Test]
    public void TestCreateChildContext()
    {
      TalesContext parent, child;
      
      parent = new TalesContext();
      child = parent.CreateChildContext();
      
      Assert.IsNotNull(parent, "Context not null");
      Assert.IsNotNull(child, "Child is not null");
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
      
      context.AddDefinition("mock", mock);
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(Int32), testObj, "Test object is correct type");
      testInt = (int) testObj;
      Assert.AreEqual(2, testInt, "Test integer has correct value");
    }

		[Test]
		[Category("Integration")]
		[Description("This test is designed to test a specific case in relation to a recently-discovered bug")]
		public void TestSpecificExpressions()
		{
			MockObject mock = new MockObject(true);
			TalesContext
				firstLevelContext = new TalesContext(),
				secondLevelContext,
				thirdLevelContext;
			
			firstLevelContext.AddDefinition("mock", mock);
			mock["first"] = "First test";
			mock["second"] = "Second test";
			mock["third"] = "Third test";
			mock["fourth"] = "Fourth test";
			mock.BooleanValue = false;
			
			secondLevelContext = firstLevelContext.CreateChildContext();
			secondLevelContext.AddDefinition("obj", mock.InnerObject);
			
			thirdLevelContext = secondLevelContext.CreateChildContext();
			
			Assert.AreEqual("sample", thirdLevelContext.CreateExpression("obj/unambiguous/baz").GetValue(), "First assert");
			Assert.IsFalse(thirdLevelContext.CreateExpression("mock/BooleanValue").GetBooleanValue(), "Second assert");
			Assert.IsTrue(thirdLevelContext.CreateExpression("mock/inner/BooleanValue").GetBooleanValue(), "Third assert");
		}
	}
}
