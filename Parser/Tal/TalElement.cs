//  
//  TalNode.cs
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

namespace CraigFowler.Web.ZPT.Tal
{
  public class TalElement : XmlElement, IHasTalesContext
  {
    #region fields
    
    private TalesContext context;
    
    #endregion
    
    #region properties
    
    public TalesContext TalesContext
    {
      get {
        return context;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException("value");
        }
        
        context = value;
      }
    }
    
    #endregion
    
    #region methods
    
    public override void WriteTo (XmlWriter w)
    {
      // TODO: Write this TAL node to the given writer
      throw new NotImplementedException();
    }
    
    private TalesContext GetContextFromParentNode(XmlElement node)
    {
      XmlElement currentNode = node;
      
      while(!(currentNode is IHasTalesContext))
      {
        currentNode = (XmlElement) currentNode.ParentNode;
      }
      
      return ((IHasTalesContext) currentNode).TalesContext;
    }
    
    #endregion

    #region constructors
    
    public TalElement(string prefix,
                      string localName,
                      string namespaceURI,
                      TalDocument document) : base(prefix, localName, namespaceURI, document)
    {
      // TODO: Write this default constructor
      throw new NotImplementedException();
    }
    
    protected TalElement(XmlElement nodeToCopy,
                         TalDocument document) : this(nodeToCopy.Prefix,
                                                      nodeToCopy.LocalName,
                                                      nodeToCopy.NamespaceURI,
                                                      document)
    {
      this.TalesContext = GetContextFromParentNode(nodeToCopy).CreateChildContext();
      
      // TODO: Write this copy-constructor
      throw new NotImplementedException();
    }
    
    #endregion
    
    #region static methods
    
    public static TalElement CreateFromXmlNode(XmlElement nodeToCopy, TalDocument document)
    {
      return new TalElement(nodeToCopy, document);
    }
    
    #endregion
  }
}
