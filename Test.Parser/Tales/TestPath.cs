
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tales;

namespace Test.CraigFowler.Web.ZPT.Tales
{
  [TestFixture]
  public class TestPath
  {
    [Test]
    public void TestConstructor()
    {
      Path path = new Path("foo/bar/baz");
      Assert.IsNotNull(path);
    }
    
    [Test]
    public void TestParts()
    {
      Path path = new Path("foo/bar/baz");
      Assert.IsNotNull(path);
      
      Assert.AreEqual(3, path.Parts.Count, "Number of parts");
      Assert.AreEqual("foo", path.Parts.Peek(), "First part");
      
      path = new Path("  foo/bar baz/sample");
      Assert.IsNotNull(path);
      
      Assert.AreEqual(3, path.Parts.Count, "Number of parts with space in a path part");
    }
    
    [Test]
    public void TestPartsWithWhitespace()
    {
      string part1, part2, part3;
      Path path = new Path("    foo/ bar baz/sample  part ");
      
      Assert.IsNotNull(path);
      Assert.AreEqual(3, path.Parts.Count, "Number of parts");
      
      part1 = path.Parts.Dequeue();
      part2 = path.Parts.Dequeue();
      part3 = path.Parts.Dequeue();
      
      Assert.AreEqual("foo", part1, "First part");
      Assert.AreEqual(" bar baz", part2, "Second part");
      Assert.AreEqual("sample  part", part3, "Third part");
    }
  }
}
