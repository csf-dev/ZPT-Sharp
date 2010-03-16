//  
//  Path.cs
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

namespace CraigFowler.Web.ZPT.Tales
{
  /// <summary>
  /// <para>Represents an individual path used within a <see cref="PathExpression"/>.</para>
  /// </summary>
  public class Path
  {
    #region constants
    
    private const char PARTS_SEPARATOR = '/';
    
    #endregion
    
    #region fields
    
    private string rawPath;
    private Queue<string> parts;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the raw text that this path represents.</para>
    /// </summary>
    public string Text
    {
      get {
        return rawPath;
      }
      private set {
        if(value == null)
        {
          rawPath = null;
        }
        else
        {
          rawPath = value.Trim();
          if(rawPath == String.Empty)
          {
            rawPath = null;
          }
        }
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets an ordered collection of parts to this path - IE the object names separated by forward-slash
    /// characters.
    /// </para>
    /// </summary>
    public Queue<string> Parts
    {
      get {
        return parts;
      }
      private set {
        parts = value;
      }
    }
    
    #endregion
    
    #region private methods
    
    /// <summary>
    /// <para>
    /// Generates a collection of the parts of the given path, separating the input by forward-slash characters.
    /// </para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="Queue<System.String>"/>
    /// </returns>
    /// <exception cref="FormatException">
    /// If the given <paramref name="path" /> contains a component that is null or empty then this exception is raised.
    /// </exception>
    private Queue<string> extractParts(string path)
    {
      Queue<string> output = new Queue<string>();
      
      foreach(string part in path.Split(new char[] { PARTS_SEPARATOR }))
      {
        if(String.IsNullOrEmpty(part))
        {
          throw new FormatException("The given path is not a correctly-formatted path specification.  " +
                                    "A part of the path is null or an empty string.");
        }
        else
        {
          output.Enqueue(part);
        }
      }
      
      return output;
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Initialises this instance with a new path string.</para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="System.String"/>
    /// </param>
    /// <exception cref="FormatException">
    /// If the given <paramref name="path" /> contains a component that is null or empty then this exception is raised.
    /// </exception>
    public Path(string path)
    {
      Text = path;
      Parts = extractParts(Text);
    }
    
    #endregion
  }
}
