//  
//  DefinitionType.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2010 Craig Fowler
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

namespace CraigFowler.Web.ZPT.Tales
{
  /// <summary>
  /// <para>Enumerates the types of TALES definitions.</para>
  /// <para>See the syntax for 'Define' at http://wiki.zope.org/ZPT/TALSpecification14</para>
  /// <seealso cref="TalesContext.AddDefinition(string, object, DefinitionType)"/>
  /// </summary>
  public enum DefinitionType : int
  {
    /// <summary>
    /// <para>
    /// A local variable definition, accessible only in the current <see cref="TalesContext"/> and child contexts.
    /// </para>
    /// </summary>
    Local           = 1,
    
    /// <summary>
    /// <para>A global variable definition, accessible to any <see cref="TalesContext"/> in the same document.</para>
    /// </summary>
    Global          = 2
  }
}
