
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tal;
using System.IO;
using System.Configuration;
using System.Xml;
using CraigFowler.Web.ZPT.Mocks;

namespace Test.CraigFowler.Web.ZPT.Tal
{
  [TestFixture]
  public class TestTalDocument
  {
		#region constants
		
		private const string
			EXPECTED_RENDERING = @"<html xmlns=""http://www.w3.org/1999/xhtml"">
  <head>
    <title>Foo</title>
  </head>
  <body>
    <div>Bar</div>
  </body>
</html>",
			EXPECTED_RENDERING_FALSE = @"<html xmlns=""http://www.w3.org/1999/xhtml"">
  <head>
    <title>Foo</title>
  </head>
  <body>
    
  </body>
</html>",
			EXPECTED_CONTENT = @"<html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:tal=""http://xml.zope.org/namespaces/tal"">
  <head>
    <title>Foo</title>
  </head>
  <body>
    <div tal:condition=""bool"" tal:define=""bool test/BooleanValue"">Bar</div>
  </body>
</html>";
		
		#endregion
		
		#region tests
		
    [Test]
    [Description("Contains no asserts but ensures that the constructor works.")]
    public void TestConstructor()
    {
      TalDocument document = new TalDocument();
      document.Render();
    }
		
		[Test]
		[Category("Integration")]
		public void TestReadNode()
		{
			TalDocument document = new TalDocument();
			
			document.Load(GetTestFileName("simple-tal-document.xml"));
			
      Assert.AreEqual(EXPECTED_CONTENT, document.OuterXml, "Document has been read correctly.");
		}
		
		[Test]
		[Category("Integration")]
		public void TestRender()
		{
			TalDocument document = new TalDocument();
      MockObject mock = new MockObject();
			
			document.Load(GetTestFileName("simple-tal-document.xml"));
			
      document.TalesContext.AddDefinition("test", mock);
			
      Assert.AreEqual(EXPECTED_RENDERING, document.Render(), "Document renders correctly without the condition");
      
      mock.BooleanValue = false;
      
      Assert.AreEqual(EXPECTED_RENDERING_FALSE, document.Render(), "Document renders correctly with the condition");
		}
		
		#endregion
		
		#region supporting methods
		
		/// <summary>
		/// <para>Gets the full filename for a test file.</para>
		/// </summary>
		/// <param name="relativeFilename">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string GetTestFileName(string relativeFilename)
		{
			string
				basePath = ConfigurationManager.AppSettings["test-data-path"],
				testFilePath;
			
			if(!Directory.Exists(basePath))
			{
				throw new DirectoryNotFoundException("Test data base path not found.");
			}
			
			testFilePath = Path.Combine(basePath, relativeFilename);
			
			if(!File.Exists(testFilePath))
			{
				throw new FileNotFoundException("Test filename does not exist within the test data path.");
			}
			
			return testFilePath;
		}
		
		#endregion
  }
}
