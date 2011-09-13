//  
//  TalContentType.cs
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

namespace CraigFowler.Web.ZPT.Tal
{
  /// <summary>
  /// <para>Enumerates the types of TAL content that can be written when rendering a <see cref="TalElement"/>.</para>
  /// </summary>
  public enum TalContentType : int
  {
    /// <summary>
    /// <para>
    /// Indicates that the content should be written directly as text, thus all special XML characters should be
    /// escaped.
    /// </para>
    /// </summary>
    Text            = 1,
    
    /// <summary>
    /// <para>Indicates that the content should be writen verbatim, retaining any embedded XML structure.</para>
    /// </summary>
    Structure
  }
}
