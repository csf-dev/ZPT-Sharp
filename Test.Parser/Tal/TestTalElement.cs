
using System;
using NUnit.Framework;
using CraigFowler.Web.ZPT.Tal;
using System.Xml;
using CraigFowler.Web.ZPT.Mocks;
using System.Collections.Generic;
using CraigFowler.Web.ZPT.Tales.Exceptions;
using CraigFowler.Web.ZPT.Tales;

namespace Test.CraigFowler.Web.ZPT.Tal
{
  [TestFixture]
  public class TestTalElement
  {
    #region general tests and supporting methods
    
    [Test]
    [Description("Contains no asserts but ensures that the constructor functions.")]
    public void TestConstructor()
    {
      TalDocument document = CreateTestTalDocument();
      Assert.IsNotNull(document, "Document is not null");
    }
    
    /// <summary>
    /// <para>Creates a valid mock TAL document.</para>
    /// </summary>
    /// <remarks>
    /// <para>The document that this code creates is as follows:</para>
    /// <code>
    /// <html xmlns="http://www.w3.org/1999/xhtml"><head><title>Foo</title></head><body><div>Bar</div></body></html>
    /// </code>
    /// </remarks>
    /// <returns>
    /// A <see cref="TalDocument"/>
    /// </returns>
    private TalDocument CreateTestTalDocument()
    {
      TalDocument output = new TalDocument();
      XmlNode htmlElement, headElement, titleElement, bodyElement, divElement;
      
      htmlElement = new TalElement(String.Empty, "html", "http://www.w3.org/1999/xhtml", output);
      output.AppendChild(htmlElement);
      
      headElement = new TalElement(String.Empty, "head", "http://www.w3.org/1999/xhtml", output);
      htmlElement.AppendChild(headElement);
      
      titleElement = new TalElement(String.Empty, "title", "http://www.w3.org/1999/xhtml", output);
      headElement.AppendChild(titleElement);
      
      titleElement.AppendChild(output.CreateTextNode("Foo"));
      
      bodyElement = new TalElement(String.Empty, "body", "http://www.w3.org/1999/xhtml", output);
      htmlElement.AppendChild(bodyElement);
      
      divElement = new TalElement(String.Empty, "div", "http://www.w3.org/1999/xhtml", output);
      bodyElement.AppendChild(divElement);
      
      divElement.AppendChild(output.CreateTextNode("Bar"));
      
      return output;
    }
    
    #endregion
    
    #region regex tests
    
    [Test]
    [Description("Tests the Regex that splits 'define' statements apart")]
    public void TestDefineStatements()
    {
      string
        singleStatement = "foo bar/baz",
        threeStatements = "foo bar/baz;wibble wobble/spong;test string:This is a test!",
        twoStatementsWithEscapedSemicolon = "foo bar/baz;test string: This is an ;; escaped test!",
        threeStatementsWithLineBreaks = @"foo bar/baz;
                                          wibble wobble/spong;
                                          test string:This is a test!";
      
      Assert.AreEqual(1, TalElement.DefineStatements.Matches(singleStatement).Count, "Single statement");
      Assert.AreEqual(3, TalElement.DefineStatements.Matches(threeStatements).Count, "Three statements");
      Assert.AreEqual(2,
                      TalElement.DefineStatements.Matches(twoStatementsWithEscapedSemicolon).Count,
                      "Two statements with an escaped double-semicolon");
      Assert.AreEqual(3,
                      TalElement.DefineStatements.Matches(threeStatementsWithLineBreaks).Count,
                      "Three statements with linebreaks");
      
      Assert.AreEqual("test string: This is an ;; escaped test!",
                      TalElement.DefineStatements.Matches(twoStatementsWithEscapedSemicolon)[1].Value,
                      "Correct value on second match of a two-statement declaration");
    }
    
