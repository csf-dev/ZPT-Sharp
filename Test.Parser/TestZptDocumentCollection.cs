using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT;
using System.IO;
using CraigFowler.Web.ZPT.Tales;

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

