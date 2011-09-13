//  
//  TestMetalDocument.cs
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
using System.IO;
using System.Xml;
using CraigFowler.Web.ZPT;
using CraigFowler.Web.ZPT.Metal;
using CraigFowler.Web.ZPT.Metal.Exceptions;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Tales.Exceptions;
using NUnit.Framework;

namespace Test.CraigFowler.Web.ZPT.Metal
{
  [TestFixture]
  public class TestMetalDocument
  {
    #region constants
    
    private const string MULTIPLE_MACRO_DOCUMENT = @"<html xmlns:metal=""http://xml.zope.org/namespaces/metal"">
<span metal:define-macro=""INNER"">
  <span metal:define-slot=""INNERSLOT"">INNERSLOT</span>
</span>

<xxx metal:use-macro=""template/macros/INNER"">
  <xxx metal:fill-slot=""INNERSLOT"">inner-argument</xxx>
</xxx>

<div metal:define-macro=""OUTER"">
<div metal:use-macro=""template/macros/INNER"">
  <xxx metal:define-slot=""OUTERSLOT"" metal:fill-slot=""INNERSLOT"">
    OUTERSLOT
  </xxx>
</div>
</div>

<div metal:use-macro=""template/macros/OUTER"">
<span>
  <xxx>
    <div metal:fill-slot=""OUTERSLOT"">outer-argument</div>
  </xxx>
</span>
</div>

<div metal:use-macro=""template/macros/OUTER"">
</div>
</html>";
    
    #endregion
    
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
    [Category("Integration")]
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
          Console.Error.WriteLine (@"Traversal exception information:

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
        Console.Error.WriteLine ("Exception information.  Path = {0}", ex.Data["Path"]);
        throw;
      }
      
      
      Assert.AreEqual("test", macro.MacroName, "Correct macro name");
    }
    
    [Test]
    [Category("Integration")]
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
    
    [Test]
    public void TestImportNode()
    {
      MetalDocument
        doc1 = new MetalDocument(),
        doc2 = new MetalDocument();
      XmlNode imported;
      
      doc1.LoadXml(@"
<html xmlns=""http://www.w3.org/1999/xhtml""
      xmlns:tal=""http://xml.zope.org/namespaces/tal""
      xmlns:metal=""http://xml.zope.org/namespaces/metal"">
<head>
<title>Macro test document</title>
</head>
<body>
  <h1>What this file is</h1>
  <p>This document is a test of the METAL macro system.  Parts of this document are delegated out to METAL macros.</p>
  <p metal:use-macro=""documents/three/macro-test-macro1/macros/test"" id=""useMacroNode"">
    This is default text provided by the master document.
  </p>
</body>
</html>");
      
      doc2.LoadXml(@"
<html xmlns=""http://www.w3.org/1999/xhtml""
      xmlns:tal=""http://xml.zope.org/namespaces/tal""
      xmlns:metal=""http://xml.zope.org/namespaces/metal"">
<head>
<title>Macro test document</title>
</head>
<body>
  <h1>What this file is</h1>
  <p>This document is a test of the METAL macro system.  Parts of this document are delegated out to METAL macros.</p>
  <p metal:use-macro=""documents/three/macro-test-macro1/macros/test"" id=""useMacroNode"">
    This is alternative text provided by the other document.
  </p>
</body>
</html>");
      
      imported = doc1.ImportNode(doc2.GetElementsByTagName("p", "http://www.w3.org/1999/xhtml")[1], true);
      
      Assert.IsNotNull(imported, "Imported node is not null");
      Assert.IsNotNull(imported.FirstChild.Value, "Imported node value is not null");
      
      Assert.AreEqual("This is alternative text provided by the other document.",
                      imported.FirstChild.Value.Trim(),
                      "Correct node value");
      
      doc1.GetElementsByTagName("body", "http://www.w3.org/1999/xhtml")[0].AppendChild(imported);
    }
    
    [Test]
    [Category("Integration")]
    public void TestRenderUseMacro()
    {
      TestConfiguration config = new TestConfiguration();
      DirectoryInfo testDataPath = config.GetTestDataDirectoryInfo().GetDirectories("Document Root")[0];
      ZptDocumentCollection testData = ZptDocumentCollection.CreateFromFilesystem(testDataPath);
      ZptDocument document = (ZptDocument) testData["macro-test-master"];
      XmlDocument renderedDocument = new XmlDocument();
      
      document.GetTemplateDocument().TalesContext.AddDefinition("documents", testData);
      
      renderedDocument.LoadXml(document.GetTemplateDocument().Render());
      
      Assert.AreEqual("This is text that is generated by the <strong xmlns=\"http://www.w3.org/1999/xhtml\">macro one document</strong> within the macro named <code xmlns=\"http://www.w3.org/1999/xhtml\">test</code>.",
                      renderedDocument.GetElementsByTagName("p", "http://www.w3.org/1999/xhtml")[1].InnerXml.Trim(),
                      "Correct inner XML");
    }
    
    [Test]
    [Category("Integration")]
    public void TestRenderUseMacroWithSlots()
    {
      TestConfiguration config = new TestConfiguration();
      DirectoryInfo testDataPath = config.GetTestDataDirectoryInfo().GetDirectories("Document Root")[0];
      ZptDocumentCollection testData = ZptDocumentCollection.CreateFromFilesystem(testDataPath);
      ZptDocument document = ((ZptDocument) ((TalesStructureProvider) testData["three"])["macro-test-macro3"]);
      XmlDocument renderedDocument = new XmlDocument();
      
      document.GetTemplateDocument().TalesContext.AddDefinition("documents", testData);
      
      renderedDocument.LoadXml(document.GetTemplateDocument().Render());
      Assert.AreEqual("This is a slot 1 filler from the master document",
                      renderedDocument.GetElementsByTagName("span", "http://www.w3.org/1999/xhtml")[0].InnerXml.Trim(),
                      "First span contents correct");
      Assert.AreEqual("This is slot 2 from the macro",
                      renderedDocument.GetElementsByTagName("span", "http://www.w3.org/1999/xhtml")[1].InnerXml.Trim(),
                      "Second span contents correct");
      Assert.AreEqual("This is a slot 3 filler from the master document",
                      renderedDocument.GetElementsByTagName("span", "http://www.w3.org/1999/xhtml")[2].InnerXml.Trim(),
                      "Third span contents correct");
    }
    
    [Test]
    [Category("Integration")]
    public void TestMultipleMacrosInSingleDocument()
    {
      MetalDocument document = new MetalDocument();
      document.LoadXml(MULTIPLE_MACRO_DOCUMENT);
      
      Assert.IsNotNull(document.Macros["INNER"], "INNER macro exists");
      Assert.IsNotNull(document.Macros["OUTER"], "OUTER macro exists");
      
      Assert.AreEqual(1, document.Macros["INNER"].GetAvailableSlots().Count, "INNER macro has one slot available.");
      Assert.AreEqual(1, document.Macros["OUTER"].GetAvailableSlots().Count, "OUTER macro has one slot available.");
      
      Assert.IsTrue(document.Macros["INNER"].GetAvailableSlots().ContainsKey("INNERSLOT"),
                    "INNER macro has a slot named INNERSLOT");
      
      Assert.IsTrue(document.Macros["OUTER"].GetAvailableSlots().ContainsKey("OUTERSLOT"),
                    "OUTER macro has a slot named OUTERSLOT");
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

