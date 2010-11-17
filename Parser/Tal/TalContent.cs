//  
//  TalContent.cs
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
using System.Xml;
namespace CraigFowler.Web.ZPT.Tal
{
	/// <summary>
	/// <para>
	/// Describes TAL content to be written to a <see cref="TalOutput"/> instance.
	/// </para>
	/// </summary>
	public class TalContent
	{
		#region fields
		
		private bool writeElement;
		
		#endregion
		
		#region properties
		
		/// <summary>
		/// <para>Read-only.  Gets the text content to be written to the output stream.</para>
		/// </summary>
		public string Content
		{
			get;
			private set;
		}
		
		/// <summary>
		/// <para>
		/// Gets and sets the type of content to be written to the output stream, which will determine whether it is passed
		/// through entity/escaping rules before being written.
		/// </para>
		/// </summary>
		public TalContentType Type
		{
			get;
			set;
		}
		
		/// <summary>
		/// <para>
		/// Gets and sets whether the containing XML element will be written to the output stream or whether it will be
		/// omitted.
		/// </para>
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method can return false even if it has been previously set to true.  This is because element nodes within
		/// the TAL or METAL namespaces are never written.
		/// </para>
		/// </remarks>
		public bool WriteElement
		{
			get {
				bool output = writeElement;
				
				if(output)
				{
					if(this.CurrentElement == null)
					{
						output = false;
					}
					else if(this.CurrentElement.NamespaceURI == TalDocument.TalNamespace ||
					   			this.CurrentElement.NamespaceURI == TalDocument.MetalNamespace)
					{
						output = false;
					}
				}
				
				return output;
			}
			set {
				writeElement = value;
			}
		}
		
		/// <summary>
		/// <para>Read-only.  Gets whether or not generated TAL <see cref="Content"/> should be written.</para>
		/// </summary>
		public bool WriteContent
		{
			get;
			private set;
		}
		
		/// <summary>
		/// <para>Gets and sets the TAL element node that contains this content.</para>
		/// </summary>
		public TalElement CurrentElement
		{
			get;
			set;
		}
		
		#endregion
		
		#region constructors
		
		/// <summary>
		/// <para>Initialises this instance with default values.</para>
		/// </summary>
		/// <param name="node">
		/// A <see cref="TalElement"/> - the node that is being dealt with.
		/// </param>
		private TalContent(TalElement node)
		{
			this.Content = null;
			this.Type = TalContentType.Text;
			this.WriteElement = true;
			this.WriteContent = false;
			this.CurrentElement = node;
		}
		
		/// <summary>
		/// <para>Initialises this instance with the text content to write to the output stream.</para>
		/// </summary>
		/// <param name="content">
		/// A <see cref="System.Object"/>
		/// </param>
		public TalContent(object content) : this(content, null) {}
		
		/// <summary>
		/// <para>Initialises this instance with the text content to write to the output stream.</para>
		/// </summary>
		/// <param name="content">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="node">
		/// A <see cref="TalElement"/> - the node that is being dealt with.
		/// </param>
		public TalContent(object content, TalElement node) : this(node)
		{
			this.Content = content.ToString();
			this.WriteContent = true;
		}
		
		/// <summary>
		/// <para>
		/// Initialises this instance with information about whether or not the XML element should be wirten or not.
		/// </para>
		/// </summary>
		/// <param name="node">
		/// A <see cref="TalElement"/> - the node that is being dealt with.
		/// </param>
		/// <param name="writeElement">
		/// A <see cref="System.Boolean"/>, whether or not to render the <paramref name="node"/> in the output.
		/// </param>
		public TalContent(TalElement node, bool writeElement) : this(node)
		{
			this.WriteElement = writeElement;
		}
		
		#endregion
	}
}

