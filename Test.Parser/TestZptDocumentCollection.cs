//  
//  TestZptDocumentCollection.cs
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
using CraigFowler.Web.ZPT.Tales;
using NUnit.Framework;

namespace Test.CraigFowler.Web.ZPT
{
  [TestFixture]
  public class TestZptDocumentCollection
  {
    [Test]
    public void TestConstructor()
    {
      TestConfiguration config = new TestConfiguration();
      DirectoryInfo testDataPath = config.GetTestDataDirectoryInfo().GetDirectories("Document Root")[0];
      ZptDocumentCollection testData = ZptDocumentCollection.CreateFromFilesystem(testDataPath);
      
      Assert.IsNotNull(testData["one"], "Document 'one' is not null");
      Assert.IsInstanceOfType(typeof(IZptDocument), testData["one"], "Document 'one' is correct type");
      
      Assert.IsNotNull(testData["two"], "Document 'two' is not null");
      Assert.IsInstanceOfType(typeof(IZptDocument), testData["two"], "Document 'two' is correct type");
      
      Assert.IsInstanceOfType(typeof(TalesStructureProvider),
                              testData["three"],
                              "Directory 'three' is a document collection");
      
      Assert.IsNotNull(((TalesStructureProvider) testData["three"])["five"], "Document 'five' is not null");
      Assert.IsInstanceOfType(typeof(IZptDocument),
                              ((TalesStructureProvider) testData["three"])["five"],
                              "Document 'five' is correct type");
    }
    
    [Test]
    [Category("Integration")]
    public void TestWithTalesContext()
    {
      TestConfiguration config = new TestConfiguration();
      DirectoryInfo testDataPath = config.GetTestDataDirectoryInfo().GetDirectories("Document Root")[0];
      ZptDocumentCollection testData = ZptDocumentCollection.CreateFromFilesystem(testDataPath);
      TalesContext context = new TalesContext();
      TalesExpression expression;
      object expressionValue;
      
      context.AddDefinition("documents", testData);
      expression = context.CreateExpression("documents/three/five");
      expressionValue = expression.GetValue();
      
      Assert.IsNotNull(expressionValue, "Expression output is not null");
      Assert.IsInstanceOfType(typeof(IZptDocument), expressionValue, "Expression output is a ZPT document");
      Assert.AreEqual(Path.Combine(testDataPath.FullName, "three/five.pt"),
                      ((IZptDocument) expressionValue).Metadata.DocumentFilePath,
                      "Document file is at correct location.");
    }
  }
}

