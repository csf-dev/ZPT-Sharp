using System;
using System.Reflection;
using CSF.Reflection;
using CSF.Zpt.Rendering;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Moq;
using System.Linq;
using System.Xml;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  public class TestZptXmlElement
  {
    #region fields

    private string _xmlSource;
    private XmlDocument _document;
    private SourceFileInfo _sourceFile;

    #endregion

    #region setup

    [TestFixtureSetUp]
    public void FixtureSetup()
    {
      _xmlSource = Assembly.GetExecutingAssembly().GetManifestResourceText(this.GetType(), "TestZptXmlElement.xml");
      _sourceFile = Mock.Of<SourceFileInfo>();
    }

    [SetUp]
    public void Setup()
    {
      _document = new XmlDocument();
      _document.LoadXml(_xmlSource);
    }

    #endregion

    #region tests

    [Test]
    public void TestToString()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement,
                               _sourceFile);

      // Act
      var result = sut.ToString().Replace(Environment.NewLine, "\n");

      // Assert
      Assert.AreEqual(_xmlSource, result);
    }

    [Test]
    public void TestGetChildElements()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement,
                                _sourceFile);

      // Act
      var result = sut.GetChildElements();

      // Assert
      Assert.AreEqual(2, result.Length, "Count of child element");
      Assert.AreEqual("head", result[0].Name, "Name of first child");
      Assert.AreEqual("body", result[1].Name, "Name of second child");
    }

    [Test]
    public void TestSearchChildrenByAttribute()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement,
                                  _sourceFile);

      // Act
      var results = sut.SearchChildrenByAttribute(new ZptNamespace(uri: "http://ns.csf-dev.com/custom"),
                                                  "parent_attrib");

      // Assert
      Assert.NotNull(results, "Result nullability");
      Assert.AreEqual(1, results.Length, "Count of results");
      Assert.AreEqual("div", results[0].Name, "Name of found element");
    }

    [Test]
    public void TestGetAttributes()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                  _sourceFile);

      // Act
      var results = sut.GetAttributes();

      // Assert
      Assert.NotNull(results, "Result nullability");
      Assert.AreEqual(2, results.Length, "Count of results");
      Assert.AreEqual("custom:parent_attrib", results[0].Name, "Attribute name one");
      Assert.AreEqual("class", results[1].Name, "Attribute name two");
    }

    //        NS                              Name              Expected name             Expected value
    //        ------                          ----              -------------             --------------
    [TestCase("http://ns.csf-dev.com/custom", "parent_attrib",  "custom:parent_attrib",   "Attribute value one")]
    [TestCase(null,                           "class",          "class",                  "class_one class_two")]
    public void TestGetAttribute(string ns, string name, string expectedName, string expectedValue)
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                  _sourceFile);

      // Act
      var result = sut.GetAttribute(new ZptNamespace(uri: ns), name);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(expectedName, result.Name, "Result name");
      Assert.AreEqual(expectedValue, result.Value, "Result value");
    }

    [Test]
    public void TestSetAttribute()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                  _sourceFile);

      // Act
      sut.SetAttribute("foo", "bar");

      // Assert
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <div custom:parent_attrib=""Attribute value one"" class=""class_one class_two"" foo=""bar"">
        <ul>
          <li custom:child_attrib=""foo"">Foo content</li>
          <li custom:child_attrib=""bar"">Bar content</li>
          <li custom:child_attrib=""baz"">Baz content</li>
        </ul>
      </div>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    [Test]
    public void TestSetAttributeWithNamespace()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                  _sourceFile);

      // Act
      sut.SetAttribute(new ZptNamespace(prefix: "ns", uri: "http://ns.csf-dev.com/custom2"), "foo", "bar");

      // Assert
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <div custom:parent_attrib=""Attribute value one"" class=""class_one class_two"" ns:foo=""bar"" xmlns:ns=""http://ns.csf-dev.com/custom2"">
        <ul>
          <li custom:child_attrib=""foo"">Foo content</li>
          <li custom:child_attrib=""bar"">Bar content</li>
          <li custom:child_attrib=""baz"">Baz content</li>
        </ul>
      </div>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    [Test]
    public void TestRemoveAttribute()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                  _sourceFile);

      // Act
      sut.RemoveAttribute("class");

      // Assert
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <div custom:parent_attrib=""Attribute value one"">
        <ul>
          <li custom:child_attrib=""foo"">Foo content</li>
          <li custom:child_attrib=""bar"">Bar content</li>
          <li custom:child_attrib=""baz"">Baz content</li>
        </ul>
      </div>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    [Test]
    public void TestRemoveAttributeWithNamespace()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                  _sourceFile);

      // Act
      sut.RemoveAttribute(new ZptNamespace(prefix: "custom", uri: "http://ns.csf-dev.com/custom"), "parent_attrib");

      // Assert
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <div class=""class_one class_two"">
        <ul>
          <li custom:child_attrib=""foo"">Foo content</li>
          <li custom:child_attrib=""bar"">Bar content</li>
          <li custom:child_attrib=""baz"">Baz content</li>
        </ul>
      </div>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    [Test]
    public void TestPurgeAttributes()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                  _sourceFile);

      // Act
      sut.PurgeAttributes(new ZptNamespace(prefix: "custom", uri: "http://ns.csf-dev.com/custom"));

      // Assert
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <div class=""class_one class_two"">
        <ul>
          <li>Foo content</li>
          <li>Bar content</li>
          <li>Baz content</li>
        </ul>
      </div>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    [Test]
    public void TestReplaceWith()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                _sourceFile);
      var replacementHtml = "<p>Replacement element</p>";
      var secondDocument = new XmlDocument();
      secondDocument.LoadXml(replacementHtml);
      var secondElement = new ZptXmlElement(secondDocument.DocumentElement,
                                          _sourceFile);

      // Act
      var result = sut.ReplaceWith(secondElement);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual("p", result.Name, "Result name");
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <p>Replacement element</p>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    [Test]
    public void TestReplaceWithString()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                  _sourceFile);

      // Act
      var result = sut.ReplaceWith("<p>Replacement element</p>", false);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(0, result.Length, "Count of results");
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>&lt;p&gt;Replacement element&lt;/p&gt;
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    [Test]
    public void TestReplaceWithStringStructure()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                  _sourceFile);

      // Act
      var result = sut.ReplaceWith("<p>Replacement <strong>element</strong></p>", true);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Length, "Count of results");
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <p>Replacement <strong>element</strong></p>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    [Test]
    public void TestReplaceChildrenWithString()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                  _sourceFile);

      // Act
      sut.ReplaceChildrenWith("<p>Replacement element</p>", false);

      // Assert
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <div custom:parent_attrib=""Attribute value one"" class=""class_one class_two"">&lt;p&gt;Replacement element&lt;/p&gt;</div>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    [Test]
    public void TestReplaceChildrenWithStringStructure()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                  _sourceFile);

      // Act
      sut.ReplaceChildrenWith("<p>Replacement <strong>element</strong></p>", true);

      // Assert
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <div custom:parent_attrib=""Attribute value one"" class=""class_one class_two"">
        <p>Replacement <strong>element</strong></p>
      </div>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    [Test]
    public void TestInsertBefore()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild.FirstChild,
                                  _sourceFile);
      var replacementXml = "<li>New element</li>";
      var secondDocument = new XmlDocument();
      secondDocument.LoadXml(replacementXml);
      var secondElement = new ZptXmlElement(secondDocument.DocumentElement,
                                             _sourceFile);

      var referenceElement = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild.FirstChild.ChildNodes[1],
                                                _sourceFile);

      // Act
      var result = sut.InsertBefore(referenceElement, secondElement);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual("li", result.Name, "Result name");
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <div custom:parent_attrib=""Attribute value one"" class=""class_one class_two"">
        <ul>
          <li custom:child_attrib=""foo"">Foo content</li>
          <li>New element</li>
          <li custom:child_attrib=""bar"">Bar content</li>
          <li custom:child_attrib=""baz"">Baz content</li>
        </ul>
      </div>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    [Test]
    public void TestInsertAfter()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild.FirstChild,
                                  _sourceFile);
      var replacementXml = "<li>New element</li>";
      var secondDocument = new XmlDocument();
      secondDocument.LoadXml(replacementXml);
      var secondElement = new ZptXmlElement(secondDocument.DocumentElement,
                                             _sourceFile);

      var referenceElement = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild.FirstChild.ChildNodes[1],
                                                _sourceFile);

      // Act
      var result = sut.InsertAfter(referenceElement, secondElement);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual("li", result.Name, "Result name");
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <div custom:parent_attrib=""Attribute value one"" class=""class_one class_two"">
        <ul>
          <li custom:child_attrib=""foo"">Foo content</li>
          <li custom:child_attrib=""bar"">Bar content</li>
          <li>New element</li>
          <li custom:child_attrib=""baz"">Baz content</li>
        </ul>
      </div>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    [Test(Description = "Test that an XmlElement instance can be constructed from an element node")]
    public void TestConstructorElementNode()
    {
      // Act
      new ZptXmlElement(_document.DocumentElement.FirstChild,
                      _sourceFile);

      // Assert
      Assert.Pass("Test passes because no exception encountered");
    }

    [Test(Description = "Test that an XmlElement instance can be constructed from a document node")]
    public void TestConstructorDocumentElement()
    {
      // Act
      new ZptXmlElement(_document.DocumentElement,
                      _sourceFile);

      // Assert
      Assert.Pass("Test passes because no exception encountered");
    }

    [Test]
    public void TestAddCommentBefore()
    {
      // Arrange
      var docElement = new ZptXmlElement(_document.DocumentElement,
                                       _sourceFile);
      var sut = docElement.GetChildElements()[1].GetChildElements()[0].GetChildElements().First();
      string expectedResult = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <!--Foo bar baz-->
      <div custom:parent_attrib=""Attribute value one"" class=""class_one class_two"">
        <ul>
          <li custom:child_attrib=""foo"">Foo content</li>
          <li custom:child_attrib=""bar"">Bar content</li>
          <li custom:child_attrib=""baz"">Baz content</li>
        </ul>
      </div>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";

      // Act
      sut.AddCommentBefore("Foo bar baz");

      // Assert
      Assert.AreEqual(expectedResult, docElement.ToString().Replace(Environment.NewLine, "\n"));
    }

    [Test]
    public void TestIsInNamespace()
    {
      // Arrange
      var doc = new XmlDocument();
      doc.LoadXml(@"<foo xmlns:ns=""http://ns.csf-dev.com/sample""><ns:bar /></foo>");

      var sut = new ZptXmlElement(doc.DocumentElement.FirstChild, _sourceFile, isRoot: true);

      // Act
      var result = sut.IsInNamespace(new ZptNamespace(prefix: "ns", uri: "http://ns.csf-dev.com/sample"));

      // Assert
      Assert.IsTrue(result);
    }

    [Test]
    public void TestIsInNamespaceDefault()
    {
      // Arrange
      var doc = new XmlDocument();
      doc.LoadXml("<foo><bar /></foo>");

      var sut = new ZptXmlElement(doc.DocumentElement.FirstChild, _sourceFile, isRoot: true);

      // Act
      var result = sut.IsInNamespace(ZptNamespace.Default);

      // Assert
      Assert.IsTrue(result);
    }

    [Test]
    public void TestOmit()
    {
      // Arrange
      var sut = new ZptXmlElement(_document.DocumentElement.ChildNodes[1].FirstChild.FirstChild,
                                   _sourceFile);

      // Act
      var result = sut.Omit();

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Length, "Result length");
      var expectedDom = @"<html xmlns:custom=""http://ns.csf-dev.com/custom"">
  <head>
    <title>Document title</title>
  </head>
  <body>
    <header>
      <ul>
        <li custom:child_attrib=""foo"">Foo content</li>
        <li custom:child_attrib=""bar"">Bar content</li>
        <li custom:child_attrib=""baz"">Baz content</li>
      </ul>
      Page header
    </header>
    <section>
      <header>
        <h1 id=""page_heading"">Page heading</h1>
      </header>
      <p>A paragraph of content</p>
    </section>
    <footer>Page footer</footer>
  </body>
</html>";
      Assert.AreEqual(expectedDom,
                      new ZptXmlElement(_document.DocumentElement, _sourceFile).ToString().Replace(Environment.NewLine, "\n"),
                      "Correct modified XML");
    }

    #endregion
  }
}

