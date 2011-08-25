//  
//  TestTalesExpression.cs
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
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Tales.Expressions;
using NUnit.Framework;

namespace Test.CraigFowler.Web.ZPT.Tales.Expressions
{
  [TestFixture]
  public class TestTalesExpression
  {
    [Test]
    public void TestExpressionFactoryInverseBoolean()
    {
      string expressionText = "not:foo/bar";
      TalesExpression expression = new TalesContext().CreateExpression(expressionText);
      
      Assert.IsInstanceOfType(typeof(InverseBooleanExpression), expression, "Expression is correct type");
      Assert.AreEqual(ExpressionType.InverseBoolean, expression.ExpressionType, "Expression reports the correct type");
    }
    
    [Test]
    public void TestExpressionFactoryPath()
    {
      string expressionText = "path:foo/bar";
      TalesExpression expression = new TalesContext().CreateExpression(expressionText);
      
      Assert.IsInstanceOfType(typeof(PathExpression), expression, "Expression is correct type");
      Assert.AreEqual(ExpressionType.Path, expression.ExpressionType, "Expression reports the correct type");
    }
    
    [Test]
    public void TestExpressionFactoryString()
    {
      string expressionText = "string:foo bar baz";
      TalesExpression expression = new TalesContext().CreateExpression(expressionText);
      
      Assert.IsInstanceOfType(typeof(StringExpression), expression, "Expression is correct type");
      Assert.AreEqual(ExpressionType.String, expression.ExpressionType, "Expression reports the correct type");
    }
    
    [Test]
    public void TestExpressionFactoryDefault()
    {
      string expressionText = "foo/bar";
      TalesExpression expression = new TalesContext().CreateExpression(expressionText);
      
      Assert.IsInstanceOfType(typeof(PathExpression), expression, "Expression is correct type");
      Assert.AreEqual(ExpressionType.Path, expression.ExpressionType, "Expression reports the correct type");
    }
    
    [Test]
    public void TextExpressionText()
    {
      string expressionText = "path:foo/bar";
      TalesExpression expression = new TalesContext().CreateExpression(expressionText);
      
      Assert.AreEqual("path:", expression.ExpressionPrefix, "Correct prefix");
      Assert.AreEqual("foo/bar", expression.ExpressionBody, "Correct body");
      Assert.AreEqual("path:foo/bar", expression.ExpressionText, "Correct full text");
    }
    
    [Test]
    public void TextExpressionTextNoPrefix()
    {
      string expressionText = "foo/bar";
      TalesExpression expression = new TalesContext().CreateExpression(expressionText);
      
      Assert.IsNull(expression.ExpressionPrefix, "Correct prefix");
      Assert.AreEqual("foo/bar", expression.ExpressionBody, "Correct body");
      Assert.AreEqual("foo/bar", expression.ExpressionText, "Correct full text");
    }
    
    [Test]
    public void TestInnerExpression()
    {
      string expressionText = "not:foo/bar";
      TalesExpression expression = new TalesContext().CreateExpression(expressionText);
      
      Assert.IsInstanceOfType(typeof(InverseBooleanExpression), expression, "Expression is correct type");
      Assert.AreEqual(ExpressionType.InverseBoolean, expression.ExpressionType, "Expression reports the correct type");
      
      Assert.IsNotNull(((InverseBooleanExpression) expression).InnerExpression,
                       "Inner expression is not null");
      Assert.IsInstanceOfType(typeof(PathExpression),
                              ((InverseBooleanExpression) expression).InnerExpression,
                              "Inner expression is correct type");
      Assert.AreEqual(ExpressionType.Path,
                      ((InverseBooleanExpression) expression).InnerExpression.ExpressionType,
                      "Inner expression reports the correct type");
      Assert.AreEqual("foo/bar",
                      ((InverseBooleanExpression) expression).InnerExpression.ExpressionBody,
                      "Inner expression has correct body");
    }
    
    [Test]
    public void TestEmptyExpression()
    {
      TalesContext context;
      TalesExpression expression;
      
      context = new TalesContext();
      expression = context.CreateExpression("");
      Assert.IsInstanceOfType(typeof(PathExpression), expression, "Expression is of correct type");
    }
    
    [Test]
    [ExpectedException(ExceptionType = typeof(ArgumentNullException))]
    public void TestNullExpression()
    {
      TalesContext context;
      TalesExpression expression;
      
      context = new TalesContext();
      expression = context.CreateExpression(null);
      Assert.Fail("If the test reaches this point then we failed");
      Assert.IsNull(expression, "Not a real test, but prevents a compiler warning");
    }
  }
}
