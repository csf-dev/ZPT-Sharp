
using System;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Test.CraigFowler.Web.ZPT.Tales.Expressions
{
  [TestFixture]
  public class TestStringExpression
  {
    private const string
      LOCATE        = @"(?<=(?<!\$)(?:\$\$)*)\$(?:(?:\{(?'var'[\w/ ]+)\})|(?'var'[\w/]+)|(?'var'\$))",
      REPLACE       = @"\$(?:(?:\{([\w/ ]+)\})|([\w/]+))",
      ESCAPE        = @"\$\$";

    [Test]
    [Category("Information")]
    [Description("This is not even a real test of the codebase, rather a trial-run of some of the code I planned to " +
                 "use in the StringExpression class.  It is really just a PoC that I used once before writing the " +
                 "actual implementation.")]
    public void TestReplacementFromVariable()
    {
      string inputString = @"Hello ${name}, you have $$100!  The weather is $weather";
      string expectedOutput = @"Hello Craig, you have $100!  The weather is sunny";
      Dictionary<string, string> variables = new Dictionary<string, string>();
      string output = inputString;
      MatchCollection matches;
      
      Regex replacer = new Regex(REPLACE);
      Regex escaper = new Regex(ESCAPE);
      
      variables["name"] = "Craig";
      variables["weather"] = "sunny";
      
      matches = Regex.Matches(inputString, LOCATE);
      
      for(int i = matches.Count - 1; i >= 0; i--)
      {
        Match match = matches[i];
        string variable = match.Groups["var"].Value;
        
        if(variable == "$")
        {
          output = escaper.Replace(output, "$$", 1, match.Index);
        }
        else
        {
          output = replacer.Replace(output, variables[variable], 1, match.Index);
        }
      }
      
      Assert.AreEqual(expectedOutput, output, "Output is correct");
    }
  }
}
