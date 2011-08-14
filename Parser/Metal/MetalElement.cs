//  
//  MetalElement.cs
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
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Metal.Exceptions;
using System.Collections.Generic;

namespace CraigFowler.Web.ZPT.Metal
{
	/// <summary>
  /// <para>Represents an <see cref="XmlElement"/> within a <see cref="MetalDocument"/>.</para>
	/// </summary>
	public class MetalElement : TalElement
	{
    #region constants
    
    /// <summary>
    /// <para>Read-only constant gets the XML element name of a METAL macro-definition attribute.</para>
    /// </summary>
    protected const string DefineMacroAttributeName         = "define-macro";
    
    /// <summary>
    /// <para>Read-only constant gets the XML element name of a METAL macro-extension attribute.</para>
    /// </summary>
    protected const string ExtendMacroAttributeName         = "extend-macro";
    
    /// <summary>
    /// <para>Read-only constant gets the XML element name of a METAL macro-invocation attribute.</para>
    /// </summary>
    protected const string UseMacroAttributeName            = "use-macro";
    
    /// <summary>
    /// <para>Read-only constant gets the XML element name of a METAL slot-definition attribute.</para>
    /// </summary>
    protected const string DefineSlotAttributeName          = "define-slot";
    
    /// <summary>
    /// <para>Read-only constant gets the XML element name of a METAL slot-usage attribute.</para>
    /// </summary>
    protected const string FillSlotAttributeName            = "fill-slot";
    
