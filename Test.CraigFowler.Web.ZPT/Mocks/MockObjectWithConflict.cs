//  
//  MockObjectWithConflict.cs
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


using CraigFowler.Web.ZPT.Tales;

namespace CraigFowler.Web.ZPT.Mocks
{
  public class MockObjectWithConflict
  {
    public string SomeProperty;
    
    [TalesMember("SomeProperty")]
    public string OtherProperty;
    
    [TalesMember("Duplicate")]
    public string SomeDuplicate;
    
    [TalesMember("Duplicate")]
    public string OtherDuplicate;
    
    public string this [string key]
    {
      get {
        return "foo";
      }
      set { }
    }
    
    public MockObjectWithConflict()
    {
      this.SomeProperty = "foo";
      this.OtherProperty = "bar";
      
      this.SomeDuplicate = "baz";
      this.OtherDuplicate = "sample";
    }
  }
}