    [Test]
    [Description("Test the Regex that catures the parts of a 'define' statement")]
    public void TestDefineSpecification()
    {
      string
        globalDefine = "global foo bar/baz",
        localDefineWithWhitespace = @"
                                      local foo bar/baz",
        impliedLocalDefine = "foo bar/baz",
        impliedLocalDefineWithInnerWhitespace = "   foo string:This is a test!";
      
      Assert.IsTrue(TalElement.DefineSpecification.Match(globalDefine).Success,
                    "Matches global define");
      Assert.IsTrue(TalElement.DefineSpecification.Match(localDefineWithWhitespace).Success,
                    "Matches local define with whitespace");
      Assert.IsTrue(TalElement.DefineSpecification.Match(impliedLocalDefine).Success,
                    "Matches implied local define");
      Assert.IsTrue(TalElement.DefineSpecification.Match(impliedLocalDefineWithInnerWhitespace).Success,
                    "Matches implied local define with inner whitespace");
      
      Assert.IsTrue(TalElement.DefineSpecification.Match(globalDefine).Groups[2].Success,
                    "Captured group 2 in the global define");
      Assert.AreEqual("global",
                      TalElement.DefineSpecification.Match(globalDefine).Groups[2].Value,
                      "Correct value for group 2 in the global define");
      Assert.AreEqual("foo",
                      TalElement.DefineSpecification.Match(globalDefine).Groups[3].Value,
                      "Correct value for group 3 in the global define");
      Assert.AreEqual("bar/baz",
                      TalElement.DefineSpecification.Match(globalDefine).Groups[4].Value,
                      "Correct value for group 4 in the global define");
      
      Assert.IsTrue(TalElement.DefineSpecification.Match(localDefineWithWhitespace).Groups[2].Success,
                    "Captured group 2 in the local define with whitespace");
      Assert.AreEqual("local",
                      TalElement.DefineSpecification.Match(localDefineWithWhitespace).Groups[2].Value,
                      "Correct value for group 2 in the local define with whitespace");
      Assert.AreEqual("foo",
                      TalElement.DefineSpecification.Match(localDefineWithWhitespace).Groups[3].Value,
                      "Correct value for group 3 in the local define with whitespace");
      Assert.AreEqual("bar/baz",
                      TalElement.DefineSpecification.Match(localDefineWithWhitespace).Groups[4].Value,
                      "Correct value for group 4 in the local define with whitespace");
      
      Assert.IsFalse(TalElement.DefineSpecification.Match(impliedLocalDefine).Groups[2].Success,
                    "Did not capture group 2 in the implied local define");
      Assert.AreEqual("foo",
                      TalElement.DefineSpecification.Match(impliedLocalDefine).Groups[3].Value,
                      "Correct value for group 3 in the implied local define");
      Assert.AreEqual("bar/baz",
                      TalElement.DefineSpecification.Match(impliedLocalDefine).Groups[4].Value,
                      "Correct value for group 4 in the implied local define");
      
      Assert.IsFalse(TalElement.DefineSpecification.Match(impliedLocalDefineWithInnerWhitespace).Groups[2].Success,
                    "Did not capture group 2 in the implied local define with inner whitespace");
      Assert.AreEqual("foo",
                      TalElement.DefineSpecification.Match(impliedLocalDefineWithInnerWhitespace).Groups[3].Value,
                      "Correct value for group 3 in the implied local define with inner whitespace");
      Assert.AreEqual("string:This is a test!",
                      TalElement.DefineSpecification.Match(impliedLocalDefineWithInnerWhitespace).Groups[4].Value,
                      "Correct value for group 4 in the implied local define with inner whitespace");
    }

