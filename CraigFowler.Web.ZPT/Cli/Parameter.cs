//  
//  Parameter.cs
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
  /// <para>Represents a single parameter.  This class is generic for the return value of the parameter.</para>
  /// </summary>
  public class Parameter<T> : IParameter
  {
    #region fields
    
    private object internalId;
    private ParameterType paramType;
    
    #endregion
    
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
    public object InternalIdentifier
    {
      get {
        return internalId;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        
        internalId = value;
      }
    }
    
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
    public List<string> LongIdentifiers
    {
      get;
      private set;
    }
    
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
    public List<string> ShortIdentifiers
    {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>Gets and sets the type of parameter that this represents.</para>
    /// </summary>
    public ParameterType Type
    {
      get {
        return paramType;
      }
      set {
        if(!Enum.IsDefined(typeof(ParameterType), value))
        {
          throw new ArgumentOutOfRangeException("value", "Unknown or unrecognised parameter type.");
        }
        
        paramType = value;
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets whether or not this parameter is valid for use.</para>
    /// </summary>
    public bool ValidForUse {
      get {
        return (this.ShortIdentifiers.Count > 0 || this.LongIdentifiers.Count > 0);
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Gets a value from this instance in a strongly-typed manner.</para>
    /// </summary>
    /// <param name="rawValue">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="T"/>
    /// </returns>
    public T GetValue(string rawValue)
    {
      if(this.Type == ParameterType.NoValue)
      {
        throw new InvalidOperationException("The current parameter represents a 'NoValue' parameter type.");
      }
      
      return (T) Convert.ChangeType(rawValue, typeof(T));
    }
    
    /// <summary>
    /// <para>
    /// Overridden, overloaded.  Determines whether this instance is equal to a given <paramref name="obj"/>.
    /// </para>
    /// </summary>
    /// <param name="obj">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public override bool Equals (object obj)
    {
      Parameter<T> param = obj as Parameter<T>;
      return ((param != null)? this.Equals(param) : false);
    }
    
    /// <summary>
    /// <para>Overloaded.  Determines whether or not this instance is equal to a given <paramref name="obj"/>.</para>
    /// </summary>
    /// <param name="obj">
    /// A parameter
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public bool Equals(Parameter<T> obj)
    {
      bool output;
      
      if(obj == null)
      {
        output = false;
      }
      else if(Object.ReferenceEquals(this, obj))
      {
        output = true;
      }
      else
      {
        output = (this.InternalIdentifier.Equals(obj.InternalIdentifier));
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
      return (this.InternalIdentifier != null)? this.InternalIdentifier.GetHashCode() : 0;
    }
    
    object IParameter.GetValue(string rawValue)
    {
      return this.GetValue(rawValue);
    }
    
    #endregion

    #region constructors
    
    /// <summary>
    /// <para>
    /// Initialises this instance with an internal identifier, a parameter type and two optional collections of
    /// identifiers.
    /// </para>
    /// </summary>
    /// <param name="internalIdentifier">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <param name="type">
    /// A <see cref="ParameterType"/>
    /// </param>
    /// <param name="longIdentifiers">
    /// A collection of <see cref="System.String"/>
    /// </param>
    /// <param name="shortIdentifiers">
    /// A collection of <see cref="System.String"/>
    /// </param>
    public Parameter(object internalIdentifier,
                     ParameterType type,
                     IList<string> longIdentifiers,
                     IList<string> shortIdentifiers)
    {
      // Initialise the properties with default values
      this.InternalIdentifier = internalIdentifier;
      this.Type = type;
      this.LongIdentifiers = new List<string>();
      this.ShortIdentifiers = new List<string>();
      
      // Populate the identifiers if they have been passed
      if(longIdentifiers != null)
      {
        this.LongIdentifiers.AddRange(longIdentifiers);
      }
      if(shortIdentifiers != null)
      {
        this.ShortIdentifiers.AddRange(shortIdentifiers);
      }
    }
    
    /// <summary>
    /// <para>Initialises this instance with an internal identifier and a parameter type.</para>
    /// </summary>
    /// <param name="internalIdentifier">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <param name="type">
    /// A <see cref="ParameterType"/>
    /// </param>
    public Parameter(object internalIdentifier, ParameterType type) : this(internalIdentifier, type, null, null) {}
    
    #endregion
  }
}

