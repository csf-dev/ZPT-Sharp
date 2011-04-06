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
    /// <para>
    /// Read-only.  Gets a cached dictionary of <see cref="System.String"/> and <see cref="MetalElement"/> that
    /// contains the METAL slots found within this macro.
    /// </para>
    /// </summary>
    protected Dictionary<string, MetalElement> CachedSlots  {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>Read-only.  Gets a cached <see cref="MetalMacro"/> that this instance extends.</para>
    /// </summary>
    protected MetalMacro CachedExtendMacro  {
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
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Overloaded. Retrieves the available METAL macro slots available within this macro.</para>
    /// </summary>
    /// <returns>
    /// A dictionary of <see cref="System.String"/> and <see cref="MetalElement"/>
    /// </returns>
    public Dictionary<string, MetalElement> GetAvailableSlots()
    {
      return this.GetAvailableSlots(false);
    }
    
    /// <summary>
    /// <para>Overloaded. Retrieves the available METAL macro slots available within this macro.</para>
    /// </summary>
    /// <param name="bypassCache">
    /// A <see cref="System.Boolean"/>
    /// </param>
    /// <returns>
    /// A dictionary of <see cref="System.String"/> and <see cref="MetalElement"/>
    /// </returns>
    public Dictionary<string, MetalElement> GetAvailableSlots(bool bypassCache)
    {
      Dictionary<string, MetalElement> output;
      
      if(this.CachedSlots != null && !bypassCache)
      {
        output = this.CachedSlots;
      }
      else
      {
        output = this.DiscoverSlots();
        
        if(!bypassCache)
        {
          this.CachedSlots = output;
        }
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Gets a reference to a <see cref="MetalMacro"/> that this macro instance should extend.</para>
    /// </summary>
    /// <param name="context">
    /// A <see cref="Tales.TalesContext"/>
    /// </param>
    /// <returns>
    /// A <see cref="MetalMacro"/>, which might be null if this instance does not extend a macro.
    /// </returns>
    public MetalMacro GetExtendMacro(Tales.TalesContext context)
    {
      // TODO: Write this method
      throw new NotImplementedException();
      
//        this.ExtendMacroPath = new TalesPath(this.GetAttribute(MetalElement.ExtendMacroAttributeName,
//                                                               TalDocument.MetalNamespace));
      
    }
    
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
      
      if(!this.HasAttribute(MetalElement.DefineMacroAttributeName, TalDocument.MetalNamespace))
      {
        string message = String.Format("The current element does not contain a metal:{0} attribute.  " +
                                       "It is not a METAL macro definition.",
                                       MetalElement.DefineMacroAttributeName);
        throw new MetalException(message);
      }
      
      this.MacroName = this.GetAttribute(MetalElement.DefineMacroAttributeName, TalDocument.MetalNamespace);
    }

    /// <summary>
    /// <para>Overloaded.  Discovers METAL slot definitions within this <see cref="MetalMacro"/> node instance.</para>
    /// </summary>
    /// <returns>
    /// A disctionary of <see cref="System.String"/> and <see cref="MetalElement"/>
    /// </returns>
    protected Dictionary<string, MetalElement> DiscoverSlots()
    {
      Dictionary<string, MetalElement> output = new Dictionary<string, MetalElement>();
      XmlNamespaceManager namespaceManager = new XmlNamespaceManager(this.OwnerDocument.NameTable);
      
      namespaceManager.AddNamespace("metal", TalDocument.MetalNamespace);
      
      foreach(XmlNode node in this.SelectNodes(String.Format("descendant::*[@metal:{0}]",
                                                             DefineSlotAttributeName),
                                               namespaceManager))
      {
        if(node is MetalElement)
        {
          MetalElement element = (MetalElement) node;
          string name = element.GetAttribute(DefineSlotAttributeName, TalDocument.MetalNamespace);
          
          if(String.IsNullOrEmpty(name))
          {
            throw new InvalidOperationException("Null or empty define-slot name.");
          }
          
          output.Add(name, element);
        }
        else
        {
          string message = "A non-MetalElement node was found whilst looking for define-slot definitions";
          InvalidOperationException ex = new InvalidOperationException(message);
          ex.Data["Node"] = node;
          ex.Data["Current element"] = this;
          throw ex;
        }
      }
      
      return output;
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
      this.CachedSlots = null;
      this.CachedExtendMacro = null;
    }
    
    /// <summary>
    /// <para>Serves as a copy-constructor for an <see cref="XmlElement"/> node.</para>
    /// </summary>
    /// <param name="elementToClone">
    /// A <see cref="XmlElement"/>
    /// </param>
    public MetalMacro(XmlElement elementToClone) : this(elementToClone, elementToClone.OwnerDocument) {}
    
    /// <summary>
    /// <para>Serves as a copy-constructor for an <see cref="XmlElement"/> node.</para>
    /// </summary>
    /// <param name="elementToClone">
    /// A <see cref="XmlElement"/>
    /// </param>
    /// <param name="ownerDocument">
    /// An <see cref="XmlDocument"/>
    /// </param>
    public MetalMacro(XmlElement elementToClone, XmlDocument ownerDocument) : this(elementToClone.Prefix,
                                                                                   elementToClone.LocalName,
                                                                                   elementToClone.NamespaceURI,
                                                                                   ownerDocument)
    {
      this.CloneFrom(elementToClone);
    }
    
    #endregion
	}
}

