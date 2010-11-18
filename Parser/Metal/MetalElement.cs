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
    protected const string DefineMacroAttributeName          = "define-macro";
    
    /// <summary>
    /// <para>Read-only constant gets the XML element name of a METAL macro-extension attribute.</para>
    /// </summary>
    protected const string ExtendMacroAttributeName          = "extend-macro";
    
    /// <summary>
    /// <para>Read-only constant gets the XML element name of a METAL macro-invocation attribute.</para>
    /// </summary>
    protected const string UseMacroAttributeName             = "use-macro";
    
    /// <summary>
    /// <para>Read-only constant gets the XML element name of a METAL slot-definition attribute.</para>
    /// </summary>
    protected const string DefineSlotAttributeName           = "define-slot";
    
    /// <summary>
    /// <para>Read-only constant gets the XML element name of a METAL slot-usage attribute.</para>
    /// </summary>
    protected const string FillSlotAttributeName             = "fill-slot";
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the name of the <see cref="MetalSlot"/> that this element fills.</para>
    /// </summary>
    protected string FillSlotName
    {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>Read-only.  Gets a reference to the <see cref="MetalSlot"/> that this element fills.</para>
    /// </summary>
    public MetalSlot FillSlot
    {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>Gets and sets a reference to the parent <see cref="MetalMacro"/> that this slot resides within.</para>
    /// </summary>
    public MetalMacro ParentMacro {
      get;
      protected set;
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
          MetalElement element = MetalElementFactory((XmlElement) node);
          element.ParentMacro = this.ParentMacro;
          
          this.AppendChild(element);
        }
        else
        {
          this.AppendChild(node.CloneNode(true));
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
      return this.GetAttribute(UseMacroAttributeName, TalDocument.MetalNamespace);
    }
    
    /// <summary>
    /// <para>Gets a <see cref="MetalMacro"/> instance for which to use with this element.</para>
    /// </summary>
    /// <param name="context">
    /// A <see cref="Tales.TalesContext"/>
    /// </param>
    /// <returns>
    /// A <see cref="MetalMacro"/>
    /// </returns>
    public MetalMacro GetUseMacro(Tales.TalesContext context)
    {
      MetalMacro output = null;
      string path = this.GetUseMacroPath();
      
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }
      
      if(path != null)
      {
        TalesExpression expression = context.CreateExpression(path);
        object expressionValue = expression.GetValue();
        
        output = expressionValue as MetalMacro;
        
        if(output == null)
        {
          MetalException ex = new MetalException("Error retrieving macro whilst parsing 'use-macro' directive.");
          ex.Data["Path"] = path;
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
    public MetalElement(string prefix,
                      	string localName,
                      	string namespaceURI,
                      	XmlDocument document) : base(prefix, localName, namespaceURI, document)
    {
      this.FillSlotName = null;
      this.FillSlot = null;
      this.ParentMacro = null;
    }
		
		/// <summary>
		/// <para>Serves as a copy-constructor for an <see cref="XmlElement"/> node.</para>
		/// </summary>
		/// <param name="elementToClone">
		/// A <see cref="XmlElement"/>
		/// </param>
		public MetalElement(XmlElement elementToClone) : this(elementToClone.Prefix,
                                                          elementToClone.LocalName,
                                                          elementToClone.NamespaceURI,
                                                          elementToClone.OwnerDocument)
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
        IMetalDocument metalDocument = elementToClone.OwnerDocument as IMetalDocument;
        MetalMacro macro = new MetalMacro(elementToClone);
        
        metalDocument.Macros[macro.MacroName] = macro;
        output = macro;
      }
      else if(elementToClone.HasAttribute(DefineSlotAttributeName, TalDocument.MetalNamespace))
      {
        MetalSlot slot = new MetalSlot(elementToClone);
        
        output = slot;
      }
      else
      {
        output = new MetalElement(elementToClone);
      }
      
      return output;
    }
    
    #endregion
	}
}

