
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
  }
}
