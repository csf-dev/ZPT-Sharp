//  
//  ParameterType.cs
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

namespace CraigFowler.Cli
{
  /// <summary>
  /// <para>Enumerates the various types of commandline parameters.</para>
  /// </summary>
  public enum ParameterType : int
  {
    /// <summary>
    /// <para>
    /// A parameter that never takes an associated value; the parameter acts as a flag.  Only the presence of this
    /// parameter will be recorded, never any value.
    /// </para>
    /// </summary>
    NoValue,
    
    /// <summary>
    /// <para>
    /// The parameter has an optional value associated with it.  If the following item on the commandline is not a
    /// parameter definition then it will be interpreted as the value to this parameter.  If the following item does
    /// appear to be a parameter definition then it will be treated as such.
    /// </para>
    /// </summary>
    ValueOptional,
    
    /// <summary>
    /// <para>
    /// Similar to <see cref="ValueOptional"/> but the following item on the commandline will be treated as a value
    /// to this parameter regardless of what it looks like.  If there is no following value then this will be treated
    /// as an error.
    /// </para>
    /// </summary>
    ValueRequired
  }
}
