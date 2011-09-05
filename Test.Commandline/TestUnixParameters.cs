
using CraigFowler.Cli;
using NUnit.Framework;

namespace Test.CraigFowler.Cli
{
  [TestFixture]
  public class TestParameterProcessorUnix
  {
    #region tests
    
    [Test]
    public void TestLongParamsNoValues()
    {
      IParameterParser processor = this.SetUpParamaterParser(ParameterType.NoValue);
      string[] commandline = {"--one", "--Number-Two", "foobarbaz", "--four"};
      ParsedParameters output;
      
      Assert.AreEqual(3, processor.ParameterCount, "Number of params");
      
      output = processor.Parse(commandline);
      
      Assert.IsTrue(output.HasParameter("one"), "Parameter 'one' is present");
      
      Assert.IsTrue(output.HasParameter("two"), "Parameter 'two' is present");
      
      Assert.IsTrue(output.HasParameter("three"), "Parameter 'three' is present");
      
      Assert.AreEqual(1, output.GetRemainingArguments().Count, "One remaining text item");
      Assert.AreEqual("foobarbaz", output.GetRemainingArguments()[0], "Remaining text is correct");
    }
    
    [Test]
    public void TestLongParamsWithValue()
    {
      IParameterParser processor = this.SetUpParamaterParser(ParameterType.ValueRequired);
      string[] commandline = {"--one", "--Number-Two", "foobarbaz", "--four"};
      ParsedParameters output;
      
      Assert.AreEqual(3, processor.ParameterCount, "Number of params");
      
      output = processor.Parse(commandline);
      
      Assert.IsTrue(output.HasParameter("one"), "Parameter 'one' is present");
      
      Assert.IsTrue(output.HasParameter("two"), "Parameter 'two' is present");
      Assert.AreEqual("foobarbaz", output.GetValue<string>("two"), "Parameter 'two' has correct value");
      
      Assert.IsTrue(output.HasParameter("three"), "Parameter 'three' is present");
      
      Assert.AreEqual(0, output.GetRemainingArguments().Count, "No remaining text items");
    }
    
    [Test]
    public void TestShortParamsNoValues()
    {
      IParameterParser processor = this.SetUpParamaterParser(ParameterType.NoValue);
      string[] commandline = {"-o", "-t", "foobarbaz", "-f"};
      ParsedParameters output;
      
      Assert.AreEqual(3, processor.ParameterCount, "Number of params");
      
      output = processor.Parse(commandline);
      
      Assert.IsTrue(output.HasParameter("one"), "Parameter 'one' is present");
      
      Assert.IsTrue(output.HasParameter("two"), "Parameter 'two' is present");
      
      Assert.IsTrue(output.HasParameter("three"), "Parameter 'three' is present");
      
      Assert.AreEqual(1, output.GetRemainingArguments().Count, "One remaining text item");
      Assert.AreEqual("foobarbaz", output.GetRemainingArguments()[0], "Remaining text is correct");
    }
    
    [Test]
    public void TestShortParamsWithValue()
    {
      IParameterParser processor = this.SetUpParamaterParser(ParameterType.ValueRequired);
      string[] commandline = {"-o", "-t", "foobarbaz", "-f"};
      ParsedParameters output;
      
      Assert.AreEqual(3, processor.ParameterCount, "Number of params");
      
      output = processor.Parse(commandline);
      
      Assert.IsTrue(output.HasParameter("one"), "Parameter 'one' is present");
      
      Assert.IsTrue(output.HasParameter("two"), "Parameter 'two' is present");
      Assert.AreEqual("foobarbaz", output.GetValue<string>("two"), "Parameter 'two' has correct value");
      
      Assert.IsTrue(output.HasParameter("three"), "Parameter 'three' is present");
      
      Assert.AreEqual(0, output.GetRemainingArguments().Count, "No remaining text items");
    }
    
    [Test]
    public void TestShortParamsAndRemainingText()
    {
      IParameterParser processor = this.SetUpParamaterParser(ParameterType.ValueRequired);
      string[] commandline = {"some action", "-o", "-t", "foobarbaz", "-f"};
      ParsedParameters output;
      
      Assert.AreEqual(3, processor.ParameterCount, "Number of params");
      
      output = processor.Parse(commandline);
      
      Assert.IsTrue(output.HasParameter("one"), "Parameter 'one' is present");
      
      Assert.IsTrue(output.HasParameter("two"), "Parameter 'two' is present");
      Assert.AreEqual("foobarbaz", output.GetValue<string>("two"), "Parameter 'two' has correct value");
      
      Assert.IsTrue(output.HasParameter("three"), "Parameter 'three' is present");
      
      Assert.AreEqual(1, output.GetRemainingArguments().Count, "Correct amount of remaining text");
      Assert.AreEqual("some action", output.GetRemainingArguments()[0], "Remaining text is correct");
    }
    
    [Test]
    public void TestHelpParameter()
    {
      IParameterParser processor;
      string[] commandline = {"--help"};
      ParsedParameters output;
      
      processor = new UnixParameters();
      processor.RegisterParameter<string>("run-scheduled-task",
                                          ParameterType.ValueRequired,
                                          new string[] { "run-scheduled-task" },
                                          new string[] { "s" });
      processor.RegisterParameter<string>("verbose",
                                          ParameterType.NoValue,
                                          new string[] { "verbose" },
                                          new string[] { "v" });
      processor.RegisterParameter<string>("quiet",
                                          ParameterType.NoValue,
                                          new string[] { "quiet" },
                                          new string[] { "q" });
      processor.RegisterParameter<string>("help",
                                          ParameterType.NoValue,
                                          new string[] { "help" },
                                          new string[] { "h" });
      
      output = processor.Parse(commandline);
      
      Assert.IsTrue(output.HasParameter("help"), "Help key is present");
    }
    
    #endregion
    
    #region helper methods
    
    /// <summary>
    /// <para>Creates and configures a <see cref="IParameterParser"/> instance for testing.</para>
    /// </summary>
    /// <param name="paramaterTwoType">
    /// A <see cref="ParameterType"/>
    /// </param>
    /// <returns>
    /// A <see cref="IParameterParser"/>
    /// </returns>
    private IParameterParser SetUpParamaterParser(ParameterType paramaterTwoType)
    {
      IParameterParser output;
      
      output = new UnixParameters();
      output.RegisterParameter<string>("one",
                                       ParameterType.NoValue,
                                       new string[] { "one" },
                                       new string[] { "o" });
      output.RegisterParameter<string>("two",
                                       paramaterTwoType,
                                       new string[] { "Number-Two" },
                                       new string[] { "t" });
      output.RegisterParameter<string>("three",
                                       ParameterType.NoValue,
                                       new string[] { "four" },
                                       new string[] { "f" });
      
      return output;
  }
    
    #endregion
}
