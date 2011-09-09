//  
//  IParameter.cs
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
  /// <para>A definition for a parameter that can be received on the commandline.</para>
  /// </summary>
  /// <remarks>
  /// <para>
  /// Each parameter must be defined and registered with an <see cref="IParameterParser"/> before it can be used.
  /// The parameter processor must know which parts of the commandline to treat as parameters (and their arguments) and
  /// which to treat as other data on the commandline (such as actions, paths and the like).
  /// </para>
  /// </remarks>
  public interface IParameter
  {
    #region properties
    
    /// <summary>
    /// <para>
    /// Gets and sets a unique identifier for this parameter.  This value is used to identify this parameter within a
    /// collection of parameters.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>It is recommended to use enumeration constants to identify parameters in a strongly-typed manner.</para>
    /// <para>
    /// This identifier will not be used on the commandline, rather it is only used on the internal API to work with
    /// the parameter from code.
    /// </para>
    /// </remarks>
    object InternalIdentifier { get; set; }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a collection of 'long-style' identifers for this parameter on the commandline, minus the
    /// platform-specific parts of the parameter.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <example>
    /// <para>
    /// For example, if a parameter is to be referenced (in a Unix-style parameter) as <c>--my-parameter</c> then a
    /// long identifier would be <c>my-parameter</c>.  This same parameter in a Windows-style might be
    /// <c>/my-parameter</c>.
    /// </para>
    /// </example>
    /// <para>
    /// Whilst it is not recommended to create many long identifiers for a parameter, it is possible (over time, as
    /// software changes) to have multiple identifiers for a single parameter.  This can occur as identifiers change
    /// meaning and/or become deprecated.  It is advised to only create one long-identifier for each parameter.
    /// </para>
    /// </remarks>
    List<string> LongIdentifiers { get; }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a collection of short (likely one-character) identifiers for this parameter on the
    /// commandline, minus the platform-specific parts of the parameter.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <example>
    /// <para>
    /// For example, if a parameter is to be referenced (in a Unix-style parameter) as <c>-m</c> then a
    /// short identifier would be <c>m</c>.  This same parameter in a Windows-style might be
    /// <c>/m</c>.
    /// </para>
    /// </example>
    /// <para>
    /// Whilst it is not recommended to create many short identifiers for a parameter, it is possible (over time, as
    /// software changes) to have multiple identifiers for a single parameter.  This can occur as identifiers change
    /// meaning and/or become deprecated.  It is advised to create at most one short-identifier for each parameter.
    /// </para>
    /// <para>
    /// Not every parameter requires a short identifier.  Short identifiers could be harder to remember and the
    /// available alphabet restricts their number somewhat.  If your application takes many possible parameters then
    /// it is probably best to give only the most commonly-used parameters short identifiers and leave the
    /// less-commonly-used parameters only with long identifiers.
    /// </para>
    /// <para>Short parameters are case-sensitive and so <c>a</c> is different from <c>A</c>.</para>
    /// </remarks>
    List<string> ShortIdentifiers { get; }
    
    /// <summary>
    /// <para>Gets and sets the type of parameter that this represents.</para>
    /// </summary>
    ParameterType Type { get; set; }
    
    /// <summary>
    /// <para>Read-only.  Gets whether or not this parameter is valid for usage.</para>
    /// </summary>
    bool ValidForUse { get; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Gets the value from this parameter.</para>
    /// </summary>
    /// <param name="rawValue">
    /// A <see cref="System.String"/> containing the raw value for this parameter.
    /// </param>
    /// <returns>
    /// A <see cref="System.Object"/>, although implementing classes may provide a strongly-typed version of this
    /// method.
    /// </returns>
    object GetValue(string rawValue);
    
    /// <summary>
    /// <para>
    /// Overridden from <see cref="System.Object"/>.  Determines whether this parameter is equal to another.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Two parameters are considered equal if their <see cref="InternalIdentifier"/>s are equal.
    /// </para>
    /// </remarks>
    /// <param name="obj">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    bool Equals(object obj);
    
    /// <summary>
    /// Overridden from <see cref="System.Object"/>.  Gets a hash code for this parameter based on its
    /// <see cref="InternalIdentifier"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="System.Int32"/>
    /// </returns>
    int GetHashCode();
    
    #endregion
  }
}

