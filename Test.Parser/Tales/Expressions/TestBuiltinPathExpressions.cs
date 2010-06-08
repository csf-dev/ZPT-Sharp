
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;

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
