//  
//  ParameterParser.cs
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
  /// <para>Base class for all processor implementations.</para>
  /// </summary>
  public abstract class ParameterParser : IParameterParser
  {
    #region constants
    
    /// <summary>
    /// <para>Read-only.  Gets a collection that represents the permitted return types for parameters.</para>
    /// </summary>
    public static readonly Type[] PermittedParameterTypes = { typeof(string),
                                                              typeof(int),
                                                              typeof(decimal),
                                                              typeof(bool),
                                                              typeof(DateTime) };
    
    #endregion
    
    #region fields
    
    /// <summary>
    /// <para>Exposes the underlying collection of registered parameters, indexed by their internal identifiers.</para>
    /// </summary>
    private Dictionary<object, IParameter> registeredParameters;
    
    // Two fields to hold cached aliases for the normalised aliases for parameters
    private IDictionary<string, IList<IParameter>> cachedLongAliases, cachedShortAliases;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets parameters from this instance by their <see cref="System.Object"/> internal identifiers.
    /// For adding/registering new parameters use <see cref="RegisterParameter(IParameter)"/> instead.
    /// </para>
    /// </summary>
    /// <param name="internalIdentifier">
    /// A <see cref="System.Object"/>
    /// </param>
    public IParameter this [object internalIdentifier]
    {
      get {
        if(!registeredParameters.ContainsKey(internalIdentifier))
        {
          throw new ArgumentOutOfRangeException("internalIdentifier",
                                                "The parameter identified by the given identifier is not registered " +
                                                "in this parameter parser instance.");
        }
        
        return registeredParameters[internalIdentifier];
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the count of registered parameters.</para>
    /// </summary>
    public int ParameterCount
    {
      get {
        return registeredParameters.Count;
      }
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the style(s) of parameter that this processor instance is capable of parsing.</para>
    /// </summary>
    public ParameterStyle Style
    {
      get;
      private set;
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Overloaded.  Registers a new parameter into this instance.</para>
    /// </summary>
    /// <param name="parameter">
    /// An <see cref="IParameter"/>
    /// </param>
    public void RegisterParameter(IParameter parameter)
    {
      if(parameter == null)
      {
        throw new ArgumentNullException("parameter");
      }
      else if(parameter.InternalIdentifier == null)
      {
        throw new ArgumentException("Internal identifier may not be null.", "parameter");
      }
      else if(registeredParameters.ContainsKey(parameter.InternalIdentifier))
      {
        throw new InvalidOperationException("Cannot register parameter: duplicate internal identifier.");
      }
      else if(!parameter.ValidForUse)
      {
        throw new ArgumentException("Parameter is not valid for use.", "parameter");
      }
      
      // Any time the registered parameters are changed, we must clear the cached aliases
      cachedLongAliases = null;
      cachedShortAliases = null;
      
      registeredParameters.Add(parameter.InternalIdentifier, parameter);
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Generic convenience method constructs a new <see cref="IParameter"/> object and registers it into
    /// this instance.
    /// </para>
    /// </summary>
    /// <param name="internalIdentifier">
    /// A <see cref="System.Object"/> corresponding to <see cref="IParameter.InternalIdentifier"/> for the
    /// newly-created parameter.
    /// </param>
    /// <param name="paramType">
    /// A <see cref="ParameterType"/> corresponding to <see cref="IParameter.Type"/> for the
    /// newly-created parameter.
    /// </param>
    /// <param name="longIdentifiers">
    /// A collection of <see cref="System.String"/> corresponding to <see cref="IParameter.LongIdentifiers"/> for the
    /// newly-created parameter.
    /// </param>
    /// <param name="shortIdentifiers">
    /// A collection of <see cref="System.String"/> corresponding to <see cref="IParameter.ShortIdentifiers"/> for the
    /// newly-created parameter.
    /// </param>
    /// <typeparam name="T">
    /// Designates the type of value that the newly-created <see cref="IParameter"/> is intended to store.
    /// </typeparam>
    public void RegisterParameter<T>(object internalIdentifier,
                                     ParameterType paramType,
                                     IList<string> longIdentifiers,
                                     IList<string> shortIdentifiers)
    {
      IParameter parameter = new Parameter<T>(internalIdentifier, paramType, longIdentifiers, shortIdentifiers);
      this.RegisterParameter(parameter);
    }
    
    /// <summary>
    /// <para>
    /// Alternative method of registering parameters.  Requires an enumeration that represents the available parameters
    /// and carries attributes that describes how they are used as parameters.
    /// </para>
    /// </summary>
    /// <param name="parameterEnumeration">
    /// A <see cref="Type"/>
    /// </param>
    public void RegisterParameters(Type parameterEnumeration)
    {
      Array enumerationValues;
      
      if(parameterEnumeration == null)
      {
        throw new ArgumentNullException("parameterEnumeration");
      }
      else if(!parameterEnumeration.IsEnum)
      {
        throw new ArgumentException("The type with which to detect and register parameters must be an enumeration",
                                    "parameterEnumeration");
      }
      
      enumerationValues = Enum.GetValues(parameterEnumeration);
      
      foreach(object enumValue in enumerationValues)
      {
        this.RegisterParameter(enumValue, parameterEnumeration);
      }
    }
    
    /// <summary>
    /// <para>
    /// Gets whether or not an <see cref="IParameter"/> has been registered with the given
    /// <paramref name="internalIdentifier"/>.
    /// </para>
    /// </summary>
    /// <param name="internalIdentifier">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public bool HasParameter(object internalIdentifier)
    {
      return registeredParameters.ContainsKey(internalIdentifier);
    }
    
    /// <summary>
    /// <para>Gets whether or not this instance can process parameters in the given <paramref name="style"/>.</para>
    /// </summary>
    /// <param name="style">
    /// A <see cref="ParameterStyle"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public bool CanProcess(ParameterStyle style)
    {
      return ((this.Style & style) == style);
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Performs parsing on the raw command line and returns an object representing the results of the
    /// parsing.
    /// </para>
    /// </summary>
    /// <param name="rawCommandLine">
    /// A collection of <see cref="System.String"/> - the raw collection of string arguments received via the
    /// commandline.
    /// </param>
    /// <returns>
    /// A <see cref="ParsedParameters"/>
    /// </returns>
    public ParsedParameters Parse(IList<string> rawCommandLine)
    {
      ParsedParameters output = new ParsedParameters();
      
      this.Parse(rawCommandLine, ref output);
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Performs parsing on the raw command line and returns an object representing the results of the
    /// parsing.  This overload stores the parameters in an existing output collection.
    /// </para>
    /// </summary>
    /// <param name="rawCommandLine">
    /// A collection of <see cref="System.String"/> - the raw collection of string arguments received via the
    /// commandline.
    /// </param>
    /// <param name="output">
    /// A <see cref="ParsedParameters"/>
    /// </param>
    public void Parse(IList<string> rawCommandLine, ref ParsedParameters output)
    {
      if(cachedLongAliases == null || cachedShortAliases == null)
      {
        this.NormalisesParameterAliases(registeredParameters.Values, out cachedLongAliases, out cachedShortAliases);
      }
      
      this.Parse(rawCommandLine, cachedLongAliases, cachedShortAliases, ref output);
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Performs parsing on the raw command line and returns an object representing the results of the
    /// parsing.  This abstract method must be provided by implementing classes.
    /// </para>
    /// </summary>
    /// <param name="rawCommandLine">
    /// A collection of <see cref="System.String"/> - the raw collection of string arguments received via the
    /// commandline.
    /// </param>
    /// <param name="longAliases">
    /// A dictionary of <see cref="System.String"/> and a collection of <see cref="IParameter"/>
    /// </param>
    /// <param name="shortAliases">
    /// A dictionary of <see cref="System.String"/> and a collection of <see cref="IParameter"/>
    /// </param>
    /// <param name="output">
    /// A <see cref="ParsedParameters"/>
    /// </param>
    protected abstract void Parse(IList<string> rawCommandLine,
                                  IDictionary<string, IList<IParameter>> longAliases,
                                  IDictionary<string, IList<IParameter>> shortAliases,
                                  ref ParsedParameters output);
    
    #endregion
    
    #region private methods
    
    /// <summary>
    /// <para>
    /// Overloaded.  Registers a parameter using an enumeration value and the type of the enumeration it came from.
    /// </para>
    /// </summary>
    /// <param name="enumValue">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <param name="enumType">
    /// A <see cref="Type"/>
    /// </param>
    private void RegisterParameter(object enumValue, Type enumType)
    {
      object[] attributes;
      IParameter parameter = null;
      
      if(enumType == null)
      {
        throw new ArgumentNullException("enumType");
      }
      else if(!enumType.IsEnum)
      {
        throw new ArgumentException("The type with which to detect and register parameters must be an enumeration",
                                    "enumType");
      }
      
      /* Discover a ParameterAttribute attribute on the enumeration constant.  If present then we are creating a
       * parameter.  If not then we are not creating one and we are going to do nothing more in this method.
       */
      attributes = enumType.GetField(enumValue.ToString()).GetCustomAttributes(typeof(ParameterAttribute), false);
      if(attributes.Length == 1)
      {
        parameter = ((ParameterAttribute) attributes[0]).ParameterFactory(enumValue);
        
        // Now we know we are creating a parameter, detect naming attributes on the enum constant and use them.
        attributes = enumType.GetField(enumValue.ToString()).GetCustomAttributes(false);
        foreach(object attrib in attributes)
        {
          if(attrib is ParameterShortNamesAttribute)
          {
            parameter.ShortIdentifiers.AddRange(((ParameterShortNamesAttribute) attrib).Names);
          }
          else if(attrib is ParameterLongNamesAttribute)
          {
            parameter.LongIdentifiers.AddRange(((ParameterLongNamesAttribute) attrib).Names);
          }
        }
        
        // Register the newly-created parameter
        this.RegisterParameter(parameter);
      }
    }
    
    /// <summary>
    /// <para>
    /// Normalises the aliases/names of parameters by compiling two dictionaries of the long/short aliases that can
    /// be used to reach the parameters.
    /// </para>
    /// </summary>
    /// <param name="parameters">
    /// A collection of <see cref="IParameter"/>
    /// </param>
    /// <param name="longAliases">
    /// A dictionary of <see cref="System.String"/> and a collection of <see cref="IParameter"/>
    /// </param>
    /// <param name="shortAliases">
    /// A dictionary of <see cref="System.String"/> and a collection of <see cref="IParameter"/>
    /// </param>
    private void NormalisesParameterAliases(IEnumerable<IParameter> parameters,
                                            out IDictionary<string, IList<IParameter>> longAliases,
                                            out IDictionary<string, IList<IParameter>> shortAliases)
    {
      longAliases = new Dictionary<string, IList<IParameter>>();
      shortAliases = new Dictionary<string, IList<IParameter>>();
      
      if(parameters == null)
      {
        throw new ArgumentNullException("parameters");
      }
      
      foreach(IParameter param in parameters)
      {
        foreach(string name in param.LongIdentifiers)
        {
          if(!longAliases.ContainsKey(name))
          {
            longAliases.Add(name, new List<IParameter>());
          }
          else if(longAliases[name].Count > 0 &&
                  param.Type != longAliases[name][0].Type)
          {
            throw new NotSupportedException("Parameters using the same commandline aliases are permitted but they " +
                                            "must be of the same parameter type.");
          }
          longAliases[name].Add(param);
        }
        
        foreach(string name in param.ShortIdentifiers)
        {
          if(!shortAliases.ContainsKey(name))
          {
            shortAliases.Add(name, new List<IParameter>());
          }
          else if(shortAliases[name].Count > 0 &&
                  param.Type != shortAliases[name][0].Type)
          {
            throw new NotSupportedException("Parameters using the same commandline aliases are permitted but they " +
                                            "must be of the same parameter type.");
          }
          shortAliases[name].Add(param);
        }
      }
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Default constructor initialises this instance with a given parameter style.</para>
    /// </summary>
    /// <param name="style">
    /// A <see cref="ParameterStyle"/>
    /// </param>
    public ParameterParser(ParameterStyle style)
    {
      this.Style = style;
      
      registeredParameters = new Dictionary<object, IParameter>();
      cachedLongAliases = null;
      cachedShortAliases = null;
    }
    
    #endregion
    
    #region static methods
    
    /// <summary>
    /// <para>Determines whether or not the <paramref name="paramType"/> is a permitted type.</para>
    /// </summary>
    /// <param name="paramType">
    /// A <see cref="Type"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool IsPermittedParameterType(Type paramType)
    {
      return ((IList<Type>) PermittedParameterTypes).Contains(paramType);
    }
    
    /// <summary>
    /// <para>Factory method creates and returns a new <see cref="IParameterParser"/> instance.</para>
    /// </summary>
    /// <param name="style">
    /// A <see cref="ParameterStyle"/>
    /// </param>
    /// <returns>
    /// A <see cref="IParameterParser"/>
    /// </returns>
    public static IParameterParser ParserFactory(ParameterStyle style)
    {
      IParameterParser output;
      
      switch(style)
      {
      case ParameterStyle.Unix:
        output = new UnixParameters();
        break;
      default:
        throw new NotSupportedException("Unsupported or unrecognised parameter style.");
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Static convenience method parses the <paramref name="rawCommandLine"/> as parameters, using
    /// parameter definitions exposed within <paramref name="parameterEnumeration"/> and the
    /// <see cref="ParameterStyle"/> indicated by <paramref name="style"/>.
    /// </para>
    /// </summary>
    /// <param name="rawCommandLine">
    /// A collection of <see cref="System.String"/>
    /// </param>
    /// <param name="style">
    /// A <see cref="ParameterStyle"/>
    /// </param>
    /// <param name="parameterEnumeration">
    /// A <see cref="Type"/>
    /// </param>
    /// <returns>
    /// A generic type that implements <see cref="ParsedParameters"/>
    /// </returns>
    public static TParameters Parse<TParameters>(IList<string> rawCommandLine,
                                                 ParameterStyle style,
                                                 Type parameterEnumeration) where TParameters : ParsedParameters, new()
    {
      IParameterParser parser;
      TParameters output;
      ParsedParameters looseOutput;
      
      parser = ParserFactory(style);
      output = new TParameters();
      looseOutput = output;
      
      parser.RegisterParameters(parameterEnumeration);
      
      parser.Parse(rawCommandLine, ref looseOutput);
      
      return output;
    }
    
    #endregion
  }
}