    private const string MACRO_NAME_TAL_VARIABLE            = "macroname";
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a reference to a <see cref="MetalMacro"/> that <see cref="GetUseMacro(Tales.TalesContext)"/>
    /// has cached (to prevent repeated resolution of expressions to fetch a macro instance).
    /// </para>
    /// </summary>
    protected MetalMacro CachedUseMacro
    {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets the <see cref="TalesContext"/> for performing METAL-related actions.  This is equivalent to
    /// the TALES context of the owner document (since TAL-style define directives are not available for METAL).
    /// </para>
    /// </summary>
    protected TalesContext MetalContext
    {
      get {
        return this.OwnerTemplateDocument.TalesContext;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>
    /// Overridden.  Populates this node with the <see cref="XmlNode"/> instances (child nodes) from the
    /// <paramref name="elementToClone"/>.
    /// </para>
    /// </summary>
    /// <param name="elementToClone">
    /// A <see cref="XmlElement"/>
    /// </param>
    protected override void CloneChildNodesFrom (XmlElement elementToClone)
    {
      if(elementToClone == null)
      {
        throw new ArgumentNullException("elementToClone");
      }
      
      /* For some strange reason I can't use a foreach here.  If I do it ends up going into a crazy endless loop
       * when it hits certain node types.
       */
      for(int i = 0; i < elementToClone.ChildNodes.Count ; i++)
      {
        XmlNode node = elementToClone.ChildNodes[i];
        
        if(node.NodeType == XmlNodeType.Element)
        {
          MetalElement element = MetalElementFactory((XmlElement) node, this.OwnerDocument);
          
          this.AppendChild(element);
        }
        else
        {
          this.AppendChild(this.OwnerDocument.ImportNode(node, true));
        }
      }
    }
    
    /// <summary>
    /// <para>
    /// Gets the path to a <see cref="MetalMacro"/> that this element uses, if applicable.
    /// </para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    protected string GetUseMacroPath()
    {
      string output = this.GetAttribute(UseMacroAttributeName, TalDocument.MetalNamespace);
      
      if(output == String.Empty)
      {
        output = null;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Overloaded.  Gets a <see cref="MetalMacro"/> instance for which to use with this element.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method provides caching via the <see cref="CachedUseMacro"/> property.  After the initial call to this
    /// method, subsequent calls will return the cached instance rather than re-evaluating the use-macro expression
    /// repeatedly.
    /// </para>
    /// </remarks>
    /// <param name="context">
    /// A <see cref="Tales.TalesContext"/>
    /// </param>
    /// <returns>
    /// A <see cref="MetalMacro"/>
    /// </returns>
    public MetalMacro GetUseMacro(Tales.TalesContext context)
    {
      return this.GetUseMacro(context, false);
    }
    
    /// <summary>
    /// <para>Overloaded.  Gets a <see cref="MetalMacro"/> instance for which to use with this element.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method provides caching via the <see cref="CachedUseMacro"/> property.  After the initial call to this
    /// method, subsequent calls will return the cached instance rather than re-evaluating the use-macro expression
    /// repeatedly.  This overload provides a mechanism to bypass that cache, via the <paramref name="bypassCache"/>
    /// parameter.
    /// </para>
    /// </remarks>
    /// <param name="context">
    /// A <see cref="Tales.TalesContext"/> to use for the discovery of the macro.
    /// </param>
    /// <param name="bypassCache">
    /// A <see cref="System.Boolean"/> that determines whether or not <see cref="CachedUseMacro"/> is used or not
    /// (if present)
    /// </param>
    /// <returns>
    /// A <see cref="MetalMacro"/>
    /// </returns>
    public virtual MetalMacro GetUseMacro(Tales.TalesContext context, bool bypassCache)
    {
      MetalMacro output = null;
      string path = this.GetUseMacroPath();
      
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }
      
      if(this.CachedUseMacro != null && !bypassCache)
      {
        output = this.CachedUseMacro;
      }
      else if(path != null)
      {
        TalesExpression expression = context.CreateExpression(path);
        object expressionValue = expression.GetValue();
        
        output = expressionValue as MetalMacro;
        
        if(output == null)
        {
          MetalException ex = new MetalException("Error retrieving macro; a 'use-macro' path expression is present " +
                                                 "but it does not evaluate to a METAL macro.");
          ex.Data["Path"] = path;
          ex.Data["Expression value"] = expressionValue;
          ex.Data["Expression value type"] = (expressionValue != null)? expressionValue.GetType().FullName : "null";
          throw ex;
        }
        
        if(!bypassCache)
        {
          this.CachedUseMacro = output;
        }
      }
      
      if(output != null)
      {
        output = output.Expand(false, context);
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Gets a collection of the child elements of the current element that contain METAL 'fill-slot' attributes,
    /// indexed by the value of the name of the slot that they fill.
    /// </para>
    /// </summary>
    /// <returns>
    /// <para>
    /// A dictionary of <see cref="System.String"/> and <see cref="MetalElement"/>.
    /// </para>
    /// <para>
    /// If <see cref="GetUseMacro(Tales.TalesContext)"/> returns a non-null <see cref="MetalMacro"/> instance then this
    /// method will always return a non-null collection, although it may be empty (if the use-macro directive contains
    /// no fill-slot directives).
    /// </para>
    /// </returns>
    /// <exception cref="MetalException">
    /// If <see cref="GetUseMacro(Tales.TalesContext)"/> returns a null reference (indicating that this element
    /// instance does not contain a 'use-macro' directive).
    /// </exception>
    public Dictionary<string,MetalElement> GetFilledSlots()
    {
      Dictionary<string,MetalElement> output = new Dictionary<string, MetalElement>();
      XmlNamespaceManager namespaceManager = new XmlNamespaceManager(this.OwnerDocument.NameTable);
      
      namespaceManager.AddNamespace("metal", TalDocument.MetalNamespace);
      
      foreach(XmlNode node in this.SelectNodes(String.Format("descendant::*[@metal:{0}]",
                                                             FillSlotAttributeName),
                                               namespaceManager))
      {
        if(node is MetalElement)
        {
          MetalElement element = (MetalElement) node;
          string name = element.GetAttribute(FillSlotAttributeName, TalDocument.MetalNamespace);
          
          if(String.IsNullOrEmpty(name))
          {
            throw new InvalidOperationException("Null or empty fill-slot name.");
          }
          
          output.Add(name, element);
        }
        else
        {
          string message = "A non-MetalElement node was found whilst looking for fill-slot definitions";
          InvalidOperationException ex = new InvalidOperationException(message);
          ex.Data["Node"] = node;
          ex.Data["Current element"] = this;
          throw ex;
        }
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Overridden.  Renders this node to the output stream.</para>
    /// </summary>
    /// <param name="output">
    /// A <see cref="TalOutput"/>
    /// </param>
    public override void Render (TalOutput output)
    {
      MetalMacro macroElement = this.GetUseMacro(this.MetalContext);
      
      if(macroElement != null)
      {
        this.SpliceWith(macroElement, output);
      }
      else
      {
        base.Render (output);
      }
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Splices the given <paramref name="element"/> as a replacement for this element node.
    /// </para>
    /// </summary>
    /// <param name="element">
    /// A <see cref="MetalElement"/>
    /// </param>
    /// <param name="output">
    /// A <see cref="TalOutput"/>
    /// </param>
    public void SpliceWith(MetalElement element, TalOutput output)
    {
      this.SpliceWith(element, output, null, false);
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Splices the given <paramref name="element"/> as a replacement for this element node.
    /// </para>
    /// </summary>
    /// <param name="element">
    /// A <see cref="MetalElement"/>
    /// </param>
    /// <param name="output">
    /// A <see cref="TalOutput"/>
    /// </param>
    /// <param name="skipRendering">
    /// A <see cref="System.Boolean"/>
    /// </param>
    public void SpliceWith(MetalElement element, TalOutput output, bool skipRendering)
    {
      this.SpliceWith(element, output, null, skipRendering);
    }
    
    /// <summary>
    /// Overloaded.  Splices the given <paramref name="element"/> as a replacement for this element node, performing
    /// slot-replacements where appropriate.
    /// </summary>
    /// <param name="element">
    /// A <see cref="MetalElement"/>
    /// </param>
    /// <param name="output">
    /// A <see cref="TalOutput"/>
    /// </param>
    /// <param name="slotReplacements">
    /// A dictionary of <see cref="System.String"/> and <see cref="MetalElement"/>
    /// </param>
    /// <param name="skipRendering">
    /// A <see cref="System.Boolean"/> that determines whether or not to skip rendering of the newly-imported node.
    /// </param>
    public void SpliceWith(MetalElement element,
                           TalOutput output,
                           Dictionary<string, MetalElement> slotReplacements,
                           bool skipRendering)
    {
      MetalElement importedElement;
      bool performSlotReplacements;
      
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      else if(output == null)
      {
        throw new ArgumentNullException("output");
      }
      
      importedElement = (MetalElement) this.OwnerDocument.ImportNode(element, true);
      performSlotReplacements = (importedElement is MetalMacro);
      
      if(performSlotReplacements == false && slotReplacements != null)
      {
        throw new NotSupportedException("The element to splice in is not a METAL macro, but slot replacements " +
                                        "have been provided.  This is unsupported.");
      }
      else if(performSlotReplacements && slotReplacements == null)
      {
        throw new ArgumentNullException("slotReplacements",
                                        "Slot replacements may not be null when we are splicing with a METAL macro.");
      }
      else if(performSlotReplacements)
      {
        MetalMacro importedMacro = (MetalMacro) importedElement;
        Dictionary<string, MetalElement> slots;
        
        if(!importedMacro.IsExpanded)
        {
          throw new MetalException("METAL macro is not expanded.");
        }
        
        slots = importedMacro.GetAvailableSlots();
        
        foreach(string key in slotReplacements.Keys)
        {
          if(!slots.ContainsKey(key))
          {
#if DEBUG
            List<string> available = new List<string>();
            available.AddRange(slots.Keys);
            Console.Error.WriteLine("Available slots were {0}.", String.Join(", ", available.ToArray()));
            Console.Error.WriteLine();
            Console.Error.WriteLine(importedMacro.OuterXml);
            Console.Error.WriteLine();
            Console.Error.WriteLine(this.OuterXml);
            Console.Error.WriteLine();
#endif
            
            string message = String.Format("Attempt to fill slot named '{0}' but the macro used does not provide a " +
                                           "slot by that name.",
                                           key);
            
            MetalException ex = new MetalException(message);
            ex.Data["Slots available"] = slots;
            throw ex;
          }
          
          slots[key].SpliceWith(slotReplacements[key], output, true);
        }
        
        importedMacro.TalesContext.AddDefinition(MACRO_NAME_TAL_VARIABLE, importedMacro.MacroName);
      }
      
      // Replace the current node with the node from the 
      if(this.ParentNode == null)
      {
        this.OwnerDocument.RemoveAll();
        this.OwnerDocument.AppendChild(importedElement);
      }
      else
      {
        this.ParentNode.ReplaceChild(importedElement, this);
      }
      
      if(!skipRendering)
      {
        importedElement.Render(output);
      }
    }
    
    /// <summary>
    /// Overloaded.  Splices the given <paramref name="macro"/> as a replacement for this element node.
    /// </summary>
    /// <param name="macro">
    /// A <see cref="MetalMacro"/>
    /// </param>
    /// <param name="output">
    /// A <see cref="TalOutput"/>
    /// </param>
    public void SpliceWith(MetalMacro macro, TalOutput output)
    {
      this.SpliceWith(macro, output, this.GetFilledSlots(), false);
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
    /// An <see cref="XmlDocument"/>
    /// </param>
    public MetalElement(string prefix,
                      	string localName,
                      	string namespaceURI,
                      	XmlDocument document) : base(prefix, localName, namespaceURI, document) {}
		
		/// <summary>
		/// <para>Serves as a copy-constructor for an <see cref="XmlElement"/> node.</para>
		/// </summary>
		/// <param name="elementToClone">
		/// A <see cref="XmlElement"/>
		/// </param>
		public MetalElement(XmlElement elementToClone) : this(elementToClone, elementToClone.OwnerDocument) {}
    
    /// <summary>
    /// <para>Serves as a copy-constructor for an <see cref="XmlElement"/> node.</para>
    /// </summary>
    /// <param name="elementToClone">
    /// A <see cref="XmlElement"/>
    /// </param>
    /// <param name="ownerDocument">
    /// A <see cref="XmlDocument"/>
    /// </param>
    public MetalElement(XmlElement elementToClone, XmlDocument ownerDocument) : this(elementToClone.Prefix,
                                                                                     elementToClone.LocalName,
                                                                                     elementToClone.NamespaceURI,
                                                                                     ownerDocument)
    {
      this.CloneFrom(elementToClone);
    }
		
		#endregion
    
    #region static methods
    
    /// <summary>
    /// <para>Factory method creates a new class instance that implements <see cref="MetalElement"/>.</para>
    /// </summary>
    /// <param name="elementToClone">
    /// A <see cref="XmlElement"/>
    /// </param>
    /// <returns>
    /// A <see cref="MetalElement"/>
    /// </returns>
    public static MetalElement MetalElementFactory(XmlElement elementToClone)
    {
      return MetalElement.MetalElementFactory(elementToClone, elementToClone.OwnerDocument);
    }
    
    /// <summary>
    /// <para>Factory method creates a new class instance that implements <see cref="MetalElement"/>.</para>
    /// </summary>
    /// <param name="elementToClone">
    /// A <see cref="XmlElement"/>
    /// </param>
    /// <param name="ownerDocument">
    /// An <see cref="XmlDocument"/>
    /// </param>
    /// <returns>
    /// A <see cref="MetalElement"/>
    /// </returns>
    public static MetalElement MetalElementFactory(XmlElement elementToClone, XmlDocument ownerDocument)
    {
      MetalElement output;
      
      if(elementToClone == null)
      {
        throw new ArgumentNullException("elementToClone");
      }
      else if(!(elementToClone.OwnerDocument is IMetalDocument))
      {
        throw new ArgumentException("Owner document for the element to clone is not an IMetalDocument",
                                    "elementToClone");
      }
      
      if(elementToClone.HasAttribute(DefineMacroAttributeName, TalDocument.MetalNamespace))
      {
        IMetalDocument metalDocument = ownerDocument as IMetalDocument;
        MetalMacro macro = new MetalMacro(elementToClone, ownerDocument);
        
        if(ownerDocument != elementToClone.OwnerDocument ||
           !metalDocument.Macros.ContainsMacro(macro.MacroName))
        {
          metalDocument.Macros[macro.MacroName] = macro;
        }
        
        output = macro;
      }
      else
      {
        output = new MetalElement(elementToClone, ownerDocument);
        if(elementToClone is MetalElement)
        {
          output.CachedUseMacro = ((MetalElement) elementToClone).CachedUseMacro;
        }
      }
      
      return output;
    }

    
    #endregion
	}
}
