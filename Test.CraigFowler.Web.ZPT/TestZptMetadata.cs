//  
//  TestZptMetadata.cs
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
using System.Configuration;
using System.IO;
using CraigFowler.Web.ZPT;
using CraigFowler.Web.ZPT.Mocks;
using NUnit.Framework;

namespace Test.CraigFowler.Web.ZPT
{
	[TestFixture]
	public class TestZptMetadata
	{
		#region constants
		
		private const string TEST_XML = @"<zptMetadata xmlns=""http://xml.craigfowler.me.uk/namespaces/ZptMetadata"">
  <documentType>Tal</documentType>
  <className>CraigFowler.Web.ZPT.ZptDocument</className>
</zptMetadata>";
		
		private const string EXPECTED_TEST_DOCUMENT_RENDERING = @"<html xmlns=""http://www.w3.org/1999/xhtml"">
  <head>
    <title>Foo</title>
  </head>
  <body>
    <div>Bar</div>
  </body>
</html>";
		
		#endregion
    
    #region set up
    
    [SetUp]
    public void SetUp()
    {
      ZptMetadata.UnregisterDocumentClass(typeof(ZptChildDocument));
    }
    
    #endregion
		
		#region tests
		
		[Test]
		public void TestCreateMetadataFromXml()
		{
			ZptMetadata output;
			
			using(TextReader reader = new StringReader(TEST_XML))
			{
				output = ZptMetadata.CreateMetadataFromXml(reader);
			}
			
			Assert.AreEqual(ZptDocumentType.Tal, output.DocumentType, "Document type");
			Assert.AreEqual(typeof(ZptDocument), output.DocumentClass, "Document class");
		}
		
		[Test]
		public void TestWriteToXml()
		{
			ZptMetadata metadata = new ZptMetadata();
			metadata.DocumentFilePath = "/home/test/A subdirectory/testFile.pt";
			metadata.DocumentType = ZptDocumentType.Tal;
			
			Assert.AreEqual(TEST_XML, metadata.WriteToXml(), "Correct XML rendering");
		}
		
		[Test]
		[Category("Integration")]
		public void TestRegisterDocumentClass()
		{
			ZptMetadata metadata;
			IZptDocument document;
			
			ZptMetadata.RegisterDocumentClass(typeof(ZptChildDocument));
			
			metadata = new ZptMetadata();
			metadata.DocumentClassName = "CraigFowler.Web.ZPT.Mocks.ZptChildDocument";
			
			document = ZptDocument.DocumentFactory(metadata);
			Assert.IsInstanceOfType(typeof(ZptChildDocument), document, "Document is expected type");
		}
    
    [Test]
    [Category("Integration")]
    public void TestDocumentClassNameWithoutRegistration()
    {
      ZptMetadata metadata;
      IZptDocument document;
      
      metadata = new ZptMetadata();
      metadata.DocumentClassName = "CraigFowler.Web.ZPT.Mocks.ZptChildDocument";
      
      document = ZptDocument.DocumentFactory(metadata);
      Assert.IsInstanceOfType(typeof(ZptChildDocument), document, "Document is expected type");
    }
		
		[Test]
		public void TestGetMetadata()
		{
			ZptMetadata metadata;
			
			metadata = ZptMetadata.GetMetadata(Path.Combine(ConfigurationManager.AppSettings["test-data-path"],
			                                                "simple-tal-document.pt"));
			Assert.AreEqual(ZptDocumentType.Tal, metadata.DocumentType, "Correct document type after reading metadata");
		}
		
		[Test]
		public void TestLoadDocument()
		{
			ZptMetadata metadata;
			ITemplateDocument document;
			
			metadata = ZptMetadata.GetMetadata(Path.Combine(ConfigurationManager.AppSettings["test-data-path"],
			                                                "simple-tal-document.pt"));
			document = metadata.LoadDocument();
			document.TalesContext.AddDefinition("test", new MockObject());
			Assert.AreEqual(EXPECTED_TEST_DOCUMENT_RENDERING, document.Render(), "Correct rendering of loaded document");
		}
		
    [Test]
    public void TestImplementsRequiredInterface()
    {
      Assert.IsTrue(ZptMetadata.ImplementsRequiredInterface(typeof(ZptDocument)),
                    "ZptDocument implements the required interface");
      Assert.IsFalse(ZptMetadata.ImplementsRequiredInterface(typeof(System.String)),
                    "System.String does not implement the required interface");
    }
    
		#endregion
	}
}

