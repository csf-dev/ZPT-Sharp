//  
//  TestBuiltinPathExpressions.cs
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


using CraigFowler.Web.ZPT.Tales;
using NUnit.Framework;

namespace Test.CraigFowler.Web.ZPT.Tales.Expressions
{
  [TestFixture]
  public class TestBuiltinPathExpressions
  {
    [Test]
    public void TestNothingExpression()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("nothing");
      
      Assert.IsNull(expression.GetValue(), "Expression of 'nothing' is null.");
    }
    
    [Test]
    public void TestDEFAULTExpression()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("default");
      object output = expression.GetValue();
      
      Assert.IsNotNull(output, "Output is not null.");
      Assert.IsInstanceOfType(typeof(DefaultValueMarker),
                              output,
                              "Expression of 'default' returns the correct value.");
    }
    
    [Test]
    public void TestNothingExpressionWithRootContext()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("CONTEXTS/nothing");
      
      Assert.IsNull(expression.GetValue(), "Expression of 'nothing' is null.");
    }
    
    [Test]
    public void TestDEFAULTExpressionWithRootContext()
    {
      TalesContext context = new TalesContext();
      TalesExpression expression = context.CreateExpression("CONTEXTS/default");
      object output = expression.GetValue();
      
      Assert.IsNotNull(output, "Output is not null.");
      Assert.IsInstanceOfType(typeof(DefaultValueMarker),
                              output,
                              "Expression of 'default' returns the correct value.");
    }
  }
}
