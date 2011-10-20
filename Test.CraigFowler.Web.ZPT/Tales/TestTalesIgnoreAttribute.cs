//  
//  TestTalesIgnoreAttribute.cs
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
using CraigFowler.Web.ZPT.Mocks;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Tales.Exceptions;
using NUnit.Framework;

namespace Test.CraigFowler.Web.ZPT.Tales
{
  [TestFixture]
  public class TestTalesIgnoreAttribute
  {
    #region tests
    
    [Test]
    public void TestWithoutIgnoreAttribute()
    {
      TalesIgnoreMock mock = new TalesIgnoreMock();
      TalesContext context = new TalesContext();
      int parsedValue;
      
      context.AddDefinition("mock", mock);
      parsedValue = (int) context.CreateExpression("mock/TestFieldOne").GetValue();
      
      Assert.AreEqual(1, parsedValue, "The value was retrieved from the public field instead of the indexer.");
    }
    
    [Test]
    public void TestWithIgnoreAttribute()
    {
      TalesIgnoreMock mock = new TalesIgnoreMock();
      TalesContext context = new TalesContext();
      int parsedValue;
      
      context.AddDefinition("mock", mock);
      parsedValue = (int) context.CreateExpression("mock/TestFieldTwo").GetValue();
      
      Assert.AreEqual(20, parsedValue, "The value was retrieved from the indexer instead of the public field.");
    }
    
    [Test]
    public void TestWithExplicitAlias()
    {
      TalesIgnoreMock mock = new TalesIgnoreMock();
      TalesContext context = new TalesContext();
      int parsedValue;
      
      context.AddDefinition("mock", mock);
      parsedValue = (int) context.CreateExpression("mock/TestFieldThree").GetValue();
      
      Assert.AreEqual(4, parsedValue, "The value was retrieved from the aliased public field instead of the named one.");
    }
    
    #endregion
  }
}