    [Test]
    [Description("Test the Regex that handles 'content' or 'replace' statements")]
    public void TestContentOrReplaceSpecification()
    {
      string
        structureExpression = "structure foo/bar/baz",
        textExpression = "text string:This is some text!",
        impliedTextExression = "foo/bar/baz";
      
      Assert.IsTrue(TalElement.ContentOrReplaceSpecification.Match(structureExpression).Success,
                    "Matches the structure expression");
      Assert.IsTrue(TalElement.ContentOrReplaceSpecification.Match(textExpression).Success,
                    "Matches the text expression");
      Assert.IsTrue(TalElement.ContentOrReplaceSpecification.Match(impliedTextExression).Success,
                    "Matches the implied text expression");
      
      Assert.IsTrue(TalElement.ContentOrReplaceSpecification.Match(structureExpression).Groups[2].Success,
                    "Matched for group 2 of the structure expression");
      Assert.AreEqual("structure",
                      TalElement.ContentOrReplaceSpecification.Match(structureExpression).Groups[2].Value,
                      "Correct match for group 2 of the structure expression");
      Assert.AreEqual("foo/bar/baz",
                      TalElement.ContentOrReplaceSpecification.Match(structureExpression).Groups[3].Value,
                      "Correct match for group 3 of the structure expression");
      
      Assert.IsTrue(TalElement.ContentOrReplaceSpecification.Match(textExpression).Groups[2].Success,
                    "Matched for group 2 of the text expression");
      Assert.AreEqual("text",
                      TalElement.ContentOrReplaceSpecification.Match(textExpression).Groups[2].Value,
                      "Correct match for group 2 of the text expression");
      Assert.AreEqual("string:This is some text!",
                      TalElement.ContentOrReplaceSpecification.Match(textExpression).Groups[3].Value,
                      "Correct match for group 3 of the text expression");
      
      Assert.IsFalse(TalElement.ContentOrReplaceSpecification.Match(impliedTextExression).Groups[2].Success,
                    "No match for group 2 of the implied text expression");
      Assert.AreEqual("foo/bar/baz",
                      TalElement.ContentOrReplaceSpecification.Match(impliedTextExression).Groups[3].Value,
                      "Correct match for group 3 of the implied text expression");
    }
    
    [Test]
    [Description("Test the Regex that handles 'repeat' statements")]
    public void TestRepeatSpecification()
    {
      string repeatStatement = "someVar foo/bar/baz";
      
      Assert.IsTrue(TalElement.RepeatSpecification.Match(repeatStatement).Success, "Matches a repeat statement");
      Assert.AreEqual("someVar",
                      TalElement.RepeatSpecification.Match(repeatStatement).Groups[1].Value,
                      "Correct value for group 1");
      Assert.AreEqual("foo/bar/baz",
                      TalElement.RepeatSpecification.Match(repeatStatement).Groups[2].Value,
                      "Correct value for group 2");
    }

