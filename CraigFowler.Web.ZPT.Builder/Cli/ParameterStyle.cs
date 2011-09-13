//  
//  ParameterStyle.cs
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
  /// <para>Enumerates the various styles of creating and parsing commandline parameters.</para>
  /// </summary>
  [Flags]
  public enum ParameterStyle : int
  {
    /// <summary>
    /// <para>Unix-style parameters.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// These parameters use single dashes to indicate short parameters and double-dashes to indicate long parameters.
    /// </para>
    /// </remarks>
    Unix            = 1,
    
    /// <summary>
    /// <para>Windows-style parameters.</para>
    /// </summary>
    Windows         = 2
  }
}
