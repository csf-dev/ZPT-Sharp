
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;

namespace Test.CraigFowler.Web.ZPT.Tales
{
  [TestFixture]
  public class TestTalesPath
  {
    [Test]
    public void TestConstructor()
    {
      TalesPath path = new TalesPath("foo/bar/baz");
      Assert.IsNotNull(path);
    }
    
    [Test]
    public void TestParts()
    {
      TalesPath path = new TalesPath("foo/bar/baz");
      Assert.IsNotNull(path);
      
      Assert.AreEqual(3, path.Parts.Count, "Number of parts");
      Assert.AreEqual("foo", path.Parts[0], "First part");
      
      path = new TalesPath("  foo/bar baz/sample");
      Assert.IsNotNull(path);
      
      Assert.AreEqual(3, path.Parts.Count, "Number of parts with space in a path part");
    }
    
    [Test]
    public void TestPartsWithWhitespace()
    {
      string part1, part2, part3;
      TalesPath path = new TalesPath("    foo/ bar baz/sample  part ");
      
      Assert.IsNotNull(path);
      Assert.AreEqual(3, path.Parts.Count, "Number of parts");
      
      part1 = path.Parts[0];
      part2 = path.Parts[1];
      part3 = path.Parts[2];
      
      Assert.AreEqual("foo", part1, "First part");
      Assert.AreEqual(" bar baz", part2, "Second part");
      Assert.AreEqual("sample  part", part3, "Third part");
    }
  }
}
