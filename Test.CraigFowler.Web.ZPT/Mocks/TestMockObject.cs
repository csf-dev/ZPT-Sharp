//  
//  TestMockObject.cs
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
using System.Reflection;
using CraigFowler.Web.ZPT.Mocks;
using NUnit.Framework;

namespace Test.CraigFowler.Web.ZPT.Mocks
{
  [TestFixture]
  public class TestMockObject
  {
    [Test]
    [Category("Information")]
    public void TestGetMembers()
    {
      MockObject mock = new MockObject(true);
      
      foreach(MemberInfo member in mock.GetType().GetMembers())
      {
        Console.WriteLine ("Member '{0}' is a '{1}'", member.Name, member.MemberType.ToString());
      }
    }
    
    [Test]
    [Category("Information")]
    public void TestGetIndexer()
    {
      MockObject mock = new MockObject(true);
      PropertyInfo property = (PropertyInfo) mock.GetType().GetMember("Item")[0];
      
      Console.WriteLine ("Property type: {0}", property.PropertyType);
      Console.WriteLine ("Number of parameters: {0}", property.GetGetMethod().GetParameters().Length);
    }
  }
}
