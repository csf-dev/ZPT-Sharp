
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tal;
using System.Collections.Generic;

namespace Test.CraigFowler.Web.ZPT.Tal
{
  [TestFixture]
  public class TestRepeatVariable
  {
    [Test]
    public void TestGenerateLetterReference()
    {
      RepeatVariable test = new RepeatVariable(new List<int>());
      
      Assert.AreEqual("a",  test.GenerateLetterReference(0),   "Letter reference for 0");
      Assert.AreEqual("g",  test.GenerateLetterReference(6),   "Letter reference for 6");
      Assert.AreEqual("aa", test.GenerateLetterReference(26),  "Letter reference for 26");
      Assert.AreEqual("ab", test.GenerateLetterReference(27),  "Letter reference for 27");
      Assert.AreEqual("tg", test.GenerateLetterReference(500), "Letter reference for 500");
    }
  }
}
