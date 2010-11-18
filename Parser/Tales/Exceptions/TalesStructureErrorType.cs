//  
//  TalesStructureErrorType.cs
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
namespace CraigFowler.Web.ZPT.Tales.Exceptions
{
	/// <summary>
	/// <para>Enumerates the possible types of errors that a <see cref="TalesStructureException"/> can describe.</para>
	/// </summary>
	public enum TalesStructureErrorType : int
	{
		/// <summary>
		/// <para>An error relating to storing an object in the structure.</para>
		/// <seealso cref="TalesStructureProvider.StoreItem(TalesPath, System.Object)"/>
		/// </summary>
		Store
	}
}

