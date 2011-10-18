//  
//  UnixParameters.cs
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
using System.Text.RegularExpressions;

namespace CraigFowler.Cli
{
  /// <summary>
  /// <para>An implementation of <see cref="ParameterParser"/> designed to work with Unix-style parameters.</para>
  /// </summary>
  public class UnixParameters : ParameterParser
  {
    #region constants
    
    private const string
      LONG_PARAMETER_PATTERN            = @"^--([A-Za-z0-9_-]{2,})$",
      SHORT_PARAMETER_PATTERN           = @"^-([A-Za-z0-9])$";
    
    private static readonly Regex
      LongParameter                     = new Regex(LONG_PARAMETER_PATTERN, RegexOptions.Compiled),
      ShortParameter                    = new Regex(SHORT_PARAMETER_PATTERN, RegexOptions.Compiled);
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>
    /// Overridden.  Provides the actual paramater parsing mechanism, outputting results to the
    /// <paramref name="output"/> collection.
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
    /// <returns>
    /// A <see cref="ParsedParameters"/>
    /// </returns>
    protected override void Parse(IList<string> rawCommandLine,
                                  IDictionary<string, IList<IParameter>> longAliases,
                                  IDictionary<string, IList<IParameter>> shortAliases,
                                  ref ParsedParameters output)
    {
      IList<IParameter> previousParameters = null;
      string previousArgument = null;
      
      foreach(string argument in rawCommandLine)
      {
        IList<IParameter> currentParameters = this.GetMatchingParameters(argument, longAliases, shortAliases);
        
        if(previousParameters != null
           && previousParameters.Count > 0
           && previousParameters[0].Type == ParameterType.ValueRequired)
        {
          /* We are dealing with the value from a value-required parameter, store the results and remove the
           * reference to the previous parameter.
           */
          foreach(IParameter param in previousParameters)
          {
            output.StoreResult(param, argument);
          }
          previousParameters = null;
        }
        else if(previousParameters != null
                && previousParameters.Count > 0
                && previousParameters[0].Type == ParameterType.ValueOptional
                && currentParameters == null)
        {
          /* We are dealing with a value from a value-optional parameter - the argument does not look like a parameter
           * itself and so we treat it as the value, exactly as above.
           */
          foreach(IParameter param in previousParameters)
          {
            output.StoreResult(param, argument);
          }
          previousParameters = null;
        }
        else if(currentParameters != null)
        {
          /* We are dealing with a new parameter definition.  If there are parameters waiting to be stored then store
           * them and then deal with this new parameter.
           */
          if(previousParameters != null)
          {
            foreach(IParameter param in previousParameters)
            {
              output.StoreResult(param, null);
            }
            previousParameters = null;
          }
          
          previousParameters = currentParameters;
        }
        else
        {
          // We are dealing with a non-parameter argument; store it.
          output.StoreResult(argument);
        }
        
        previousArgument = argument;
      }
        
      /* After we have finished parsing everything else, if we still have 'previous parameters' buffered and not
       * stored then store them unless they are "value required" parameters, in which case add their argument as a
       * non-parameter argument.
       */
      if(previousParameters != null)
      {
        foreach(IParameter param in previousParameters)
        {
          if(param.Type != ParameterType.ValueRequired)
          {
            output.StoreResult(param, null);
          }
          else
          {
            output.StoreResult(previousArgument);
          }
        }
      }
    }
    
    /// <summary>
    /// <para>
    /// Compares the <paramref name="argument"/> against the long and short aliases for parameters and (if a matching
    /// collection of parameters is found) returns that collection of parameters.
    /// </para>
    /// </summary>
    /// <param name="argument">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="longAliases">
    /// A dictionary of <see cref="System.String"/> and a collection of <see cref="IParameter"/>
    /// </param>
    /// <param name="shortAliases">
    /// A dictionary of <see cref="System.String"/> and a collection of <see cref="IParameter"/>
    /// </param>
    /// <returns>
    /// A collection of <see cref="IParameter"/>
    /// </returns>
    private IList<IParameter> GetMatchingParameters(string argument,
                                                    IDictionary<string, IList<IParameter>> longAliases,
                                                    IDictionary<string, IList<IParameter>> shortAliases)
    {
      Match longMatch, shortMatch;
      IList<IParameter> output = null;
      
      if(argument == null)
      {
        throw new ArgumentNullException("argument");
      }
      else if(longAliases == null)
      {
        throw new ArgumentNullException("longAliases");
      }
      else if(shortAliases == null)
      {
        throw new ArgumentNullException("shortAliases");
      }
      
      longMatch = LongParameter.Match(argument);
      shortMatch = ShortParameter.Match(argument);
      
      if(longMatch.Success && longAliases.ContainsKey(longMatch.Groups[1].Value))
      {
        output = longAliases[longMatch.Groups[1].Value];
      }
      else if(shortMatch.Success && shortAliases.ContainsKey(shortMatch.Groups[1].Value))
      {
        output = shortAliases[shortMatch.Groups[1].Value];
      }
      
      return output;
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Initialises this instance.</para>
    /// </summary>
    public UnixParameters() : base(ParameterStyle.Unix) {}
    
    #endregion
  }
}
