//  
//  TestMetalMacro.cs
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
using System.Collections.Generic;
using CraigFowler.Web.ZPT.Metal;
using CraigFowler.Web.ZPT.Tales;
using CraigFowler.Web.ZPT.Tales.Exceptions;
using NUnit.Framework;

namespace Test.CraigFowler.Web.ZPT.Metal
{
  [TestFixture]
  public class TestMetalMacro
  {
    #region constants
    
    private const string BASE_MACRO = @"<!-- fakeplone is a fictional user interface created by a large,
well-focused team of graphics designers -->
<html metal:define-macro=""page"" xmlns:metal=""http://xml.zope.org/namespaces/metal"">
<head>
<title metal:define-slot=""title"">Title here</title>
<metal:block define-slot=""local-styles"">
</metal:block>
</head>
<body>
<div>
  <div metal:define-slot=""annoying-quote"">
  ""The early bird gets the worm, but the second mouse gets the cheese.""
  </div>
  <a href=""#"">Preferences...</a>
</div>
<div metal:define-slot=""content"">
  Content here
</div>
<div metal:define-slot=""page-footer"">
  page footer
</div>
</body>
</html>
";
    
      private const string MACRO = @"<!-- This is ACME's generic look and feel, which is based on
PNOME's look and feel. -->
<html metal:extend-macro=""documents/pnome_template/macros/page""
      metal:define-macro=""page""
      xmlns:metal=""http://xml.zope.org/namespaces/metal"">
<head>
<title metal:fill-slot=""title"">ACME Look and Feel</title>
</head>
<body>
<div metal:fill-slot=""page-footer"">
Copyright 2004 Acme Inc.
<div metal:define-slot=""disclaimer"">
Standard disclaimers apply.
</div>
</div>
</body>
</html>
";
    
      private const string USAGE_DOCUMENT = @"<!-- ACME's document_list uses the ACME look and feel -->
<html metal:use-macro=""documents/acme_template/macros/page""
      xmlns:metal=""http://xml.zope.org/namespaces/metal"">
<head>
<title metal:fill-slot=""title"">Acme Document List</title>
<style metal:fill-slot=""local-styles"" type=""text/css"">
  body { background-color: white; }
</style>
</head>
<body>
<div metal:fill-slot=""content"">
<h1>Documents</h1>
<ul>
<li>Rocket Science for Dummies</li>
<li>Birds for the Gourmet Chef</li>
</ul>
</div>
<div metal:fill-slot=""disclaimer"">
This document list is classified.
</div>
</body>
</html>
";
    
    #endregion
    
    #region properties
    
    public Dictionary<string,MetalDocument> DocumentCollection  {
      get;
      private set;
    }
    
    #endregion
    
    #region set up
    
    [SetUp]
    public void SetUp()
    {
      MetalDocument usageDocument, macroDocument, baseMacroDocument;
      
      this.DocumentCollection = new Dictionary<string, MetalDocument>();
      
      usageDocument = new MetalDocument();
      usageDocument.LoadXml(USAGE_DOCUMENT);
      
      macroDocument = new MetalDocument();
      macroDocument.LoadXml(MACRO);
      
      baseMacroDocument = new MetalDocument();
      baseMacroDocument.LoadXml(BASE_MACRO);
      
      this.DocumentCollection.Add("pnome_template", baseMacroDocument);
      this.DocumentCollection.Add("acme_template", macroDocument);
      this.DocumentCollection.Add("document_list", usageDocument);
      
      usageDocument.TalesContext.AddDefinition("documents", this.DocumentCollection);
      macroDocument.TalesContext.AddDefinition("documents", this.DocumentCollection);
      baseMacroDocument.TalesContext.AddDefinition("documents", this.DocumentCollection);
    }
    
    #endregion
    
    #region tests
    
    [Test]
    public void TestExpand()
    {
      MetalMacro macro = null;
      MetalDocument
        macroDocument = this.DocumentCollection["acme_template"],
        baseMacroDocument = this.DocumentCollection["pnome_template"];
      
      Assert.IsNotNull(macroDocument.TalesContext.CreateExpression("documents/pnome_template").GetValue(),
                       "'pnome_template' is not null.");
      
      Assert.IsNotNull(macroDocument.TalesContext.CreateExpression("documents/pnome_template/macros").GetValue(),
                       "'pnome_template/macros' is not null.");
      
      Assert.AreEqual(baseMacroDocument.Macros["page"],
                      macroDocument.TalesContext.CreateExpression("documents/pnome_template/macros/page").GetValue(),
                      "The path to the 'page' macro should evaluate OK");
      
      try
      {
        macro = macroDocument.Macros["page"].Expand(false, macroDocument.TalesContext);
      }
      catch(TraversalException ex)
      {
          foreach(TalesPath path in ex.Attempts.Keys)
          {
            Console.Error.WriteLine("{0}: {1}",
                                    path.ToString(),
                                    ex.Attempts[path].Message);
          }
          throw;
      }
      
      Assert.IsTrue(macro.GetAvailableSlots().ContainsKey("title"), "Macro contains a 'title' slot.");
    }
    
    #endregion
  }
}

