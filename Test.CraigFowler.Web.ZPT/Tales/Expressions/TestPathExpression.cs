//  
//  TestPathExpression.cs
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
  public class TestPathExpression
  {
    [Test]
    public void TestPathPieces()
    {
      TalesContext context;
      PathExpression expression;
      string path = "foo/bar/baz | spong/wibble | blah";
      
      context = new TalesContext();
      expression = context.CreateExpression(path) as PathExpression;
      
      Assert.AreEqual(3, expression.Paths.Count, "3 pieces to the path");
      Assert.AreEqual("foo/bar/baz", expression.Paths[0].Text, "Correct text for first path");
    }
    
    [Test]
    public void TestValidExpressions()
    {
      string[] testPaths = new string[] { "foo",
                                          "foo/bar/baz",
                                          "foo|bar",
                                          "foo/bar|baz",
                                          "Hello World/name/Craig | Goodbye",
                                          "",
                                          "|foo",
                                          "foo|",
                                          "foo/../bar",
																					"local:foo/bar",
																					"global:foo/bar" };
			
      for(int i = 0; i < testPaths.Length; i++)
      {
        Assert.IsTrue(PathExpression.IsValid(testPaths[i]),
                      String.Format("Path {0} is valid: '{1}'",
                                    i,
                                    testPaths[i]));
      }
    }
    
    [Test]
    public void TestInvalidExpressions()
    {
      string[] testPaths = new string[] { null };
      
      for(int i = 0; i < testPaths.Length; i++)
      {
        Assert.IsFalse(PathExpression.IsValid(testPaths[i]),
                       String.Format("Path {0} is invalid: '{1}'",
                                     i,
                                     testPaths[i]));
      }
    }
  }
}
