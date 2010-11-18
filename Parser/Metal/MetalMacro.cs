//  
//  MetalMacro.cs
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
using System.Xml;
using CraigFowler.Web.ZPT.Tal;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Metal.Exceptions;

namespace CraigFowler.Web.ZPT.Metal
{
	/// <summary>
	/// <para>Describes a macro that may appear within a METAL document.</para>
	/// </summary>
	public class MetalMacro : MetalElement
	{
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the slots that this macro contains.</para>
    /// </summary>
    public Dictionary<string, MetalSlot> Slots
    {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the name of this macro within the parent METAL document.</para>
    /// </summary>
    public string MacroName
    {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a <see cref="TalesPath"/> for the <see cref="MetalMacro"/> that this instance extends.
    /// </para>
    /// </summary>
    public TalesPath ExtendMacroPath
    {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>Read-only.  Gets a reference to the <see cref="MetalMacro"/> that this instance extends.</para>
    /// </summary>
    public MetalMacro ExtendMacro
    {
      get;
      private set;
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>
    /// Overridden.  Populates this node with attributes and the child nodes of the given
    /// <paramref name="elementToClone"/>.  This includes searching that node for METAL macro definitions and also
    /// enumerating the slots that this macro contains.
    /// </para>
    /// </summary>
    /// <param name="elementToClone">
    /// A <see cref="XmlElement"/>
    /// </param>
    protected override void CloneFrom (XmlElement elementToClone)
    {
      base.CloneFrom (elementToClone);
      this.GetMacroInformation();
    }
    
    /// <summary>
    /// <para>
    /// Populates the members of this instance:  <see cref="MacroName"/> and <see cref="Slots"/> with information
    /// found on the element and its children.
    /// </para>
    /// </summary>
    protected virtual void GetMacroInformation()
    {
      if(!this.HasAttribute(MetalElement.DefineMacroAttributeName, TalDocument.MetalNamespace))
      {
        string message = String.Format("The current element does not contain a {0}:{1} attribute.  " +
                                       "It is not a METAL macro definition.",
                                       TalDocument.MetalNamespace,
                                       MetalElement.DefineMacroAttributeName);
        throw new MetalException(message);
      }
      
      // Get the macro name (mandatory)
      this.MacroName = this.GetAttribute(MetalElement.DefineMacroAttributeName, TalDocument.MetalNamespace);
      
      // Get the TALES path to another macro that this macro extends (optional)
      if(this.HasAttribute(MetalElement.ExtendMacroAttributeName, TalDocument.MetalNamespace))
      {
        this.ExtendMacroPath = new TalesPath(this.GetAttribute(MetalElement.ExtendMacroAttributeName,
                                                               TalDocument.MetalNamespace));
      }
      else
      {
        this.ExtendMacroPath = null;
      }
      
      // Get the slots defined within this macro (might be none)
      this.Slots = DiscoverSlots();
    }

    /// <summary>
    /// <para>Overloaded.  Discovers METAL slot definitions within this <see cref="MetalMacro"/> node instance.</para>
    /// </summary>
    /// <returns>
    /// A disctionary of <see cref="System.String"/> and <see cref="MetalSlot"/>
    /// </returns>
    protected Dictionary<string, MetalSlot> DiscoverSlots()
    {
      Dictionary<string, MetalSlot> output = new Dictionary<string, MetalSlot>();
      
      // Discover MetalSlots within the macro node.
      this.DiscoverSlots(this, ref output);
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Discovers METAL slot definitions within a <see cref="XmlElement"/> and its children.
    /// This method will recurse into itself where appropriate.
    /// </para>
    /// </summary>
    /// <param name="currentNode">
    /// A <see cref="XmlElement"/>
    /// </param>
    /// <param name="output">
    /// A dictionary of <see cref="System.String"/> and <see cref="MetalSlot"/>
    /// </param>
    protected void DiscoverSlots(XmlElement currentNode, ref Dictionary<string, MetalSlot> output)
    {
      if(currentNode == null)
      {
        throw new ArgumentNullException("currentNode");
      }
      else if(output == null)
      {
        throw new ArgumentNullException("output");
      }
      
      /* If the current node is a MetalSlot then we probably want to add it to the output collection.  The only
       * possible hiccup could be if we encounter the same slot twice within a macro (slot names must be unique within
       * a macro).
       */
      if(currentNode is MetalSlot)
      {
        MetalSlot slot = (MetalSlot) currentNode;
        
        if(output.ContainsKey(slot.SlotName))
        {
          throw new MetalException(String.Format("Duplicate definition for METAL slot '{0}'", slot.SlotName));
        }
        
        output.Add(slot.SlotName, slot);
      }
      
      /* Recurse into ourself for every XmlElement contained within the current element in order to discover
       * more slots.
       * 
       * Another workaround for the 'foreach' bug - there's a danger that if I use foreach here I will end up
       */
      for(int i = 0; i < currentNode.ChildNodes.Count; i++)
      {
        XmlNode node = currentNode.ChildNodes[i];
        
        if(node is XmlElement)
        {
          DiscoverSlots((XmlElement) node, ref output);
        }
      }
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
    public MetalMacro(string prefix,
                      string localName,
                      string namespaceURI,
                      XmlDocument document) : base(prefix, localName, namespaceURI, document)
    {
      this.Slots = new Dictionary<string, MetalSlot>();
      this.MacroName = null;
      this.ExtendMacro = null;
      this.ExtendMacroPath = null;
    }
    
    /// <summary>
    /// <para>Serves as a copy-constructor for an <see cref="XmlElement"/> node.</para>
    /// </summary>
    /// <param name="elementToClone">
    /// A <see cref="XmlElement"/>
    /// </param>
    public MetalMacro(XmlElement elementToClone) : this(elementToClone.Prefix,
                                                        elementToClone.LocalName,
                                                        elementToClone.NamespaceURI,
                                                        elementToClone.OwnerDocument)
    {
      this.CloneFrom(elementToClone);
    }
    
    #endregion
	}
}

