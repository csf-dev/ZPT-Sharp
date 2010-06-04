
using System;
using NUnit.Framework;
using CraigFowler.Console;

namespace Test.CraigFowler.Console
{
  [TestFixture]
  public class TestParameterParserUnix
  {
    [Test]
    public void TestLongParamsNoValues()
    {
      ParameterParser processor;
      string[] commandline = {"--one", "--Number-Two", "foobarbaz", "--four"};
      
      processor = new ParameterParser(ParameterStyle.Unix, commandline);
      processor.Definitions.Add(new ParameterDefinition("one", new string[1] {"one"}));
      processor.Definitions.Add(new ParameterDefinition("two", new string[1] {"Number-Two"}));
      processor.Definitions.Add(new ParameterDefinition("three", new string[1] {"four"}));
      
      Assert.AreEqual(3, processor.Parameters.Count, "Number of params");
      Assert.IsNull(processor.Parameters["one"], "Parameter 'one' is present and null");
      Assert.IsNull(processor.Parameters["two"], "Parameter 'two' is present and null");
      Assert.IsNull(processor.Parameters["three"], "Parameter 'three' is present and null");
      Assert.AreEqual(1, processor.RemainingText.Length, "One remaining text item");
      Assert.AreEqual("foobarbaz", processor.RemainingText[0], "Remaining text is correct");
    }
    
    [Test]
    public void TestLongParamsWithValue()
    {
      ParameterParser processor;
      ParameterDefinition def;
      string[] commandline = {"--one", "--Number-Two", "foobarbaz", "--four"};
      
      processor = new ParameterParser(ParameterStyle.Unix, commandline);
      processor.Definitions.Add(new ParameterDefinition("one", new string[1] {"one"}));
      def = new ParameterDefinition("two", new string[1] {"Number-Two"});
      def.Type = ParameterType.ValueRequired;
      processor.Definitions.Add(def);
      processor.Definitions.Add(new ParameterDefinition("three", new string[1] {"four"}));
      
      Assert.AreEqual(3, processor.Parameters.Count, "Number of params");
      Assert.IsNull(processor.Parameters["one"], "Parameter 'one' is present and null");
      Assert.AreEqual("foobarbaz", processor.Parameters["two"], "Parameter 'two' is present and has correct value");
      Assert.IsNull(processor.Parameters["three"], "Parameter 'three' is present and null");
    }
    
    [Test]
    public void TestShortParamsNoValues()
    {
      ParameterParser processor;
      string[] commandline = {"-o", "-t", "foobarbaz", "-f"};
      
      processor = new ParameterParser(ParameterStyle.Unix, commandline);
      processor.Definitions.Add(new ParameterDefinition("one", null, new string[] {"o"}));
      processor.Definitions.Add(new ParameterDefinition("two", null, new string[] {"t"}));
      processor.Definitions.Add(new ParameterDefinition("three", null, new string[] {"f"}));
      
      Assert.AreEqual(3, processor.Parameters.Count, "Number of params");
      Assert.IsNull(processor.Parameters["one"], "Parameter 'one' is present and null");
      Assert.IsNull(processor.Parameters["two"], "Parameter 'two' is present and null");
      Assert.IsNull(processor.Parameters["three"], "Parameter 'three' is present and null");
      Assert.AreEqual(1, processor.RemainingText.Length, "One remaining text item");
      Assert.AreEqual("foobarbaz", processor.RemainingText[0], "Remaining text is correct");
    }
    
    [Test]
    public void TestShortParamsWithValue()
    {
      ParameterParser processor;
      ParameterDefinition def;
      string[] commandline = {"-o", "-t", "foobarbaz", "-f"};
      
      processor = new ParameterParser(ParameterStyle.Unix, commandline);
      processor.Definitions.Add(new ParameterDefinition("one", null, new string[] {"o"}));
      def = new ParameterDefinition("two", null, new string[] {"t"});
      def.Type = ParameterType.ValueRequired;
      processor.Definitions.Add(def);
      processor.Definitions.Add(new ParameterDefinition("three", null, new string[] {"f"}));
      
      Assert.AreEqual(3, processor.Parameters.Count, "Number of params");
      Assert.IsNull(processor.Parameters["one"], "Parameter 'one' is present and null");
      Assert.AreEqual("foobarbaz", processor.Parameters["two"], "Parameter 'two' is present and has correct value");
      Assert.IsNull(processor.Parameters["three"], "Parameter 'three' is present and null");
    }
    
    [Test]
    public void TestShortParamsAndRemainingText()
    {
      ParameterParser processor;
      ParameterDefinition def;
      string[] commandline = {"some action", "-o", "-t", "foobarbaz", "-f"};
      
      processor = new ParameterParser(ParameterStyle.Unix, commandline);
      processor.Definitions.Add(new ParameterDefinition("one", null, new string[] {"o"}));
      def = new ParameterDefinition("two", null, new string[] {"t"});
      def.Type = ParameterType.ValueRequired;
      processor.Definitions.Add(def);
      processor.Definitions.Add(new ParameterDefinition("three", null, new string[] {"f"}));
      
      Assert.AreEqual(3, processor.Parameters.Count, "Number of params");
      Assert.IsNull(processor.Parameters["one"], "Parameter 'one' is present and null");
      Assert.AreEqual("foobarbaz", processor.Parameters["two"], "Parameter 'two' is present and has correct value");
      Assert.IsNull(processor.Parameters["three"], "Parameter 'three' is present and null");
      
      Assert.AreEqual(1, processor.RemainingText.Length, "Correct amount of remaining text");
      Assert.AreEqual("some action", processor.RemainingText[0], "The remaining text");
    }
    
    [Test]
    public void TestHelpParameter()
    {
      ParameterParser processor;
      ParameterDefinition param;
      string[] commandline = {"--help"};
      
      processor = new ParameterParser(ParameterStyle.Unix, commandline);
      
      param = new ParameterDefinition("run-scheduled-task",
                                      new string[] {"run-scheduled-task"},
                                      new string[] {"s"});
      param.Type = ParameterType.ValueRequired;
      processor.Definitions.Add(param);
      
      param = new ParameterDefinition("verbose",
                                      new string[] {"verbose"},
                                      new string[] {"v"});
      param.Type = ParameterType.FlagOnly;
      processor.Definitions.Add(param);
      
      param = new ParameterDefinition("quiet",
                                      new string[] {"quiet"},
                                      new string[] {"q"});
      param.Type = ParameterType.FlagOnly;
      processor.Definitions.Add(param);
      
      param = new ParameterDefinition("help",
                                      new string[] {"help"},
                                      new string[] {"h"});
      param.Type = ParameterType.FlagOnly;
      processor.Definitions.Add(param);
      
      Assert.IsTrue(processor.Parameters.ContainsKey("help"), "Help key is present");
  }
}
