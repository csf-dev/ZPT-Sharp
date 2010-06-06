//  
//  PathTraversalException.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2010 Craig Fowler
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

namespace CraigFowler.Web.ZPT.Tales.Exceptions
{
  public class TraversalException : TalesException
  {
    #region constants
    
    private const string DEFAULT_MESSAGE = "Could not traverse any of the given paths.";
    
    #endregion
      
    #region fields
    
    private Dictionary<TalesPath, TalesException> attempts;
    
    #endregion
    
    #region properties
    
    public Dictionary<TalesPath, TalesException> Attempts
    {
      get {
        return attempts;
      }
      private set {
        attempts = value;
      }
    }
    
    public override bool PermanentError
    {
      get {
        bool output = true;
        
        foreach(TalesPath path in this.Attempts.Keys)
        {
          if(this.Attempts[path] != null && this.Attempts[path].PermanentError == false)
          {
            output = false;
          }
        }
        
        return output;
      }
    }

    
    #endregion
    
    #region constructor
    
    public TraversalException() : base(DEFAULT_MESSAGE, null)
    {
      this.Attempts = new Dictionary<TalesPath, TalesException>();
    }
    
    #endregion
  }
}
