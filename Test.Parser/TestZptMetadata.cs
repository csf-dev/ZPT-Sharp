using System;
using System.IO;
using CraigFowler.Web.ZPT;
using CraigFowler.Web.ZPT.Mocks;
using NUnit.Framework;
using System.Configuration;

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
			
			Assert.IsTrue(ZptMetadata.RegisterDocumentClass(typeof(ZptChildDocument)), "Register the new document type");
			
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
		
		#endregion
	}
}

