using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Metal;
using CraigFowler.Web.ZPT;
using System.IO;
using System.Xml;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Tales.Exceptions;
using CraigFowler.Web.ZPT.Metal.Exceptions;
using System.Collections.Generic;

namespace Test.CraigFowler.Web.ZPT.Metal
{
  [TestFixture]
  [Category("Integration")]
  public class TestMetalDocument
  {
    #region properties
    
    public TestConfiguration Config
    {
      get;
      private set;
    }
    
    #endregion
    
    #region tests
    
    [Test]
    public void TestLoad()
    {
      MetalDocument testDoc = GetMasterDocument();
      
      Assert.IsNotNull(testDoc, "Document is not null");
    }
    
    [Test]
    public void TestGetUseMacro()
    {
      TalesContext context = new TalesContext();
      MetalDocument testDoc = GetMasterDocument();
      Assert.IsNotNull(testDoc, "Document is not null");
      
      XmlNode targetNode = testDoc.GetElementsByTagName("p", "http://www.w3.org/1999/xhtml")[1];
      Assert.IsInstanceOfType(typeof(MetalElement), targetNode, "Target node is a METAL element node.");
      
      context.AddDefinition("documents", this.Config.GetTestDocuments());
      
      MetalElement element = targetNode as MetalElement;
      MetalMacro macro = null;
      
      try
      {
        macro = element.GetUseMacro(context);
      }
      catch(TraversalException ex)
      {
        foreach(TalesPath path in ex.Attempts.Keys)
        {
          Console.WriteLine (@"Traversal exception information:

Path
----
{0}

Exception
---------
{1}",
                             path.ToString(),
                             ex.Attempts[path].ToString());
        }
        
        throw;
      }
      catch(MetalException ex)
      {
        Console.WriteLine ("Exception information.  Path = {0}", ex.Data["Path"]);
        throw;
      }
      
      
      Assert.AreEqual("test", macro.MacroName, "Correct macro name");
    }
    
    [Test]
    public void TestRetrieveMacroUsingTalesExpression()
    {
      MetalDocument innerDoc = new MetalDocument();
      ZptDocumentCollection documentCollection = new ZptDocumentCollection();
      ZptDocument document;
      TalesContext context = new TalesContext();
      
      innerDoc.LoadXml("<p xmlns:metal=\"http://xml.zope.org/namespaces/metal\" metal:define-macro=\"test\">Foo</p>");
      
      document = new ZptDocument(new ZptMetadata(), innerDoc);
      document.Metadata.DocumentType = ZptDocumentType.Metal;
      
      documentCollection.StoreItem(new TalesPath("foo/bar"), document);
      
      context.AddDefinition("documents", documentCollection);
      
      Assert.IsInstanceOfType(typeof(MetalMacroCollection),
                              context.CreateExpression("documents/foo/bar/macros").GetValue(),
                              "Macro collection is at correct location");
      
      Assert.IsInstanceOfType(typeof(MetalMacro),
                              context.CreateExpression("documents/foo/bar/macros/test").GetValue(),
                              "Test macro is at correct location");
      
      MetalMacro macro = context.CreateExpression("documents/foo/bar/macros/test").GetValue() as MetalMacro;
      
      Assert.AreEqual("test", macro.MacroName, "Correct macro name");
    }
    
    #endregion
    
    #region helper methods
    
    private MetalDocument GetMasterDocument()
    {
      MetalDocument output = new MetalDocument();
      string filePath = Path.Combine(this.Config.GetTestDataDirectoryInfo().FullName,
                                     "Document Root/macro-test-master.pt");
      
      output.Load(filePath);
      
      return output;
    }
    
    #endregion
    
    #region constructor
    
    public TestMetalDocument ()
    {
      this.Config = new TestConfiguration();
    }
    
    #endregion
  }
}

