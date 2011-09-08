//  
//  TestUnixParameters.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2011 Craig Fowler
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.


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
}
