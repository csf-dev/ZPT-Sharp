//  
//  IZptDocument.cs
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
using CraigFowler.Web.ZPT.Tal;
using CraigFowler.Web.ZPT.Metal;
using CraigFowler.Web.ZPT.Tales;
using System.Collections.Generic;

namespace CraigFowler.Web.ZPT
{
	/// <summary>
	/// <para>Interface describes a Zope Page Templates document.</para>
	/// <seealso cref="ZptMetadata.DocumentClass"/>
	/// </summary>
	public interface IZptDocument
	{
		/// <summary>
		/// <para>Gets and sets the document metadata for this ZPT document.</para>
		/// </summary>
		ZptMetadata Metadata { get; set; }
		
		/// <summary>
		/// <para>
		/// Read-only.  Gets whether this ZPT document has macro-expansions (METAL) enabled or not.  If this property is
		/// false then the underlying document will be a <see cref="TalDocument"/> instead of a
		/// <see cref="MetalDocument"/>.
		/// </para>
		/// </summary>
		bool MetalEnabled { get; }
		
		/// <summary>
		/// <para>Gets the XML template document that is associated with this instance.</para>
		/// </summary>
		/// <returns>
		/// An object instance that implements <see cref="ITemplateDocument"/>.
		/// </returns>
		ITemplateDocument GetTemplateDocument();
    
    /// <summary>
    /// <para>Gets a collection of the <see cref="MetalMacro"/> instances that this instance contains.</para>
    /// </summary>
    /// <returns>
    /// A dictionary of <see cref="MetalMacro"/>, indexed by <see cref="System.String"/>
    /// </returns>
    [TalesAlias("macros")]
    MetalMacroCollection GetMacros();
	}
}

