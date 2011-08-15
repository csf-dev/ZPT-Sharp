//  
//  TestInverseBooleanExpression.cs
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
