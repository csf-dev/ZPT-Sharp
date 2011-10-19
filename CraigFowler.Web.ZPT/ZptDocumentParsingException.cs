//  
//  ZptDocumentParsingException.cs
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
using System.Runtime.Serialization;

namespace CraigFowler.Web.ZPT
{
  /// <summary>
  /// <para>
  /// An exception that was occurred whilst trying to parse an <see cref="ITemplateDocument"/>.
  /// </para>
  /// </summary>
  public class ZptDocumentParsingException : Exception
  {
    #region constants
    
    private const string
      DEFAULT_MESSAGE = "Error whilst attempting to parse an ITemplateDocument.";
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Gets and sets the filename of the ZPT document file that caused this error.</para>
    /// </summary>
    public string Filename
    {
      get;
      set;
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises this instance with default values.</para>
    /// </summary>
    public ZptDocumentParsingException () : this(DEFAULT_MESSAGE, null) {}
    
    /// <summary>
    /// <para>Initialises this instance with a message and information about an inner exception.</para>
    /// </summary>
    /// <param name="message">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="inner">
    /// A <see cref="Exception"/>
    /// </param>
    public ZptDocumentParsingException(string message, Exception inner) : base(message, inner)
    {
      this.Filename = null;
    }
    
    /// <summary>
    /// <para>Constructor used to deserialise this instance.</para>
    /// </summary>
    /// <param name="info">
    /// A <see cref="SerializationInfo"/>
    /// </param>
    /// <param name="context">
    /// A <see cref="StreamingContext"/>
    /// </param>
    public ZptDocumentParsingException (SerializationInfo info, StreamingContext context) : base(info, context) {}
    
    #endregion
  }
}

