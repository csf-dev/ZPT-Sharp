
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Tales.Expressions;

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
                                          "foo/../bar" };
      
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
