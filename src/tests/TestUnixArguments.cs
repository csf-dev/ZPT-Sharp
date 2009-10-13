using System;
using NUnit.Framework;
using CraigFowler;

namespace Test.CraigFowler
{
  [TestFixture]
  public class TestUnixArguments
  {
    private UnixArguments args;
    
    [Test]
    public void TestConstructor()
    {
      args = new UnixArguments();
    }
    
    [Test]
    public void TestRegisterParams()
    {
      args.RegisterParameter("foo", true);
      args.RegisterParameter("spong", "s", true);
      args.RegisterParameter("1");
      args.RegisterParameter("2");
      args.RegisterParameter("3");
    }
    
    [Test]
    public void TestParseSimple()
    {
      string simple = "--foo^bar^-s^\"spong wibble spong\"^-123";
      string[] commandLine = simple.Split(new char[] {'^'});
      args.Parse(commandLine);
    }
    
    [Test]
    public void TestRetrieveSimple()
    {
      Assert.AreEqual(true, args.Contains("foo"), "Contains 'foo'");
      Assert.AreEqual("foo", args["foo"][0].AliasUsed, "Alias was 'foo'");
      Assert.AreEqual("bar", args["foo"][0].Value, "Value was 'bar'");
      Assert.AreEqual(true, args.Contains("spong"), "Contains 'spong'");
      Assert.AreEqual("s", args["spong"][0].AliasUsed, "Alias was 's'");
      Assert.AreEqual("spong wibble spong", args["spong"][0].Value, "Value was 'spong wibble spong'");
      Assert.AreEqual(true, args.Contains("1"), "Contains '1'");
      Assert.AreEqual(true, args.Contains("2"), "Contains '2'");
      Assert.AreEqual(true, args.Contains("3"), "Contains '3'");
    }
  }
}
