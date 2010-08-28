//  
//  DefaultValueMarker.cs
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

namespace CraigFowler.Web.ZPT.Tales
{
  /// <summary>
  /// <para>A simple marker type for the TALES 'default value' root context.</para>
  /// </summary>
  public class DefaultValueMarker
  {
    #region public methods
    
    /// <summary>
    /// <para>Overridden.  Determines equality with another object.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This will only return true if the <paramref name="obj"/> is another <see cref="DefaultValueMarker"/>.
    /// </para>
    /// </remarks>
    /// <param name="obj">
    /// A <see cref="System.Object"/> to compare with.
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/> indicating whether or not this instance is equal to <paramref name="obj"/>.
    /// </returns>
    public override bool Equals (object obj)
    {
      bool output = false;
      
      if(obj is DefaultValueMarker)
      {
        output = true;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Overridden.  Gets a hash code for this instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Int32"/>
    /// </returns>
    public override int GetHashCode ()
    {
      return "TALES PATH EXPRESSION DEFAULT VALUE MARKER".GetHashCode();
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Default constructor for a <see cref="DefaultValueMarker"/>.</para>
    /// </summary>
    public DefaultValueMarker() {}
    
    #endregion
  }
}
