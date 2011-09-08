//  
//  ParameterLongNamesAttribute.cs
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
using System.Collections.Generic;

namespace CraigFowler.Cli
{
  /// <summary>
  /// <para>
  /// Used to decorate an enumeration constant, where that constant represents a parameter that an application
  /// accepts.  This attribute is used to identify "long names" for the parameter.  Most parameters have exactly one
  /// long name.
  /// </para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class ParameterLongNamesAttribute : Attribute
  {
    #region properties
    
    /// <summary>
    /// <para>Gets and sets a collection of the names for the parameter.</para>
    /// </summary>
    public IList<string> Names {
      get;
      set;
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Initialises this instance with a collection of names.</para>
    /// </summary>
    /// <param name="names">
    /// A <see cref="System.String[]"/>
    /// </param>
    public ParameterLongNamesAttribute (params string[] names)
    {
      this.Names = names;
    }
    
    #endregion
  }
}

