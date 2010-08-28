//  
//  PathException.cs
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
  /// <para>
  /// Represents a <see cref="TalesException"/> that is encountered when traversing a <see cref="TalesPath"/>.
  /// </para>
  /// </summary>
  public class PathException : TalesException
  {
    #region constants
    
    private const string DEFAULT_MESSAGE = "There was a problem whilst traversing the TALES path expression.";
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Gets and sets the <see cref="TalesPath"/> that caused this exception.</para>
    /// </summary>
    public TalesPath Path
    {
      get {
        TalesPath output;
        
        if(this.Data["path"] is TalesPath)
        {
          output = this.Data["path"] as TalesPath;
        }
        else
        {
          output = null;
        }
        
        return output;
      }
      set {
        this.Data["path"] = value;
      }
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises this instance with the given <see cref="TalesPath"/>.</para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="TalesPath"/>
    /// </param>
    public PathException(TalesPath path) : this(path, DEFAULT_MESSAGE, null) {}
    
    /// <summary>
    /// <para>Initialises this instance with the given <see cref="TalesPath"/> and an inner exception.</para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="TalesPath"/>
    /// </param>
    /// <param name="inner">
    /// A <see cref="Exception"/>
    /// </param>
    public PathException(TalesPath path, Exception inner) : this(path, DEFAULT_MESSAGE, inner) {}
    
    /// <summary>
    /// <para>Initialises this instance with the given <see cref="TalesPath"/> and an error message.</para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="TalesPath"/>
    /// </param>
    /// <param name="message">
    /// A <see cref="System.String"/>
    /// </param>
    public PathException(TalesPath path, string message) : this(path, message, null) {}
    
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
    public PathException(TalesPath path, string message, Exception inner) : base(message, inner)
    {
      this.PermanentError = false;
      this.Path = path;
    }
    
    #endregion
  }
}
