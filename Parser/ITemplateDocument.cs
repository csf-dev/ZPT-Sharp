//  
//  ITemplateDocument.cs
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
using CraigFowler.Web.ZPT.Tal;
using System.Xml;

namespace CraigFowler.Web.ZPT
{
	/// <summary>
	/// <para>Interface describes a document template which may expose the TAL and/or METAL functionality.</para>
	/// </summary>
	public interface ITemplateDocument
	{
		/// <summary>
		/// <para>
		/// Gets and sets whether or not this document should force the reindentation and reformatting of the document on
		/// rendering.  This property is only applicable if <see cref="XmlDocument.PreserveWhitespace"/> is false.
		/// </para>
		/// </summary>
		bool ReformatDocument { get; set; }
		
    /// <summary>
    /// <para>Gets and sets the <see cref="TalesContext"/> for this document.</para>
    /// </summary>
		TalesContext TalesContext { get; set; }
		
		/// <summary>
    /// <para>Overloaded.  Renders this node and its children using the given <see cref="TalOutput"/> instance.</para>
		/// </summary>
		/// <param name="output">
		/// A <see cref="TalOutput"/>
		/// </param>
		void Render(TalOutput output);
		
    /// <summary>
    /// <para>Overloaded.  Renders the output of this document to the given <see cref="XmlWriter"/>.</para>
    /// </summary>
    /// <param name="writer">
    /// An <see cref="XmlWriter"/> to write the rendere output of this document to.
    /// </param>
		void Render(XmlWriter writer);
		
    /// <summary>
    /// <para>Overloaded.  Convenience method that renders the output of this document to a string.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/>, the rendered output document.
    /// </returns>
		string Render();
		
		/// <summary>
		/// <para>Overloaded.  Reads XML from a file at <paramref name="path"/> into this instance.</para>
		/// </summary>
		/// <param name="path">
		/// A <see cref="System.String"/>
		/// </param>
		void Load(string path);
	}
}

