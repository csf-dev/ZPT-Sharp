//  
//  MetalDocument.cs
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
using System.Collections.Generic;
using CraigFowler.Web.ZPT.Tal;
using CraigFowler.Web.ZPT.Tales;
using System.Xml;

namespace CraigFowler.Web.ZPT.Metal
{
	/// <summary>
	/// <para>Describes an XML document that exposes both the TAL and METAL functionality.</para>
	/// </summary>
	public class MetalDocument : TalDocument, IMetalDocument
	{
    #region properties
        
    /// <summary>
    /// <para>
    /// Read-only.  Gets a collection of the <see cref="MetalMacro"/>s that are defined within this document.
    /// </para>
    /// </summary>
    [TalesAlias("macros")]
    public MetalMacroCollection Macros
    {
      get;
      private set;
    }

    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>
    /// Overridden.  Reads a node from an <see cref="XmlReader"/> and returns either a simple <see cref="XmlNode"/> or
    /// - if the node type indicated is appropriate, a <see cref="MetalElement"/> node.
    /// </para>
    /// </summary>
    /// <param name="reader">
    /// A <see cref="XmlReader"/>
    /// </param>
    /// <returns>
    /// A <see cref="XmlNode"/>
    /// </returns>
    public override XmlNode ReadNode (XmlReader reader)
    {
      XmlNode output;
      
      if(reader == null)
      {
        throw new ArgumentNullException("reader");
      }
      
      output = base.ReadNode(reader);
      
      // If we are dealing with an XmlElement node then clone it as a TalElement node and return that instead.
      if(output != null && output.NodeType == XmlNodeType.Element)
      {
        output = MetalElement.MetalElementFactory((XmlElement) output);
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Overridden.  Imports an <see cref="XmlNode"/> into this document instance, so that the node may be used as if
    /// it were a first-class part of this document.
    /// </para>
    /// </summary>
    /// <param name="node">
    /// An <see cref="XmlNode"/> to clone as a member of this document.
    /// </param>
    /// <param name="deep">
    /// A <see cref="System.Boolean"/> that indicates whether a deep or shallow copy is to be performed.  In a
    /// <see cref="MetalDocument"/> this should almost always be true.
    /// </param>
    /// <returns>
    /// The newly-imported <see cref="XmlNode"/> instance.
    /// </returns>
    public override XmlNode ImportNode (XmlNode node, bool deep)
    {
      XmlNode output;
      
      if(node is XmlElement)
      {
        if(!deep)
        {
          throw new NotSupportedException("Importing XML element nodes into a TAL document as shallow " +
                                          "copies is not supported.");
        }
        
        output = MetalElement.MetalElementFactory((XmlElement) node, this);
      }
      else
      {
        output = base.ImportNode (node, deep);
      }
      
      return output;
    }
		
		#endregion
		
		#region constructors
		
		/// <summary>
		/// <para>
		/// Initialises this instance with default values and a new/empty <see cref="TalesContext"/>.  DTD resolution is
		/// disabled in order to improve performance.
		/// </para>
		/// </summary>
		public MetalDocument () : this(false) {}
		
		/// <summary>
		/// <para>
		/// Initialises this instance with default values and a new/empty <see cref="TalesContext"/> and a given setting
		/// for whether DTDs will be resolved or not.
		/// </para>
		/// </summary>
		/// <param name="useDTDResolver">
		/// A <see cref="System.Boolean"/> that determines whether DTDs are resolved or not.  Setting this to false will
		/// greatly improve performance.
		/// </param>
		public MetalDocument (bool useDTDResolver) : base(useDTDResolver)
    {
      this.Macros = new MetalMacroCollection();
    }
		
		#endregion
	}
}

