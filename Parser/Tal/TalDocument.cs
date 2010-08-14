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
  public class TalDocument : XmlDocument, IHasTalesContext
  {
    #region constants
    
    public const string TalNamespace = "http://xml.zope.org/namespaces/tal";
    
    #endregion
    
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
      // TODO: Write this TAL document to the given writer
      throw new NotImplementedException();
    }
    
    public string Render()
    {
      StringBuilder output = new StringBuilder();
      
      using(TextWriter writer = new StringWriter(output))
      {
        using (XmlWriter xmlWriter = new XmlTextWriter(writer))
        {
          this.WriteTo(xmlWriter);
        }
      }
      
      return output.ToString();
    }
    
    private TalElement CreateTalElement(XmlElement elementToCopy)
    {
      return TalElement.CreateFromXmlNode(elementToCopy, this);
    }
    
    #endregion
    
    #region constructor
    
    public TalDocument() : base()
    {
      this.TalesContext = new TalesContext();
    }
    
    protected TalDocument(XmlDocument document) : this()
    {
      // TODO: Create this copy-constructor
      throw new NotImplementedException();
    }
    
    #endregion
    
    #region static methods
    
    public TalDocument ParseXmlDocument(XmlDocument document)
    {
      return new TalDocument(document);
    }
    
    #endregion
  }
}
