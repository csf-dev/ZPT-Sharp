//  
//  TalesException.cs
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

namespace CraigFowler.Web.ZPT.Tales.Exceptions
{
  /// <summary>
  /// <para>Base class for exceptions encountered whilst parsing and evaluating TALES expressions.</para>
  /// </summary>
  public abstract class TalesException : Exception
  {
    #region contants
    
    private const string
      DEFAULT_MESSAGE         = "There was an error relating to a TALES expression.";
    
    #endregion
    
    #region fields
    
    private bool fatal;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>
    /// Gets and sets a value that indicates whether this exception indicates a permanent error in parsing the
    /// expression.
    /// </para>
    /// <para>
    /// In a permanent error, changing the values within the domain model will not help resolving the error.
    /// </para>
    /// </summary>
    public virtual bool PermanentError
    {
      get {
        return fatal;
      }
      protected set {
        fatal = value;
      }
    }
    
    #endregion
    
    #region constructors
    
    public TalesException() : this(DEFAULT_MESSAGE, null) {}
    
    public TalesException(string message) : this(message, null) {}
    
    public TalesException(Exception inner) : this(DEFAULT_MESSAGE, inner) {}
    
    public TalesException(string message, Exception inner) : base(message, inner)
    {
      this.PermanentError = true;
    }
    
    #endregion
  }
}