    [Test]
    [Description("Test the Regex that handles 'attributes' statements")]
    public void TestAttributesSpecification()
    {
      string
        attributeWithNamespace = "html:href foo/bar/baz",
        attributeWithoutNamespace = "selected item/isSelected",
        attributeWithWhitespace = @"
                                    html:checked string:checked";
      
      Assert.IsTrue(TalElement.AttributesSpecification.Match(attributeWithNamespace).Success,
                    "Matched attribute with namespace");
      Assert.IsTrue(TalElement.AttributesSpecification.Match(attributeWithoutNamespace).Success,
                    "Matched attribute without namespace");
      Assert.IsTrue(TalElement.AttributesSpecification.Match(attributeWithWhitespace).Success,
                    "Matched attribute with whitespace");
      
      Assert.IsTrue(TalElement.AttributesSpecification.Match(attributeWithNamespace).Groups[2].Success,
                    "Matched group 2 of the attribute with a namespace");
      Assert.AreEqual("html",
                      TalElement.AttributesSpecification.Match(attributeWithNamespace).Groups[2].Value,
                      "Correct match for group 2 of the attribute with a namespace");
      Assert.AreEqual("href",
                      TalElement.AttributesSpecification.Match(attributeWithNamespace).Groups[3].Value,
                      "Correct match for group 3 of the attribute with a namespace");
      Assert.AreEqual("foo/bar/baz",
                      TalElement.AttributesSpecification.Match(attributeWithNamespace).Groups[4].Value,
                      "Correct match for group 4 of the attribute with a namespace");
      
      Assert.IsFalse(TalElement.AttributesSpecification.Match(attributeWithoutNamespace).Groups[2].Success,
                     "No match for group 2 of the attribute without a namespace");
      Assert.AreEqual("selected",
                      TalElement.AttributesSpecification.Match(attributeWithoutNamespace).Groups[3].Value,
                      "Correct match for group 3 of the attribute without a namespace");
      Assert.AreEqual("item/isSelected",
                      TalElement.AttributesSpecification.Match(attributeWithoutNamespace).Groups[4].Value,
                      "Correct match for group 4 of the attribute without a namespace");
      
      Assert.IsTrue(TalElement.AttributesSpecification.Match(attributeWithWhitespace).Groups[2].Success,
                    "Matched group 2 of the attribute with whitespace");
      Assert.AreEqual("html",
                      TalElement.AttributesSpecification.Match(attributeWithWhitespace).Groups[2].Value,
                      "Correct match for group 2 of the attribute with whitespace");
      Assert.AreEqual("checked",
                      TalElement.AttributesSpecification.Match(attributeWithWhitespace).Groups[3].Value,
                      "Correct match for group 3 of the attribute with whitespace");
      Assert.AreEqual("string:checked",
                      TalElement.AttributesSpecification.Match(attributeWithWhitespace).Groups[4].Value,
                      "Correct match for group 4 of the attribute with whitespace");
    }
    
    #endregion
    
    #region attribute tests
    
    [Test]
    public void TestRenderWithDefineAttribute()
    {
      TalDocument document = CreateTestTalDocument();
      TalElement element;
      MockObject mock = new MockObject();
      
      element = (TalElement) document.GetElementsByTagName("div")[0];
      element.SetAttribute("condition", TalDocument.TalNamespace, "bool");
      element.SetAttribute("define", TalDocument.TalNamespace, "bool test/BooleanValue");
      
      document.TalesContext.AddDefinition("test", mock);
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Foo</title></head>" +
                      "<body><div>Bar</div></body></html>",
                      document.Render(),
                      "Document renders correctly without the condition");
      
