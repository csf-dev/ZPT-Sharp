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
  /// <para>Represents an individual path used within a <see cref="Expressions.PathExpression"/>.</para>
  /// </summary>
  public class TalesPath
  {
    #region constants
    
    private const char PARTS_SEPARATOR = '/';
    
    #endregion
    
    #region fields
    
    private string rawPath;
    private List<string> parts;
    
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
        rawPath = null;
        
        if(value != null)
        {
          rawPath = value.Trim();
        }
        
        if(String.IsNullOrEmpty(rawPath))
        {
          rawPath = null;
        }
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets an ordered collection of parts to this path - IE the object names separated by forward-slash
    /// characters.
    /// </para>
    /// </summary>
    public List<string> Parts
    {
      get {
        return parts;
      }
      private set {
        parts = value;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Returns a string representation of this path instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    public override string ToString ()
    {
      return this.ToString(this.Parts.Count);
    }
    
    /// <summary>
    /// <para>
    /// Returns a string representation of this path instance, showing only the given number of path pieces.
    /// </para>
    /// </summary>
    /// <param name="partCount">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    public string ToString(int partCount)
    {
      if(partCount < 0)
      {
        throw new ArgumentOutOfRangeException("partCount", "Parameter 'partCount' must be more than or equal to one.");
      }
      
      return String.Join(PARTS_SEPARATOR.ToString(), this.Parts.ToArray(), 0, partCount);
    }
    
    /// <summary>
    /// <para>Overridden.  Evaluates whether this instance equals the given <paramref name="obj"/>.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This will only return true if <paramref name="obj"/> is a <see cref="TalesPath"/> and its
    /// <see cref="TalesPath.Text"/> matches the Text property on this instance.
    /// </para>
    /// </remarks>
    /// <param name="obj">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public override bool Equals (object obj)
    {
      bool output;
      
      if(obj != null && obj is TalesPath)
      {
        output = ((TalesPath) obj).Text == this.Text;
      }
      else
      {
        output = false;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Gets a hash code for this instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Int32"/>
    /// </returns>
    public override int GetHashCode ()
    {
      return (this.Text != null)? this.Text.GetHashCode() : "{NULL PATH}".GetHashCode();
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
    /// A collection of <see cref="System.String"/> instances.
    /// </returns>
    /// <exception cref="FormatException">
    /// If the given <paramref name="path" /> contains a component that is null or empty then this exception is raised.
    /// </exception>
    private List<string> ExtractPathParts(string path)
    {
      string[] rawParts;
      List<string> output = new List<string>();
      
      // In case the input is null then we create zero parts
      rawParts = (path == null)? new string[0] : path.Split(new char[] { PARTS_SEPARATOR });
      
      foreach(string part in rawParts)
      {
        if(String.IsNullOrEmpty(part))
        {
          throw new FormatException("The given path is not a correctly-formatted path specification.  " +
                                    "A part of the path is null or an empty string.");
        }
        else
        {
          output.Add(part);
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
    public TalesPath(string path)
    {
      this.Text = path;
      this.Parts = this.ExtractPathParts(this.Text);
    }
    
    #endregion
  }
}
