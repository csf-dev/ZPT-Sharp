//  
//  ExpressionException.cs
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
  /// <para>Represents a type of <see cref="PathException"/> that is encountered when a TALES path is not valid.</para>
  /// </summary>
  public class PathInvalidException : PathException
  {
    #region constants
    
    private const string
      DEFAULT_MESSAGE         = "The TALES path expression cannot be traversed, it is invalid.",
      INVALID_MESSAGE         = "The TALES path expression is invalid.";
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Gets and sets a raw string-based path that caused this exception.</para>
    /// </summary>
    public string RawPath
    {
      get {
        return (string) this.Data["rawPath"];
      }
      set {
        this.Data["rawPath"] = value;
      }
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises this instance with a given, invalid, string path.</para>
    /// </summary>
    /// <param name="rawPath">
    /// A <see cref="System.String"/>
    /// </param>
    public PathInvalidException(string rawPath) : this(null, INVALID_MESSAGE, null)
    {
      this.RawPath = rawPath;
    }
    
    /// <summary>
    /// <para>Initialises this instance with the given <see cref="TalesPath"/>.</para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="TalesPath"/>
    /// </param>
    public PathInvalidException(TalesPath path) : this(path, DEFAULT_MESSAGE, null) {}
    
    /// <summary>
    /// <para>Initialises this instance with the given <see cref="TalesPath"/> and an inner exception.</para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="TalesPath"/>
    /// </param>
    /// <param name="inner">
    /// A <see cref="Exception"/>
    /// </param>
    public PathInvalidException(TalesPath path, Exception inner) : this(path, DEFAULT_MESSAGE, inner) {}
    
    /// <summary>
    /// <para>Initialises this instance with the given <see cref="TalesPath"/> and an error message.</para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="TalesPath"/>
    /// </param>
    /// <param name="message">
    /// A <see cref="System.String"/>
    /// </param>
    public PathInvalidException(TalesPath path, string message) : this(path, message, null) {}
    
    /// <summary>
    /// <para>
    /// Initialises this instance with the given <see cref="TalesPath"/>, an error message and an inner exception.
    /// </para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="TalesPath"/>
    /// </param>
    /// <param name="message">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="inner">
    /// A <see cref="Exception"/>
    /// </param>
    public PathInvalidException(TalesPath path, string message, Exception inner) : base(path, message, inner)
    {
      this.PermanentError = true;
      this.RawPath = null;
    }
    
    #endregion
  }
}
