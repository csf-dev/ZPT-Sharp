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
using CraigFowler.Web.ZPT.Tales.Expressions;

namespace CraigFowler.Web.ZPT.Metal
{
	/// <summary>
	/// <para>Describes a macro that may appear within a METAL document.</para>
	/// </summary>
	public class MetalMacro : MetalElement
	{
    #region fields
    
    private bool hasCachedExtendMacro;
    
    #endregion
    
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
    /// <para>A cached <see cref="MetalMacro"/> that was created during the <see cref="Expand"/> process.</para>
    /// </summary>
    protected MetalMacro CachedExpandedMacro {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>Read-only.  Gets whether or not this macro instance represents an expanded macro or not.</para>
    /// </summary>
    public bool IsExpanded
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
    /// <para>Gets and sets a collection of the slots that this macro has inherited by way of macro extension.</para>
    /// </summary>
    public Dictionary<string,MetalElement> InheritedSlots
    {
      get;
      set;
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
      
      if(this.InheritedSlots != null)
      {
        foreach(string slotName in this.InheritedSlots.Keys)
        {
          if(!output.ContainsKey(slotName))
          {
            output.Add(slotName, this.InheritedSlots[slotName]);
          }
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
      if(!hasCachedExtendMacro)
      {
        MetalMacro macro;
        PathExpression extendPath = new PathExpression(this.GetAttribute(MetalElement.ExtendMacroAttributeName,
                                                                         TalDocument.MetalNamespace),
                                                       context);
        macro = extendPath.GetValue() as MetalMacro;
        
        if(macro != null)
        {
          this.CachedExtendMacro = macro.Expand(false, context);
        }
        hasCachedExtendMacro = true;
      }
      
      return this.CachedExtendMacro;
    }
    
    /// <summary>
    /// <para>
    /// Fully expands a METAL macro element by getting any macros that it extends and splicing them into this macro.
    /// The returned/aggregate macro contains all of the slots from all macros in the inheritance chain.
    /// </para>
    /// </summary>
    /// <param name="bypassCache">
    /// A <see cref="System.Boolean"/> that indicates whether or not to bypass the caching of the macro expansion
    /// process.
    /// </param>
    /// <param name="context">
    /// A <see cref="TalesContext"/> with which to use for locating the macro.
    /// </param>
    /// <returns>
    /// A <see cref="MetalMacro"/>
    /// </returns>
    public MetalMacro Expand(bool bypassCache, TalesContext context)
    {
      MetalMacro output;
      
      if(this.CachedExpandedMacro == null || bypassCache)
      {
        MetalMacro extendMacro = this.GetExtendMacro(context);
        
        /* If there is a macro to extend from then get the result of that extension, otherwise we already have a
         * fully expanded macro stored in this instance.  We expand the macro that we are extending from as well,
         * just in case it also extends another macro.
         */
        output = (extendMacro != null)? this.ExtendFrom(extendMacro) : this;
        output.IsExpanded = true;
        
        if(!bypassCache)
        {
          this.CachedExpandedMacro = output;
        }
      }
      else
      {
        output = this.CachedExpandedMacro;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Overridden.  Renders this node to the output stream.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// METAL macros that extend other macros should be expanded before they are rendered.  Otherwise they are rendered
    /// normally.
    /// </para>
    /// </remarks>
    /// <param name="output">
    /// A <see cref="TalOutput"/>
    /// </param>
    public override void Render (TalOutput output)
    {
      if(this.GetExtendMacro(this.MetalContext) != null)
      {
        this.Expand(false, this.MetalContext).Render(output);
      }
      else
      {
        base.Render(output);
      }
    }
    
    /// <summary>
    /// <para>
    /// Overridden, overloaded.  Gets a <see cref="MetalMacro"/> instance for which to use with this element.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is additionally capable of discovering and returning an <c>extend-macro</c> if appropriate.
    /// </para>
    /// </remarks>
    /// <param name="context">
    /// A <see cref="Tales.TalesContext"/>
    /// </param>
    /// <param name="bypassCache">
    /// A <see cref="System.Boolean"/>
    /// </param>
    /// <returns>
    /// A <see cref="MetalMacro"/>
    /// </returns>
    public override MetalMacro GetUseMacro(Tales.TalesContext context, bool bypassCache)
    {
      MetalMacro output, extendMacro, useMacro;
      
      extendMacro = this.GetExtendMacro(context);
      useMacro = base.GetUseMacro(context, bypassCache);
      
      if(extendMacro != null && extendMacro != this)
      {
        output = extendMacro;
      }
      else if(useMacro != null && useMacro != this)
      {
        output = useMacro;
      }
      else
      {
        output = null;
      }
      
      return output;
    }

    
    /// <summary>
    /// <para>
    /// Creates a new <see cref="MetalMacro"/> instance that is the result of an <c>extend-macro</c> directive.
    /// </para>
    /// </summary>
    /// <param name="extendMacro">
    /// A <see cref="MetalMacro"/> - the macro to extend from.
    /// </param>
    /// <returns>
    /// A <see cref="MetalMacro"/> that is the result of the extension.
    /// </returns>
    protected MetalMacro ExtendFrom(MetalMacro extendMacro)
    {
      MetalMacro output;
      Dictionary<string, MetalElement> availableSlots, filledSlots;
      
      if(extendMacro == null)
      {
        throw new ArgumentNullException("extendMacro");
      }
      else if(!extendMacro.IsExpanded)
      {
        throw new ArgumentException("Macro to extend from is not expanded.");
      }
      
      output = (MetalMacro) this.OwnerDocument.ImportNode(extendMacro, true);
      output.InheritedSlots = extendMacro.GetAvailableSlots();
      
      availableSlots = output.GetAvailableSlots();
      filledSlots = this.GetFilledSlots();
      
      foreach(string key in filledSlots.Keys)
      {
        if(!availableSlots.ContainsKey(key))
        {
          string message = String.Format("Attempt to fill slot named '{0}' but the macro used does not provide a " +
                                         "slot by that name.",
                                         key);
          
          MetalException ex = new MetalException(message);
          ex.Data["Slots available"] = availableSlots;
          throw ex;
        }
        
        availableSlots[key].ParentNode.ReplaceChild(filledSlots[key], availableSlots[key]);
      }
      
      return output;
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
      MetalMacro cloneMacro = elementToClone as MetalMacro;
      base.CloneFrom (elementToClone);
      
      if(!this.HasAttribute(MetalElement.DefineMacroAttributeName, TalDocument.MetalNamespace))
      {
        string message = String.Format("The current element does not contain a metal:{0} attribute.  " +
                                       "It is not a METAL macro definition.",
                                       MetalElement.DefineMacroAttributeName);
        throw new MetalException(message);
      }
      
      this.MacroName = this.GetAttribute(MetalElement.DefineMacroAttributeName, TalDocument.MetalNamespace);
      
      // If the element to clone is a macro already then clone a few of its important properties also
      if(cloneMacro != null)
      {
        Dictionary<string, MetalElement> filledSlots = this.GetFilledSlots();
        
        foreach(string name in cloneMacro.InheritedSlots.Keys)
        {
          if(filledSlots.ContainsKey(name))
          {
            this.InheritedSlots.Add(name, filledSlots[name]);
          }
          else
          {
            MetalElement imported = (MetalElement) this.OwnerDocument.ImportNode(cloneMacro.InheritedSlots[name],
                                                                                 true);
            this.InheritedSlots.Add(name, imported);
          }
        }
        
        this.IsExpanded = cloneMacro.IsExpanded;
      }
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
      
      foreach(XmlNode node in this.SelectNodes(String.Format("descendant::*[@metal:{0}] | descendant::metal:*[@{0}]",
                                                             DefineSlotAttributeName),
                                               namespaceManager))
      {
        AddSlot(node, ref output);
      }
      
      if(this.GetExtendMacro(this.MetalContext) != null)
      {
        foreach(XmlNode node in this.SelectNodes(String.Format("descendant::*[@metal:{0}] | descendant::metal:*[@{0}]",
                                                               FillSlotAttributeName),
                                                 namespaceManager))
        {
          AddSlot(node, ref output);
        }
      }
      
      
      return output;
    }
    
    /// <summary>
    /// <para>Adds a slot to an output list of available slots to fill.</para>
    /// </summary>
    /// <param name="node">
    /// An <see cref="XmlNode"/>
    /// </param>
    /// <param name="output">
    /// A dictionary of <see cref="System.String"/> and <see cref="MetalElement"/>.
    /// </param>
    protected void AddSlot(XmlNode node, ref Dictionary<string, MetalElement> output)
    {
      MetalElement element = node as MetalElement;
      string name = null;
      
      if(element == null)
      {
        string message = "A non-MetalElement node was found whilst looking for define-slot definitions";
        InvalidOperationException ex = new InvalidOperationException(message);
        ex.Data["Node"] = node;
        ex.Data["Current element"] = this;
        throw ex;
      }
      
      if(element.HasAttribute(DefineSlotAttributeName, TalDocument.MetalNamespace))
      {
        name = element.GetAttribute(DefineSlotAttributeName, TalDocument.MetalNamespace);
      }
      else if(this.GetExtendMacro(this.MetalContext) != null &&
              element.HasAttribute(FillSlotAttributeName, TalDocument.MetalNamespace))
      {
        name = element.GetAttribute(FillSlotAttributeName, TalDocument.MetalNamespace);
      }
      else if(element.NamespaceURI == TalDocument.MetalNamespace &&
              element.HasAttribute(DefineSlotAttributeName))
      {
        name = element.GetAttribute(DefineSlotAttributeName);
      }
      else if(this.GetExtendMacro(this.MetalContext) != null &&
              element.NamespaceURI == TalDocument.MetalNamespace &&
              element.HasAttribute(FillSlotAttributeName))
      {
        name = element.GetAttribute(FillSlotAttributeName);
      }
      
      if(String.IsNullOrEmpty(name))
      {
        throw new InvalidOperationException("Null or empty define-slot name.");
      }
      
      output.Add(name, element);
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
      this.CachedExpandedMacro = null;
      this.IsExpanded = false;
      this.MacroName = null;
      this.InheritedSlots = new Dictionary<string, MetalElement>();
      
      hasCachedExtendMacro = false;
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