      mock.BooleanValue = false;
      
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Foo</title></head>" +
                      "<body /></html>",
                      document.Render(),
                      "Document renders correctly with the condition");
    }
    
    [Test]
    public void TestRenderWithMultipleDefineAttributes()
    {
      TalDocument document = CreateTestTalDocument();
      TalElement element;
      MockObject mock = new MockObject();
      
      element = (TalElement) document.GetElementsByTagName("div")[0];
      element.SetAttribute("condition", TalDocument.TalNamespace, "bool");
      
      element = (TalElement) document.GetElementsByTagName("title")[0];
      element.SetAttribute("content", TalDocument.TalNamespace, "string:Foo ${stringValue}");
      
      element = (TalElement) document.GetElementsByTagName("html")[0];
      element.SetAttribute("define", TalDocument.TalNamespace, @"stringValue test/unambiguous/foo;
                                                                 bool test/BooleanValue");
      
      document.TalesContext.AddDefinition("test", mock);
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Foo bar</title></head>" +
                      "<body><div>Bar</div></body></html>",
                      document.Render(),
                      "Document renders correctly without the condition");
      
      mock.BooleanValue = false;
      
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Foo bar</title></head>" +
                      "<body /></html>",
                      document.Render(),
                      "Document renders correctly with the condition");
    }
    
    [Test]
    public void TestRenderWithConditionAttribute()
    {
      TalDocument document = CreateTestTalDocument();
      TalElement element;
      MockObject mock = new MockObject();
      
      element = (TalElement) document.GetElementsByTagName("div")[0];
      element.SetAttribute("condition", TalDocument.TalNamespace, "test/BooleanValue");
      
      document.TalesContext.AddDefinition("test", mock);
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Foo</title></head>" +
                      "<body><div>Bar</div></body></html>",
                      document.Render(),
                      "Document renders correctly without the condition");
      
      mock.BooleanValue = false;
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Foo</title></head>" +
                      "<body /></html>",
                      document.Render(),
                      "Document renders correctly with the condition");
    }
    
    [Test]
    public void TestRenderWithRepeatAttribute()
    {
      List<string> list = new List<string>();
      TalDocument document = CreateTestTalDocument();
      TalElement element;
      string renderedOutput = String.Empty;
      
      list.Add("foo");
      list.Add("bar");
      list.Add("baz");
      
      document.TalesContext.AddDefinition("list", list);
      element = (TalElement) document.GetElementsByTagName("div")[0];
      element.SetAttribute("repeat", TalDocument.TalNamespace, "item list");
      element.SetAttribute("content",
                           TalDocument.TalNamespace,
                           "string:Item is ${item} which is even? ${repeat/item/even}");
      
      try
      {
        renderedOutput = document.Render();
      }
      catch(TraversalException ex)
      {
        foreach(TalesPath path in ex.Attempts.Keys)
        {
          Console.WriteLine ("Path:      {0}\nException: {1}", path.ToString(), ex.Attempts[path].ToString());
        }
      }
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Foo</title></head>" +
                      "<body><div>Item is foo which is even? True</div>" +
                      "<div>Item is bar which is even? False</div>" +
                      "<div>Item is baz which is even? True</div></body></html>",
                      renderedOutput,
                      "Document renders correctly");
    }
    
    [Test]
    public void TestRenderWithContentAttribute()
    {
      TalDocument document = CreateTestTalDocument();
      TalElement element;
      MockObject mock = new MockObject();
      
      element = (TalElement) document.GetElementsByTagName("div")[0];
      element.SetAttribute("content", TalDocument.TalNamespace, "test/BooleanValue");
      
      document.TalesContext.AddDefinition("test", mock);
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Foo</title></head>" +
                      "<body><div>True</div></body></html>",
                      document.Render(),
                      "Document renders correctly with true");
      
      mock.BooleanValue = false;
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Foo</title></head>" +
                      "<body><div>False</div></body></html>",
                      document.Render(),
                      "Document renders correctly with false");
    }
    
    [Test]
    public void TestRenderWithOmitTagAttribute()
    {
      TalDocument document = CreateTestTalDocument();
      TalElement element;
      MockObject mock = new MockObject();
      
      element = (TalElement) document.GetElementsByTagName("div")[0];
      element.SetAttribute("omit-tag", TalDocument.TalNamespace, "test/BooleanValue");
      
      document.TalesContext.AddDefinition("test", mock);
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Foo</title></head>" +
                      "<body>Bar</body></html>",
                      document.Render(),
                      "Document renders correctly with true");
      
      mock.BooleanValue = false;
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Foo</title></head>" +
                      "<body><div>Bar</div></body></html>",
                      document.Render(),
                      "Document renders correctly with false");
    }
    
    [Test]
    public void TestRenderWithAttributesAttribute()
    {
      TalDocument document = CreateTestTalDocument();
      TalElement element;
      MockObject mock = new MockObject();
      
      element = (TalElement) document.GetElementsByTagName("div")[0];
      element.SetAttribute("attributes", TalDocument.TalNamespace, "class test/BooleanValue");
      
      document.TalesContext.AddDefinition("test", mock);
      
      Assert.AreEqual("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Foo</title></head>" +
                      "<body><div class=\"True\">Bar</div></body></html>",
                      document.Render(),
                      "Document renders correctly");
    }
    
    #endregion
  }
}
