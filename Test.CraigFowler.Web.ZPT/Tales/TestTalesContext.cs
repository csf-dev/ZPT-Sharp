//  
//  TestTalesContext.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2011 Craig Fowler
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.


using System;
using CraigFowler.Web.ZPT.Mocks;
using CraigFowler.Web.ZPT.Tales;
using NUnit.Framework;
using System.Collections.Generic;

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
    
    [Test]
    [Category("Integration")]
    [Description("A test to help me fix a specific bug where options definitions seem not to work")]
    public void TestOptionsPathExpression()
    {
      TalesContext
        firstLevelContext = new TalesContext(),
        secondLevelContext,
        thirdLevelContext;
      
      firstLevelContext.Options.Add("foo", "bar");
      secondLevelContext = firstLevelContext.CreateChildContext();
      thirdLevelContext = secondLevelContext.CreateChildContext();
      
      Assert.IsNotNull(thirdLevelContext.CreateExpression("options").GetValue(), "Options is not null");
      
      Assert.IsInstanceOfType(typeof(Dictionary<string,object>),
                              thirdLevelContext.CreateExpression("options").GetValue(),
                              "Options is correct type");
      
      Assert.IsTrue(((Dictionary<string,object>) thirdLevelContext.CreateExpression("options").GetValue()).ContainsKey("foo"),
                    "Options dictionary contains expected item");
      
      Assert.AreEqual("bar",
                      ((Dictionary<string,object>) thirdLevelContext.CreateExpression("options").GetValue())["foo"],
                      "Options dictionary 'foo' item is as expected");
      
      Assert.AreEqual("bar",
                      thirdLevelContext.CreateExpression("options/foo").GetValue(),
                      "Expression value is as expected.");
      
      thirdLevelContext.GetToplevelContext().Options.Add("spong", "wibble");
      
      Assert.AreEqual("wibble",
                      thirdLevelContext.CreateExpression("options/spong").GetValue(),
                      "Expression value is as expected after being spliced in.");
    }
	}
}
