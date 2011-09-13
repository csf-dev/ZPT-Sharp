//  
//  TalesBenchmarks.cs
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
using CraigFowler.Web.ZPT.Mocks;
using CraigFowler.Web.ZPT.Tales;
using NUnit.Framework;

namespace Test.CraigFowler.Web.ZPT.Tales
{
  [TestFixture]
  [Category("Benchmark")]
  [Category("Information")]
  [Explicit]
  public class TalesBenchmarks
  {
    #region tests
    
    [Test]
    public void TestBenchmarkPathExpressions()
    {
      int startTicks, endTicks, iterations = 10000;
      double totalTime;
      
      startTicks = System.Environment.TickCount;
      
      for(int i = 0; i < iterations; i++)
      {
        ParsePathExpression();
      }
      
      endTicks = System.Environment.TickCount;
      
      totalTime = (endTicks - startTicks) / 1000d;
      
      Console.WriteLine ("Performed {2} parsings in {0} seconds.\n" +
                         "That would be {1} milliseconds per operation",
                         totalTime,
                         totalTime / 10d, iterations);
    }
    
    #endregion
    
    #region private methods
    
    private void ParsePathExpression()
    {
      TalesContext context;
      TalesExpression expression;
      MockObject mock = new MockObject(true);
      object testObj;
      int testInt;
      
      mock.InnerObject.IntegerValue = 2;
      
      context = new TalesContext();
      context.AddDefinition("mock", mock);
      
      expression = context.CreateExpression("mock/InnerObject/IntegerValue");
      
      testObj = expression.GetValue();
      
      Assert.IsInstanceOfType(typeof(Int32), testObj, "Test object is correct type");
      testInt = (int) testObj;
      Assert.AreEqual(2, testInt, "Test integer has correct value");
    }
    
    #endregion
  }
}
