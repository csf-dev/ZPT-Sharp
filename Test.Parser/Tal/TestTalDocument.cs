
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
      MockObject mock = new MockObject();
			
			document.Load(GetTestFileName("simple-tal-document.xml"));
			
      document.TalesContext.AddDefinition("test", mock);
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
      								"<head><title>Foo</title></head>" +
                      "<body><div>Bar</div></body></html>",
                      document.Render(),
                      "Document renders correctly without the condition");
      
      mock.BooleanValue = false;
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
      								"<head><title>Foo</title></head>" +
                      "<body /></html>",
                      document.Render(),
                      "Document renders correctly with the condition");
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
		private string GetTestFileName(string relativeFilename)
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
