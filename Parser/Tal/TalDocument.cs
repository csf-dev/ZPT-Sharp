//  
//  TalDocument.cs
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
using CraigFowler.Web.ZPT.Tales;
using System.IO;
using System.Text;

namespace CraigFowler.Web.ZPT.Tal
{
  /// <summary>
  /// <para>
  /// A ZPT template document.  This is a <see cref="XmlDocument"/> that contains ZPT-specific logic to assist in
  /// rendering the final output.
  /// </para>
  /// </summary>
  public class TalDocument : XmlDocument, ITalElement
  {
    #region constants
    
    /// <summary>
    /// <para>Read-only.  Constant represents the registered XML namespace for TAL.</para>
    /// </summary>
    public const string TalNamespace = "http://xml.zope.org/namespaces/tal";
    
    /// <summary>
    /// <para>Read-only.  Constant represents the registered XML namespace for METAL.</para>
    /// </summary>
    public const string MetalNamespace = "http://xml.zope.org/namespaces/metal";
    
		/// <summary>
		/// <para>Read-only.  Constant value gets the XML namespace for the 'xmlns' attribute.</para>
		/// </summary>
		public const string XmlnsNamespace = "http://www.w3.org/2000/xmlns/";
		
    #endregion
    
    #region fields
    
    private TalesContext context;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Gets and sets the <see cref="TalesContext"/> for this TAL document.</para>
    /// </summary>
    public TalesContext TalesContext
    {
      get {
        return context;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        
        context = value;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Overloaded.  Convenience method that renders the output of this document to a string.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/>, the rendered output document.
    /// </returns>
    public string Render()
    {
      StringBuilder output = new StringBuilder();
      
      using(TextWriter writer = new StringWriter(output))
      {
        using (XmlWriter xmlWriter = new XmlTextWriter(writer))
        {
					xmlWriter.Settings.NewLineChars = "\n";
					xmlWriter.Settings.IndentChars = "  ";
					xmlWriter.Settings.Indent = true;
					
          this.Render(xmlWriter);
        }
      }
      
      return output.ToString();
    }
    
    /// <summary>
    /// <para>Overloaded.  Renders the output of this document to the given <see cref="XmlWriter"/>.</para>
    /// </summary>
    /// <param name="writer">
    /// An <see cref="XmlWriter"/> to write the rendere output of this document to.
    /// </param>
    public void Render(XmlWriter writer)
    {
      foreach(XmlNode node in this.ChildNodes)
      {
        if(node is ITalElement)
        {
          ((ITalElement) node).Render(writer);
        }
        else
        {
          node.WriteTo(writer);
					writer.WriteWhitespace(writer.Settings.NewLineChars);
        }
      }
    }
    
    /// <summary>
    /// <para>Gets the parent TALES context.  Always returns null, since the root/document element has no parent.</para>
    /// </summary>
    /// <returns>
    /// Always returns null.
    /// </returns>
    public TalesContext GetParentTalesContext()
    {
      return null;
    }
    
		/// <summary>
		/// <para>
		/// Overridden.  Reads a node from an <see cref="XmlReader"/> and returns either a simple <see cref="XmlNode"/> or
		/// - if the node type indicated is appropriate, a <see cref="TalElement"/> node.
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
				output = new TalElement((XmlElement) output);
			}
			
			return output;
		}
		
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>
    /// Initialises this instance with a new/empty <see cref="TalesContext"/> that also prevents resolving of DTDs
    /// in order to improve performance.
    /// </para>
    /// </summary>
    public TalDocument() : this(false) {}
		
		/// <summary>
    /// <para>
    /// Initialises this instance with a new/empty <see cref="TalesContext"/> with a given setting for whether DTDs
    /// will be resolved or not.
    /// </para>
		/// </summary>
		/// <param name="useDTDResolver">
		/// A <see cref="System.Boolean"/> that determines whether DTDs are resolved or not.  Setting this to false will
		/// greatly improve performance.
		/// </param>
		public TalDocument (bool useDTDResolver) : base()
		{
      this.TalesContext = new TalesContext();
			
			if(!useDTDResolver)
			{
				this.XmlResolver = null;
			}
		}
    
    #endregion
  }
}
