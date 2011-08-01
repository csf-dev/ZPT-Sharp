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
    #region static fields
    
    /// <summary>
    /// <para>Read-only runtime-constant returns a singleton instance of the default value marker.</para>
    /// </summary>
    public static readonly DefaultValueMarker Marker = new DefaultValueMarker();
    
    #endregion
    
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
      return (this == obj);
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
    
    #region operator overloads
    
    /// <summary>
    /// <para>Operator overload for testing equality.</para>
    /// </summary>
    /// <param name="obj1">
    /// A <see cref="DefaultValueMarker"/>
    /// </param>
    /// <param name="obj2">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool operator ==(DefaultValueMarker obj1, object obj2)
    {
      bool output;
      
      if(Object.ReferenceEquals(obj1, obj2))
      {
        output = true;
      }
      else if((object) obj1 != null && obj2 is DefaultValueMarker)
      {
        output = true;
      }
      else
      {
        output = false;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Operator overload for testing inequality.</para>
    /// </summary>
    /// <param name="obj1">
    /// A <see cref="DefaultValueMarker"/>
    /// </param>
    /// <param name="obj2">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool operator !=(DefaultValueMarker obj1, object obj2)
    {
      return !(obj1 == obj2);
    }
    
    #endregion
  }
}
