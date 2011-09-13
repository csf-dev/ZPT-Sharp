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
  public class TalesException : Exception
  {
    #region contants
    
    private const string
      DEFAULT_MESSAGE         = "There was an error relating to a TALES expression.";
    
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
        return (bool) this.Data["Fatal"];
      }
      protected set {
        this.Data["Fatal"] = value;
      }
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises this instance with default values.</para>
    /// </summary>
    public TalesException() : this(DEFAULT_MESSAGE, null) {}
    
    /// <summary>
    /// <para>Initialises this instance with an exception message.</para>
    /// </summary>
    /// <param name="message">
    /// A <see cref="System.String"/>
    /// </param>
    public TalesException(string message) : this(message, null) {}
    
    /// <summary>
    /// <para>Initialises this instance with an inner exception.</para>
    /// </summary>
    /// <param name="inner">
    /// A <see cref="Exception"/>
    /// </param>
    public TalesException(Exception inner) : this(DEFAULT_MESSAGE, inner) {}
    
    /// <summary>
    /// <para>Initialises this instance with an exception message and an inner exception.</para>
    /// </summary>
    /// <param name="message">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="inner">
    /// A <see cref="Exception"/>
    /// </param>
    public TalesException(string message, Exception inner) : base(message, inner)
    {
      this.PermanentError = true;
    }
    
    #endregion
  }
}
