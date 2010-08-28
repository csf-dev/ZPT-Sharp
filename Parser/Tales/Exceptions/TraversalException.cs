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
  /// <summary>
  /// <para>
  /// Represents a <see cref="TalesException"/> encountered when none of a collection of <see cref="TalesPath"/>
  /// instances could be traversed.
  /// </para>
  /// </summary>
  public class TraversalException : TalesException
  {
    #region constants
    
    private const string DEFAULT_MESSAGE = "Could not traverse any of the given paths.";
    
    #endregion
      
    #region fields
    
    private Dictionary<TalesPath, TalesException> attempts;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a ictionary of <see cref="TalesException"/> instances representing the problems encountered in
    /// traversing paths, indexed by the <see cref="TalesPath"/> that they are associated with.
    /// </para>
    /// </summary>
    public Dictionary<TalesPath, TalesException> Attempts
    {
      get {
        return attempts;
      }
      private set {
        attempts = value;
      }
    }
    
    /// <summary>
    /// <para>Overridden.  Gets whether this is a permanent error.</para>
    /// </summary>
    public override bool PermanentError
    {
      get {
        bool output = true;
        
        foreach(TalesPath path in this.Attempts.Keys)
        {
          /* If just one of the errors encountered whilst traversing the paths was not permanent, then this error is
           * not permanent either.
           */
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
    
    /// <summary>
    /// <para>Initialises this instance with default values and an empty <see cref="Attempts"/> collection.</para>
    /// </summary>
    public TraversalException() : base(DEFAULT_MESSAGE, null)
    {
      this.Attempts = new Dictionary<TalesPath, TalesException>();
    }
    
    #endregion
  }
}
