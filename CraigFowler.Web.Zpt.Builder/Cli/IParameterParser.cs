//  
//  IParameterParser.cs
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
  /// <para>Interface for all commandline parameter processors.</para>
  /// </summary>
  /// <remarks>
  /// <para>
  /// There may be many types of parameter processors, each processing a different style of passing parameters.
  /// </para>
  /// </remarks>
  public interface IParameterParser
  {
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
    IParameter this[object internalIdentifier] { get; }
    
    /// <summary>
    /// <para>Read-only.  Gets the count of registered parameters.</para>
    /// </summary>
    int ParameterCount { get; }
    
    /// <summary>
    /// <para>Read-only.  Gets the parameter style(s) that this parameter parser instance can process.</para>
    /// </summary>
    ParameterStyle Style { get; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Overloaded.  Registers a new parameter into this instance.</para>
    /// </summary>
    /// <param name="parameter">
    /// An <see cref="IParameter"/>
    /// </param>
    void RegisterParameter(IParameter parameter);
    
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
    void RegisterParameter<T>(object internalIdentifier,
                              ParameterType paramType,
                              IList<string> longIdentifiers,
                              IList<string> shortIdentifiers);
    
    /// <summary>
    /// <para>
    /// Alternative method of registering parameters.  Requires an enumeration that represents the available parameters
    /// and carries attributes that describes how they are used as parameters.
    /// </para>
    /// </summary>
    /// <param name="parameterEnumeration">
    /// A <see cref="Type"/>
    /// </param>
    void RegisterParameters(Type parameterEnumeration);
    
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
    bool HasParameter(object internalIdentifier);
    
    /// <summary>
    /// <para>Gets whether or not this instance can process parameters in the given <paramref name="style"/>.</para>
    /// </summary>
    /// <param name="style">
    /// A <see cref="ParameterStyle"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    bool CanProcess(ParameterStyle style);
    
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
    ParsedParameters Parse(IList<string> rawCommandLine);
    
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
    void Parse(IList<string> rawCommandLine, ref ParsedParameters output);
    
    #endregion
  }
}

