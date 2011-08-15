//  
//  TestStringExpression.cs
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


using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;

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
