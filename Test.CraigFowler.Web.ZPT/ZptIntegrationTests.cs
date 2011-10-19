//  
//  ZptIntegrationTests.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2011 Craig Fowler
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
using NUnit.Framework;
using CraigFowler.Web.ZPT;
using System.Configuration;
using System.IO;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Tales.Exceptions;
using System.Xml;

namespace Test.CraigFowler.Web.ZPT
{
  [TestFixture]
  public class ZptIntegrationTests
  {
    #region tests
    
    [Test]
    [ExpectedException(ExceptionType = typeof(ZptDocumentParsingException))]
    public void TestParsingErrorInZptDocumentCollection()
    {
      ZptDocumentCollection testDocuments = this.GetInvalidDocumentDirectory();
      ITemplateDocument templateDoc;
      ZptDocument typedPage;
      object page;
      
      page = testDocuments.RetrieveItem("root/valid-page");
      typedPage = page as ZptDocument;
      Assert.IsNotNull(typedPage, "Page is correct type");
      
      templateDoc = typedPage.GetTemplateDocument();
      templateDoc.TalesContext.AddDefinition("documents", testDocuments);
      
      try
      {
        typedPage.GetTemplateDocument().Render();
      }
      catch(ZptDocumentParsingException ex)
      {
        Assert.IsNotNull(ex.InnerException, "Inner exception not null");
        Assert.IsInstanceOfType(typeof(XmlException), ex.InnerException, "Inner exception is correct type");
        throw;
      }
    }
    
    #endregion
    
    #region helper methods
    
    private ZptDocumentCollection GetInvalidDocumentDirectory()
    {
      string
        basePath = ConfigurationManager.AppSettings["test-data-path"],
        documentRootSubdirectory = "invalid-pages";
      
      return ZptDocumentCollection.CreateFromFilesystem(Path.Combine(basePath, documentRootSubdirectory));
    }
    
    #endregion
  }
}

