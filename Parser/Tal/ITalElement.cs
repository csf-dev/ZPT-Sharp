//  
//  IHasTalesContext.cs
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
using CraigFowler.Web.ZPT.Tales;
using System.Xml;

namespace CraigFowler.Web.ZPT.Tal
{
  /// <summary>
  /// <para>
  /// Interface used to mark <see cref="XmlNode"/> instances that are important to the TAL rendering process.
  /// </para>
  /// </summary>
  public interface ITalElement
  {
    /// <summary>
    /// <para>Read-only.  Gets the <see cref="TalesContext"/> for this node.</para>
    /// </summary>
    TalesContext TalesContext { get; }
    
    /// <summary>
    /// <para>Gets the <see cref="TalesContext"/> from the parent <see cref="ITalElement" />.  This may be null.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="TalesContext"/>
    /// </returns>
    TalesContext GetParentTalesContext();
    
    /// <summary>
    /// <para>Renders this node and its children to the given <see cref="XmlWriter"/> instance.</para>
    /// </summary>
    /// <param name="writer">
    /// A <see cref="XmlWriter"/>
    /// </param>
    void Render(XmlWriter writer);
  }
}
