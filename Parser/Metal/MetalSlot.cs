//  
//  MetalSlot.cs
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
using CraigFowler.Web.ZPT.Tal;
using CraigFowler.Web.ZPT.Metal.Exceptions;

namespace CraigFowler.Web.ZPT.Metal
{
  /// <summary>
  /// <para>Describes a single METAL 'slot' element.</para>
  /// </summary>
  public class MetalSlot : MetalElement
  {
    #region properties
    
    /// <summary>
    /// <para>Gets and sets the name of this slot within the <see cref="ParentMacro"/>.</para>
    /// </summary>
    public string SlotName {
      get;
      set;
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>
    /// Overridden.  Clones the given <see cref="XmlElement"/>, interpreting it as a <see cref="MetalSlot"/>.
    /// </para>
    /// </summary>
    /// <param name="elementToClone">
    /// A <see cref="XmlElement"/>
    /// </param>
    protected override void CloneFrom (XmlElement elementToClone)
    {
      if(!this.HasAttribute(MetalElement.DefineSlotAttributeName, TalDocument.MetalNamespace))
      {
        string message = String.Format("The current element does not contain a {0}:{1} attribute.  " +
                                       "It is not a METAL slot definition.",
                                       TalDocument.MetalNamespace,
                                       MetalElement.DefineSlotAttributeName);
        throw new MetalException(message);
      }
      
      // Populate the slot name
      this.SlotName = this.GetAttribute(MetalElement.DefineSlotAttributeName, TalDocument.MetalNamespace);
      
      base.CloneFrom(elementToClone);
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises this instance with the given information.</para>
    /// </summary>
    /// <param name="prefix">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="localName">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="namespaceURI">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="document">
    /// A <see cref="TalDocument"/>
    /// </param>
    public MetalSlot(string prefix,
                     string localName,
                     string namespaceURI,
                     XmlDocument document) : base(prefix, localName, namespaceURI, document)
    {
      this.SlotName = null;
    }
    
    /// <summary>
    /// <para>Serves as a copy-constructor for an <see cref="XmlElement"/> node.</para>
    /// </summary>
    /// <param name="elementToClone">
    /// A <see cref="XmlElement"/>
    /// </param>
    public MetalSlot(XmlElement elementToClone) : this(elementToClone.Prefix,
                                                       elementToClone.LocalName,
                                                       elementToClone.NamespaceURI,
                                                       elementToClone.OwnerDocument)
    {
      this.CloneFrom(elementToClone);
    }
    
    #endregion
  }
}

