
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tal;

namespace Test.CraigFowler.Web.ZPT.Tal
{
  [TestFixture]
  public class TestTalDocument
  {
    [Test]
    [Description("Contains no asserts but ensures that the constructor works.")]
    public void TestConstructor()
    {
      TalDocument document = new TalDocument();
      document.Render();
    }
  }
}
