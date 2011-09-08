//  
//  TestUnixParametersWithAttributes.cs
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

using System;
using NUnit.Framework;
using CraigFowler.Cli;
using CraigFowler.Cli.Mocks;

namespace Test.CraigFowler.Cli
{
  [TestFixture]
  public class TestUnixParametersWithAttributes
  {
    #region tests
    
    [Test]
    [ExpectedException(ExceptionType = typeof(ArgumentException),
                       ExpectedMessage = "Parameter is not valid for use.\nParameter name: parameter")]
    public void TestErroneousEnumeration()
    {
      IParameterParser processor = new UnixParameters();
      processor.RegisterParameters(typeof(ErroneousParametersEnumeration));
    }
    
    [Test]
    public void TestParametersEnumeration()
    {
      IParameterParser processor = new UnixParameters();
      processor.RegisterParameters(typeof(ParametersEnumeration));
      ParsedParameters output;

      Assert.AreEqual(5, processor.ParameterCount, "Correct count of parameters");
      Assert.IsInstanceOfType(typeof(Parameter<int>),
                              processor[ParametersEnumeration.Count],
                              "Count parameter is correct generic type.");

      output = processor.Parse(new string[] { "-A",
                                              "Do something interesting",
                                              "/home/craig/foo/bar",
                                              "--help",
                                              "--explode",
                                              "--count",
                                              "78" });
      
      Assert.IsTrue(output.HasParameter(ParametersEnumeration.Action), "Action parameter is present.");
      Assert.AreEqual("Do something interesting",
                      output.GetValue<string>(ParametersEnumeration.Action),
                      "Action parameter has correct value.");
      
      Assert.IsTrue(output.HasParameter(ParametersEnumeration.Help), "Help parameter is present.");
      Assert.IsFalse(output.HasParameter(ParametersEnumeration.Verbose), "Verbose parameter is not present.");
      
      Assert.AreEqual(1, output.GetRemainingArguments().Count, "Correct count of remaining args");
      Assert.AreEqual("/home/craig/foo/bar", output.GetRemainingArguments()[0], "Correct remaining arg");
      
      Assert.IsTrue(output.HasParameter(ParametersEnumeration.Explode), "Explode parameter is present.");
      
      Assert.IsTrue(output.HasParameter(ParametersEnumeration.Count), "Count parameter is present.");
      Assert.AreEqual(78,
                      output.GetValue<int>(ParametersEnumeration.Count),
                      "Count parameter has correct value.");
    }
    
    #endregion
  }
}

