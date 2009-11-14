
using System;
using NUnit.Framework;
using CraigFowler;

namespace Test.Commandline
{
  [TestFixture]
  public class TestUnixParameters
  {
    [Test]
    public void TestLongParamsNoValues()
    {
      CommandlineParameterProcessor processor;
      string[] commandline = {"--one", "--Number-Two", "foobarbaz", "--four"};
      
      processor = new CommandlineParameterProcessor(ParameterStyle.Unix, commandline);
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
      CommandlineParameterProcessor processor;
      ParameterDefinition def;
      string[] commandline = {"--one", "--Number-Two", "foobarbaz", "--four"};
      
      processor = new CommandlineParameterProcessor(ParameterStyle.Unix, commandline);
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
      CommandlineParameterProcessor processor;
      string[] commandline = {"-o", "-t", "foobarbaz", "-f"};
      
      processor = new CommandlineParameterProcessor(ParameterStyle.Unix, commandline);
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
      CommandlineParameterProcessor processor;
      ParameterDefinition def;
      string[] commandline = {"-o", "-t", "foobarbaz", "-f"};
      
      processor = new CommandlineParameterProcessor(ParameterStyle.Unix, commandline);
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
  }
}
