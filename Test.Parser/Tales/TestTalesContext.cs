
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;

namespace Test.CraigFowler.Web.ZPT.Tales
{
  [TestFixture]
  public class TestTalesContext
  {
    [Test]
    public void TestConstructor()
    {
      TalesContext parent, child;
      
      parent = new TalesContext();
      child = new TalesContext(parent);
      
      Assert.IsNotNull(child, "Child is not null");
      Assert.AreEqual(parent, child.ParentContext, "Parent context is as expected");
      Assert.IsNull(parent.ParentContext, "Parent context is null");
    }
  }
}
