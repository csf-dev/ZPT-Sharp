//  
//  TalProcessingException.cs
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

namespace CraigFowler.Web.ZPT.Tal.Exceptions
{
  /// <summary>
  /// <para>
  /// Represents a <see cref="TalException"/> encountered whilst parsing a <see cref="TalDocument"/> or a
  /// <see cref="TalElement"/>.
  /// </para>
  /// </summary>
  public class TalParsingException : TalException
  {
    #region constants
    
    private const string DEFAULT_MESSAGE = "There was a fatal error whilst parsing a TAL document or element.";
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>
    /// Gets and sets the <see cref="System.String"/> (usually a TAL attribute value) that has caused this error.
    /// </para>
    /// </summary>
    public string ProblemString
    {
      get {
        return (string) this.Data["problematic statement"];
      }
      set {
        this.Data["problematic statement"] = value;
      }
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises this instance with default values.</para>
    /// </summary>
    public TalParsingException() : this(DEFAULT_MESSAGE, null) {}
    
    /// <summary>
    /// <para>Initialises this instance with an inner exception.</para>
    /// </summary>
    /// <param name="inner">
    /// A <see cref="Exception"/>
    /// </param>
    public TalParsingException(Exception inner) : this(DEFAULT_MESSAGE, inner) {}
    
    /// <summary>
    /// <para>Initialises this instance with an exception message.</para>
    /// </summary>
    /// <param name="message">
    /// A <see cref="System.String"/>
    /// </param>
    public TalParsingException(string message) : this(message, null) {}
    
    /// <summary>
    /// <para>Initialises this instance with an exception message and an inner exception.</para>
    /// </summary>
    /// <param name="message">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="inner">
    /// A <see cref="Exception"/>
    /// </param>
    public TalParsingException(string message, Exception inner) : base(message, inner)
    {
      this.PermanentError = true;
    }
    
    #endregion
  }
}
