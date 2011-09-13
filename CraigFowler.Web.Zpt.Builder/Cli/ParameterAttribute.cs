//  
//  ParameterAttribute.cs
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
  /// <para>
  /// Used to decorate an enumeration constant, where that constant represents a parameter that an application
  /// accepts.  This is the bare minimum attribute that must decorate an enumeration constant in order to use it as
  /// a parameter.
  /// </para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class ParameterAttribute : Attribute
  {
    #region fields
    
    private ParameterType type;
    private Type valueType;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Gets and sets the type of parameter that the decorated enumeration constant represents.</para>
    /// </summary>
    public ParameterType Type {
      get {
        return type;
      }
      set {
        if(!Enum.IsDefined(typeof(ParameterType), value))
        {
          throw new ArgumentOutOfRangeException("value", "Unknown or unrecognised parameter type.");
        }
        
        type = value;
      }
    }
    
    /// <summary>
    /// <para>
    /// Gets and sets the return value type of the parameter that the decoration enumeration constant represents.
    /// </para>
    /// </summary>
    public Type ValueType {
      get {
        return valueType;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        else if(!ParameterParser.IsPermittedParameterType(value))
        {
          throw new NotSupportedException("Unsupported parameter value type.");
        }
        
        valueType = value;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>
    /// Factory method creates a new <see cref="IParameter"/> from this attribute and the enumeration constant that
    /// it was decorating.
    /// </para>
    /// </summary>
    /// <param name="enumerationValue">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="IParameter"/>
    /// </returns>
    public IParameter ParameterFactory(object enumerationValue)
    {
      IParameter output;
      
      if(this.ValueType == typeof(string))
      {
        output = new Parameter<string>(enumerationValue, this.Type);
      }
      else if(this.ValueType == typeof(int))
      {
        output = new Parameter<int>(enumerationValue, this.Type);
      }
      else if(this.ValueType == typeof(decimal))
      {
        output = new Parameter<decimal>(enumerationValue, this.Type);
      }
      else if(this.ValueType == typeof(bool))
      {
        output = new Parameter<bool>(enumerationValue, this.Type);
      }
      else if(this.ValueType == typeof(DateTime))
      {
        output = new Parameter<DateTime>(enumerationValue, this.Type);
      }
      else
      {
        throw new InvalidOperationException("Invalid or unrecognised parameter value type.  Theoretically " +
                                            "impossible to reach this point.");
      }
      
      return output;
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Default constructor initialises this instance with default values.</para>
    /// </summary>
    public ParameterAttribute ()
    {
      this.Type = ParameterType.ValueOptional;
      this.ValueType = typeof(string);
    }
    
    #endregion
  }
}

