//  
//  IMetalDocument.cs
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

namespace CraigFowler.Web.ZPT.Metal
{
  /// <summary>
  /// <para>Marker interface for a <see cref="ITemplateDocument"/> that is METAL-capable.</para>
  /// </summary>
  public interface IMetalDocument : ITemplateDocument
  {
    /// <summary>
    /// <para>
    /// Read-only.  Gets a collection of the <see cref="MetalMacro"/>s that are defined within this document.
    /// </para>
    /// </summary>
    MetalMacroCollection Macros { get; }
  }
}

