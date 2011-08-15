//  
//  TestTalDocument.cs
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


using System.IO;
using CraigFowler.Web.ZPT;
using CraigFowler.Web.ZPT.Mocks;
using CraigFowler.Web.ZPT.Tal;
using NUnit.Framework;

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
		private string GetTestFileName(string relativeFilename)
		{
      DirectoryInfo basePath = new TestConfiguration().GetTestDataDirectoryInfo();
			string testFilePath;
			
			if(!basePath.Exists)
			{
				throw new DirectoryNotFoundException("Test data base path not found.");
			}
			
			testFilePath = Path.Combine(basePath.FullName, relativeFilename);
			
			if(!File.Exists(testFilePath))
			{
				throw new FileNotFoundException("Test filename does not exist within the test data path.");
			}
			
			return testFilePath;
		}
		
		#endregion
  }
}
