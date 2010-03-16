
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;

namespace Test.CraigFowler.Web.ZPT.Tales
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
      expression = new PathExpression(path, context);
      
      Assert.AreEqual(3, expression.Paths.Count, "3 pieces to the path");
      Assert.AreEqual("foo/bar/baz", expression.Paths.Peek().Text, "Correct text for first path");
    }
  }
}
