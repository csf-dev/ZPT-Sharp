//  
//  MockObject.cs
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
using CraigFowler.Web.ZPT.Tales;

namespace CraigFowler.Web.ZPT.Mocks
{
  public class MockObject
  {
    #region private fields
    
    private Dictionary<string, string> dict;
    
    #endregion
    
    #region public fields
    
    [TalesAlias("inner")]
    public MockObject InnerObject;
    
    public int IntegerValue;
    
    public bool BooleanValue;
    
    #endregion
    
    #region properties
    
    [TalesAlias("unambiguous")]
    public string this [string key]
    {
      get {
        string output;
        
        if(dict.ContainsKey(key))
        {
          output = dict[key];
        }
        else
        {
          output = null;
        }
        
        return output;
      }
      set {
        if(dict.ContainsKey(key))
        {
          dict[key] = value;
        }
        else
        {
          dict.Add(key, value);
        }
      }
    }
    
    #endregion
    
    #region constructors
    
    public MockObject() : this(false) {}
    
    public MockObject(bool createInner)
    {
      this.InnerObject = createInner? new MockObject() : null;
      this.IntegerValue = 0;
      
      dict = new Dictionary<string, string>();
      this["foo"] = "bar";
      this["baz"] = "sample";
      
      this.BooleanValue = true;
    }
    
    #endregion
  }
}
