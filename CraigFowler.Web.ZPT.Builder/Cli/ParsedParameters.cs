//  
//  ParsedParameters.cs
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
  /// Represents a collection of <see cref="IParameter"/>s that are present within a commandline string, indexed by
  /// their <see cref="System.Object"/> internal identifiers, <see cref="IParameter.InternalIdentifier"/>.
  /// </para>
  /// </summary>
  public class ParsedParameters
  {
    #region fields
    
    private IList<string> remainingArguments;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the underlying results of the parsing process.</para>
    /// </summary>
    protected Dictionary<object, ParameterValuePair> UnderlyingResults
    {
      get;
      private set;
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Gets whether or not this instance contains the given parameter.</para>
    /// </summary>
    /// <param name="internalIdentifier">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public bool HasParameter(object internalIdentifier)
    {
      return this.UnderlyingResults.ContainsKey(internalIdentifier);
    }
    
    /// <summary>
    /// <para>Overloaded.  Gets the value from a parameter that provided a value.</para>
    /// </summary>
    /// <param name="internalIdentifier">
    /// A <see cref="System.Object"/> that corresponds to the parameter's <see cref="IParameter.InternalIdentifier"/>.
    /// </param>
    /// <returns>
    /// A <see cref="System.Object"/> that contains the value from the parameter.
    /// </returns>
    public object GetValue(object internalIdentifier)
    {
      IParameter param;
      
      if(!this.HasParameter(internalIdentifier))
      {
        throw new InvalidOperationException("This collection of parameters does not contain the requested parameter.");
      }
      
      param = this.UnderlyingResults[internalIdentifier].Parameter;
      
      return param.GetValue(this.UnderlyingResults[internalIdentifier].RawValue);
    }
    
    /// <summary>
    /// <para>Overloaded.  Gets the value from a parameter that provided a value.</para>
    /// </summary>
    /// <param name="internalIdentifier">
    /// A <see cref="System.Object"/> that corresponds to the parameter's <see cref="IParameter.InternalIdentifier"/>.
    /// </param>
    /// <returns>
    /// A value that contains the value from the parameter.
    /// </returns>
    public T GetValue<T>(object internalIdentifier)
    {
      Parameter<T> param;
      
      if(!this.HasParameter(internalIdentifier))
      {
        throw new InvalidOperationException("This collection of parameters does not contain the requested parameter.");
      }
      
      param = this.UnderlyingResults[internalIdentifier].Parameter as Parameter<T>;
      
      if(param == null)
      {
        throw new InvalidOperationException("The requested parameter exists but it is of the wrong generic type.");
      }
      
      return param.GetValue(this.UnderlyingResults[internalIdentifier].RawValue);
    }
    
    /// <summary>
    /// <para>
    /// Gets the remaining commandline arguments that remain after all of the data recognised as
    /// registered parameters is parsed and removed.
    /// </para>
    /// </summary>
    /// <returns>
    /// A collection of <see cref="System.String"/>
    /// </returns>
    public IList<string> GetRemainingArguments()
    {
      return remainingArguments;
    }
    
    /// <summary>
    /// <para>
    /// Resets the state of this instance to an empty state and then stores parameter information within it.
    /// </para>
    /// </summary>
    /// <param name="parametersAndValues">
    /// A dictionary of <see cref="IParameter"/> and <see cref="System.String"/>
    /// </param>
    /// <param name="remainingArgs">
    /// A collection of <see cref="System.String"/>
    /// </param>
    public void StoreResults(Dictionary<IParameter, string> parametersAndValues, IList<string> remainingArgs)
    {
      this.UnderlyingResults = new Dictionary<object, ParameterValuePair>();
      
      this.StoreResult(remainingArgs);
      
      foreach(IParameter parameter in parametersAndValues.Keys)
      {
        this.StoreResult(parameter, parametersAndValues[parameter]);
      }
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Stores a single parameter/value pair result in the underlying collection for this type.
    /// </para>
    /// </summary>
    /// <param name="parameter">
    /// A <see cref="IParameter"/>
    /// </param>
    /// <param name="rawValue">
    /// A <see cref="System.String"/>
    /// </param>
    public void StoreResult(IParameter parameter, string rawValue)
    {
      if(parameter == null)
      {
        throw new ArgumentNullException("parameter");
      }
      
      ParameterValuePair result = new ParameterValuePair(parameter, rawValue);
      this.UnderlyingResults.Add(result.Parameter.InternalIdentifier, result);
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Stores the remaining arguments (after parameters are parsed) into this instance.
    /// </para>
    /// </summary>
    /// <param name="remainingArgs">
    /// A collection of <see cref="System.String"/>
    /// </param>
    public void StoreResult(IList<string> remainingArgs)
    {
      if(remainingArgs == null)
      {
        remainingArguments = new List<string>();
      }
      else
      {
        remainingArguments = remainingArgs;
      }
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Stores a single remaining argument (that is not part of a parameter) into this instance.
    /// </para>
    /// </summary>
    /// <param name="remainingArgument">
    /// A <see cref="System.String"/>
    /// </param>
    public void StoreResult(string remainingArgument)
    {
      if(remainingArguments == null)
      {
        remainingArguments = new List<string>();
      }
      
      remainingArguments.Add(remainingArgument);
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Default constructor initialises this instance with empty values.</para>
    /// </summary>
    public ParsedParameters ()
    {
      remainingArguments = new List<string>();
      
      this.UnderlyingResults = new Dictionary<object, ParameterValuePair>();
    }
    
    /// <summary>
    /// <para>
    /// Initialises this instance with a collection of parameters and raw <see cref="System.String"/> values as well as
    /// a collection of the <paramref name="remainingArgs"/> found on the commandline.
    /// </para>
    /// </summary>
    /// <param name="parametersAndValues">
    /// A dictionary of <see cref="IParameter"/> and <see cref="System.String"/>
    /// </param>
    /// <param name="remainingArgs">
    /// A collection of <see cref="System.String"/>
    /// </param>
    public ParsedParameters (Dictionary<IParameter, string> parametersAndValues, IList<string> remainingArgs) : this()
    {
      this.StoreResults(parametersAndValues, remainingArgs);
    }
    
    #endregion
    
    #region contained classes
    
    /// <summary>
    /// <para>Represents a parameter-and-value pair.</para>
    /// </summary>
    protected class ParameterValuePair
    {
      /// <summary>
      /// <para>Gets and sets the <see cref="IParameter"/> for this parameter/value pair.</para>
      /// </summary>
      public IParameter Parameter {
        get;
        set;
      }
      
      /// <summary>
      /// <para>Gets and sets the <see cref="System.String"/> raw value for this parameter/value pair.</para>
      /// </summary>
      public string RawValue {
        get;
        set;
      }
      
      /// <summary>
      /// <para>Initialises this instance with a parameter and a value.</para>
      /// </summary>
      /// <param name="param">
      /// A <see cref="IParameter"/>
      /// </param>
      /// <param name="val">
      /// A <see cref="System.String"/>
      /// </param>
      public ParameterValuePair (IParameter param, string val)
      {
        this.Parameter = param;
        this.RawValue = val;
      }
    }
    
    #endregion
  }
}

