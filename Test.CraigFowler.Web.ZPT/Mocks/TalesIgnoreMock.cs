//  
//  TalesIgnoreMock.cs
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
using System.Collections.Generic;
using CraigFowler.Web.ZPT.Tales;

namespace CraigFowler.Web.ZPT.Mocks
{
  /// <summary>
  /// <para>Mock object to test the order of discovery for members on an object.</para>
  /// </summary>
  public class TalesIgnoreMock
  {
    #region private fields
    
    private Dictionary<string, int> privateDictionary;
    
    #endregion
    
    #region public fields
    
    public int TestFieldOne;
    
    [TalesMember(true)]
    public int TestFieldTwo;
    
    public int TestFieldThree;
    
    [TalesMember("TestFieldThree")]
    public int TestFieldFour;
    
    [TalesMember(true)]
    public int TestFieldFive;
    
    #endregion
    
    #region properties
    
    public int this [string key]
    {
      get {
        return privateDictionary[key];
      }
      set {
        privateDictionary[key] = value;
      }
    }
    
    #endregion
    
    #region constructor
    
    public TalesIgnoreMock ()
    {
      privateDictionary = new Dictionary<string, int>();
      
      this["TestFieldOne"] = 10;
      this["TestFieldTwo"] = 20;
      this["TestFieldThree"] = 30;
      
      this.TestFieldOne = 1;
      this.TestFieldTwo = 2;
      this.TestFieldThree = 3;
      this.TestFieldFour = 4;
      this.TestFieldFive = 5;
    }
    
    #endregion
  }
}

